declare  @startWeek int,@endWeek int,@departmentName nvarchar(20),@startNum int,@endNum int ,@dayW int
set @startWeek = 11
set @endWeek = 12
set @departmentName = '0sxy'
set @startNum =3
set @endNum =4
set @dayW = 1

select aaa.*,t.currentFlag from 
	(
	select  aa.*,d.strRoomName,d.strDepart,d.strCDep from 
		(
		select distinct s.intWeek ,RTRIM(a.strRoom) as strRoom, a.intDay,a.intStartNum,a.intEndNum,a.yearID ,a.strWeekReg
		from RoomApply a 
		inner join RoomApplySub s on a.id = s.F_id
		) as aa 
	inner join RoomDetail d on aa.strRoom = d.strRoomName 
	where d.strDepart= @departmentName 
	--and aa.intWeek >= @startWeek and aa.intWeek <= @endWeek and aa.intDay = @dayW
	--and (( aa.intStartNum  >=@startNum and aa.intStartNum <=@endNum)
	-- or(aa.intEndNum  >=@startNum and aa.intEndNum <=@endNum)
	-- or((aa.intStartNum  < @startNum and aa.intEndNum > @endNum)))
	) as aaa 
inner join TitleStartEnd t on aaa.yearID = t.yearID and t.currentFlag = 'true' order by 1

--"and (( aa.intStartNum  >= "+startNum+" and aa.intStartNum <= "+@endNum+" )or(aa.intEndNum  >= "+@startNum+" and aa.intEndNum <= "+@endNum+" )or((aa.intStartNum  < "+@startNum+" and aa.intEndNum > "+@endNum+" )))"

--select aaa.*,t.currentFlag from 
--	(
--	select  aa.*,d.strRoomName,d.strDepart,d.strCDep from 
--		(
--		select distinct s.intWeek, RTRIM(a.strRoom) as strRoom,a.intDay,a.intStartNum,a.intEndNum,a.yearID 
--		from RoomApply a inner join RoomApplySub s on a.id = s.F_id
--		) as aa 
--	inner join RoomDetail d on aa.strRoom = d.strRoomName 
--	where aa.intWeek >= '11' and aa.intWeek <= '17' and d.strDepart= '多媒体' 
--	and (( aa.intStartNum  >= 1 and aa.intStartNum <= 10 )or(aa.intEndNum  >= 1 and aa.intEndNum <= 10 )or((aa.intStartNum  < 1 and aa.intEndNum > 10 )))
--	) as aaa 
--inner join TitleStartEnd t on aaa.yearID = t.yearID and t.currentFlag = 'true'

