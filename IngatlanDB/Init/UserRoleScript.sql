SET IDENTITY_INSERT dbo.UserRole ON

INSERT INTO dbo.UserRole(Id, Name)
VALUES (1, 'admin');
INSERT INTO dbo.UserRole(Id, Name)
VALUES (2, 'advertiser');

SET IDENTITY_INSERT dbo.UserRole OFF