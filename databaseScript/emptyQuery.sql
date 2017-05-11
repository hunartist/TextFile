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

select aaa.roomid,aaa.intWeek,aaa.intStartNum,aaa.intEndNum from 
	(
	select  aa.*,d.strRoomName,d.depid from 
		(
		select distinct s.intWeek ,a.roomid, a.intDay,a.intStartNum,a.intEndNum,l.yearID ,a.strWeekReg 
		from RoomApply a 
		inner join RoomApplySub s on a.id = s.F_id
		inner join ApplyList l on a.applyid = a.applyid
		) as aa 
	inner join RoomDetail d on aa.roomid = d.roomid 
	where d.depid= @depid and aa.intWeek >= @week1T and aa.intWeek <= @week2T and aa.intDay = @dayT 
	and (( aa.intStartNum  >= @num1T and aa.intStartNum <= @num2T ) 
		or(aa.intEndNum  >= @num1T and aa.intEndNum <= @num2T ) 
		or((aa.intStartNum  < @num1T and aa.intEndNum > @num2T )))
	) as aaa 
inner join TitleStartEnd t on aaa.yearID = t.yearID and t.currentFlag = 'true' order by 1
