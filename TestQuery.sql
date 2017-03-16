select * from RoomApply
select * from RoomApplySub

--select distinct a.strRoom,a.intDay,a.intPeriodNum,a.strName,a.strClass,a.strTeacher,a.intStartWeek,a.intEndWeek,a.strApplication from RoomApply a inner join RoomApplySub s on a.Id=s.F_id

select a.strRoom,a.intDay,a.intStartNum,a.intEndNum,a.strName,a.strClass,a.strTeacher from RoomApply a inner join RoomApplySub s on a.Id=s.F_id 
