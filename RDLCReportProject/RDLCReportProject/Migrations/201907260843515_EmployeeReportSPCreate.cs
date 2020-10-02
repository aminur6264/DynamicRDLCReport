namespace RDLCReportProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class EmployeeReportSPCreate : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE PROC [dbo].[EmployeeReport]
                    @Columns VARCHAR(MAX),
                    @gender VARCHAR(MAX),
                    @religion VARCHAR(MAX),
                    @designations VARCHAR(MAX),
                    @bloodGroup VARCHAR(MAX)
                    AS
                    BEGIN
	                    DECLARE @condition VARCHAR(MAX)
	                    SET @condition =''

	                    IF @gender <> ''
	                    BEGIN
		                    IF @condition <> ''
		                    BEGIN
			                    SET @condition = @condition + ' AND '
		                    END
		                    SET @condition = @condition + ' Gender = '''+@gender+''' ' 
	                    END

	                    IF @religion <> ''
	                    BEGIN
		                    IF @condition <> ''
		                    BEGIN
			                    SET @condition = @condition + ' AND '
		                    END
		                    SET @condition = @condition + ' Religion = '''+@religion+''' ' 
	                    END
	                    
	                    IF @designations <> ''
	                    BEGIN
		                    IF @condition <> ''
		                    BEGIN
			                    SET @condition = @condition + ' AND '
		                    END
		                    SET @condition = @condition + ' Designation = '''+@designations+''' ' 
	                    END

	                    IF @bloodGroup <> ''
	                    BEGIN
		                    IF @condition <> ''
		                    BEGIN
			                    SET @condition = @condition + ' AND '
		                    END
		                    SET @condition = @condition + ' BloodGroup = '''+@bloodGroup+''' ' 
	                    END

	                    IF @condition <> ''
	                    BEGIN
		                    SET @condition = ' WHERE ' + @condition
	                    END

	                    PRINT @condition



	                     IF OBJECT_ID('tempDb..##Temp') IS NOT NULL
	                     DROP TABLE ##Temp;

	                      IF OBJECT_ID('tempDb..##TempData') IS NOT NULL
	                     DROP TABLE ##TempData;
	                     exec ('SELECT Id, '+@Columns +' INTO ##TempData FROM Employees '+@condition+'')
	                     

	                     DECLARE 
		                    @ObjectName VARCHAR(100) = '##TempData',
		                    @KeyColumn VARCHAR(100) = 'Id'

	                    DECLARE 
		                    @ColumnNames NVARCHAR(MAX) = '',
		                    @Values NVARCHAR(MAX) = '',
		                    @SQL NVARCHAR(MAX) = ''


	                    SELECT 
		                    @ColumnNames +=','+QUOTENAME(name),
		                    @Values += ','+ QUOTENAME(name)+' = CONVERT(VARCHAR(100),'+QUOTENAME(name)+')'
		                    FROM tempdb.sys.columns WHERE [object_id] = OBJECT_ID('tempdb..##TempData') AND name <> @KeyColumn;

	                    --PRINT @ColumnNames

	                    SET @SQL = 'SELECT * INTO ##Temp
		                    FROM
			                    (
				                    SELECT '+@KeyColumn + @Values +' FROM '+ @ObjectName +'
			                    ) AS DRV

			                    UNPIVOT
			                    (
				                    Value FOR ColumnName IN ('+STUFF(@ColumnNames,1,1,'')+')
			                    ) AS UnPVT; '

	                    EXEC sp_executesql @SQL

	                    --exec ('SELECT * FROM ##Temp ORDER BY ColumnName asc')
	                    SELECT * FROM ##Temp 
	                    ORDER BY case when ColumnName = 'FirstName' then 1
                                  when ColumnName = 'LastName' then 2
                                  when ColumnName = 'Gender' then 3
                                  when ColumnName = 'BloodGroup' then 4
                                  else 5
                             end 
	                    
                    END");
        }

        public override void Down()
        {
            Sql("DROP PROC EmployeeReport");
        }
    }
}
