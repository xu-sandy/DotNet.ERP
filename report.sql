EXEC dbo.StockQuery @startDate = '2016-05-01', -- varchar(20)
    @endDate = '2016-06-06', -- varchar(20)
    @storeId = '', -- varchar(100)
    @companyId=1,
    @supplierId = '' -- varchar(100)

EXEC dbo.Sys_UserList @Key = N'', -- nvarchar(100)
    @Status = 1, -- smallint
    @OrganizationId = 0, -- int
    @DepartmentId = 0, -- int
    @RroleGroupsId = '', -- varchar(2000)
    @CurrentPage = 1, -- int
    @PageSize = 20 -- int
    
EXEC dbo.Rpt_BeforeProductSaleDetails @startDate = '2016-07-28', -- varchar(20)
    @endDate = '2016-07-30', -- varchar(20)
    @storeIds = '18', -- varchar(100)
    @companyId=1,
    @cashiers = '', -- varchar(2000)
    @salers = '', -- varchar(100)
    @apicodes = '', -- varchar(500)
    @title = '', -- varchar(100)
    @type = '', -- varchar(100)
    @CurrentPage = 1, -- int
    @PageSize = 20, -- int
    @ispage = 0 -- smallint

EXEC dbo.Rpt_CashierSaleOrderDay @startDate = '2016-06-18', -- varchar(20)
	@endDate = '2016-06-19', -- varchar(20)
    @companyid = 1, -- varchar(20)
    @storeId = '', -- varchar(100)
    @chshier = '', -- varchar(500)
    @saler = '', -- varchar(500)
    @sortField = 'date', -- varchar(20)
    @datelen='7',
    @CurrentPage = 1, -- int
    @PageSize = 20, -- int
    @ispage = 1 -- smallint
   
EXEC dbo.Rpt_StoreSaleOrderDay @startDate = '2016-06-01', -- varchar(20)
    @endDate = '2016-07-01', -- varchar(20)
    @companyid = 1, -- varchar(20)
    @storeId = '', -- varchar(100)
    @datelen='7',
    @sortField = 'date', -- varchar(20)
    @CurrentPage = 1, -- int
    @PageSize = 20, -- int
    @ispage = 1 -- smallint

EXEC dbo.Rpt_StoreStockDetail @startDate = '2016-06-28', -- varchar(20)
    @endDate = '2016-06-29', -- varchar(20)
    @companyid = 1, -- varchar(20)
    @storeId = '', -- varchar(100)
    @categorySN = '', -- varchar(20)
    @supplierId = '', -- varchar(20)
    @title = '', -- varchar(20)
    @CurrentPage = 1, -- int
    @PageSize = 50, -- int
    @ispage = 1-- smallint

EXEC dbo.Rpt_IndexSalesData @startDate = '2016/07/23', -- varchar(10)
    @endDate = '2016/07/30', -- varchar(10)
    @type = '1', -- char(1)
    @storeId = '' -- varchar(20)
    
EXEC dbo.StockQuery 
	@companyId=1,
	@startDate = '2016-05-01', -- varchar(20)
    @endDate = '2016-06-25', -- varchar(20)
    @storeId = '', -- varchar(100)
    @categorySn = '', -- varchar(300)
    @supplierid='',
    @title='',
    @CurrentPage =1, -- int
    @PageSize = 20, -- int
	@ispage = 0

EXEC dbo.Rpt_InvoicingSummary @startDate = '2016-05-01', -- varchar(20)
    @endDate = '2016-06-01', -- varchar(20)
    @companyId=1,
    @storeId = '', -- varchar(100)
    @categorySn = '', -- varchar(300)
    @title = '', -- varchar(50)
    @CurrentPage = 1, -- int
    @PageSize = 20, -- int
    @ispage = 1
    
EXEC Rpt_ProductSaleDetails @startDate = '2016-07-28', -- varchar(20)
	@companyId=1,
    @endDate = '2016-07-30', -- varchar(20)
    @storeId = '18', -- varchar(20)
    @bigCategorySN = '', -- varchar(300)
    @sortField = '' -- varchar(20)
    
EXEC Rpt_ProductSaleDetailDays @startDate = '2016-06-18', -- varchar(20)
	@companyId=1,
    @endDate = '2016-06-19', -- varchar(20)
    @storeId = '', -- varchar(20)
    @bigCategorySN = '' -- varchar(300)
    
EXEC dbo.GetCurrentPrice @companyId = 1, -- int
    @barcode = '10089', -- varchar(30)
    @type = 1 -- smallint

EXEC dbo.Rpt_IndexSalesData @startDate = '2016/06/01', -- varchar(10)
    @endDate = '2016/06/16', -- varchar(10)
    @type = '1', -- char(1)
    @storeId = '' -- varchar(20)
    
EXEC OutInNumDetails @TakingTime='2016-05-01 17:38:01',@companyId=1,@barcodes='0213030035,0500214300001,10089,0500214300002,10139,10188'

EXEC dbo.Sys_HomeMenusByUID @UID = 'e10b0206a05c4fab97e526db06e72aee' -- varchar(50)
EXEC dbo.Sys_AllLimitList @companyId = 1,@roleIds='4' -- int

SELECT * FROM  SysLimits l
WHERE EXISTS(SELECT 1 FROM dbo.SplitString((SELECT LimitsIds FROM dbo.SysRoles WHERE CompanyId=1 AND RoleId='7'),',',1) WHERE Value=l.LimitId)

SELECT * FROM  SysLimits l where l.LimitId=103

SELECT DISTINCT t.LimitId FROM(
SELECT LimitId FROM dbo.SysLimits WHERE LimitId IN(
SELECT DISTINCT l.PLimitId FROM  SysLimits l 
where EXISTS(SELECT 1 FROM dbo.SplitString((SELECT LimitsIds+',' FROM dbo.SysRoles WHERE CompanyId=1 AND dbo.Comm_F_NumIsInGroup(RoleId,'7')=1 FOR XML PATH('')),',',1) WHERE Value=CAST(l.LimitId AS VARCHAR(4)))
)
UNION all
SELECT CAST(Value AS INT) FROM dbo.SplitString((SELECT LimitsIds+',' FROM dbo.SysRoles WHERE CompanyId=1 AND dbo.Comm_F_NumIsInGroup(RoleId,'7')=1 FOR XML PATH('')),',',1)
) t 

AND Title='查看角色'

SELECT l.PLimitId FROM  SysLimits l 
where EXISTS(SELECT 1 FROM dbo.SplitString((SELECT LimitsIds+',' FROM dbo.SysRoles WHERE CompanyId=1 AND dbo.Comm_F_NumIsInGroup(RoleId,'7')=1 FOR XML PATH('')),',',1) WHERE Value=CAST(l.LimitId AS VARCHAR(4)))
AND Title='查看角色'

SELECT * FROM dbo.SysLimits WHERE PLimitId IN(
SELECT LimitId FROM dbo.SysLimits WHERE Title='订单管理')

SELECT * FROM dbo.SysLimits WHERE LimitId IN(
	SELECT DISTINCT l.PLimitId FROM  SysLimits l 
)
EXEC dbo.SQLCompareDB @dbname1 = 'Pharos_Dev', -- varchar(250)
    @dbname2 = 'Dongben_beta', -- varchar(250)
    @isGenerateSql = 1, -- bit
    @ruleOut = '' -- varchar(500)
