USE [MindFlavor];
GO

DECLARE @txt NVARCHAR(MAX);
WHILE 1=1
BEGIN
	SET @txt = N'Message from ' + @@SERVERNAME + ' (SPID ' + CONVERT(NVARCHAR, @@SPID) + '). I am ' + USER_NAME() + '. Local time is ' + CONVERT(NVARCHAR, GETDATE());
	
	EXEC Redis.Publish  
		'10.50.50.1',
		--'ubuntu-spare.pelucchi.local',
		6379, N'rieducational:channel', @txt;

	WAITFOR DELAY '00:00:01';
END

USE [master];
GO