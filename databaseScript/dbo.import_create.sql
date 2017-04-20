
GO

/****** 对象: SqlProcedure [dbo].[import] 脚本日期: 2017/3/29 8:55:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

alter PROCEDURE [dbo].[import]
	@strRoom_iGroup nvarchar(1000),--'301(120)','302(108)','304(120)','306(120)'
	@intDay_i int ,
	@intStartNum_i int,
	@intEndNum_i int,
	@intStartWeek_i int,
	@intEndWeek_i int,
	@strName_i varchar(50),
	@strClass_i varchar(20),
	@strTeacher_i varchar(20),
	@intOddEvenFlag_i int,
	@strYearID_i varchar(20)

	
AS
	DECLARE	@return_value Int
	declare @firstid varchar(20)
	declare @tempid varchar(20)
	declare @i varchar(2)
	declare @tempRoom varchar(20)
	set @firstid =REPLACE(CONVERT(VARCHAR(100), GETDATE(), 12) + CONVERT(VARCHAR(100), GETDATE(), 108),':','')+ 'import'
	set @i = '1'

	create table #roomname (strRoomName varchar(100))
	exec  ('insert into #roomname (strRoomName) select strRoomName from RoomDetail where strRoomName in ('+@strRoom_iGroup+')')
	declare rid CURSOR for select strRoomName from #roomname
	OPEN rid
	FETCH NEXT from rid into @tempRoom
	while @@FETCH_STATUS = 0
	begin
	--select @tempRoom,@tempid+CONVERT(varchar(5),@i)
	if (CAST(@i as int)<10)
		begin
		set @i='0'+@i
		end
	set @tempid = @firstid+@i
	exec [dbo].[RoomApplyAction]
			@Action = N'insert',
			@strRoom = @tempRoom,
			@intDay = @intDay_i,
			@intStartNum = @intStartNum_i,
			@intEndNum = @intEndNum_i,
			@intStartWeek = @intStartWeek_i,
			@intEndWeek = @intEndWeek_i,
			@strName = @strName_i,
			@strClass = @strClass_i,
			@strTeacher = @strTeacher_i,
			@strYearID = @strYearID_i,
			@intOddEvenFlag = @intOddEvenFlag_i,
			@id = @tempid
	set @i = CAST(@i as int) + 1
	FETCH NEXT from rid into @tempRoom
	end
	close rid
	deallocate  rid
	drop table #roomname
RETURN 0
