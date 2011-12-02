Feature: Get root <hudson> element
	As Niles I need to be able to retrieve root information about a hudson instance.

Scenario: Get empty instance
	Given a node named Test
	#Given the following jobs:
	#	| name | url |
	#And the following views:
	#	| name | url |
	When I get the hudson element
