Feature: TestTask
Check User can access the website via URL, then select the Dresses menu, then the highest value dress then add to the Cart

@test
Scenario: [TEST001] - Mercator Test Task
	Given The Site is available
	Then I click on the Dresses Menu item
	Then I select the highest price item
	Then I select the highest price item to the cart


