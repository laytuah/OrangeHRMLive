using FluentAssertions;
using Newtonsoft.Json;
using OrangeHRMLive.Model.API.Response;
using Reqnroll;
using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace OrangeHRMLive.Utilities.Api
{
    /// <summary>
    /// Fluent, single-parse wrapper for asserting and consuming JSON API responses.
    /// Usage:
    ///   using var r = new ApiResponseWrapper(_api.LastResponse, _output);
    ///   r.LogBody()
    ///    .RootIs("array")
    ///    .Has("0.bookingid")
    ///    .TypeIs("0.bookingid","integer");
    ///
    ///   var list = r.Deserialize<List<BookingListItem>>();
    /// </summary>
    public sealed class ApiResponseReader : IDisposable
    {
        private readonly ApiResponse _response;
        private readonly IReqnrollOutputHelper? _output;
        private readonly JsonDocument _doc;

        public ApiResponseReader(ApiResponse response, IReqnrollOutputHelper? output = null, int? logBodyMaxChars = null)
        {
            _response = response ?? throw new ArgumentNullException(nameof(response));
            _output = output;

            _response.Content.Should().NotBeNullOrWhiteSpace("response body should not be empty");
            _doc = JsonDocument.Parse(_response.Content!);

            if (_output != null && logBodyMaxChars.HasValue)
            {
                LogBody(logBodyMaxChars.Value);
            }
        }

        public string RawBody => _response.Content ?? string.Empty;
        public long DurationMs => _response.DurationMs;
        public JsonElement Root => _doc.RootElement;

        // ---------- Deserialize & Logging ----------
        public T Deserialize<T>()
        {
            RawBody.Should().NotBeNullOrWhiteSpace("response body should not be empty");
            var result = JsonConvert.DeserializeObject<T>(RawBody);
            result.Should().NotBeNull("failed to deserialize API response into {0}", typeof(T).Name);
            return result!;
        }

        public ApiResponseReader LogBody(int maxChars = 2000)
        {
            if (_output == null) return this;

            var body = RawBody;
            var truncated = body.Length > maxChars ? body[..maxChars] + "…(truncated)" : body;

            _output.WriteLine("---- API Response Body ----");
            _output.WriteLine(truncated);
            _output.WriteLine("---------------------------");
            return this;
        }

        // ---------- Core assertions (6) ----------
        // 1) Exists
        public ApiResponseReader Has(string path)
        {
            JsonElement _;
            JsonPath.TryGet(Root, path, out _).Should().BeTrue($"response should contain '{path}'");
            return this;
        }

        // 2) Type checks: integer|number|string|boolean|object|array|null
        public ApiResponseReader TypeIs(string path, string type)
        {
            JsonPath.TryGet(Root, path, out var el).Should().BeTrue($"response should contain '{path}'");

            switch (type.Trim().ToLowerInvariant())
            {
                case "integer":
                    el.ValueKind.Should().Be(JsonValueKind.Number, $"'{path}' should be a number");
                    el.TryGetInt64(out _).Should().BeTrue($"'{path}' should be an integer");
                    break;
                case "number":
                    el.ValueKind.Should().Be(JsonValueKind.Number, $"'{path}' should be a number");
                    break;
                case "string":
                    el.ValueKind.Should().Be(JsonValueKind.String, $"'{path}' should be a string");
                    break;
                case "boolean":
                    (el.ValueKind == JsonValueKind.True || el.ValueKind == JsonValueKind.False)
                        .Should().BeTrue($"'{path}' should be a boolean");
                    break;
                case "object":
                    el.ValueKind.Should().Be(JsonValueKind.Object, $"'{path}' should be an object");
                    break;
                case "array":
                    el.ValueKind.Should().Be(JsonValueKind.Array, $"'{path}' should be an array");
                    break;
                case "null":
                    el.ValueKind.Should().Be(JsonValueKind.Null, $"'{path}' should be null");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, "Supported: integer, number, string, boolean, object, array, null");
            }
            return this;
        }

        // 3) Equality (auto-detect numbers/bools/null; otherwise string compare)
        public ApiResponseReader EqualTo(string path, string expectedRaw)
        {
            JsonPath.TryGet(Root, path, out var el).Should().BeTrue($"response should contain '{path}'");

            switch (el.ValueKind)
            {
                case JsonValueKind.Number:
                    if (long.TryParse(expectedRaw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i))
                    {
                        el.TryGetInt64(out var actualInt).Should().BeTrue($"'{path}' should be an integer");
                        actualInt.Should().Be(i);
                        return this;
                    }
                    if (double.TryParse(expectedRaw, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var d))
                    {
                        el.GetDouble().Should().Be(d);
                        return this;
                    }
                    throw new AssertionException($"Expected numeric for '{path}', but '{expectedRaw}' is not numeric.");

                case JsonValueKind.True:
                case JsonValueKind.False:
                    bool.TryParse(expectedRaw, out var b).Should().BeTrue($"'{expectedRaw}' should be boolean text");
                    (el.ValueKind == JsonValueKind.True).Should().Be(b);
                    return this;

                case JsonValueKind.String:
                    el.GetString().Should().Be(expectedRaw);
                    return this;

                case JsonValueKind.Null:
                    expectedRaw.Trim().Equals("null", StringComparison.OrdinalIgnoreCase)
                        .Should().BeTrue($"'{path}' is null");
                    return this;

                default:
                    el.ToString().Should().Be(expectedRaw);
                    return this;
            }
        }

        // 4) Contains (strings and arrays)
        public ApiResponseReader ContainsText(string path, string expectedText)
        {
            JsonPath.TryGet(Root, path, out var el).Should().BeTrue($"response should contain '{path}'");

            if (el.ValueKind == JsonValueKind.String)
            {
                el.GetString()!.Should().Contain(expectedText);
                return this;
            }

            if (el.ValueKind == JsonValueKind.Array)
            {
                var anyMatch = el.EnumerateArray().Any(item =>
                {
                    if (item.ValueKind == JsonValueKind.String) return item.GetString()!.Contains(expectedText);
                    return item.ToString().Contains(expectedText);
                });
                anyMatch.Should().BeTrue($"array at '{path}' should contain an element matching '{expectedText}'");
                return this;
            }

            el.ToString().Should().Contain(expectedText);
            return this;
        }

        // 5) Array length (exact)
        public ApiResponseReader ArrayLengthIs(string path, int expectedLen)
        {
            JsonPath.TryGet(Root, path, out var el).Should().BeTrue($"response should contain '{path}'");
            el.ValueKind.Should().Be(JsonValueKind.Array, $"'{path}' should be an array");
            el.GetArrayLength().Should().Be(expectedLen, $"array at '{path}' should have length {expectedLen}");
            return this;
        }

        // 6) Root shape
        public ApiResponseReader RootIs(string shape)
        {
            switch (shape.Trim().ToLowerInvariant())
            {
                case "object":
                    Root.ValueKind.Should().Be(JsonValueKind.Object, "root should be an object");
                    break;
                case "array":
                    Root.ValueKind.Should().Be(JsonValueKind.Array, "root should be an array");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(shape), shape, "Only 'object' or 'array' are supported");
            }
            return this;
        }

        public void Dispose() => _doc.Dispose();

        // ---------- Minimal JSON path helper ----------
        private static class JsonPath
        {
            public static bool TryGet(JsonElement root, string path, out JsonElement found)
            {
                found = root;

                if (string.IsNullOrWhiteSpace(path))
                    return true;

                var p = path.Trim();
                if (p.StartsWith("$"))
                    p = p.Length > 1 && p[1] == '.' ? p[2..] : p[1..];
                p = p.TrimStart('.');

                foreach (var token in Tokenize(p))
                {
                    if (token.IsIndex)
                    {
                        if (found.ValueKind != JsonValueKind.Array) return false;
                        if (token.Index < 0 || token.Index >= found.GetArrayLength()) return false;
                        found = found[token.Index];
                    }
                    else
                    {
                        if (found.ValueKind != JsonValueKind.Object) return false;
                        if (!found.TryGetProperty(token.Name!, out var next)) return false;
                        found = next;
                    }
                }
                return true;
            }

            private static IEnumerable<PathToken> Tokenize(string path)
            {
                foreach (var part in SplitTopLevel(path))
                {
                    if (string.IsNullOrEmpty(part)) continue;

                    var nameMatch = Regex.Match(part, @"^[^\[\]]+");
                    if (nameMatch.Success)
                    {
                        yield return PathToken.FromName(nameMatch.Value);
                    }

                    foreach (Match m in Regex.Matches(part, @"\[(\d+)\]"))
                    {
                        yield return PathToken.FromIndex(int.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture));
                    }

                    // purely numeric token like "0" (from "a.0.b") => index
                    if (!nameMatch.Success && int.TryParse(part, NumberStyles.Integer, CultureInfo.InvariantCulture, out var idxOnly))
                    {
                        yield return PathToken.FromIndex(idxOnly);
                    }
                }
            }

            private static IEnumerable<string> SplitTopLevel(string s)
            {
                var current = "";
                var bracket = 0;
                foreach (var ch in s)
                {
                    if (ch == '[') bracket++;
                    if (ch == ']') bracket--;
                    if (ch == '.' && bracket == 0)
                    {
                        yield return current;
                        current = "";
                    }
                    else
                    {
                        current += ch;
                    }
                }
                if (current.Length > 0) yield return current;
            }

            private readonly struct PathToken
            {
                public bool IsIndex { get; }
                public int Index { get; }
                public string? Name { get; }

                private PathToken(int index)
                {
                    this.IsIndex = true;
                    this.Index = index;
                    this.Name = null;
                }

                private PathToken(string name)
                {
                    this.IsIndex = false;
                    this.Index = -1;
                    this.Name = name;
                }

                public static PathToken FromIndex(int i) => new PathToken(i);
                public static PathToken FromName(string n) => new PathToken(n);
            }
        }

        private sealed class AssertionException : Exception
        {
            public AssertionException(string message) : base(message) { }
        }
    }
}
