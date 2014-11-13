USE [master];
GO

ALTER DATABASE [MindFlavor] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO
DROP DATABASE [MindFlavor];
GO
DROP LOGIN [MindFlavor_Login];
GO
DROP ASYMMETRIC KEY [MindFlavor_Key];
GO