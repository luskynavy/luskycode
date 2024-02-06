#Add Calc.feature to VS History Files exclusion to avoid specflow to generate .feature.cs with VS history files
# Tool > Options... > VS History Files > VS History Exclusions > +

Feature: Add or multiply 2 integers


Scenario: Do an addition
Given that I have 2 integers 1 and 2
When I ask for an addition
Then the result is 3

Scenario: Do a multiplication
Given that I have 2 integers 2 and 3
When I ask for a multiplication
Then the result is 6


Scenario Outline: Do additions
  Given that I have 2 integers <x> and <y>
  When I ask for an addition
  Then the result is <result>

  Examples:
    | x     | y   | result |
    |    12 |   5 |    17  |
    |    20 |   5 |    25  |