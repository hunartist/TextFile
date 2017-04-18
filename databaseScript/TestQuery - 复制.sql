select distinct a.id,a.strRoom,a.intDay,a.intStartNum,a.intEndNum,a.intStartWeek,a.intEndWeek,
RTRIM(a.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher,a.yearID 
from RoomApply a ,RoomDetail d,TitleStartEnd w 
where a.strRoom = d.strRoomName and a.yearID = w.yearID and w.currentFlag = 'true' 
and a.intStartWeek <= 1 and a.intEndWeek >= 1 and d.strDepart = '多媒体'
 order by a.id desc


