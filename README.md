# Sandwich Club API

## Controller Endpoints
##### ControllerBase<T>

Controllers extending this class all contain the following endpoints.

**BaseUri:** [controller]
|Uri|Method|Body|Response|Description|
|-----|----|----|--------|-----------|
||GET||T[]|Get all items|
|{id}|GET||T|Get a single item
||POST|T|T|Insert an item
||PUT|T|T|Update an item
|{id}|DELETE||T|Delete an item

##### UserController : ControllerBase<User>
...
##### WeekController : ControllerBase<Week>
..
|Uri|Method|Body|Response|Description|
|-----|----|----|--------|-----------|
|current|GET||Week|Get the current week
|{weekId}/links|GET||WeekUserLink[]|Get all existing links fror this week
|{weekId}/links/{userId}|GET||WeekUserLink|Get a specific users link for this week
|{weekId}/links/{userId}|POST|WeekUserLink|WeekUserLink|Insert/Update a link

## DTOS
**User:**
|Field|Type|Description|
|-----|----|-----------|
|Id|int|
|FirstName|string|
|LastName|string|
|Email|string|
|AvatarUrl|string|
|Shopper|bool|True if the user is a volunteer for shopping
|BankDetails|string|Bank details for paying the user
|PhoneNumber|string|The users phone number for paying them
|BankName|string|The users bank
|FirstLogin|bool|

**Week:**
|Field|Type|Description|
|-----|----|-----------|
|Id|int|Weeks since 1 Jan 1970 starting on a Monday
|Shopper|int?|The id of the shopper for this week
|Cost|double|The cost of sandwich club for this week
|Links|IEnumerable<WeekUserLink>|

**WeekUserLink:**
|Field|Type|Description|
|-----|----|-----------|
|UserId|int|The user this link belongs to
|WeekId|int|The week this link belongs to
|Paid|double|The amount the user paid the shopper
|Slices|int|
