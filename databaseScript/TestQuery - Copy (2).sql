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

--delete from RoomApplySub
--delete from RoomApply

--select distinct a.id,a.strRoom,a.intDay,a.intStartNum,a.intEndNum,a.strWeekReg,a.strWeekData,RTRIM(a.strName) as strName,
--RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher,a.yearID 
--from RoomApply a ,RoomDetail d,TitleStartEnd w,RoomApplySub s 
--where a.strRoom = d.strRoomName  and a.yearID = w.yearID and w.currentFlag = 'true' and a.id = s.F_id 
--and d.strDepart = @depN_CP and a.strRoom in ({0}) and s.intWeek in ({1}) and a.intDay in ({2}) 
--and (
--	(a.strName like '%'+ @searchTextBox_CP + '%') or 
--	(a.strTeacher like '%'+ @searchTextBox_CP + '%') or 
--	(a.strClass like '%'+ @searchTextBox_CP + '%') or 
--	(@searchTextBox_CP = 'init')
--) order by a.id desc", sRoom, sWeek, sDay

 select aa.intWeek,aa.intStartNum,aa.intEndNum from 
 (
 select distinct s.intWeek,a.intStartNum,a.intEndNum,l.yearID from RoomApply a,RoomApplySub s,ApplyList l where a.id = s.f_id and a.applyid = l.applyid 
 and a.strRoom = '505                                               ' and  a.intDay = 2 and a.id != 't2_3                     '
  ) as aa 
 inner join TitleStartEnd t on aa.yearID = t.yearID and t.currentFlag = 'true' and aa.intWeek in (3,4)
