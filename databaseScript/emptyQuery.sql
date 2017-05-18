declare  @week1T int,@week2T int,@depid int,@num1T int,@num2T int ,@dayT int ,@yearid nvarchar(20)
set @week1T = 1
set @week2T = 3
set @depid = 1
set @num1T =7
set @num2T =10
set @dayT = 2
set @yearid = '1617_2'

--select distinct RTRIM(d.strRoomName) as strRoomName,w.intWeek,'0123456789z' as num 
--from RoomDetail d 
--right join WeekStartEnd w on 1=1 and w.yearID = @yearid
--where d.depid = @depid and w.intWeek >= @week1T and w.intWeek <= @week2T
--select * from TitleStartEnd

--select aaa.roomid,aaa.intWeek,aaa.intStartNum,aaa.intEndNum from 
--	(
--	select  aa.*,d.strRoomName,d.depid from 
--		(
--		select distinct s.intWeek ,a.roomid, a.intDay,a.intStartNum,a.intEndNum,l.yearID ,a.strWeekReg,a.strClass,a.strTeacher 
--		from RoomApply a 
--		inner join RoomApplySub s on a.id = s.F_id 
--		inner join ApplyList l on a.applyid = l.applyid 
--		where 1=1 
--		and (
--			((a.strClass not like '%会计1401%') and (a.strTeacher <> '')) or ('会计1401' = '') or ('' = '')
--			)
--		) as aa 
--	inner join RoomDetail d on aa.roomid = d.roomid 
--	where d.depid= '1' and aa.intWeek >= 13 and aa.intWeek <= 13 and aa.intDay = 1 
--	and (
--		( aa.intStartNum  >= 1 and aa.intStartNum <= 2 ) 
--		or(aa.intEndNum  >= 1 and aa.intEndNum <= 2 ) 
--		or((aa.intStartNum  < 1 and aa.intEndNum > 2 ))
--		)
--	) as aaa 
--inner join TitleStartEnd t on aaa.yearID = t.yearID and t.currentFlag = 'true' order by 1



select aaa.roomid,aaa.intWeek,aaa.intStartNum,aaa.intEndNum,aaa.strClass,aaa.strTeacher from 
	(
	select  aa.*,d.strRoomName,d.depid from 
		(
		select distinct s.intWeek ,a.roomid, a.intDay,a.intStartNum,a.intEndNum,l.yearID ,a.strWeekReg,a.strClass,a.strTeacher 
		from RoomApply a 
		inner join RoomApplySub s on a.id = s.F_id 
		inner join ApplyList l on a.applyid = l.applyid 
		where 1=1 and (((a.strClass not like '%111%') and (a.strTeacher <> 'js201')) or ('111' = '') or ('js201' = ''))
		) as aa 
	inner join RoomDetail d on aa.roomid = d.roomid 
	where d.depid= '3' and aa.intWeek >= 1 and aa.intWeek <= 1 and aa.intDay = 1 
	and (( aa.intStartNum  >= 1 and aa.intStartNum <= 2 ) or(aa.intEndNum  >= 1 and aa.intEndNum <= 2 ) or((aa.intStartNum  < 1 and aa.intEndNum > 2 )))
	) as aaa 
inner join TitleStartEnd t on aaa.yearID = t.yearID and t.currentFlag = 'true' order by 1

--select aaa.roomid,aaa.intWeek,aaa.intStartNum,aaa.intEndNum from 
--(select  aa.*,d.strRoomName,d.depid from 
--(select distinct s.intWeek ,a.roomid, a.intDay,a.intStartNum,a.intEndNum,l.yearID ,a.strWeekReg,a.strClass,a.strTeacher 
--from RoomApply a inner join RoomApplySub s on a.id = s.F_id 
--inner join ApplyList l on a.applyid = l.applyid 
--where 1=1 and (((a.strClass not like '%111%') and (a.strTeacher <> '李岚')) or ('111' = '') or ('李岚' = ''))
--) as aa 
--inner join RoomDetail d on aa.roomid = d.roomid 
--where d.depid= '1' and aa.intWeek >= 1 and aa.intWeek <= 1 and aa.intDay = 1 
--and (( aa.intStartNum  >= 1 and aa.intStartNum <= 1 ) or(aa.intEndNum  >= 1 and aa.intEndNum <= 1 ) or((aa.intStartNum  < 1 and aa.intEndNum > 1 )))
--) as aaa 
--inner join TitleStartEnd t on aaa.yearID = t.yearID and t.currentFlag = 'true' order by 1