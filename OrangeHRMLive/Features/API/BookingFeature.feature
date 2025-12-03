@api
Feature: BookingFeature

Scenario: Get all bookings
	Given that user sends a GET request to 'booking' endpoint
	Then the response should be 200 'OK'
	#And the response should contain BookingID '2284'