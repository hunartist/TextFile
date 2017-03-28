USE [webTest]
GO

DECLARE	@return_value Int

EXEC	@return_value = [dbo].[RoomApplyAction]
		@Action = N'update',
		@strRoom = N'200',
		@intDay = 5,
		@intStartNum = 5,
		@intEndNum = 7,
		@intStartWeek = 2,
		@intEndWeek = 5,
		@strName = N's',
		@strClass = N'c',
		@strTeacher = N't',
		@id = 5

SELECT	@return_value as 'Return Value'

GO
