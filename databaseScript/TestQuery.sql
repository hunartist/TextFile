 --select * from RoomApply  where yearID = '1617_1'
 --select * from TitleStartEnd
 --select * from WeekStartEnd where intWeek = 1

--SELECT distinct [strRoomName] FROM [RoomDetail] 

--select * from ApplyList
--select * from RoomApply
--select * from RoomApplySub

 declare 
 @room varchar(50) = '501' ,
 @week int = 1,
 @dep varchar(50) = '0sxy',
 @name varchar(50) = 'test'

select RTRIM(t.currentFlag) as currentFlag
,aaa.* from
(
	 select aa.*
	,w.datePeriod,w.intWeek as wWeek ,w.yearID as wYear
	,d.strRoomName,d.strDepart
	 from 
	(
		select l.applyid,RTRIM(l.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher ,
		l.yearID ,a.strRoom,a.intDay ,a.intStartNum ,a.intEndNum ,a.strWeekReg ,a.strWeekData 		
		,s.intweek 
		from ApplyList l
		inner join RoomApply a on l.applyid = a.G_id
		inner join RoomApplySub s on a.id = s.F_id
		where l.strName = @name
	) as aa
	right join WeekStartEnd w on w.yearID = aa.yearID
	right join RoomDetail d on d.strDepart = @dep
	where d.strRoomName =  @room and w.intWeek = @week
) as aaa
left join TitleStartEnd t on aaa.wYear = t.yearID and t.currentFlag = 'true'
where t.currentFlag = 'true'

--"select t.currentFlag ,aaa.* from(select aa.*,w.datePeriod,w.intWeek as wWeek ,w.yearID as wYear	,d.strRoomName,d.strDepart from (select a.id,a.yearID ,a.strRoom,a.intDay ,a.intStartNum ,a.intEndNum ,a.intStartWeek ,a.intEndWeek ,RTRIM(a.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher ,s.intweek from RoomApply a inner join RoomApplySub s on a.id = s.F_id	where a.strRoom = '"+RoomName+"' and s.intWeek = "+weekNum+") as aa	right join WeekStartEnd w on w.yearID = aa.yearID right join RoomDetail d on d.strDepart = '"+departmentName+"' where d.strRoomName =  '"+RoomName+"' and w.intWeek = "+weekNum+") as aaa left join TitleStartEnd t on aaa.wYear = t.yearID and t.currentFlag = 'true'where t.currentFlag = 'true'"

--"select t.currentFlag,aaa.* from(select aa.*,w.datePeriod,w.intWeek as wWeek ,w.yearID as wYear	,d.strRoomName,d.strDepart from (select a.id,a.yearID ,a.strRoom,a.intDay ,a.intStartNum ,a.intEndNum ,a.intStartWeek ,a.intEndWeek ,RTRIM(a.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher ,s.intweek from RoomApply a inner join RoomApplySub s on a.id = s.F_id	where a.strRoom = '"+RoomName+"' and s.intWeek = "+weekNum+") as aa	right join WeekStartEnd w on w.yearID = aa.yearID right join RoomDetail d on d.strDepart = '"+departmentName+"'	where d.strRoomName =  '"+RoomName+"' and w.intWeek = "+weekNum+") as aaa left join TitleStartEnd t on aaa.wYear = t.yearID and t.currentFlag = 'true'"



----select aaa.*,t.currentFlag from 
----	(
----	select aa.*,d.strRoomName,d.strDepart,d.strCDep,w.datePeriod,w.yearID from 
----		(
----		select distinct s.intWeek,a.yearID as Ayear, a.strRoom,a.intDay,a.intStartNum,a.intEndNum,RTRIM(a.strName) as strName,
----		RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher 
----		from RoomApply a 
----		inner join RoomApplySub s on a.id = s.F_id
----		) as aa 
----	right join RoomDetail d on aa.strRoom = d.strRoomName and aa.intWeek = 1 
----	right join WeekStartEnd w on  w.intWeek = 1 and aa.Ayear = w.yearID 
----	where d.strRoomName = '301(120)' and d.strDepart= '多媒体' 
----	) as aaa 
----right join TitleStartEnd t on aaa.yearID = t.yearID and t.currentFlag = 'true'
			
--			select RTRIM(t.currentFlag) as currentFlag ,aaa.* from(select aa.*,w.datePeriod,w.intWeek as wWeek ,w.yearID as wYear	,d.strRoomName,d.strDepart from (select a.id,a.yearID ,a.strRoom,a.intDay ,a.intStartNum ,a.intEndNum ,a.intStartWeek ,a.intEndWeek ,RTRIM(a.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher ,s.intweek from RoomApply a inner join RoomApplySub s on a.id = s.F_id	where a.strRoom = '301(120)' and s.intWeek = 11) as aa	right join WeekStartEnd w on w.yearID = aa.yearID right join RoomDetail d on d.strDepart = '多媒体' where d.strRoomName =  '301(120)' and w.intWeek = 11) as aaa left join TitleStartEnd t on aaa.wYear = t.yearID and t.currentFlag = 'true'where t.currentFlag = 'true'

