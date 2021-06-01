Feature: CRUD

@Create
Scenario: Add record to DB
	Given DB exists
	And Computer is connected to DB
	When I send create request
	Then record become in DB

@Delete
Scenario: Delete record from DB
	Given DB exists
	And Computer is connected to DB
	And Record exist
	When I send delete request
	Then record deleted from DB

@Update
Scenario: Update record in DB
	Given DB exists
	And Computer is connected to DB
	And Record exist
	When I send update request
	Then record updated in DB

@Update
Scenario: Update not existed record in DB
	Given DB exists
	And Computer is connected to DB
	When I send update request with dont exist record
	Then records dont change

