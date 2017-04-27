declare  @week1T int,@week2T int,@depT nvarchar(20),@num1T int,@num2T int ,@dayT int
set @week1T = 1
set @week2T = 3
set @depT = '1other'
set @num1T =1
set @num2T =3
set @dayT = 1

--select aaa.strRoom,aaa.intWeek,aaa.intStartNum,aaa.intEndNum from 
--	(
--	select  aa.*,d.strRoomName,d.strDepart,d.strCDep from 
--		(
--		select distinct s.intWeek ,RTRIM(a.strRoom) as strRoom, a.intDay,a.intStartNum,a.intEndNum,a.yearID ,a.strWeekReg
--		from RoomApply a 
--		inner join RoomApplySub s on a.id = s.F_id
--		) as aa 
--	inner join RoomDetail d on aa.strRoom = d.strRoomName 
--	where d.strDepart= @depT 

--	and aa.intWeek >= @week1T and aa.intWeek <= @week2T and aa.intDay = @dayT
--	and (( aa.intStartNum  >=@num1T and aa.intStartNum <=@num2T)
--	 or(aa.intEndNum  >=@num1T and aa.intEndNum <=@num2T)
--	 or((aa.intStartNum  < @num1T and aa.intEndNum > @num2T)))
--	) as aaa 
--inner join TitleStartEnd t on aaa.yearID = t.yearID and t.currentFlag = 'true' order by 1

--"select aaa.strRoom,aaa.intWeek,aaa.intStartNum,aaa.intEndNum from (select  aa.*,d.strRoomName,d.strDepart,d.strCDep from (select distinct s.intWeek ,RTRIM(a.strRoom) as strRoom, a.intDay,a.intStartNum,a.intEndNum,a.yearID ,a.strWeekReg from RoomApply a inner join RoomApplySub s on a.id = s.F_id) as aa inner join RoomDetail d on aa.strRoom = d.strRoomName where d.strDepart= '"+depT +"' and aa.intWeek >= "+week1T+" and aa.intWeek <= "+week2T+" and aa.intDay = "+dayT+" and (( aa.intStartNum  >= "+num1T+" and aa.intStartNum <= "+num2T+" ) or(aa.intEndNum  >= "+num1T+" and aa.intEndNum <= "+num2T+" ) or((aa.intStartNum  < "+num1T+" and aa.intEndNum > "+@num2T+" )))) as aaa inner join TitleStartEnd t on aaa.yearID = t.yearID and t.currentFlag = 'true' order by 1"


select distinct s.F_id, s.intWeek ,RTRIM(a.strRoom) as strRoom, a.intDay,a.yearID ,a.strWeekData 
from RoomApply a inner join RoomApplySub s on a.id = s.F_id
order by 1
