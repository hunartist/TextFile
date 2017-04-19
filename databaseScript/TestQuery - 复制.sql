declare @searchTextBox_CP nvarchar(50),@depN_CP  nvarchar(50)
set @searchTextBox_CP = 'init'
set @depN_CP ='多媒体'



select distinct a.id,a.strRoom,a.intDay,a.intStartNum,a.intEndNum,a.intStartWeek,a.intEndWeek,
RTRIM(a.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher,a.yearID 
from RoomApply a ,RoomDetail d,TitleStartEnd w ,RoomApplySub s
where a.strRoom = d.strRoomName and a.yearID = w.yearID and w.currentFlag = 'true' and a.id = s.F_id
--and s.intWeek in (1)
and d.strDepart = '多媒体' and a.strRoom in ('402(108)')
and ((a.strName like '%999%') or (a.strTeacher like '%999%') or (a.strClass like '%999%') or (@searchTextBox_CP = 'init')) 
 order by a.id desc


