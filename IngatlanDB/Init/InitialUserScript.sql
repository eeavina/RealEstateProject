SET IDENTITY_INSERT [dbo].[User] ON

INSERT [dbo].[User] ([Id], [UserName],[Email],[Password],[UserRoleId],[Salt])
VALUES (1, 'admin', 'admin@email.com', 'ECD03C98E99E42007BE867EE2A63EE250F890D813956D54E12C843A654381F27', 1, 'iAwuzWo9o/mtRP2+lbKyMg==')

INSERT [dbo].[User] ([Id],[UserName],[Email],[Password],[UserRoleId],[Salt])
VALUES (2, 'testuser1', 'testuser1@email.com', '0DC0B462FEFA07A2FB0EAB142B28BB7F8DB069BFC65D4483E28F3CECB3845F29', 2, 'f1tq3cjc4r9sNfW+PutTyg==')

INSERT [dbo].[User] ([Id],[UserName],[Email],[Password],[UserRoleId],[Salt])
VALUES (3, 'testuser2', 'testuser2@email.com', '7DD3B21E8045AF35E8D24E23B3CD1F4A3282C4FE47B7DB3A80BACBF5D8453483', 2, '9BXUWBXOLFMpagQ4YoiXmA==')

SET IDENTITY_INSERT [dbo].[User] OFF