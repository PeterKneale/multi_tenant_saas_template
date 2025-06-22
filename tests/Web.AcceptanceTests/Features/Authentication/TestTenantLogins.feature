@authentication
Feature: Login

    Scenario: User does not exist

        Given I navigate to the login page
        And I complete the login form
          | Field    | Value                 |
          | Email    | JohnSmith@example.com |
          | Password | passwordpassword      |
        When I click the login button
        Then I should see an 'danger' alert containing 'authentication was not successful'

    Scenario: Honey pot field trapping a bot

        Given I navigate to the login page
        And I complete the login form
          | Field    | Value                 |
          | Email    | JohnSmith@example.com |
          | Password | passwordpassword      |
        And I enter 'X' in the honeypot field
        When I click the login button
        Then I should see an 'danger' alert containing 'bot'

    Scenario: Incomplete form submission

        Given I navigate to the login page
        And I complete the login form
          | Field    | Value      |
          | Email    | <email>    |
          | Password | <password> |
        When I click the login button
        Then the Email field status should be <email_valid>
        And the Password field status should be <password_valid>

    Examples:
      | email   | password         | email_valid | password_valid |
      |         |                  | false       | false          |
      | a@b.com |                  | true        | false          |
      |         | passwordpassword | false       | true           |