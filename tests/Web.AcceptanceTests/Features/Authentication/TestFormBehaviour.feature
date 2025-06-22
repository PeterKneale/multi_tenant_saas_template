@authentication
Feature: Test form behaviour

    Scenario: Can see that both fields are required

        Given I navigate to the login page
        Then the Email label should indicate the field is required
        And the Email field should be required
        And the Password label should indicate the field is required
        And the Password field should be required