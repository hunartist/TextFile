﻿



--''301(120)'',''302(108)'',''304(120)'',''306(120)'',''402(108)'',''403(60)'',''404(60)'',''405(108)'',''406(60)'',''408(60)'',''410(60)'',''412(108)'',
--''510(108)'',''512(108)'',''601(74)'',''602(108)'',''603(74)'',''604(60)'',''605(118)'',''606(60)'',''607(74)'',''608(60)'',''609(74)'',''610(60)'',
--''611(74)'',''612(60)'',''613(74)'',''614(60)'',''616(60)'',''报告厅'',''录播室''
DECLARE	@return_value Int
EXEC	@return_value = [dbo].[import]
		@strRoom_iGroup = '''301(120)'',''302(108)'',''304(120)'',''306(120)'',''412(108)'',''510(108)'',''512(108)'',''602(108)'',''603(74)'',''604(60)'',''605(118)'',''607(74)'',''609(74)'',''610(60)'',''611(74)'',''612(60)'',''613(74)'',''614(60)'',''616(60)''',
		@intDay_i = 1,
		@intStartNum_i = 9,
		@intEndNum_i = 10,
		@intStartWeek_i = 5,
		@intEndWeek_i = 17,
		@strName_i = N'初始数据',
		@strClass_i = N'初始数据',
		@strTeacher_i = N'初始数据',
		@strYearID_i = N'1617_2'
		SELECT	@return_value as 'Return Value'

DECLARE	@return_value Int
EXEC	@return_value = [dbo].[import]
		@strRoom_iGroup = '''301(120)'',''302(108)'',''304(120)'',''306(120)'',''402(108)'',''602(108)'',''603(74)'',''605(118)'',''607(74)'',''608(60)'',''609(74)'',''610(60)'',''611(74)'',''612(60)'',''613(74)'',''614(60)'',''616(60)''',
		@intDay_i = 2,
		@intStartNum_i = 9,
		@intEndNum_i = 10,
		@intStartWeek_i = 5,
		@intEndWeek_i = 17,
		@strName_i = N'初始数据',
		@strClass_i = N'初始数据',
		@strTeacher_i = N'初始数据',
		@strYearID_i = N'1617_2'
		SELECT	@return_value as 'Return Value'

DECLARE	@return_value Int
EXEC	@return_value = [dbo].[import]
		@strRoom_iGroup = '''301(120)'',''302(108)'',''304(120)'',''306(120)'',''402(108)'',''403(60)'',''405(108)'',''408(60)'',''410(60)'',''510(108)'',''512(108)'',''602(108)'',''603(74)'',''604(60)'',''605(118)'',''607(74)'',''608(60)'',''609(74)'',''610(60)'',''611(74)'',''612(60)'',''613(74)'',''614(60)'',''616(60)''',
		@intDay_i = 3,
		@intStartNum_i = 9,
		@intEndNum_i = 10,
		@intStartWeek_i = 5,
		@intEndWeek_i = 17,
		@strName_i = N'初始数据',
		@strClass_i = N'初始数据',
		@strTeacher_i = N'初始数据',
		@strYearID_i = N'1617_2'
		SELECT	@return_value as 'Return Value'

DECLARE	@return_value Int
EXEC	@return_value = [dbo].[import]
		@strRoom_iGroup = '''301(120)'',''302(108)'',''304(120)'',''306(120)'',''402(108)'',''404(60)'',''510(108)'',''512(108)'',''602(108)'',''603(74)'',''604(60)'',''605(118)'',''607(74)'',''608(60)'',''609(74)'',''610(60)'',''611(74)'',''612(60)'',''613(74)'',''614(60)'',''616(60)''',
		@intDay_i = 4,
		@intStartNum_i = 9,
		@intEndNum_i = 10,
		@intStartWeek_i = 5,
		@intEndWeek_i = 17,
		@strName_i = N'初始数据',
		@strClass_i = N'初始数据',
		@strTeacher_i = N'初始数据',
		@strYearID_i = N'1617_2'
		SELECT	@return_value as 'Return Value'


DECLARE	@return_value Int
EXEC	@return_value = [dbo].[import]
		@strRoom_iGroup = '''301(120)'',''302(108)'',''304(120)'',''306(120)'',''602(108)'',''603(74)'',''604(60)'',''605(118)'',''607(74)'',''609(74)'',''610(60)'',''611(74)'',''612(60)'',''613(74)'',''614(60)'',''616(60)''',
		@intDay_i = 5,
		@intStartNum_i = 9,
		@intEndNum_i = 10,
		@intStartWeek_i = 5,
		@intEndWeek_i = 17,
		@strName_i = N'初始数据',
		@strClass_i = N'初始数据',
		@strTeacher_i = N'初始数据',
		@strYearID_i = N'1617_2'


SELECT	@return_value as 'Return Value'

GO
