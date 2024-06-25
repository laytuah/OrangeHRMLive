Feature: HRMLogin

Scenario: User can login with details provided on page for login
	Given that user navigates to HRMLive page
	When the user supplies the provided login details
	Then the user must land on the homepage