USE [test]
GO

/****** 对象: SqlProcedure [dbo].[RoomApplyAction] 脚本日期: 2017/3/28 22:39:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[RoomApplyAction]
	@Action varchar(10),
	@strRoom varchar(50) = null,
	@intDay int = null,
	@intStartNum int = null,
	@intEndNum int = null,
	@intStartWeek int = null,
	@intEndWeek int = null,
	@strName varchar(50) = null,
	@strClass varchar(20) = null,
	@strTeacher varchar(20) = null,
	@strYearID varchar(20) = null,
	@id varchar(30)
	
AS
	declare @count int
	if (@Action = 'update')
	begin
		UPDATE [RoomApply] SET [strRoom] = @strRoom , [intDay] = @intDay , [intStartNum] = @intStartNum , [intEndNum] = @intEndNum , [intStartWeek] = @intStartWeek , [intEndWeek] = @intEndWeek , [strName] = @strName , [strClass] = @strClass , [strTeacher] = @strTeacher WHERE [id] = @id
		delete from [RoomApplySub] where [F_id] = @id		
		set @count = 0
		while @count <= @intEndWeek - @intStartWeek
		begin
			insert into [RoomApplySub] values (@id,@intStartWeek+@count)
			set @count = @count + 1
		end
	end
	else if (@Action = 'insert')
	begin
		insert into [RoomApply] values (@id,@strRoom,@intDay,@intStartNum,@intEndNum,@intStartWeek,@intEndWeek,@strName,@strClass,@strTeacher,'','',GETDATE(),@strYearID)
		set @count = 0
		delete from [RoomApplySub] where [F_id] = @id
		while @count <= @intEndWeek - @intStartWeek
		begin
			insert into [RoomApplySub] values (@id,@intStartWeek+@count)
			set @count = @count + 1
		end
	end
	else if (@Action = 'delete')
	begin
		delete from [RoomApplySub] where [F_id] = @id
		delete from [RoomApply] where [id] = @id		
	end
RETURN 0