INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
VALUES
(NEWID(), 'Participant', 'PARTICIPANT', NEWID()),
(NEWID(), 'Organizer', 'ORGANIZER', NEWID()),
(NEWID(), 'Admin', 'ADMIN', NEWID());