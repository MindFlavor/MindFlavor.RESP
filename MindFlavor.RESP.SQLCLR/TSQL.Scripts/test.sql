USE [MindFlavor];
GO

DECLARE @txt NVARCHAR(MAX);
WHILE 1=1
BEGIN
	SET @txt = N'sample message from SPID ' + CONVERT(NVARCHAR, @@SPID) + '. I am ' + USER_NAME() + ' at local time is ' + CONVERT(NVARCHAR, GETDATE());
	EXEC Redis.Publish  
		'ubuntu-spare.pelucchi.local',
		6379, N'channel', @txt;

	WAITFOR DELAY '00:00:01';
END

USE [master];
GO