--USE [test]
--GO

/****** 对象: Table Valued Function [dbo].[ufn_SplitStringToTable] 脚本日期: 2017/4/21 8:44:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER FUNCTION [dbo].[ufn_SplitStringToTable]
(
  @str VARCHAR(MAX) ,
  @split VARCHAR(10)
)
RETURNS TABLE
    AS 
RETURN
    ( SELECT    B.weekid
      FROM      ( SELECT    [value] = CONVERT(XML , '<v>' + REPLACE(@str , @split , '</v><v>')
                            + '</v>')
                ) A
      OUTER APPLY ( SELECT  weekid = N.v.value('.' , 'varchar(100)')
                    FROM    A.[value].nodes('/v') N ( v )
                  ) B
    )
