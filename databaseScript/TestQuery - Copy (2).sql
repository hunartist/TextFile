declare  @weekNum int,@departmentName nvarchar(20),@startNum int,@endNum int
set @weekNum = 5
set @departmentName = '0sxy'
set @startNum =2
set @endNum =5



select aaa.*,t.currentFlag from 
	(
	select  aa.*,d.strRoomName,d.strDepart,d.strCDep from 
		(
		select distinct s.intWeek ,RTRIM(a.strRoom) as strRoom, a.intDay,a.intStartNum,a.intEndNum,a.yearID 
		from RoomApply a 
		inner join RoomApplySub s on a.id = s.F_id
		) as aa 
	inner join RoomDetail d on aa.strRoom = d.strRoomName 
	--where aa.intWeek = @weekNum and d.strDepart= @departmentName
	where d.strDepart= @departmentName and aa.intWeek >= 11 and aa.intWeek <=12 and (( aa.intStartNum  >=@startNum and aa.intStartNum <=@endNum)or(aa.intEndNum  >=@startNum and aa.intEndNum <=@endNum)or((aa.intStartNum  < @startNum and aa.intEndNum > @endNum)))
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

SELECT distinct RTRIM(strDepart) strDepart FROM [RoomDetail] order by 1 desc