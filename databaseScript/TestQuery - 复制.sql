
select t.currentFlag
,aaa.* from
(
	 select aa.*
	,w.datePeriod,w.intWeek as wWeek ,w.yearID as wYear
	--,RTRIM(t.currentFlag) as currentFlag
	,d.strRoomName,d.strDepart
	 from 
	(
		select a.id,a.yearID ,a.strRoom,a.intDay ,a.intStartNum ,a.intEndNum ,a.intStartWeek ,a.intEndWeek 
		,RTRIM(a.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher 
		,s.intweek 
		from RoomApply a 
		inner join RoomApplySub s on a.id = s.F_id
		where a.strRoom =  '606(60)'  and s.intWeek = 6
	) as aa
	right join WeekStartEnd w on w.yearID = aa.yearID
	right join RoomDetail d on d.strDepart ='多媒体'
	where d.strRoomName =  '606(60)' and w.intWeek = 6
) as aaa
left join TitleStartEnd t on aaa.wYear = t.yearID and t.currentFlag = 'true'
where t.currentFlag = 'true'



