Feature: HRMFeatures
	
Background:
	Given that user navigates to HRMLive page
	When the user supplies the provided login details

Scenario: User can login with details provided on page for login
	Then the user must land on the homepage

Scenario: User can add a new employee record
	When the user adds a new employee record
	Then newly created record must be found on employee list

Scenario: User can update existing employee record
	When the user updates the last employee on employee list
	Then the last employee record must be updated

Scenario: User can delete existing employee record
	When the user deletes the last employee on employee list
	Then the last record must be deleted from employee list
