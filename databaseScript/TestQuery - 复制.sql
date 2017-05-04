declare @searchTextBox_CP nvarchar(50),@depN_CP  nvarchar(50)
set @searchTextBox_CP = 'init'
set @depN_CP ='0sxy'

--select distinct a.id,a.strRoom,a.intDay,a.intStartNum,a.intEndNum,a.intStartWeek,a.intEndWeek,
--RTRIM(a.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher,a.yearID 
--from RoomApply a ,RoomDetail d,TitleStartEnd w ,RoomApplySub s
--where a.strRoom = d.strRoomName and a.yearID = w.yearID and w.currentFlag = 'true' and a.id = s.F_id
----and s.intWeek in (1)
--and d.strDepart = '多媒体' and a.strRoom in ('402(108)')
--and ((a.strName like '%999%') or (a.strTeacher like '%999%') or (a.strClass like '%999%') or (@searchTextBox_CP = 'init')) 
-- order by a.id desc

--page_load
--select aa.*,t.currentFlag from 
--	(
--	select distinct a.id,a.strRoom,a.intDay,a.intStartNum,a.intEndNum,a.strWeekReg,a.strWeekData,RTRIM(a.strName) as strName,
--	RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher,a.yearID 
--	from RoomApply a ,RoomDetail d 
--	where a.strRoom = d.strRoomName and d.strDepart = @depN_CP 
--	) as aa 
--inner join TitleStartEnd t on aa.yearID = t.yearID and t.currentFlag = 'true'  order by aa.id desc

--depFlit
select distinct l.applyid,a.strRoom,a.intDay,a.intStartNum,a.intEndNum,a.strWeekReg,a.strWeekData,RTRIM(l.strName) as strName,
RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher,l.yearID 
from ApplyList l, RoomApply a ,RoomDetail d,TitleStartEnd w 
where l.applyid = a.G_id and a.strRoom = d.strRoomName  and l.yearID = w.yearID and w.currentFlag = 'true' and d.strDepart = @depN_CP 
order by l.applyid desc

----TotalFlit
--select distinct a.id,a.strRoom,a.intDay,a.intStartNum,a.intEndNum,a.strWeekReg,a.strWeekData,RTRIM(a.strName) as strName,
--RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher,a.yearID 
--from RoomApply a ,RoomDetail d,TitleStartEnd w,RoomApplySub s 
--where a.strRoom = d.strRoomName  and a.yearID = w.yearID and w.currentFlag = 'true' and a.id = s.F_id 
--and d.strDepart = @depN_CP and a.strRoom in ({0}) and s.intWeek in ({1}) and a.intDay in ({2}) 
--and (
--	(a.strName like '%'+ @searchTextBox_CP + '%') 
--	or (a.strTeacher like '%'+ @searchTextBox_CP + '%') 
--	or (a.strClass like '%'+ @searchTextBox_CP + '%') 
--	or (@searchTextBox_CP = 'init')
--	) 
--order by a.id desc", sRoom , sWeek , sDay

select * from ApplyList
select * from RoomApply
select * from RoomApplySub


