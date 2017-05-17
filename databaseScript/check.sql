 declare 
 @roomid varchar(50) = '1501' ,
 @week int = 3,
 @depid varchar(50) = '1'


select RTRIM(t.currentFlag) as currentFlag  ,dp.strDepart ,aaa.* from
	(
	select aa.*,w.datePeriod,w.intWeek as wWeek ,w.yearID as wYear,d.strRoomName,d.depid from 
		(
		select l.applyid,a.id,RTRIM(l.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher,a.strRemark ,l.yearID, 
		a.roomid, a.intDay, a.intStartNum, a.intEndNum, a.strWeekReg, a.strWeekData, s.intweek from ApplyList l 
		inner join RoomApply a on l.applyid = a.applyid 
		inner join RoomApplySub s on a.id = s.F_id  
		where a.strClass like '%会计1403%' and s.intWeek >= 5 and s.intWeek <=6
		) as aa	
	right join WeekStartEnd w on w.yearID = aa.yearID 
	right join RoomDetail d on d.depid =  '1' and d.roomid = aa.roomid
	where w.intWeek = aa.intWeek
	) as aaa 
left join TitleStartEnd t on aaa.wYear = t.yearID 
inner join Department dp on aaa.depid = dp.depid 
where t.currentFlag = 'true' and intweek = 6
order by id

--"select RTRIM(t.currentFlag) as currentFlag  ,dp.strDepart ,aaa.* from(select aa.*,w.datePeriod,w.intWeek as wWeek ,w.yearID as wYear,d.strRoomName,d.depid from (select l.applyid,a.id,RTRIM(l.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher,a.strRemark ,l.yearID, a.roomid, a.intDay, a.intStartNum, a.intEndNum, a.strWeekReg, a.strWeekData, s.intweek from ApplyList l inner join RoomApply a on l.applyid = a.applyid inner join RoomApplySub s on a.id = s.F_id  where a.roomid = '" + roomid + "' and s.intWeek >= " + week1 + " and s.intWeek <= " + week2 + " ) as aa	right join WeekStartEnd w on w.yearID = aa.yearID right join RoomDetail d on d.depid =  '" + depid + "' where d.roomid =  '" + roomid + "' and w.intWeek = aa.intWeek) as aaa left join TitleStartEnd t on aaa.wYear = t.yearID inner join Department dp on aaa.depid = dp.depid where t.currentFlag = 'true'"

select RTRIM(t.currentFlag) as currentFlag  ,dp.strDepart ,aaa.* from
	(
	select aa.*,w.datePeriod,w.intWeek as wWeek ,w.yearID as wYear,d.strRoomName,d.depid from 
		(
		select l.applyid,a.id,RTRIM(l.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher,a.strRemark ,
		l.yearID, a.roomid, a.intDay, a.intStartNum, a.intEndNum, a.strWeekReg, a.strWeekData, s.intweek 
		from ApplyList l 
		inner join RoomApply a on l.applyid = a.applyid 
		inner join RoomApplySub s on a.id = s.F_id  
		where a.strClass like '%会计1403%' and s.intWeek >= 1 and s.intWeek <= 1 
		) as aa	
	right join WeekStartEnd w on w.yearID = aa.yearID 
	right join RoomDetail d on d.depid =  '1' 
	where aa.strClass like  '%会计1403%' and w.intWeek = aa.intWeek and d.roomid = aa.roomid
	) as aaa 
left join TitleStartEnd t on aaa.wYear = t.yearID 
inner join Department dp on aaa.depid = dp.depid 
where t.currentFlag = 'true'