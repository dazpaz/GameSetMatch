Feature: Can view the Tournaments that make up the Tour

  As an user of the GSM Admin Console
  I want to view a list of tournaments
  So that I can see the tournaments that are part of the tour

  # The list of tournaments is shown and contains the desired information  

  Scenario: Can view the list of the active tournaments
    Given I access the tour management page
    Then I should see a list of the active tournaments
    And The list should contain the correct set of tournaments

  Scenario: The details of each tournament is shown
    Given I access the tour management page
    Then The properties of each tournament are displayed
      | Tournament      |
      | Location        |
      | Level           |
      | Surface         |
      | Men's Singles   |
      | Women's Singles |
      | Men's Doubles   |
      | Women's Doubles |
      | Mixed Doubles   |

  # Able to select if inactive tournaments are shown

  Scenario: Initially inactive tournaments are not shown
    Given I access the tour management page
    Then The option for showing inactive tournaments is "off"
    And Inactive tournaments are not shown

  Scenario: Can opt to show inactive tournaments
    Given I access the tour management page
    And The option for showing inactive tournaments is "off"
    When I opt to "view" inactive tournaments
    Then The option for showing inactive tournaments is "on"
    And Inactive tournaments are included in the list

  Scenario: With inactive tournaments shown, can hide them again
    Given I access the tour management page
    And I have inactive tournaments shown
    When I opt to "hide" inactive tournaments
    Then The option for showing inactive tournaments is "off"
    And Inactive tournaments are not shown

  Scenario: Visual indication of inactive tournaments
    Given I access the tour management page
    And I have inactive tournaments shown
    Then Inactive tournaments are marked with a visual indicator

  # can search the tournament list by title

  Scenario: Can search the tournament list by title
    Given I access the tour management page
    When I search the tournament list using a search text
    Then Only the tournaments whose title contains the search text are shown

  Scenario: Search text is case insensitive
    Given I access the tour management page
    When I search the tournament list using a search text with different casing
    Then Only the tournaments whose title contains the search text are shown

  Scenario: Clear the search text shows all tournaments
    Given I access the tour management page
    And I have searched the tournament list using a search text
    When I clear the search text
    Then All tournaments are shown
    
  # Sorting the tournament list

  @skip
  Scenario: Initially the tournaments are sorted by title ascending
    Given I access the tour management page
    Then The tournaments are shown in ascending title order

  @skip
  Scenario: Can sort the list of tournaments by title
    Given I access the tour management page
    When I sort the tournament list by title
    Then the list of tournaments is sorted by title

  @skip
  Scenario: Can sort the list of tournaments by level
    Given I access the tour management page
    When I sort the tournament list by level
    Then the list of tournaments is sorted by level

  @skip
  Scenario: Can sort the list of tournaments by surface
    Given I access the tour management page
    When I sort the tournament list by surface
    Then the list of tournaments is sorted by surface

  # Filtering the tournament list

  @skip
  Scenario: Can filter the tournament list by level
    Given I access the tour management page
    When I filter the tournament list by level
    Then Only the tournaments of the filtered levels are shown

  @skip
  Scenario: Can filter the tournament list by surface
    Given I access the tour management page
    When I filter the tournament list by surface
    Then Only the tournaments of the filtered surfaces are shown

  @skip
  Scenario: Filter by Events included in the tournament
    Given I access the tour management page
    When I filter the tournament list by Events
    Then Only the tournaments that contain the selected event are shown
    