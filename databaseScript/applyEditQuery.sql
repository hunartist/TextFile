declare @searchTextBox_CP nvarchar(50),@strCdep  nvarchar(50)
set @searchTextBox_CP = 'init'
set @strCdep ='0s'




select aaa.*,t.currentFlag,de.strDepart from 
	(
	select  aa.*,d.strRoomName,d.depid from 
		(
		select distinct s.intWeek, a.roomid,a.intDay,a.intStartNum,a.intEndNum,l.yearID 
		from RoomApply a 
		inner join ApplyList l on a.applyid = l.applyid
		inner join RoomApplySub s on a.id = s.F_id
		) as aa 
	inner join RoomDetail d on aa.roomid = d.roomid 	
	where aa.intWeek = 1 and d.depid = 1
	) as aaa 
inner join TitleStartEnd t on aaa.yearID = t.yearID and t.currentFlag = 'true'
inner join Department de on aaa.depid = de.depid

select aaa.*,t.currentFlag,de.strDepart from (select  aa.*,d.strRoomName,d.depid from (select distinct s.intWeek, a.roomid,a.intDay,a.intStartNum,a.intEndNum,l.yearID from RoomApply a inner join ApplyList l on a.applyid = l.applyid	inner join RoomApplySub s on a.id = s.F_id) as aa inner join RoomDetail d on aa.roomid = d.roomid where aa.intWeek = " + prtWeekNum + " and d.depid = '" + prtDep + "') as aaa inner join TitleStartEnd t on aaa.yearID = t.yearID and t.currentFlag = 'true'inner join Department de on aaa.depid = de.depid