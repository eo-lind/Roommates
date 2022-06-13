SELECT
	Chore.Id, Chore.Name, Roommate.FirstName
FROM Chore
LEFT JOIN RoommateChore
ON RoommateChore.ChoreId = Chore.Id
LEFT JOIN Roommate
ON RoommateChore.RoommateId = Roommate.Id
WHERE Roommate.Id IS NULL;