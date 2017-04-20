USE [Test]
GO

DECLARE	@return_value Int

EXEC	@return_value = [dbo].[RoomApplyAction]
		@Action = N'update',
		@strRoom = N'301(120)',
		@intDay = 5,
		@intStartNum = 5,
		@intEndNum = 7,
		@intStartWeek = 1,
		@intEndWeek = 17,
		@strName = N's',
		@strClass = N'c',
		@strTeacher = N't',
		@intOddEvenFlag = 1,
		@id = 'testOddEven1'

SELECT	@return_value as 'Return Value'
select a.intOddEvenFlag,s.intWeek,a.* from RoomApply a,RoomApplySub s where a.id = s.F_id and a.id = 'testOddEven1'

GO
