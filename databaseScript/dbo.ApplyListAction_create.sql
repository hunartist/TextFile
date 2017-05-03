--USE [raTest]
GO

/****** 对象: SqlProcedure [dbo].[ApplyListAction] 脚本日期: 2017/5/2 11:31:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[ApplyListAction]
	@Action varchar(10),	
	@strName varchar(50) = null,	
	@strYearID varchar(50) = null,
	@strCDep varchar(50) = null,
	@strRemark varchar(500) = null,
	@applyid varchar(20) = null
	
AS
	declare @countWeek int
	if (@Action = 'update')
	begin
		UPDATE [ApplyList] SET [strName] = @strName , [yearID] = @strYearID , [strCDep] = @strCDep , [strRemark] = @strRemark WHERE [applyid] = @applyid	
	end
	else if (@Action = 'insert')
	begin
		insert into [ApplyList] values (@applyid,@strName,@strYearID,@strCDep,@strRemark)		
	end
	else if (@Action = 'delete')
	begin
		delete from [RoomApplySub] where [F_id] in (select distinct [id] from [RoomApply] where [applyid] = @applyid)
		delete from [RoomApply] where [applyid] = @applyid
		delete from [ApplyList] where [applyid] = @applyid			
	end
RETURN 0
