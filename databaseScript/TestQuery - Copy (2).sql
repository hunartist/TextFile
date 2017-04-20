--select s.intWeek,a.* from RoomApply a,RoomApplySub s where a.id = s.F_id and a.strRoom = '录播室'

--select * from RoomApplySub where F_id = '170412213356import01'

 select aa.intWeek,aa.intStartNum,aa.intEndNum from 
	(
	select distinct s.intWeek,a.intStartNum,a.intEndNum,a.yearID from RoomApply a,RoomApplySub s  
	where a.id = s.f_id and a.strRoom = '616(60)   ' and  a.intDay = 5 and a.id != '170329223617import19' 
	) as aa 
	inner join TitleStartEnd t on aa.yearID = t.yearID and t.currentFlag = 'true' 