select * from RoomApply
select * from RoomApplySub
SELECT distinct RTRIM(strDepart) as strDepart FROM [RoomDetail] WHERE [strDepart] in ('多媒体','0sxy')
select * from [RoomDetail]
--delete from RoomApplySub
--delete from RoomApply

--select distinct a.strRoom,a.intDay,a.intPeriodNum,a.strName,a.strClass,a.strTeacher,a.intStartWeek,a.intEndWeek,a.strApplication from RoomApply a inner join RoomApplySub s on a.Id=s.F_id

select a.strRoom,a.intDay,a.intStartNum,a.intEndNum,a.strName,a.strClass,a.strTeacher from RoomApply a inner join RoomApplySub s on a.Id=s.F_id 

--select * from 
--	(select distinct s.intWeek, a.strRoom,a.intDay,a.intStartNum,a.intEndNum,
--	RTRIM(a.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher from RoomApply a 
--	inner join RoomApplySub s on a.id = s.F_id) as aa 
--right join RoomDetail d on aa.strRoom = d.strRoomName and aa.intWeek = 7
--inner join WeekStartEnd w on  w.id = 7 
--where aa.intWeek = 7 and d.strDepart= '多媒体'
--where d.strRoomName = '501' and d.strDepart= '0sxy'

select * from 
	(select distinct s.intWeek, a.strRoom,a.intDay,a.intStartNum,a.intEndNum
	 from RoomApply a 
	inner join RoomApplySub s on a.id = s.F_id) as aa 
inner join RoomDetail d on aa.strRoom = d.strRoomName -- and aa.intWeek = 7
where aa.intWeek = 7 and d.strDepart= '多媒体'

select * from raUser 
