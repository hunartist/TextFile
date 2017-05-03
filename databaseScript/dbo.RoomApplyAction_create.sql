
GO

/****** 对象: SqlProcedure [dbo].[RoomApplyAction] 脚本日期: 2017/4/20 15:13:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[RoomApplyAction]
	@Action varchar(10),
	@strRoom varchar(50) = null,
	@intDay int = null,
	@intStartNum int = null,
	@intEndNum int = null,
	@strWeekReg varchar(200) = null,
	@strWeekData varchar(500) = null,
	@strName varchar(50) = null,
	@strClass varchar(20) = null,
	@strTeacher varchar(20) = null,
	@strYearID varchar(20) = null,	
	@id varchar(30)
	
AS
	declare @countWeek int
	if (@Action = 'update')
	begin
		UPDATE [RoomApply] SET [strRoom] = @strRoom , [intDay] = @intDay , [intStartNum] = @intStartNum , [intEndNum] = @intEndNum , [strWeekReg] = @strWeekReg , [strWeekData] = @strWeekData , [strClass] = @strClass , [strTeacher] = @strTeacher WHERE [id] = @id
		delete from [RoomApplySub] where [F_id] = @id		
		set @countWeek = 0
		declare wid CURSOR for SELECT weekid FROM [dbo].[ufn_SplitStringToTable](@strWeekData,N',')
		OPEN wid
		FETCH NEXT from wid into @countWeek
		while @@FETCH_STATUS = 0
		begin
			insert into [RoomApplySub] values (@id,@countWeek)
			FETCH NEXT from wid into @countWeek
		end
		close wid
		deallocate  wid
	end
	else if (@Action = 'insert')
	begin
		insert into [RoomApply] values (@id,@strRoom,@intDay,@intStartNum,@intEndNum,'','',@strName,@strClass,@strTeacher,'','',GETDATE(),@strYearID,'',@strWeekReg,@strWeekData)
		set @countWeek = 0
		delete from [RoomApplySub] where [F_id] = @id
		declare wid CURSOR for SELECT weekid FROM [dbo].[ufn_SplitStringToTable](@strWeekData,N',')
		OPEN wid
		FETCH NEXT from wid into @countWeek
		while @@FETCH_STATUS = 0
		begin
			insert into [RoomApplySub] values (@id,@countWeek)
			FETCH NEXT from wid into @countWeek
		end
		close wid
		deallocate  wid

	end
	else if (@Action = 'delete')
	begin
		delete from [RoomApplySub] where [F_id] = @id
		delete from [RoomApply] where [id] = @id		
	end
RETURN 0
