select aa.intWeek, aa.intStartNum, aa.intEndNum,aa.strClass,aa.strTeacher from
	(
	select distinct s.intWeek, a.intStartNum, a.intEndNum, l.yearID ,a.strClass,a.strTeacher
	from RoomApply a, RoomApplySub s, ApplyList l 
	where a.id = s.f_id and a.applyid = l.applyid and a.strClass = '会计1403-1404' and  a.intDay = 1 
	and a.id != 201705111326152433       
	) as aa 
inner join TitleStartEnd t on aa.yearID = t.yearID and t.currentFlag = 'true' and aa.intWeek in (1,2,3,4,5)

--select * from roomapply