declare @searchTextBox_CP nvarchar(50),@strCdep  nvarchar(50)
set @searchTextBox_CP = 'init'
set @strCdep ='0s'

SELECT distinct l.applyid,RTRIM(l.strName) as strName,l.yearID,l.cdepid,l.strRemark,a.strClass,a.strTeacher
FROM ApplyList l, RoomApply a, RoomApplySub s, RoomDetail d, TitleStartEnd w 
WHERE l.cdepid = 0 and l.applyid = a.applyid and a.id = s.F_id and a.roomid = d.roomid and l.yearID = w.yearID and w.currentFlag = 'true' 
and a.roomid in (1501,1505,1506,1507,1508,1802,1804,1809,3001,3002,3003,3004,3005,3006,3007,3008,3009,3010,3011,3012,3013,3014,3015,3016,3017,3018,3019,3020,3021,3022,3023,3024,3025,3026,3027,3028,3029,3030,3031) 
and s.intWeek in (1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17) and a.intDay in (1,2,3,4,5,6,7) 
and (((l.strName like '%成本会计%') or (a.strTeacher like '%成本会计%') or (a.strClass like '%成本会计%') or ('成本会计' = 'init')) 
or ((l.strName like '%游凤%') or (a.strTeacher like '%游凤%') or (a.strClass like '%游凤%') or ('游凤' = 'init')) 
or ((l.strName like '%敖艳%') or (a.strTeacher like '%敖艳%') or (a.strClass like '%敖艳%') or ('敖艳' = 'init'))) order by l.applyid desc




--select aaa.*,t.currentFlag,de.strDepart from 
--	(
--	select  aa.*,d.strRoomName,d.depid from 
--		(
--		select distinct s.intWeek, a.roomid,a.intDay,a.intStartNum,a.intEndNum,l.yearID 
--		from RoomApply a 
--		inner join ApplyList l on a.applyid = l.applyid
--		inner join RoomApplySub s on a.id = s.F_id
--		) as aa 
--	inner join RoomDetail d on aa.roomid = d.roomid 	
--	where aa.intWeek = 1 and d.depid = 1
--	) as aaa 
--inner join TitleStartEnd t on aaa.yearID = t.yearID and t.currentFlag = 'true'
--inner join Department de on aaa.depid = de.depid

--SELECT distinct l.applyid,RTRIM(l.strName) as strName,l.yearID,l.cdepid,l.strRemark 
--FROM ApplyList l, RoomApply a, RoomApplySub s, RoomDetail d, TitleStartEnd w 
--WHERE l.cdepid = 1 and l.applyid = a.applyid and a.id = s.F_id and a.roomid = d.roomid and l.yearID = w.yearID and w.currentFlag = 'true' 
--and a.roomid in (2001,2000) and s.intWeek in (1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17) and a.intDay in (1,2,3,4,5,6,7) 
--and ((l.strName like '%init%') or (a.strTeacher like '%init%') or (a.strClass like '%init%') or ('init' = 'init')) order by l.applyid desc