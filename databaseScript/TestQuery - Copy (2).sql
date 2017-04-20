select a.intOddEvenFlag,s.intWeek,a.* from RoomApply a,RoomApplySub s where a.id = s.F_id and a.id = '201704201117550297'

--declare @count int,@intStartWeek int ,@intEndWeek int

--set @intStartWeek = 1
--set @intEndWeek = 9
--set @count = @intStartWeek

----if (@intOddEvenFlag = 1)
--while @count <= @intEndWeek
--begin
--	if @count%2=0
--	begin
--		print @count		
--	end
--	set @count = @count + 1
--end
