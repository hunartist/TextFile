select * from RoomApply where strRoom = '302(108)' order by yearID
select * from RoomApplySub where F_id = '170411214413import01'
SELECT distinct RTRIM(strDepart) as strDepart FROM [RoomDetail] WHERE [strDepart] in ('多媒体','0sxy')
select * from [RoomDetail]
--delete from RoomApplySub
--delete from RoomApply

--select distinct a.strRoom,a.intDay,a.intPeriodNum,a.strName,a.strClass,a.strTeacher,a.intStartWeek,a.intEndWeek,a.strApplication from RoomApply a inner join RoomApplySub s on a.Id=s.F_id

select a.strRoom,a.intDay,a.intStartNum,a.intEndNum,a.strName,a.strClass,a.strTeacher from RoomApply a inner join RoomApplySub s on a.Id=s.F_id 

--select * from 
--	(select distinct s.intWeek, a.strRoom,a.intDay,a.intStartNum,a.yearID,
--	a.intEndNum,RTRIM(a.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher 
--	from RoomApply a 
--	inner join RoomApplySub s on a.id = s.F_id) as aa
--right join RoomDetail d on aa.strRoom = d.strRoomName and aa.intWeek = 1
--inner join TitleStartEnd t on aa.yearID = t.yearID and t.currentFlag = 'true'
--inner join WeekStartEnd w on  w.intWeek = 1 and aa.yearID = w.yearID
--where d.strRoomName = '301(120)' and d.strDepart= '多媒体'


SELECT [intWeek],'第'+CAST(intWeek as varchar(10)) + '周 ' + datePeriod  as detail FROM [WeekStartEnd] w inner join TitleStartEnd t on w.yearID = t.yearID and t.currentFlag = 'true'

select aaa.*,t.currentFlag from 
	(
	select * from 
		(
		select distinct s.intWeek,a.yearID as Ayear, a.strRoom,a.intDay,a.intStartNum,a.intEndNum,RTRIM(a.strName) as strName,
		RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher from RoomApply a 
		inner join RoomApplySub s on a.id = s.F_id) as aa 
	right join RoomDetail d on aa.strRoom = d.strRoomName and aa.intWeek = 1 
	right join WeekStartEnd w on  w.intWeek = 1	and aa.Ayear = w.yearID
	where d.strRoomName = '302(108)' and d.strDepart= '多媒体' ) as aaa 
right join TitleStartEnd t on aaa.yearID = t.yearID and t.currentFlag = 'true'

"select * from (
	select aa.*,d.strRoomName,d.strDepart,d.strCDep,w.datePeriod,w.yearID from 
		(select distinct s.intWeek, a.strRoom,a.intDay,a.intStartNum,a.intEndNum,RTRIM(a.strName) as strName,RTRIM(a.strClass) as strClass,
		RTRIM(a.strTeacher) as strTeacher 
		from RoomApply a 
		inner join RoomApplySub s on a.id = s.F_id) as aa 
	right join RoomDetail d on aa.strRoom = d.strRoomName and aa.intWeek = "+weekNum+"
	inner join WeekStartEnd w on  w.intWeek = "+weekNum+"
	where d.strRoomName = '"+RoomName+"' and d.strDepart= '"+departmentName+"' ) as aaa
inner join TitleStartEnd t on aaa.yearID = t.yearID and t.currentFlag = 'true'"

"select * from (select aa.*,d.strRoomName,d.strDepart,d.strCDep,w.datePeriod,w.yearID from (select distinct s.intWeek, a.strRoom,a.intDay,a.intStartNum,a.intEndNum,RTRIM(a.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher from RoomApply a inner join RoomApplySub s on a.id = s.F_id) as aa right join RoomDetail d on aa.strRoom = d.strRoomName and aa.intWeek = "+weekNum+" inner join WeekStartEnd w on  w.intWeek = "+weekNum+"	where d.strRoomName = '"+RoomName+"' and d.strDepart= '"+departmentName+"' ) as aaa inner join TitleStartEnd t on aaa.yearID = t.yearID and t.currentFlag = 'true'"



 select aa.intWeek,aa.intStartNum,aa.intEndNum from 
 (select distinct s.intWeek,a.intStartNum,a.intEndNum,a.yearID from RoomApply a,RoomApplySub s  
 where a.id = s.f_id and a.strRoom = '301(120)  ' and  a.intDay = 3 and a.id != '201704131411580430' ) as aa 
 inner join TitleStartEnd t on aa.yearID = t.yearID and t.currentFlag = 'true' 