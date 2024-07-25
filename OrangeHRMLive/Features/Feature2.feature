Feature: Feature2

Background:
	Given that user navigates to HRMLive page
	When the user supplies the provided login details

Scenario: User can login with details provided on page for login
	Then the user must land on the homepage
