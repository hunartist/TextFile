declare @searchTextBox_CP nvarchar(50),@strCdep  nvarchar(50)
set @searchTextBox_CP = 'init'
set @strCdep ='0s'




--SELECT distinct a.id, a.applyid, a.strRoom, a.intDay, a.intStartNum, a.intEndNum, RTRIM(a.strClass) as strClass, RTRIM(a.strTeacher) as strTeacher,
--a.strWeekReg, a.strWeekData FROM RoomApply a, RoomApplySub s, RoomDetail d, TitleStartEnd w, ApplyList l 
--WHERE applyid = 't1' and a.id = s.F_id and a.strRoom = d.strRoomName and a.applyid = l.applyid and l.yearID = w.yearID and w.currentFlag = 'true' and a.strRoom in ('501')


select * from RoomApply
select * from RoomDetail