Feature: Cancel tournament creation

  Scenario: Confirm whether to discard unsaved changes
    Given I have changed a tournament detail
    When I cancel tournament creation
    And I choose not to discard my changes
    Then I remain on the tournament form with my changes
    When I cancel tournament creation
    And I choose to discard my changes
    Then I return to the tournament list
