@authentication
Feature: TestAdminLogin

    Scenario: Can login

        Given I navigate to the login page
        And I complete the login form
          | Field    | Value               |
          | Email    | admin@localhost.com |
          | Password | passwordpassword    |
        When I click the login button
        Then I should be on the admin home page

    Scenario: Can login with incorrect case email (Case insensitive emails)

        Given I navigate to the login page
        And I complete the login form
          | Field    | Value               |
          | Email    | Admin@LocalHost.COM |
          | Password | passwordpassword    |
        When I click the login button
        Then I should be on the admin home page

    Scenario: Cant login with incorrect password

        Given I navigate to the login page
        And I complete the login form
          | Field    | Value                   |
          | Email    | admin@localhost.com     |
          | Password | wrong_password_password |
        When I click the login button
        Then I should see an 'danger' alert containing 'authentication was not successful'

    Scenario: Cant login with incorrect password (Case sensitive passwords)

        Given I navigate to the login page
        And I complete the login form
          | Field    | Value               |
          | Email    | admin@localhost.com |
          | Password | PASSWORDPASSWORD    |
        When I click the login button
        Then I should see an 'danger' alert containing 'authentication was not successful'