

DECLARE	@return_value Int

EXEC	@return_value = [dbo].[RoomApplyAction]
		@Action = N'insert',
		@strRoom = N'501',
		@intDay = 3,
		@intStartNum = 3,
		@intEndNum = 3,
		@strClass = 3,
		@strTeacher = 3,
		@applyid = 'test',
		@strWeekReg = 3,
		@strWeekData = 3,
		@id = 'insert'	
		

SELECT	@return_value as 'Return Value'
select * from RoomApply where id = 'insert'
select * from RoomApplySub where F_id = 'insert'

GO
