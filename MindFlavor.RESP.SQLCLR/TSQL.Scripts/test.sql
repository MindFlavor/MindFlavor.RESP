USE [MindFlavor];
GO

DECLARE @txt NVARCHAR(MAX);
WHILE 1=1
BEGIN
	SET @txt = N'sample message from SPID ' + CONVERT(NVARCHAR, @@SPID) + '.';
	EXEC Redis.Publish 
		--'10.50.50.1', 
		'gitlab.pelucchi.local',
		6379, N'channel', @txt;

	WAITFOR DELAY '00:00:01';
END

USE [master];
GO