Feature: Create Booking
	As a user
	I want to be able to book a room

Background: 
	And a range of occupied dates
	| From       |  To        |
	| 08/11/2019 | 10/11/2019 |
	| 11/11/2019 | 12/11/2019 |

@mytag
Scenario: Create booking in available dates
	Given a <startDate> and <endDate>
	And a range of occupied dates
	When creating the booking
	Then a new active booking is created

	Examples:
	| startDate  | endDate    |
	| 05/11/2019 | 07/11/2019 |
	| 13/11/2019 | 15/11/2019 |

@mytag
Scenario: Create booking within the occupied range
	Given a <startDate> and <endDate>
	And a range of occupied dates
	When creating the booking
	Then no new booking is created

	Examples:
	| startDate  | endDate    |
	| 07/11/2019 | 13/11/2019 |
	| 09/11/2019 | 13/11/2019 |
	| 07/11/2019 | 09/11/2019 |