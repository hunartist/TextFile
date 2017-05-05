declare @searchTextBox_CP nvarchar(50),@strCdep  nvarchar(50)
set @searchTextBox_CP = 'init'
set @strCdep ='0s'

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
SELECT l.applyid,RTRIM(l.strName) as strName,l.yearID,l.strCDep,l.strRemark FROM ApplyList l WHERE l.strCdep = @strCdep 
--SELECT [id], [applyid], [strRoom], [intDay], [intStartNum], [intEndNum], RTRIM([strClass]) as strClass, RTRIM([strTeacher]) as strTeacher, [strWeekReg], [strWeekData] FROM [RoomApply] WHERE ([applyid] = @applyid)

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

SELECT distinct l.applyid,RTRIM(l.strName) as strName,l.yearID,l.strCDep,l.strRemark 
FROM ApplyList l,RoomApply a,RoomApplySub s,RoomDetail d,TitleStartEnd w 
WHERE l.strCdep = @strCdep and l.applyid = a.applyid and a.id = s.F_id and a.strRoom = d.strRoomName and a.yearID = w.yearID 
and w.currentFlag = 'true' and a.strRoom in ({0}),sRoom

SELECT distinct l.applyid,RTRIM(l.strName) as strName,l.yearID,l.strCDep,l.strRemark 
FROM ApplyList l, RoomApply a, RoomApplySub s, RoomDetail d, TitleStartEnd w 
WHERE l.strCdep = '0s' and l.applyid = a.applyid and a.id = s.F_id and a.strRoom = d.strRoomName 
and l.yearID = w.yearID and w.currentFlag = 'true' and a.strRoom in ('507')


