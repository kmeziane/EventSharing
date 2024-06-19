-- Associer des rôles aux utilisateurs

DECLARE @userId1 nvarchar(450) = (SELECT Id FROM AspNetUsers WHERE Email = 'participant1@test.com')
DECLARE @roleId1 nvarchar(450) = (SELECT Id FROM AspNetRoles WHERE Name = 'Participant')
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES (@userId1, @roleId1)

DECLARE @userId2 nvarchar(450) = (SELECT Id FROM AspNetUsers WHERE Email = 'organisateur1@test.com')
DECLARE @roleId2 nvarchar(450) = (SELECT Id FROM AspNetRoles WHERE Name = 'Organizer')
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES (@userId2, @roleId2)

DECLARE @userId3 nvarchar(450) = (SELECT Id FROM AspNetUsers WHERE Email = 'admin1@test.com')
DECLARE @roleId3 nvarchar(450) = (SELECT Id FROM AspNetRoles WHERE Name = 'Admin')
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES (@userId3, @roleId3)