--declare @strWeekData nvarchar(500),@countWeek nvarchar(20)
--set @strWeekData = '1,2,3,5,6,9,10,15'
--set @countWeek = ''

--declare wid CURSOR for SELECT weekid FROM [dbo].[ufn_SplitStringToTable](@strWeekData,N',')
--OPEN wid
--FETCH NEXT from wid into @countWeek
--while @@FETCH_STATUS = 0
--begin
--	print @countWeek
--	FETCH NEXT from wid into @countWeek
--end
--close wid
--deallocate  wid

--delete from RoomApplySub

select s.intWeek,a.* from RoomApply a,RoomApplySub s where a.id = s.F_id and a.id IN ('201704241229282879','201704241228435053')
--delete from RoomApplySub where F_id = '201704241147200023'
--delete from RoomApply where id = '201704241147200023'

 select * from (select distinct s.intWeek,s.F_id,a.intStartNum,a.intEndNum,a.yearID from RoomApply a,RoomApplySub s  where a.id = s.f_id and a.strRoom = '录播室       ' and  a.intDay = 1 and a.id != '201704241148147904' ) as aa inner join TitleStartEnd t on aa.yearID = t.yearID and t.currentFlag = 'true' 
