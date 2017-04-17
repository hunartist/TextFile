select distinct a.id,a.strRoom,a.intDay,a.intStartNum,a.intEndNum,a.intStartWeek,a.intEndWeek,RTRIM(a.strName) as strName,
RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher,a.yearID 
from RoomApply a ,RoomDetail d,TitleStartEnd w 
where a.strRoom = d.strRoomName  and a.yearID = w.yearID and w.currentFlag = 'true' 
and d.strCDep = '1o' --and ((a.strRoom = '200') or (@roomN_CP = 'all')) 
order by a.id desc


