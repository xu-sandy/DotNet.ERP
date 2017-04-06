SELECT * FROM  dbo.SysUserInfo;
SELECT * FROM dbo.IndentOrder;
SELECT * FROM dbo.Supplier;
SELECT * FROM dbo.SysDataDictionary;
SELECT * FROM dbo.ProductRecord a LEFT JOIN dbo.ProductCategory b
ON a.BigCategorySN=b.CategorySN AND a.SubCategorySN=b.CategorySN
SELECT * FROM dbo.Notice
UPDATE SysUserInfo SET UId='dd'

SELECT * FROM dbo.ProductRecord WHERE Barcode='20150601175001_540'
SELECT * FROM dbo.Vw_Product WHERE SupplierId='7dd2985a85e04fc4b2fea76c59cd562b'

SELECT * FROM dbo.ProductGroup
SELECT SUM(d.AcceptNum) FROM dbo.IndentOrder c
inner JOIN dbo.IndentOrderList d ON d.IndentOrderId=c.Id
WHERE c.SupplierID='7dd2985a85e04fc4b2fea76c59cd562b' and d.State=6 AND d.Barcode='20150601175001_540'

SELECT * FROM dbo.Vw_Order
EXEC dbo.Sys_GenerateNewProductCode @CompanyId = 1, -- int
    @CategorySN = 226, -- int
    @ValuationType = 1 -- smallint
SELECT * FROM dbo.Warehouse

SELECT * FROM dbo.Contract
UPDATE IndentOrder SET State=-1
SELECT * FROM dbo.IndentOrderList;
UPDATE IndentOrderList SET IndentOrderId='20150602150221398' WHERE IndentOrderId IS null
SELECT * FROM dbo.Vw_Product
SELECT * FROM dbo.ProductRecord
SELECT * FROM dbo.SysUserInfo
EXEC Sys_HomeMenusByUID ''
SELECT * FROM dbo.Attachment
SELECT * FROM dbo.Vw_Product WHERE SubCategorySN=1
SELECT SubCategoryTitle,SubCategorySN,BigCategorySN,BigCategoryTitle,SUM(AcceptNums),BrandTitle,BrandSN FROM dbo.Vw_Product
--WHERE SubCategorySN=2
GROUP BY SubCategorySN,SubCategoryTitle,BigCategorySN,BigCategoryTitle,BrandTitle,BrandSN
HAVING SubCategorySN=2
exec Sys_GenerateNewProductCode 1,30,1
SELECT * FROM dbo.SysDataDictionary
SELECT * FROM dbo.Contract
SELECT *,(SELECT count(*) FROM dbo.ProductRecord WHERE BigCategorySN=a.CategorySN OR SubCategorySN=a.CategorySN) nums FROM dbo.ProductCategory a

SELECT *,(SELECT count(*) FROM dbo.ProductRecord WHERE BrandSN=ProductBrand.BrandSN) num FROM dbo.ProductBrand 

update	dbo.ProductCategory SET CategorySN=6 WHERE Id=6
DELETE FROM dbo.ProductRecord WHERE id=6
SELECT * FROM dbo.ProductCategory

SELECT a.*,dbo.F_StoreNameById(a.StoreId) StoreTitle,dbo.F_UserNameById(a.OrdererUID) OrderTitle,
dbo.F_SupplierNameById(a.SupplierID) SupplierTitle FROM dbo.IndentOrder a

SELECT SUM(IndentNum) IndentNums,SUM(DeliveryNum) DeliveryNums,SUM(AcceptNum) AcceptNums,IndentOrderId FROM dbo.IndentOrderList GROUP BY IndentOrderId
SELECT * FROM dbo.Attachment
SELECT * FROM dbo.ContractBoth;
SELECT * FROM dbo.Contract WHERE SupplierId='7dd2985a85e04fc4b2fea76c59cd562b';
SELECT * FROM dbo.Supplier
SELECT * FROM dbo.Receipts
SELECT * FROM dbo.Attachment;
SELECT * FROM dbo.SysLog ORDER BY id desc
SELECT * FROM dbo.Supplier
SELECT * FROM dbo.SysDataDictionary WHERE DicPSN=11
SELECT * FROM dbo.CommodityPromotion a WHERE a.PromotionType=1
SELECT * FROM dbo.ProductCategory
--订单
SELECT * FROM IndentOrder a
INNER JOIN dbo.IndentOrderList b ON b.IndentOrderId=a.IndentOrderId
WHERE a.IndentOrderId='20150804143928759'

--单品
SELECT * FROM dbo.CommodityPromotion a
INNER join dbo.CommodityDiscount b ON b.CommodityId=a.Id
WHERE a.id='6b4d4e4819c549af9c643793e98d526c'
UPDATE CommodityPromotion SET State=1 WHERE Id='6b4d4e4819c549af9c643793e98d526c'
--捆绑
SELECT * FROM dbo.CommodityPromotion a
INNER join dbo.Bundling b ON b.CommodityId=a.Id
INNER JOIN dbo.BundlingList c ON c.CommodityId=a.Id
WHERE a.Id='aea6820a30124f8a80f81c811c1b02f3'
SELECT * FROM dbo.ProductRecord WHERE Barcodes LIKE '%4444444444%'
--组合与满元
SELECT * FROM dbo.CommodityPromotion a
left JOIN dbo.PromotionBlend b ON b.CommodityId=a.Id
LEFT JOIN dbo.PromotionBlendList c ON c.CommodityId=a.Id ORDER BY a.CreateDT desc
WHERE b.CommodityId='b4d6ec0fa5844177a3d2f192b29fe738'
SELECT * FROM dbo.CommodityPromotion a  ORDER BY a.CreateDT desc
--买赠
SELECT * FROM dbo.CommodityPromotion a
INNER join dbo.FreeGiftPurchase b ON b.CommodityId=a.Id
LEFT JOIN dbo.FreeGiftPurchaseList c ON c.GiftId=b.GiftId
WHERE b.CommodityId='bf95692466324f8b9d59ccd8a3ccc298'

SELECT * FROM dbo.FreeGiftPurchase WHERE CommodityId='3bc88e1a392d4741878e9b843955129e'
SELECT * FROM dbo.FreeGiftPurchaseList WHERE GiftId='08279b7e2fdd45cd9aa96dec9dc89936'
UPDATE FreeGiftPurchase SET GiftType=1 WHERE id=8
--返利
SELECT * FROM dbo.PrivilegeSolution;
SELECT * FROM dbo.PrivilegeRegion WHERE PrivilegeSolutionId=3;
SELECT * FROM dbo.PrivilegeProduct WHERE PrivilegeSolutionId=2;
SELECT * FROM dbo.PrivilegeRegionVal WHERE PrivilegeProductId IN(1,3,23,24)
SELECT * FROM dbo.PrivilegeCalc
SELECT * FROM dbo.PrivilegeCalcDetail 
SELECT * FROM dbo.PrivilegeCalcResult
SELECT * FROM dbo.PrivilegeCalc a
INNER join dbo.PrivilegeCalcDetail b ON b.PrivilegeCalcId=a.Id
INNER join dbo.PrivilegeCalcResult c ON c.PrivilegeCalcId=a.Id
WHERE a.id=6
SELECT * FROM PrivilegeCalcDetail WHERE PrivilegeCalcId=10;
SELECT * FROM PrivilegeCalcResult WHERE PrivilegeCalcId=10;
delete dbo.PrivilegeCalc
delete dbo.PrivilegeCalcDetail
delete dbo.PrivilegeCalcResult

SELECT * FROM dbo.PrivilegeSolution a
INNER join dbo.PrivilegeRegion b ON b.PrivilegeSolutionId=a.Id
INNER join dbo.PrivilegeProduct c ON c.PrivilegeSolutionId=a.Id
INNER JOIN dbo.PrivilegeRegionVal d ON d.PrivilegeProductId=c.Id AND d.PrivilegeRegionId=b.Id
WHERE a.Id=2
SELECT * FROM syslog ORDER BY id DESC
--加密
select substring(sys.fn_sqlvarbasetostr(HashBytes('MD5','admin666888')),3,32)

--入库
SELECT * FROM dbo.InboundGoods a
INNER JOIN dbo.InboundList b ON b.InboundId=a.Id
SELECT * FROM dbo.IndentOrder
SELECT * FROM dbo.Supplier WHERE Id='7dd2985a85e04fc4b2fea76c59cd562b'

--邮件
SELECT * FROM sysmailsender
SELECT * FROM sysmailreceive
SELECT * FROM dbo.Attachment WHERE SourceClassify=3
SELECT * FROM dbo.SysUserInfo
UPDATE sysmailsender SET sendercode=10002 WHERE sendercode=0
delete sysmailsender
delete sysmailreceive
delete dbo.Attachment WHERE SourceClassify=3
--盘点
SELECT * FROM dbo.TreasuryLocks
SELECT * FROM dbo.StockTaking
SELECT * FROM dbo.Commodity

--收货明细
SELECT * FROM dbo.OrderDistribution WHERE id='83817626685f4afa9fe67a720fc8fccd'
SELECT * FROM dbo.Commodity WHERE DistributionBatch='2222'
SELECT * FROM dbo.OrderReturns
SELECT * FROM dbo.SysDataDictionary WHERE DicPSN=9
UPDATE OrderDistribution SET State=0 WHERE id='dd52bf3c59254039a7c37d068ddb1437'
DELETE OrderReturns WHERE Id=5
--退换申请
SELECT * FROM dbo.SalesReturns
SELECT * FROM dbo.SalesReturnsDetailed
SELECT * FROM dbo.SaleOrders
SELECT * FROM dbo.SaleDetail
SELECT * FROM ConsumptionPayment
SELECT * FROM dbo.ApiLibrary

SELECT * FROM dbo.ProductRecord
SELECT * FROM ProductCategory WHERE CategoryPSN=14
DELETE FROM dbo.ProductCategory WHERE categorypsn=9
SELECT * FROM dbo.Supplier
SELECT * FROM dbo.SysUserInfo
SELECT * FROM dbo.Vw_Order
SELECT * FROM dbo.IndentOrderList WHERE IndentOrderId='20150611134102581'
SELECT * FROM dbo.SysLog ORDER BY id desc
SELECT * FROM dbo.SysDataDictionary WHERE DicPSN=1
SELECT * FROM dbo.SaleOrders
SELECT * FROM dbo.SaleDetail
SELECT * FROM dbo.ConsumptionPayment
SELECT * FROM dbo.Vw_Product
SELECT * FROM dbo.Commodity a
INNER JOIN dbo.Vw_Product b ON b.Barcode=a.Barcode
LEFT OUTER JOIN (
SELECT c.* from dbo.CommodityDiscount c 
INNER join CommodityPromotion d on d.Id=c.CommodityId AND d.State=1
) c ON c.Barcode=a.Barcode

SELECT * FROM CommodityPromotion
SELECT * FROM dbo.SysUserInfo
SELECT * FROM dbo.Warehouse
--支付库
SELECT * FROM dbo.ApiLibrary
--售后退换
SELECT * FROM dbo.SalesReturns
--销售明细
SELECT * FROM dbo.SaleDetail WHERE PaySN='20150624'
--更新编码
DECLARE @maxcode varchar(30);
SELECT @maxcode=CAST(MAX(ProductCode) AS INT)+1 FROM dbo.ProductRecord
UPDATE dbo.ProductRecord SET ProductCode=REPLICATE('0',6-LEN(@maxcode))+CAST(@maxcode AS VARCHAR) WHERE id=4542
                
SELECT ProductCode + ' ' + BrandTitle + ' '+ Title FROM dbo.Vw_Product v WHERE v.Barcode='0001010112002'
SELECT * FROM dbo.Vw_Product v
    WHERE not EXISTS(
        SELECT 1 FROM dbo.CommodityPromotion a 
	INNER JOIN dbo.CommodityDiscount b ON a.Id=b.CommodityId
	WHERE
	((StartDate>= '2015-06-08' AND StartDate<='2015-06-12')
	or (EndDate>= '2015-06-08' AND EndDate<='2015-06-12' )
	OR (StartDate<= '2015-06-08' AND EndDate>='2015-06-12'))
	AND ','+StoreId+',' like '%,'+',1,2'+',%'
	AND a.CustomerObj=1 
	AND b.Barcode=v.Barcode
	)

--查询数据库中所有的表名及行数
SELECT * FROM sysobjects
SELECT a.name, b.rows
FROM sysobjects AS a INNER JOIN sysindexes AS b ON a.id = b.id
WHERE (a.type = 'u') AND (b.indid IN (0, 1))
ORDER BY b.rows DESC,a.name
        
     
SELECT * FROM dbo.IndentOrder WHERE IndentOrderId='20150731172251989'
UPDATE dbo.IndentOrder SET State=-1  WHERE IndentOrderId='20150731172251989'

;
SELECT * FROM dbo.Commodity a
INNER JOIN dbo.Warehouse b ON a.storeid=b.storeid
WHERE a.Barcode='0001010112003'

SELECT * FROM dbo.OrderDistribution WHERE IndentOrderId='20150804154604457'
SELECT * FROM dbo.IndentOrder WHERE IndentOrderId='20150804154604457';
SELECT * FROM dbo.IndentOrderList WHERE IndentOrderId='20150804154604457';

UPDATE dbo.OrderDistribution SET State=4 WHERE id='4412c06c7970419b93a5252800e853fd'
SELECT * FROM dbo.SysUserInfo;

SELECT * FROM dbo.Warehouse;
SELECT * FROM dbo.SysLog ORDER BY Id DESC;
SELECT (ROW_NUMBER() OVER ( ORDER BY [CreateDT] DESC)) AS RSNO,* FROM dbo.SysLog 

SELECT * FROM SysMenus
UPDATE SysMenus SET MenuId=-1 WHERE id=2

SELECT * FROM dbo.Vw_Product
SELECT * FROM dbo.SysLog ORDER BY id desc;
SELECT * FROM dbo.Supplier;
SELECT * FROM dbo.STHouseMove ;
UPDATE STHouseMove SET State=1 WHERE id='3b8a08c5557a4577b612cc20f2cc03c3'

SELECT * FROM dbo.ProductCategory WHERE CategoryPSN=0

EXEC Sys_HomeMenusByUID @UID=''


BEGIN
DELETE FROM dbo.ProductRecord;
DELETE FROM dbo.ProductCategory;
DELETE FROM dbo.Commodity;
DELETE FROM dbo.CommodityPromotion;
DELETE FROM dbo.CommodityDiscount;
DELETE FROM dbo.FreeGiftPurchase;
DELETE FROM dbo.FreeGiftPurchaseList;
DELETE FROM dbo.Bundling;
DELETE FROM dbo.BundlingList;
DELETE FROM dbo.PromotionBlend;
DELETE FROM dbo.PromotionBlendList;
DELETE FROM dbo.IndentOrder;
DELETE FROM dbo.IndentOrderList;
DELETE FROM dbo.OrderDistribution;
DELETE FROM dbo.OrderReturns;
DELETE FROM dbo.SaleDetail;
DELETE FROM dbo.SaleOrders;
DELETE FROM dbo.SalesReturnsDetailed;
DELETE FROM dbo.SalesReturns;
END
BEGIN
DELETE FROM dbo.CommodityPromotion;
DELETE FROM dbo.CommodityDiscount;
DELETE FROM dbo.FreeGiftPurchase;
DELETE FROM dbo.FreeGiftPurchaseList;
DELETE FROM dbo.Bundling;
DELETE FROM dbo.BundlingList;
DELETE FROM dbo.PromotionBlend;
DELETE FROM dbo.PromotionBlendList;
END

BEGIN
DELETE FROM dbo.IndentOrder WHERE IndentOrderId='2015082917135'
DELETE FROM dbo.IndentOrderList WHERE IndentOrderId='2015082917135'
end
SELECT * FROM dbo.ProductRecord
SELECT * FROM dbo.Vw_Product
SELECT * FROM dbo.SysUserInfo
SELECT * FROM dbo.SysDataDictionary WHERE DicSN=4
SELECT * FROM dbo.SysLog ORDER BY id desc;
SELECT * FROM dbo.Supplier;
SELECT * FROM dbo.IndentOrder;
SELECT * FROM dbo.IndentOrderList;
SELECT * FROM dbo.Receipts;
SELECT * FROM dbo.Warehouse;
SELECT * FROM dbo.ProductCategory;
SELECT * FROM dbo.Commodity;
SELECT * FROM dbo.ProductRecord a
SELECT * FROM dbo.SysDataDictionary WHERE DicPSN=9

SELECT * FROM dbo.SaleOrders a
LEFT JOIN dbo.SaleDetail b ON b.PaySN=a.PaySN
LEFT JOIN dbo.ProductRecord c ON c.Barcode=c.Barcode

SELECT * FROM ProductRecord WHERE Nature=1
SELECT * FROM SaleDetail WHERE Barcode='6902031301382'

--销售月报

EXEC dbo.Rpt_SupplierSaleDetail
    @startDate='2015-08-01',@endDate='2015-09-01',
    @storeId = '', -- varchar(20)
    @supplierId = '', -- varchar(40)
    @sortField = '' -- varchar(20)
    
EXEC dbo.Rpt_ProductSaleDetail 
	@startDate='2015-08-01',@endDate='2015-09-01',
    @storeId = '', -- varchar(20)
    @bigCategorySN = 0, -- int
    @sortField = '' -- varchar(20)
    
EXEC dbo.Rpt_InvoicingSummary @startDate = '2015-11-01', -- varchar(20)
    @endDate = '2015-12-18', -- varchar(20)
    @storeId = '', -- varchar(40)
    @categorySn = '', -- int
    @title = '', -- varchar(50)
	@ispage=0
SELECT * FROM dbo.SaleOrders
SELECT * FROM dbo.SaleDetail WHERE PaySN='20151218104117835'
SELECT * FROM dbo.Vw_Product 

EXEC  dbo.Rpt_PromotionSaleDetail @startDate='2015-08-01',@endDate='2015-09-01',@categorySn=2

SELECT * FROM dbo.CommodityPromotion WHERE CreateDT BETWEEN '2015-05-10' AND '2015-08-11'
SELECT * FROM dbo.Area WHERE AreaPID=5;

SELECT * FROM dbo.CommodityPromotion a 
INNER JOIN dbo.CommodityDiscount b ON a.id=b.CommodityId

SELECT * FROM dbo.CommodityPromotion a 
INNER JOIN dbo.FreeGiftPurchase b ON a.id=b.CommodityId

SELECT * FROM dbo.Commodity WHERE StoreId=6;
SELECT * FROM dbo.SysLog ORDER BY id desc;
SELECT * FROM dbo.Receipts;
SELECT * FROM dbo.SysUserInfo;
SELECT * FROM dbo.Notice
SELECT * FROM dbo.SysDepartments;
SELECT * FROM dbo.SysMailReceive;
SELECT * FROM dbo.SysMailSender;
SELECT * FROM dbo.ProductRecord;
SELECT * FROM dbo.SysStoreUserInfo
select Id,MenuId,PMenuId,SortOrder,Title,URL,Status from SysMenus order by SortOrder;
SELECT * FROM dbo.Warehouse;
UPDATE dbo.Commodity SET StoreId='6' WHERE StoreId='7'

SELECT * FROM dbo.PosIncomePayout

EXEC dbo.Rpt_IndexSaleDay @date='2015-08-24',@type='2'
SELECT * FROM dbo.Vw_Product WHERE ProductCode='900007'
SELECT * FROM dbo.Warehouse;
SELECT * FROM dbo.IndentOrder;
SELECT * FROM dbo.Vw_Order
SELECT * FROM dbo.SysMenus;
SELECT * FROM dbo.SysCustomMenus
SELECT * FROM dbo.TreasuryLocks;
SELECT * FROM dbo.SysUsersLimits;
SELECT * FROM dbo.SysLimits;
SELECT * FROM dbo.SysUserInfo;
SELECT * FROM dbo.SysStoreUserInfo;
SELECT * FROM dbo.SysRoles
EXEC Sys_HomeMenusByUID @UID='D039733AD0804D0AB68A80F184697D97'

SELECT STUFF('abcdef', 2, 1, '');
SELECT * FROM dbo.TreasuryLocks;
EXEC dbo.Auto_PromotionState;
EXEC dbo.Rpt_IndexSaleDay @date = '2015-08-11', -- varchar(20)
    @type = '1',@storeId='1' -- char(1)

--查看锁表
select request_session_id spid,OBJECT_NAME(resource_associated_entity_id) tableName 
from sys.dm_tran_locks where resource_type='OBJECT'
KILL 57

SELECT * FROM dbo.SaleOrders a
INNER JOIN dbo.SaleDetail b ON a.PaySN=b.PaySN
WHERE CreateDT BETWEEN '2015-08-01' AND '2015-09-01' 


SELECT a.LoginName,b.LoginPwd,b.OperateAuth FROM dbo.SysUserInfo a
INNER JOIN dbo.SysStoreUserInfo b ON a.UID=b.UID
WHERE b.Status=1
EXEC Sys_AllStoreRoles
SELECT * FROM dbo.Warehouse
SELECT * FROM dbo.Supplier
--验证促销
SELECT dbo.F_PromotionValidMsg('3','2015/08/27','4891338001922','') 
SELECT * FROM dbo.Vw_Product WHERE Barcode LIKE '%0204030004%'
SELECT * FROM dbo.CommodityPromotion ORDER BY StartDate DESC;
SELECT * FROM dbo.ProductCategory WHERE CategoryPSN=1;
SELECT * FROM dbo.SysMailReceive
SELECT * FROM dbo.Vw_Product WHERE Barcode='6900157527785';

SELECT * FROM dbo.SysLog WHERE Type=4 ORDER BY id desc
SELECT * FROM dbo.SysUserInfo WHERE UID='8c29bd4fe7254e68a51aa351bd4f1254'
SELECT * FROM dbo.PosIncomePayout;
SELECT * FROM dbo.SalesReturns;
SELECT * FROM dbo.SalesReturnsDetailed;
SELECT * FROM dbo.SaleOrders;
SELECT * FROM dbo.SaleDetail;
SELECT * FROM ConsumptionPayment;
SELECT * FROM dbo.SysStoreUserInfo

SELECT * FROM dbo.Vw_Product ORDER BY id desc
SELECT * FROM dbo.Vw_Product;

SELECT * FROM dbo.SysMenus WHERE Title='邮件管理'
SELECT [UID],FullName FROM dbo.SysUserInfo WHERE [Status]=1
UNION ALL 
SELECT [UID],FullName FROM dbo.SysUserInfo WHERE [Status]!=1

SELECT [UID],FullName FROM dbo.SysUserInfo WHERE [Status]=1 or uid='911cf383a3d44b988585c3a52450414a'

EXEC dbo.Sys_UserList @Key = N'', -- nvarchar(100)
	@status=1,
    @OrganizationId = 0, -- int
    @DepartmentId = 0, -- int
    @RroleGroupsId = '', -- varchar(2000)
    @CurrentPage = 1, -- int
    @PageSize = 20 -- int

SELECT * FROM dbo.StockTaking WHERE CheckBatch='0115083101';
SELECT * FROM dbo.StockTaking WHERE CheckBatch='0115082901';
SELECT * FROM dbo.ApiLibrary WHERE ApiType IN(
43,98,101)
SELECT * FROM dbo.SysStoreUserInfo;

SELECT * FROM  dbo.ProductRecord ORDER BY id desc
SELECT * FROM dbo.ProductMultPrice WHERE Barcode='0290006401'
SELECT * FROM dbo.ProductMultSupplier
SELECT * FROM dbo.ProductGroup;
SELECT * FROM dbo.ProductCategory;
SELECT * FROM dbo.ChangePriceLog;
SELECT * FROM dbo.SaleOrders WHERE PaySN='20150831213948500'

SELECT * FROM dbo.Vw_Product;
SELECT * FROM dbo.Warehouse;
SELECT * FROM dbo.IndentOrderList WHERE IndentOrderId='201509140019'
SELECT * FROM dbo.Supplier;
SELECT * FROM dbo.SysStoreUserInfo;
SELECT * FROM dbo.Commodity WHERE Barcode='0290006800003'
SELECT * FROM dbo.CommodityPromotion WHERE Id='935ca0a9a8fb42cea34082299ea05c65'

SELECT * FROM dbo.TreasuryLocks;
SELECT * FROM dbo.SaleDetail a
INNER JOIN SaleOrders b ON a.PaySN=b.PaySN
WHERE a.PaySN='20150831212108135'

SELECT * FROM SaleOrders WHERE PaySN='20150831212108135'
SELECT * FROM  dbo.SysMenus SET url='' WHERE url IS NULL

SELECT * FROM dbo.ProductRecord WHERE Barcode='0290007800004'

DELETE FROM dbo.ProductRecord WHERE oldBarcode='0290006200001'

SELECT * FROM dbo.ProductRecord WHERE oldBarcode='0290006200001'

SELECT * FROM dbo.ProductRecord WHERE Nature=2 AND ValuationType=1 ORDER BY Barcode
SELECT * FROM dbo.ProductGroup WHERE Barcode='0290007800005'

SELECT * FROM dbo.SalesReturns WHERE NewPaySn='20150924102142060'
SELECT * FROM dbo.SalesReturnsDetailed WHERE ReturnId='a1b86156d888403c857565ea0ffa2a6d'

SELECT * FROM dbo.SaleOrders WHERE PaySN='20150923105325739';
SELECT * FROM dbo.Supplier WHERE MasterAccount='admin'
SELECT * FROM dbo.SysUserInfo WHERE UID='e10b0206a05c4fab97e526db06e72aee';
SELECT * FROM dbo.StockTaking WHERE Barcode='4715219778638'
SELECT * FROM dbo.SysStoreUserInfo
delete FROM dbo.ProductCategory WHERE id IN(201,
202,203);
SELECT * FROM dbo.ProductCategory
SELECT * FROM dbo.SysMenus

SELECT * from ProductCategory WHERE CategoryPSN=4

SELECT ROW_NUMBER() OVER(ORDER BY id),id FROM dbo.ProductCategory WHERE id=90

SELECT * FROM dbo.ProductRecord WHERE Title LIKE '%t1%'
DELETE dbo.ProductCategory WHERE Title LIKE '%t1%'


SELECT a.name filedName,a.xtype,a.length filedLen,b.name tableName,a.isnullable FROM syscolumns a INNER JOIN sysobjects b ON a.id=b.id AND b.xtype='u'
WHERE b.name='Notice' AND a.xtype IN(167,231) AND a.length>0

SELECT * FROM dbo.ProductBrand ORDER BY BrandSN

SELECT * FROM dbo.ProductRecord WHERE Barcode='0200410801'

SELECT * FROM dbo.SysDataDictionary WHERE DicPSN=11
SELECT * FROM dbo.Supplier WHERE Title LIKE'%猫%'
SELECT * FROM dbo.ProductCategory  WHERE CategorySN=4
SELECT * FROM dbo.OrderDistribution WHERE DistributionBatch='2015100900002'
SELECT * FROM dbo.OrderDistribution WHERE IndentOrderId='201510100012'
UPDATE IndentOrderList SET State=2 WHERE IndentOrderId='201510080009'
DELETE OrderDistribution WHERE IndentOrderId='201510080008'
SELECT * FROM dbo.Commodity WHERE DistributionBatch='2015100900002' ORDER BY id DESC
SELECT * FROM dbo.IndentOrderList WHERE IndentOrderId='201510100012';
SELECT * FROM [OrderDistributionGift];
delete dbo.ProductRecord WHERE ProductCode IN('004110','004109','004108')
SELECT * FROM dbo.ChangePriceLog
SELECT * FROM dbo.Vw_Product WHERE SupplierId='b83f434fc7f84494a858737bc595d3ca'
SELECT * FROM dbo.SysLog WHERE Type=4 ORDER BY id DESC

--gift>0是赠品
SELECT *,(SELECT COUNT(*) FROM dbo.OrderDistributionGift g,OrderDistribution o WHERE g.OrderDistributionId=o.Id and o.DistributionBatch=c.DistributionBatch AND g.Barcode=c.barcode) gift
 FROM Commodity c WHERE DistributionBatch='2015101000003'
 
 SELECT * FROM OrderDistributionGift
 SELECT * FROM dbo.IndentOrder
 
SELECT a.IndentOrderId,a.CreateDT OrderDate,b.IndentNum,b.Price,b.Subtotal,d.*
from dbo.IndentOrder a
INNER JOIN dbo.IndentOrderList b ON a.IndentOrderId=b.IndentOrderId
INNER JOIN dbo.Vw_Product d ON b.Barcode=d.Barcode
 WHERE a.State=5 AND b.Nature=0 AND NOT EXISTS(SELECT 1 FROM IndentOrderList WHERE IndentOrderId=a.IndentOrderId AND Nature=1 AND ResBarcode=b.Barcode)
 
 SELECT * FROM dbo.IndentOrderList WHERE IndentOrderId IN('201509300001')
 
 SELECT * FROM CommodityPromotion WHERE id='8d88ff4a63184d72a085d3757cce3a37'
 SELECT * FROM dbo.FreeGiftPurchase WHERE CommodityId='8d88ff4a63184d72a085d3757cce3a37'
 SELECT * FROM dbo.FreeGiftPurchaseList ORDER BY id desc
 UPDATE FreeGiftPurchase SET GiftType=1 WHERE id=35
 SELECT * FROM dbo.CommodityPromotion a
INNER join dbo.FreeGiftPurchase b ON b.CommodityId=a.Id
LEFT JOIN dbo.FreeGiftPurchaseList c ON c.GiftId=b.GiftId

with my1 as(select * from ProductCategory where ProductCategory.CategorySN =5 --and CategorySN in (5)
                 union all select ProductCategory.* from my1, ProductCategory where my1.CategorySN = ProductCategory.CategoryPSN)
                select * from my1 ORDER BY my1.CategorySN
                
                SELECT * FROM dbo.ProductRecord WHERE CategorySN=4
                SELECT * FROM dbo.ProductCategory
                
                
EXEC dbo.Rpt_IndexSaleDay @date = '2015-10-13', -- varchar(20)
    @type = '3', -- char(1)
    @storeId = '' -- varchar(20)
    
    SELECT * FROM dbo.CommodityPromotion a
INNER join dbo.CommodityDiscount b ON b.CommodityId=a.Id


UPDATE CommodityPromotion SET State=1 WHERE EXISTS(SELECT 1 FROM CommodityDiscount where CommodityId=CommodityPromotion.Id)

SELECT ','+t.Barcode FROM(
SELECT DISTINCT b.Barcode FROM dbo.CommodityPromotion a
		INNER JOIN dbo.CommodityDiscount b ON b.CommodityId=a.Id
		INNER JOIN dbo.SplitString('6901049113055',',','') c ON b.Barcode=c.Value
		WHERE a.State!=2 and a.EndDate>='2015-10-15') t FOR XML PATH('')
		
		DECLARE @msg VARCHAR(200)
		SELECT @msg=(SELECT ','+tb.msg FROM(
			SELECT DISTINCT
				(SELECT ','+b.Barcode FROM dbo.BundlingList b
				INNER JOIN dbo.SplitString('6942031805343,6942031801307,6942031800034,6901049102318,6901049101304',',','') c ON b.Barcode=c.Value
				WHERE b.CommodityId=a.Id FOR XML PATH('')) msg
			 FROM dbo.CommodityPromotion a
			WHERE a.State!=2 and a.EndDate>='2015-10-15'
		) tb WHERE tb.msg IS NOT NULL FOR XML PATH(''))
		SELECT REPLACE(@msg,',,',',')
				
		SELECT dbo.F_PromotionValidMsg('4','2015-10-15','6942031805343,6942031801307,6942031800034,6901049102318,6901049101304','')
		
		SELECT * FROM dbo.CommodityPromotion a WHERE 
		SELECT b.* FROM dbo.BundlingList b
		INNER JOIN dbo.SplitString('6942031805343,6942031801307,6942031800034,6901049102318,6901049101304',',','') c ON b.Barcode=c.Value
				
	SELECT ','+c.Value FROM dbo.CommodityPromotion a
			INNER JOIN dbo.PromotionBlendList b ON b.CommodityId=a.Id
			INNER JOIN dbo.PromotionBlend d ON d.CommodityId=a.Id
			INNER JOIN dbo.SplitString('6901049101373,6901049102028,6942031800034,6901049102318,6901049101304',',','') c ON b.BarcodeOrCategorySN=c.Value
			WHERE a.State!=2 and a.EndDate>='2015-10-15' AND b.BlendType=1 AND d.RuleType=1 FOR XML PATH('')

				SELECT * FROM Attachment
				SELECT * FROM dbo.SysMailSender;
				SELECT * FROM dbo.SysMailReceive;
				
				SELECT PaySN FROM dbo.SaleOrders GROUP BY PaySN HAVING COUNT(*)>1
				SELECT * FROM dbo.SaleOrders WHERE PaySN='20151013152854351'
				SELECT * FROM dbo.Notice;
				SELECT * FROM dbo.IndentOrder;
				SELECT * FROM dbo.SysUserInfo
				
				select dbo.F_PromotionValidMsg('3','2015-10-16','4714221134111,6901049113055','')



SELECT * FROM dbo.SaleOrders


    
EXEC dbo.StockQuery @startDate = '2016-01-15', -- varchar(20)
    @endDate = '2016-01-21', -- varchar(20)
    @storeId = '', -- varchar(100)
    @supplierId = '', -- varchar(100)
    @categorySn = '', -- varchar(2000)
    @title = '', -- varchar(50)
    @CurrentPage = 1, -- int
    @PageSize = 20, -- int
    @ispage =1 -- smallint



SELECT * FROM dbo.InventoryBalance WHERE Barcode='0101010007' ORDER BY BalanceDate desc

SELECT * FROM dbo.Vw_Product a
OUTER APPLY dbo.F_GetStockRecords('2016-01-15','2016-01-20','15',a.Barcode)
LEFT JOIN ( SELECT
                                                          o.Barcode ,
                                                          SUM(o.PurchaseNumber) PurchaseNumber ,
                                                          SUM(o.PurchaseNumber
                                                          * o.ActualPrice) SaleAmount ,
                                                          SUM(o.PurchaseNumber
                                                          * o.ActualPrice)
                                                          / SUM(o.PurchaseNumber) SaleAveragePrice
                                                    FROM  dbo.SaleOrders s ,
                                                          dbo.SaleDetail o
                                                    WHERE s.PaySN = o.PaySN
                                                    GROUP BY o.Barcode
                                                  ) d ON d.Barcode = a.Barcode
WHERE a.Barcode='0101010007'
SELECT * FROM dbo.SysLog ORDER BY id desc
SELECT * FROM F_GetStockRecords('2016-01-15','2016-01-20','','6932041202119')
SELECT * FROM dbo.ProductRecord WHERE Barcode='6942031805985'
SELECT * FROM dbo.ProductCategory WHERE CategorySN=6
SELECT * FROM dbo.Inventory WHERE Barcode='6932041202119'

SELECT * FROM dbo.InventoryBalance WHERE Barcode='6932041202119' ORDER BY BalanceDate desc
SELECT * FROM dbo.InventoryRecord WHERE Barcode='6901049107016' ORDER BY CreateDT desc

EXEC dbo.Rpt_ProductOrderDetail 
	@startDate = '2015-12-01', -- varchar(20)
    @endDate = '2016-01-16', -- varchar(20)
    @storeId = '', -- varchar(20)
    @bigCategorySN = '', -- varchar(300)
    @brandSN = '', -- varchar(40)
    @supplierId = '', -- varchar(40)
    @sortField = 'MLE desc', -- varchar(20)
    @CurrentPage = 1, -- int
    @PageSize = 20, -- int
    @ispage = 1 -- smallint

EXEC dbo.Rpt_OrderSaleDetailDay @startDate = '2015-11-18', -- varchar(20)
    @endDate = '2015-11-19', -- varchar(20)
    @chshier = '', -- varchar(40)
    @saler = '', -- varchar(40)
    @storeId = '' -- varchar(20)

EXEC dbo.Rpt_MembersSaleDetailDay @startDate = '2015-11-01', -- varchar(10)
    @endDate = '2015-12-01', -- varchar(10)
    @storeId = '', -- varchar(3)
    @memberKeyword = '', -- varchar(100)
	@CurrentPage =1, -- int
    @PageSize = 20, -- int
	@ispage = 0

EXEC dbo.Auto_RefreshInventoryBalance @date = '2015-12-25' -- date
EXEC dbo.SQLCompareDB @dbname1 = 'Pharos_Dev', -- varchar(250)
    @dbname2 = 'Pharos_test' -- varchar(250)



SELECT *,(SELECT * FROM dbo.ProductRecord WHERE Barcode) FROM  dbo.SaleOrders
 WHERE  1=1
        AND CreateDT BETWEEN '2015-10-01' AND '2015-11-01'
     
order by id desc

SELECT * FROM dbo.SaleDetail s 
INNER JOIN dbo.SaleOrders o ON s.PaySN=o.PaySN
 WHERE Barcode='6901049107016'
 
SELECT * FROM dbo.CommodityDiscount WHERE Barcode='6901049107016'



	SELECT b.Barcode,b.Way FROM dbo.CommodityPromotion a
	INNER JOIN dbo.CommodityDiscount b ON b.CommodityId=a.Id
	INNER JOIN dbo.SplitString('4171234567899,4710035338004,8010107002007',',','') c ON b.Barcode=c.Value
	WHERE a.State!=2 and a.EndDate>='2015-10-20'

SELECT dbo.F_SupplierNameById(a.supplierid) supplierTitle,dbo.F_TrimStrMore(STUFF((SELECT '、'+w.Title FROM dbo.Warehouse w INNER join dbo.SplitString(a.storeId,',',1) s ON s.Value=w.StoreId FOR XML PATH('')),1,1,''),30) as storeTitle,
c.Barcode,c.Title,c.Size,c.BuyPrice,c.SysPrice,b.BuyPrice BuyPrice2,b.SysPrice SysPrice2,CAST(ROUND((CAST(c.SysPrice AS NUMERIC)-c.BuyPrice)/c.SysPrice,2),CAST(ROUND((CAST(c.SysPrice AS NUMERIC)-c.BuyPrice)/c.SysPrice*100,2) AS varchar(20))+'%' Rate,
b.GrossprofitRate+'%' Rate2,b.Memo,b.ChangePriceId,b.Id,a.State,b.State flag,a.createdt,
(CASE WHEN b.EndDate IS NULL THEN CONVERT(VARCHAR(20),b.StartDate,102)+'-不限' ELSE CONVERT(VARCHAR(20),b.StartDate,102)+'-'+CONVERT(VARCHAR(20),b.EndDate,102) end) DateSpacing
 FROM ProductChangePrice a
INNER JOIN ProductChangePriceList b ON b.ChangePriceId=a.Id
INNER JOIN dbo.ProductRecord c ON b.barcode=c.Barcode
ORDER BY ChangePriceId,a.CreateDT desc

select   round(12.555,2)
SELECT STUFF((SELECT '、'+w.Title FROM dbo.Warehouse w INNER join dbo.SplitString(a.storeId,',',1) s ON s.Value=w.StoreId FOR XML PATH('')),1,1,'') as StoreTitle,
b.SysPrice,a.AuditorDT,a.CreateDT,dbo.F_UserNameById(a.AuditorUID) FullName
 FROM ProductChangePrice a
INNER join dbo.ProductChangePriceList b ON b.ChangePriceId = a.Id
WHERE a.State=1 AND b.state=1 AND b.Barcode='6901049107122'
ORDER BY a.AuditorDT DESC,a.CreateDT desc

SELECT * FROM dbo.SysLog ORDER BY id desc;
SELECT * FROM dbo.Vw_Product
SELECT  s.* FROM dbo.SaleDetail s
INNER JOIN dbo.SaleOrders o ON s.PaySN=o.PaySN
WHERE CreateDT BETWEEN '2015-09-01' AND '2015-11-06'
AND s.Barcode='4712865440025' AND s.PaySN='20151104175213828'
--GROUP BY s.paysn HAVING COUNT(*)>1

SELECT  s.* FROM dbo.SaleDetail s
INNER JOIN dbo.SaleOrders o ON s.PaySN=o.PaySN
WHERE CreateDT BETWEEN '2015-09-01' AND '2015-11-06'
AND s.Barcode='6901073807555' AND s.PaySN='20151104175213828'
GROUP BY s.paysn HAVING COUNT(*)>1

SELECT * FROM dbo.ProductCategory WHERE CategorySN IN(47,49)

SELECT PaySN FROM dbo.SaleOrders GROUP BY PaySN HAVING COUNT(*)>1
SELECT * FROM dbo.SaleOrders WHERE PaySN='20151029145504060'
SELECT * FROM dbo.SaleDetail  WHERE Barcode='0102010071'
DELETE dbo.SaleOrders WHERE id IN(54)
DELETE dbo.SaleDetail WHERE id IN(147)

SELECT * FROM dbo.ProductChangePriceList
SELECT * FROM dbo.ProductCategory WHERE CategoryPSN=0

SELECT * FROM dbo.SysDataDictionary ORDER BY DicSN
SELECT * FROM dbo.PrivilegeSolution

INSERT into Dongben_Release.dbo.SysDataDictionary(DicSN,DicPSN,SortOrder,Title,Depth,Status)
SELECT DicSN,DicPSN,SortOrder,Title,Depth,Status FROM dbo.SysDataDictionary WHERE id IN(181,182,183)

SELECT * FROM Dongben_Release.dbo.SaleOrders ORDER BY CreateDT desc

SELECT SUM(TakeNum) FROM PrivilegeCalcDetail
SELECT * FROM PrivilegeCalcResult

SELECT * FROM dbo.PrivilegeRegion WHERE PrivilegeSolutionId=5 and 
(
(StartVal <=363 AND EndVal>363) OR (EndVal<363 AND EndVal=100)
or
(StartVal=100 and EndVal IS null)
)
ORDER BY EndVal


SELECT * FROM dbo.SysUserInfo

--连接数
SELECT * FROM
[Master].[dbo].[SYSPROCESSES] WHERE [DBID] IN ( SELECT 
   [DBID]
FROM 
   [Master].[dbo].[SYSDATABASES]
WHERE 
   NAME='Pharos_Dev'
)
EXEC sys.sp_who @loginame = 'pharos_admin' -- sysname

SELECT * FROM dbo.TreasuryLocks
SELECT * FROM  dbo.StockTaking WHERE CheckBatch='1515110502'
SELECT * FROM dbo.StockTakingLog
SELECT * FROM dbo.SaleOrders 


SELECT * FROM dbo.OrderDistribution
SELECT * FROM dbo.Vw_Order
SELECT * FROM dbo.IndentOrderList WHERE IndentOrderId='201511100001'
SELECT * FROM dbo.IndentOrder
SELECT * FROM dbo.SysLog ORDER BY id desc;
SELECT * FROM dbo.Supplier WHERE ClassifyId=52
SELECT * FROM dbo.SysDataDictionary WHERE DicPSN=1

SELECT * FROM dbo.PrivilegeSolution

select COUNT(SortOrder) from SysMenus where PMenuId = 108

SELECT * FROM dbo.Commodity WHERE Barcode LIKE '%801010%'

SELECT * FROM  dbo.ProducttradePrice
SELECT * FROM  dbo.ProducttradePriceList


SELECT * FROM dbo.ProductChangePriceList a WHERE a.StartDate >= '2015-11-05' AND a.EndDate<='2015-11-05'


SELECT * FROM dbo.SaleDetail WHERE PaySN='20151118174925529'
SELECT * FROM dbo.SaleOrders ORDER BY CreateDT DESC


SELECT * FROM dbo.Area;
SELECT * FROM dbo.Commodity WHERE Barcode='6923450656181' ORDER BY id

SELECT * FROM dbo.InboundList WHERE Barcode='6923450656181' ORDER BY id


SELECT * FROM dbo.StockTakingLog WHERE CheckBatch='0115112601'
SELECT * FROM dbo.StockTaking WHERE CheckBatch='0115112601' AND Barcode='6911575888396'
UPDATE StockTaking SET ActualNumber=NULL WHERE id=261

SELECT * FROM dbo.StockTaking WHERE CheckBatch='1515112301' AND NOT EXISTS(SELECT 1 FROM StockTakingLog WHERE CheckBatch=StockTaking.CheckBatch AND State=1 AND Barcode=StockTaking.Barcode)


SELECT a.Id,a.ActualNumber,dbo.F_StoreNameById(b.LockStoreID) StoreTitle,b.LockDate,c.Barcode,c.Title,c.BrandTitle,c.ProductCode,c.SubUnit,
dbo.F_UserNameById(a.CheckUID) FullName,dbo.F_UserNameById(a.CreateUID)CreateName FROM dbo.StockTaking a 
INNER JOIN dbo.TreasuryLocks b ON b.CheckBatch=a.CheckBatch
INNER JOIN dbo.Vw_Product c ON a.Barcode=c.Barcode

SELECT * FROM dbo.Vw_Product WHERE Barcode='0200077300003'

ALTER table dbo.Members add Insider NCHAR(1)
ALTER TABLE dbo.Members DROP COLUMN InsiderUID

SELECT * FROM dbo.ProductRecord WHERE ISNULL(OldBarcode,'')<>''
SELECT * FROM dbo.Commodity WHERE Barcode='4719858573920'
SELECT * FROM dbo.Commodity WHERE Barcode='4719852247315'
SELECT * FROM dbo.ChangePriceLog


SELECT * FROM dbo.IndentOrder a
INNER JOIN dbo.IndentOrderList b ON a.IndentOrderId=b.IndentOrderId
INNER JOIN dbo.Vw_Product c ON b.Barcode=c.Barcode
 WHERE a.state=5
 
 SELECT * FROM dbo.Members
 
 SELECT * FROM dbo.ProductMultSupplier
 ALTER TABLE dbo.ProductMultPrice ADD SupplierId VARCHAR(40)
 ALTER TABLE dbo.ProductMultPrice ADD BuyPrice MONEY
  ALTER TABLE dbo.ProductRecord ADD StockRate MONEY
  ALTER TABLE dbo.ProductRecord ADD SaleRate MONEY
 EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'进项税率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductRecord', @level2type=N'COLUMN',@level2name=N'StockRate'
 EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'销售税率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductRecord', @level2type=N'COLUMN',@level2name=N'SaleRate'
   ALTER TABLE dbo.OrderDistribution ADD AssistBarcode VARCHAR(30)
   ALTER TABLE dbo.StockTaking ADD CorrectNumber MONEY
 EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'库存纠正' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StockTaking', @level2type=N'COLUMN',@level2name=N'CorrectNumber'
 EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'副条码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderDistributionGift', @level2type=N'COLUMN',@level2name=N'AssistBarcode'
 
 ALTER TABLE dbo.IndentOrderList ADD SysPrice MONEY
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统售价' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IndentOrderList', @level2type=N'COLUMN',@level2name=N'SysPrice'

 SELECT * FROM  dbo.MemberIntegralSet
  SELECT * FROM  dbo.MemberIntegralSetList
  SELECT * FROM dbo.ProductCategory
  SELECT * FROM dbo.Commodity WHERE Barcode='6923450656181'
  SELECT * FROM dbo.PrivilegeProduct
  
  
  select a.id,a.BarcodeOrCategorySN+'~0' StrId,0 BrandSN,
                    a.BarcodeOrCategorySN CategorySN,
                    dbo.F_ProductCategoryDescForSN(a.BarcodeOrCategorySN,1,1) BigCategoryTitle,
                    dbo.F_ProductCategoryDescForSN(a.BarcodeOrCategorySN,2,1) MidCategoryTitle ,
                    dbo.F_ProductCategoryDescForSN(a.BarcodeOrCategorySN,3,1) SubCategoryTitle
                FROM dbo.MemberIntegralSetList a
                WHERE a.SetType=2 AND a.IntegralId=1
                
EXEC dbo.Rpt_MembersSaleDetailDay @startDate = '2015-12-04', -- varchar(10)
    @endDate = '2015-12-05', -- varchar(10)
    @storeId = '', -- varchar(3)
    @memberKeyword = '' -- varchar(100)

SELECT * FROM dbo.ProductMultPrice WHERE StoreId='8';
SELECT * FROM dbo.ProductMultSupplier;
SELECT * FROM ProductSplitLog
SELECT * FROM dbo.ChangePriceLog WHERE Barcode='0200411102' ORDER BY id DESC;
DELETE FROM dbo.ChangePriceLog WHERE id IN(4228,4231)
SELECT * FROM dbo.SysLog ORDER BY id desc;
DELETE dbo.ProductRecord WHERE Barcode='0500085714107'
SELECT * FROM dbo.ProductRecord WHERE oldBarcode='9556196008524'
SELECT * FROM dbo.Vw_Product WHERE Barcode IN('0500085714108') ORDER BY id DESC
SELECT Barcode FROM dbo.ProductRecord GROUP BY Barcode HAVING count(*)>1
SELECT * FROM ChangePriceLog WHERE Barcode='0500085714108'
SELECT * FROM dbo.SysUserInfo
SELECT * FROM dbo.SysRoles;
SELECT * FROM dbo.PayNotifyResult
SELECT * FROM ProductMultSupplier
SELECT * FROM dbo.IndentOrderList WHERE IndentOrderId='201512090011'
SELECT * FROM dbo.TreasuryLocks WHERE 1
SELECT * FROM dbo.StockTaking WHERE CheckBatch='12'
SELECT * FROM dbo.Supplier WHERE Id='b83f434fc7f84494a858737bc595d3ca'
SELECT * FROM dbo.SaleOrders
SELECT * FROM dbo.IndentOrder
SELECT * FROM STHouseMove;
UPDATE dbo.STHouseMove SET State=2 WHERE id='6a8f1051d9da433790a72b9686195803'
DELETE InventoryRecord
DELETE Inventory
SELECT * FROM dbo.InventoryRecord WHERE Barcode IN('6923006800242','')
SELECT * FROM dbo.Inventory WHERE StoreId='16' and Barcode IN('6923006800242','')
SELECT * FROM dbo.ProductRecord WHERE Barcode IN('6901049107018','6901049101304') or Barcodes LIKE '%6901049107018%'
SELECT * FROM dbo.SaleOrders

UPDATE dbo.InboundGoods SET State=0 WHERE InboundGoodsId='20151224192022128'
DELETE Inventory WHERE id=75

SELECT * FROM dbo.OutboundList WHERE OutboundId='20151225104953494'
SELECT * FROM dbo.OutboundGoods WHERE OutboundId='20151225104953494'
DELETE dbo.OutboundGoods WHERE OutboundId='20151225104551918'
DELETE dbo.OutboundList WHERE OutboundId='20151225104551918'

UPDATE OutboundList SET AssistBarcode='6901049107018',Barcode='0101041057' WHERE id=29

SELECT * FROM dbo.IndentOrderList WHERE IndentOrderId='201512250028'
SELECT * FROM OrderDistribution a
INNER JOIN dbo.ProductRecord b ON a.Barcode=b.Barcode OR ','+a.Barcode+',' LIKE ','+b.Barcodes+','
 WHERE IndentOrderId='201512250028' 
 
 SELECT * FROM dbo.ProductRecord WHERE Barcode='0101041056'
 SELECT * FROM dbo.CommodityReturns WHERE ReturnId='2015120004'
 UPDATE CommodityReturns SET State=1 WHERE id=15
 
 
SELECT * FROM dbo.inboundList WHERE InboundGoodsId='20151225152216017'
SELECT * FROM dbo.inboundGoods WHERE InboundGoodsId='20151225152216017'

    @ispage = 1-- smallint

SELECT * FROM dbo.ProductChangePriceList ORDER BY id desc
SELECT * FROM dbo.ProductRecord WHERE Nature=2 AND Barcode='0500420400001'
SELECT * FROM dbo.InventoryBalance WHERE Barcode='6932041202126'
SELECT * FROM dbo.SysUserInfo
SELECT * FROM dbo.Vw_Order
SELECT * FROM (
SELECT id, Theme AS Title,NoticeContent Content,url,reader,CONVERT(VARCHAR(30),CreateDT,120) CreateDT,CASE WHEN ','+reader+',' LIKE '%,1002,%' THEN 1 ELSE 0 END rd  FROM dbo.Notice 
) t ORDER BY t.rd,t.CreateDT desc

SELECT * FROM dbo.IndentOrder
SELECT * FROM dbo.SysLog ORDER BY Id desc
SELECT SUM(b.IndentNum) 采购入库数,SUM(Subtotal) 采购入库金额,Barcode  FROM dbo.IndentOrder a, dbo.IndentOrderList b
	WHERE a.IndentOrderId=b.IndentOrderId and Nature=0 AND a.State=5
	--AND (@startDate='' OR @endDate='' OR (CreateDT BETWEEN @startDate AND @endDate))
	--AND (@storeId='' OR EXISTS(SELECT * FROM dbo.SplitString(@storeId,',',1) WHERE Value=a.StoreId))
	GROUP BY Barcode
	
	SELECT * FROM dbo.InventoryBalance
	SELECT * FROM dbo.OrderDistributionGift
	
	SELECT * FROM dbo.Vw_Product WHERE CategorySN IN(56,57)
	SELECT * FROM dbo.ProductCategory WHERE CategorySN IN(56,57)
	
	SELECT SUM(a.ReceivedNum) 赠送入库数量,SUM(a.ReceivedNum*c.Price) 赠送入库金额,a.Barcode  FROM dbo.OrderDistributionGift a 
	INNER JOIN dbo.OrderDistribution d ON d.Id=a.OrderDistributionId
	INNER join dbo.IndentOrder b ON b.IndentOrderId=d.IndentOrderId
	INNER join IndentOrderList c ON b.IndentOrderId=c.IndentOrderId AND a.Barcode=c.Barcode AND c.Nature=1 AND d.Barcode=c.ResBarcode
	--WHERE d.State=5
	--AND (@startDate='' OR @endDate='' OR (d.ReceivedDT BETWEEN @startDate AND @endDate))
	--AND (@storeId='' OR EXISTS(SELECT * FROM dbo.SplitString(@storeId,',',1) WHERE Value=b.StoreId))
	GROUP BY a.Barcode
	
	SELECT * FROM Vw_StockTaking WHERE CheckBatch='1515122901' and Barcode='6901049107122'
	
	SELECT * FROM dbo.InventoryBalance WHERE Barcode='6932041202119'
	SELECT * FROM dbo.Inventory WHERE Barcode='0500266500002'
	
	
	SELECT * FROM dbo.SaleInventoryHistory a,dbo.SaleDetail b WHERE a.PaySN=b.PaySN AND a.SaleBarcode=b.Barcode AND a.Mode=3
		--and (@startDate='' OR @endDate='' OR EXISTS(SELECT 1 FROM dbo.SaleOrders WHERE PaySN=b.PaySN and CreateDT BETWEEN @startDate AND @endDate))
		--AND (@storeId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@storeId,',',1) WHERE Value=a.StoreId))
		GROUP BY b.Barcode
		
		SELECT * FROM dbo.Inventory WHERE Barcode='6932041202119'
		SELECT * FROM dbo.Vw_Product WHERE Barcode='6955193403661'
		
		SELECT * FROM dbo.SaleDetail WHERE Barcode='6920609801034'
		SELECT * FROM dbo.SysUserInfo ORDER BY id DESC
		SELECT * FROM dbo.Vw_StockTaking WHERE CheckBatch='1515122901' AND Barcode='6932041202126'
		
		SELECT * FROM OrderDistribution
		SELECT * FROM dbo.OrderDistributionGift
		select * from Vw_StockTaking WHERE State=0
		SELECT * FROM dbo.StockTakingLog WHERE CheckBatch='1615123003'
		SELECT * FROM dbo.IndentOrderList WHERE Barcode='6932041202119' IndentOrderId='201512310001'
		
		SELECT * FROM dbo.CommodityReturns a
		INNER JOIN dbo.CommodityReturnsDetail b ON a.ReturnId=b.ReturnId
		
		SELECT * FROM dbo.OrderDistribution a 
	INNER join dbo.IndentOrder b ON b.IndentOrderId=a.IndentOrderId
	INNER join IndentOrderList c ON a.IndentOrderId=c.IndentOrderId AND a.Barcode=c.Barcode AND c.Nature=0
	WHERE a.State=5
	
	SELECT * FROM dbo.SaleInventoryHistory WHERE SaleBarcode='0200265700001'
	SELECT b.Barcode,SUM(a.Number) 组合数量,sum(a.Number*b.ActualPrice) 组合金额 FROM dbo.SaleInventoryHistory a,dbo.SaleDetail b 
	WHERE a.PaySN=b.PaySN AND a.SaleBarcode=b.Barcode AND a.Mode=3 AND b.barcode='0200265700001'
	GROUP BY b.Barcode
	
	SELECT b.Barcode,a.Number 组合数量,a.Number*b.ActualPrice 组合金额,* FROM dbo.SaleInventoryHistory a,dbo.SaleDetail b 
	WHERE a.PaySN=b.PaySN AND a.SaleBarcode=b.Barcode AND a.Mode=3 AND b.barcode='0200265700001'
	
	SELECT * FROM SaleDetail WHERE Barcode='0200265700001'
	SELECT * FROM dbo.SaleInventoryHistory WHERE SaleBarcode='0200265700001'  PaySn='20151231153457459'
	SELECT * FROM dbo.HouseMove a,dbo.HouseMoveList b WHERE a.MoveId=b.MoveId and a.State=4 
	
	SELECT * FROM dbo.InventoryBalance WHERE Barcode='6901073807555'
	SELECT * FROM dbo.InventoryRecord WHERE Barcode='6901073807555'
	SELECT * FROM dbo.Inventory WHERE Barcode='6932041208692'
	SELECT * FROM dbo.Warehouse
	
	SELECT DENSE_RANK() OVER(ORDER BY a.Id) RSNO,dbo.F_SupplierNameById(a.supplierid) SupplierTitle,dbo.F_TrimStrMore(STUFF((SELECT '、'+w.Title FROM dbo.Warehouse w INNER join dbo.SplitString(a.storeId,',',1) s ON s.Value=w.StoreId FOR XML PATH('')),1,1,''),60) as StoreTitle,
                c.Barcode,c.Title,c.Size,b.CurBuyPrice ,b.CurSysPrice , b.OldSysPrice ,b.OldBuyPrice ,b.OldGrossprofitRate,b.CurGrossprofitRate,b.Memo,b.ChangePriceId,b.Id,b.State Flag,a.State,a.CreateDT,
                (CASE WHEN b.EndDate IS NULL THEN CONVERT(VARCHAR(20),b.StartDate,102)+'-不限' ELSE CONVERT(VARCHAR(20),b.StartDate,102)+'至'+CONVERT(VARCHAR(20),b.EndDate,102) end) DateSpacing
                 FROM ProductChangePrice a
                INNER JOIN ProductChangePriceList b ON b.ChangePriceId=a.Id
                INNER JOIN dbo.ProductRecord c ON b.barcode=c.Barcode where 1=1
                
SELECT DENSE_RANK() OVER(ORDER BY t.NewBarcode2) row,* FROM (           
SELECT a.CreateDT,a.StartDate,a.EndDate,a.CustomerObj,a.State,b.CommodityId,b.NewBarcode,b.BundledPrice,b.TotalBundled,c.Id,c.Number,b.NewBarcode NewBarcode2,a.StoreId,
d.Title,d.BrandSN,d.BrandTitle,d.SysPrice,d.ProductCode,d.Barcode,CONVERT(VARCHAR(20),a.StartDate,23)+'至'+CONVERT(VARCHAR(20),a.EndDate,23) [BetWeen],
CASE a.CustomerObj WHEN 1 THEN '内部' WHEN 2 THEN 'VIP' ELSE '不限' END Customer,CASE a.State WHEN 1 THEN '活动中' WHEN 2 THEN '已过期' ELSE '未开始' END StateTitle
FROM dbo.CommodityPromotion a
INNER JOIN dbo.Bundling b ON b.CommodityId=a.Id
INNER JOIN dbo.BundlingList c ON c.CommodityId=a.Id
INNER JOIN dbo.Vw_Product d ON c.Barcode=d.Barcode OR ','+d.Barcodes+',' LIKE '%,'+c.Barcode+',%' WHERE 1=1
) t

SELECT * FROM dbo.InventoryRecord WHERE Barcode='6920609801034' ORDER BY id desc
SELECT SUM(StockNumber) FROM Commodity WHERE Barcode='6950310300489' AND StockNumber>0 ORDER BY id DESC
SELECT SUM(InboundNumber) FROM dbo.InboundList WHERE Barcode='6950310300489'
SELECT * FROM dbo.InboundGoods

SELECT * FROM dbo.OutboundGoods a
INNER JOIN dbo.OutboundList b ON b.OutboundId=a.OutboundId
UPDATE OutboundGoods SET State=0

EXEC dbo.Sys_GenerateNewProductCode @CategorySN = 71 -- int
SELECT * FROM dbo.ProductRecord WHERE Barcode='0114010002'
SELECT * FROM dbo.ProductCategory WHERE CategorySN=71

SELECT OperatId FROM dbo.InventoryRecord WHERE Barcode='6950310300489' GROUP BY OperatId HAVING(COUNT(*)>1)
SELECT * FROM dbo.InventoryRecord WHERE OperatId='20151130173002385'
SELECT * FROM dbo.SysLog ORDER BY Id DESC
SELECT * FROM dbo.ProductRecord WHERE Nature=2 ORDER BY CreateDT
SELECT * FROM dbo.Inventory WHERE StoreId='-1'
DELETE dbo.Inventory WHERE Id=429
SELECT * FROM dbo.Inventory WHERE Barcode='6901049108129'
SELECT * FROM dbo.InventoryBalance WHERE Barcode='6901049108129' ORDER BY Barcode, BalanceDate
SELECT * FROM dbo.InventoryRecord WHERE Barcode='6901049108129' ORDER BY CreateDT
EXEC dbo.Auto_RefreshInventoryBalance @date = '2016-01-01 05:08:57' -- date

SELECT StoreId,Barcode FROM dbo.Inventory GROUP BY StoreId,Barcode HAVING count(*)>1
SELECT COUNT(*) FROM dbo.Inventory
SELECT * FROM dbo.Inventory WHERE Barcode IN('0200265700001','0305020001','6901049107122','6901073807555')
DELETE dbo.Inventory WHERE id IN(266,278,284,322)
DELETE dbo.ProductRecord WHERE Barcode='0000001'
SELECT * FROM dbo.ProductGroup WHERE Barcode='0000001'
SELECT * FROM dbo.SaleDetail WHERE Barcode='0115010192'
SELECT * FROM dbo.SalesReturns
SELECT * FROM dbo.SalesReturnsDetailed WHERE Barcode='0115010192'
SELECT dbo.F_GetStockRecord('','','6901049107122')
EXEC dbo.Auto_RefreshInventoryBalance @date = '2016-01-15 03:37:02' -- date

 SELECT * FROM  master..spt_values where type='p' 
 
 SELECT CONVERT(CHAR(10), DATEADD(DAY, number, GETDATE()), 120) AS [日期],number
FROM MASTER..spt_values WHERE TYPE='P' AND number>0

SELECT * FROM dbo.InventoryBalance WHERE Barcode='6932041202119'
AND BalanceDate BETWEEN '2016-01-01' AND '2016-01-20'
 ORDER BY BalanceDate DESC,StoreId
 
 SELECT dbo.F_GetStockRecords('','0101010007','2016-01-01','2016-01-20')
 
 SELECT * FROM dbo.StockTaking WHERE CheckBatch='1616011502' AND Barcode='0213060007'
  
 SELECT * FROM dbo.StockTakinglog WHERE CheckBatch='1616011502' AND Barcode='0213060007'
 
 SELECT  b.Barcode ,
                SUM(b.PurchaseNumber) giftNumber ,
                SUM(b.PurchaseNumber * b.SysPrice) giftTotal
        FROM    dbo.SaleOrders a
                INNER JOIN dbo.SaleDetail b ON b.PaySN = a.PaySN
        WHERE   b.ActualPrice = 0
        GROUP BY b.Barcode
        
        SELECT Barcode FROM dbo.ProductRecord GROUP BY Barcode HAVING COUNT(*)>1
        SELECT * FROM dbo.ProductRecord WHERE Barcode='0110010003'
        DELETE dbo.ProductRecord WHERE id=4665
        SELECT * FROM dbo.SaleDetail WHERE Barcode='0110010003'
        SELECT * FROM Vw_StockTaking WHERE CheckBatch='1516010701'
        SELECT * FROM Vw_OrderList ORDER BY CreateDT DESC
        SELECT * FROM dbo.InventoryBalance WHERE Barcode='0101010007' ORDER BY BalanceDate desc
        SELECT * FROM dbo.PayNotifyResult
        SELECT * FROM dbo.IndentOrderList WHERE Barcode='0112070002'
        SELECT * FROM dbo.Vw_OrderList WHERE Barcode='0112070002'
        SELECT * FROM dbo.InboundList WHERE Barcode='0103080013' AND IsGift=1
        SELECT * FROM dbo.InboundGoods
        SELECT * FROM OrderDistributionGift WHERE Barcode='0112070002'
        SELECT * FROM dbo.Reader
       
SELECT * FROM (
                    SELECT id,Theme AS Title,NoticeContent Content,url,CONVERT(VARCHAR(30),CreateDT,120) CreateDT,CASE WHEN EXISTS(SELECT 1 FROM dbo.Reader WHERE Type=1 AND MainId=a.id AND ReadCode='1004') THEN 1 ELSE 0 END Flag FROM dbo.Notice a
                    
                ) t where 1=1  ORDER BY t.Flag,t.CreateDT DESC
                
                
                SELECT * FROM dbo.Vw_OrderList WHERE Barcode='0103080013'
                SELECT * FROM dbo.IndentOrderList WHERE Barcode='0103080013'
                SELECT * FROM dbo.Vw_Order WHERE SupplierID='b83f434fc7f84494a858737bc595d3ca' AND State=1
                SELECT * FROM dbo.Supplier WHERE MasterAccount='laozuofang'
                SELECT * FROM dbo.ProductRecord WHERE dbo.F_GetIsRelationship(ProductRecord.Barcode)=1
                SELECT * FROM dbo.SaleDetail WHERE Barcode='0101060004'
                SELECT * FROM dbo.SaleOrders WHERE PaySN IN('20160122103923860','20160122103809626')
                SELECT * FROM dbo.SalesReturns
                SELECT * FROM dbo.SalesReturnsDetailed WHERE Barcode='0101060004'
                SELECT * FROM dbo.Inventory WHERE Barcode='0102010063'
                
                SELECT * FROM dbo.SalesReturns a
                INNER join dbo.SalesReturnsDetailed b ON a.ReturnId=b.ReturnId
                 WHERE Barcode='0101060004'
                 SELECT @@DBTS
                 
        SELECT  b.Barcode ,
                SUM(b.Number) returnNumber ,
                SUM(b.Number * b.TradingPrice) returnTotal
        FROM    dbo.SalesReturns a
                INNER JOIN dbo.SalesReturnsDetailed b ON b.ReturnId = a.ReturnId
        WHERE   a.ReturnType IN ( 0, 2 )
                --AND a.CreateDT BETWEEN @startDate AND @endDate
                AND b.Barcode='0101060004'
        GROUP BY b.Barcode
        
        SELECT * FROM dbo.SalesReturns a WHERE  EXISTS (SELECT ReturnId FROM dbo.SaleOrders WHERE ReturnId like '%,'+a.ReturnId+',%'    )
        
        
        SELECT * FROM dbo.CommodityDiscount WHERE Barcode='6901049107122'
        
        SELECT * FROM dbo.WeighingSet
        SELECT * FROM dbo.Vw_Product where Barcode='6901049107122'
        select * from Inventory where Barcode='6901049113055'
select * from InventoryRecord where Barcode='6901049113055'
select * from InventoryBalance where Barcode='6901049113055' ORDER BY BalanceDate desc
SELECT * FROM dbo.SysUserInfo WHERE UID='D039733AD0804D0AB68A80F184697D97'
SELECT * FROM dbo.Warehouse
SELECT * FROM dbo.InboundGoods WHERE InboundGoodsId='P20151231173720708'
SELECT * FROM dbo.ProductCategory WHERE CategorySN IN(14,84)
SELECT * FROM dbo.ProductCategory WHERE CategoryPSN IN(14,84)
SELECT * FROM dbo.ProductRecord WHERE CategorySN IN(98,84) AND ValuationType=2

SELECT * FROM dbo.StockTaking WHERE CheckBatch='1516011901' AND Barcode='6955193403661'
SELECT * FROM dbo.StockTakingLog WHERE CheckBatch='1616012201' AND Barcode='0101030006'

SELECT * FROM dbo.Vw_Product WHERE SupplierId='b83f434fc7f84494a858737bc595d3ca'



SELECT *,ISNULL((SELECT SUM(acceptnum) FROM dbo.Vw_OrderList WHERE ((OrderType='采购' AND State=5) OR OrderType='入库') and SupplierId=v.SupplierId AND barcode=v.barcode GROUP BY Barcode),0) AcceptNum FROM dbo.Vw_Product v
WHERE v.SupplierId='b83f434fc7f84494a858737bc595d3ca'
SELECT LoginName,Handsign,LoginPwd,Status,RoleIds FROM dbo.SysUserInfo ORDER BY CreateDT DESC
UPDATE SysUserInfo SET Handsign='1,4,5,6' WHERE id=51
SELECT * FROM( SELECT * FROM dbo.ProductCategory a PIVOT (SUM(CategorySN) FOR Title IN(特产,珠宝)) b) t WHERE t.CategoryPSN=0

SELECT * FROM dbo.Warehouse
SELECT * FROM dbo.Reader WHERE Type=2 AND MainId=76
DELETE FROM dbo.Reader WHERE MainId=183
SELECT * FROM dbo.SysLog ORDER BY id DESC
SELECT * FROM dbo.SysUserInfo
SELECT * FROM dbo.Vw_Supplier
SELECT * FROM dbo.StockTaking WHERE CheckBatch='1616012802' AND Barcode='0101010001'

SELECT * FROM dbo.SalesReturns WHERE StoreId='15'
SELECT * FROM dbo.IndentOrderList WHERE IndentOrderId='201602020060' ORDER BY IndentOrderId DESC
SELECT * FROM dbo.IndentOrder ORDER BY IndentOrderId DESC

SELECT t.StoreId, CONVERT(VARCHAR(20),t.CreateDT,23),SUM(t.num) FROM (
SELECT a.StoreId,a.PaySN,a.CreateDT,(SELECT COUNT(1) FROM dbo.SaleDetail b WHERE a.PaySN=b.PaySN AND b.ActualPrice=0) num FROM dbo.SaleOrders a WHERE a.Type=0 AND a.State=0
) t GROUP BY t.StoreId,CONVERT(VARCHAR(20),t.CreateDT,23)

SELECT * FROM dbo.Supplier WHERE MasterAccount='lvdi'
SELECT * FROM dbo.Vw_StockTaking WHERE CheckBatch='1616020301'
SELECT * FROM dbo.StockTakingLog WHERE CheckBatch='1516012101' AND Barcode='0110020003' ORDER BY CreateDT
SELECT * FROM dbo.PayNotifyResult ORDER BY CreateDT
SELECT * FROM dbo.SysPaymentSetting WHERE State=1 AND PayType=1 AND (StoreId='0' OR ','+StoreId+',' LIKE '%,15,%') ORDER BY ISNULL(AlterDT,CreateDT) desc
SELECT * FROM dbo.ProductRecord
SELECT * FROM (
    SELECT id,Theme AS Title,State,NoticeContent Content,url,CONVERT(VARCHAR(30),CreateDT,120) CreateDT,CASE WHEN EXISTS(SELECT 1 FROM dbo.Reader WHERE Type=1 AND MainId=a.id AND ReadCode=1002) THEN 1 ELSE 0 END Flag FROM dbo.Notice a
) t where 1=1 AND State=1 ORDER BY t.Flag,t.CreateDT DESC

SELECT * FROM dbo.Vw_Product
UPDATE dbo.SaleOrders SET ProductCount=0

DELETE FROM dbo.Reader WHERE Type=2 AND id>324
SELECT * FROM dbo.SysUserInfo WHERE LoginName='caisf'
select * from(select (ROW_NUMBER() OVER ( ORDER BY [Id] desc)) AS RSNO,* from (select * from OMS_CompanyAuthorize) tb) t

SELECT * FROM dbo.SysLimits WHERE LimitId IN(186,187)
SELECT * FROM dbo.OMS_CompanyAuthorize
SELECT * FROM dbo.IndentOrderList WHERE IndentOrderId='201603020135'
SELECT * FROM dbo.SaleDetail
UPDATE SaleDetail SET AveragePrice=0 WHERE Total IS NULL
SELECT * FROM syscolumns WHERE name='companyid'
SELECT * FROM master.dbo.sysobjects
SELECT * FROM dbo.MemberIntegralSet

SELECT d.name 表名,  
a.colorder 字段序号,a.name 字段名,
(case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then '√'else '' end) 标识, 
(case when (SELECT count(*) FROM sysobjects  
WHERE (name in (SELECT name FROM sysindexes  
WHERE (id = a.id) AND (indid in  
(SELECT indid FROM sysindexkeys  
WHERE (id = a.id) AND (colid in  
(SELECT colid FROM syscolumns WHERE (id = a.id) AND (name = a.name)))))))  
AND (xtype = 'PK'))>0 then '√' else '' end) 主键,b.name 类型,a.length 占用字节数,  
COLUMNPROPERTY(a.id,a.name,'PRECISION') as 长度,  
isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0) as 小数位数,(case when a.isnullable=1 then '√'else '' end) 允许空,  
isnull(e.text,'') 默认值,isnull(g.[value], ' ') AS [说明]
FROM  syscolumns a 
left join systypes b on a.xtype=b.xusertype  
inner join sysobjects d on a.id=d.id and d.xtype='U' and d.name<>'dtproperties' 
left join syscomments e on a.cdefault=e.id  
left join sys.extended_properties g on a.id=g.major_id AND a.colid=g.minor_id
left join sys.extended_properties f on d.id=f.class and f.minor_id=0
where b.name is not null
AND a.name='companyid'
order by a.id,a.colorder
SELECT * FROM dbo.Warehouse
SELECT * FROM dbo.Reader WHERE Type=2 AND MainId=248
DELETE dbo.Reader WHERE MainId IN(248) 
SELECT * FROM dbo.IndentOrder WHERE IndentOrderId='201603180201'
SELECT * FROM dbo.SysUserInfo
SELECT * FROM dbo.SaleDetail
SELECT * FROM dbo.SaleOrders
SELECT * FROM dbo.Vw_Product WHERE Barcode='041602010003'
SELECT * FROM dbo.Bundling WHERE NewBarcode='041602010003'

SELECT TOP 10 *,dbo.F_ProductNameBybarcode(Barcode) Title FROM (
                SELECT a.Barcode,SUM(a.PurchaseNumber) PurchaseNumber,SUM(a.PurchaseNumber*a.ActualPrice) ActualTotal FROM dbo.SaleDetail a 
                INNER JOIN dbo.SaleOrders b ON a.PaySN=b.PaySN where storeid='15' and createdt between '2016-02-01' and '2016-02-02'
                GROUP BY a.Barcode) t ORDER BY t.ActualTotal DESC
                


SELECT * FROM Vw_StockTaking WHERE CheckBatch='1516030301' ORDER BY LockDate
SELECT * FROM dbo.ProductBrand
DELETE ProductBrand WHERE id >=250
UPDATE dbo.SysDataDictionary SET Title='其他' WHERE id=141
SELECT * FROM dbo.SysDataDictionary WHERE DicPSN=5
DELETE dbo.SysDataDictionary WHERE id=199
SELECT * FROM dbo.ImportSet
SELECT * FROM dbo.ProductRecord ORDER BY CreateDT DESC
SELECT * FROM dbo.Vw_Product
SELECT * FROM dbo.ProductCategory
SELECT * FROM dbo.SysDataDictionary WHERE DicPSN=4
DELETE dbo.ProductRecord WHERE CreateDT>'2016-03-11'
UPDATE dbo.ProductRecord SET JoinPrice=0,TradePrice=0 WHERE JoinPrice IS NULL OR TradePrice IS NULL
SELECT * FROM dbo.SysLog ORDER BY id DESC
SELECT * FROM dbo.Bundling
SELECT * FROM dbo.Supplier  ORDER BY CreateDT DESC
SELECT * FROM dbo.ProductRecord ORDER BY id desc
SELECT * FROM dbo.SysUserInfo ORDER BY CreateDT DESC
SELECT * FROM dbo.ImportSet
SELECT * FROM dbo.PayNotifyResult
SELECT * FROM dbo.Vw_StockTaking WHERE CheckBatch='1516022502'
SELECT * FROM dbo.OMS_CompanyAuthorize
SELECT * FROM dbo.OMS_CompanyAuthorize

exec SQLCompareDB 'pharos_dev','Dongben_Test';
SELECT * FROM dbo.ProductTradePrice 

SELECT * FROM dbo.Members WHERE MobilePhone='15259166152'
UPDATE dbo.Members SET CompanyId=1 WHERE id=53
SELECT * FROM dbo.IndentOrder ORDER BY CreateDT DESC
SELECT * FROM dbo.Vw_Order WHERE IndentOrderId='201603140168'
SELECT * FROM dbo.Supplier WHERE MasterAccount='gongyingshang'
UPDATE dbo.IndentOrder SET CreateDT=DATEADD(DAY,-1,CreateDT) WHERE CreateDT>'2016-03-17'
SELECT SUM(TotalAmount) FROM dbo.SaleOrders WHERE Type=2 or State=1
SELECT * FROM dbo.SaleOrders ORDER BY CreateDT desc
SELECT * FROM dbo.SaleOrders WHERE PaySN='S20160331141127429'
SELECT * FROM dbo.SaleDetail  WHERE PaySN='S20160331104118681'
SELECT * FROM dbo.SaleDetail WHERE Barcode='6920609801898'
SELECT * FROM dbo.WipeZero WHERE PaySN='S20160331132901473'
SELECT * FROM dbo.WipeZero WHERE PaySN='S20160328141900278'
SELECT * FROM dbo.ApiLibrary
SELECT * FROM ConsumptionPayment WHERE PaySN='S20160328140856136'
SELECT * FROM dbo.SaleDetail a,SaleOrders b WHERE a.PaySN = b.PaySN and CreateDT BETWEEN '2016-03-17' AND '2016-03-18'
AND b.State=0 AND b.Type=0 
SELECT * FROM dbo.Vw_Product WHERE Barcode='0104040001'
SELECT PaySN FROM dbo.SaleOrders GROUP BY PaySN HAVING(COUNT(*))>1
UPDATE dbo.SaleOrders SET storeid='15' where PaySN='S20160328141900278'
UPDATE dbo.SaleDetail SET scanbarcode=Barcode WHERE ISNULL(scanbarcode,'')=''

SELECT * FROM dbo.ProductBrand WHERE 
DELETE dbo.ProductBrand WHERE Title='品牌4'

SELECT * FROM dbo.Vw_Supplier WHERE BusinessType=2
SELECT * FROM dbo.Supplier
UPDATE dbo.Supplier SET CompanyId=1 WHERE id IN('2014019047704fa1a5b65dd407ee4685','404c765557fc46a28662513b4a5df2d1')
EXEC dbo.Auto_PromotionState

SELECT * FROM dbo.ProductRecord WHERE Nature=2 and OldBarcode IS NULL
SELECT * FROM dbo.ProductRecord WHERE Barcode='0305040084'
SELECT * FROM dbo.ChangePriceLog WHERE Barcode='0500432900002' ORDER BY CreateDT DESC
SELECT * FROM dbo.Warehouse
SELECT * FROM dbo.Inventory WHERE Barcode='6955193407225'
SELECT * FROM dbo.InventoryBalance WHERE Barcode='0103020002'
SELECT * FROM  dbo.OutboundGoods SET State=0 WHERE OutboundId='6955193407225'
SELECT * FROM dbo.CommodityPromotion

SELECT a.StoreId, a.CreateDT,a.PaySN,a.Receive,b.Id,b.Barcode,b.Title,b.ProductCode,b.PurchaseNumber,(CASE WHEN a.State=1 THEN '退整单' WHEN a.type=1 THEN '换货' ELSE '退货' END) ReturnType,dbo.F_UserNameById(a.CreateUID) FullName FROM dbo.SaleOrders a 
INNER JOIN dbo.SaleDetail b ON a.PaySN=b.PaySN AND b.CompanyId = a.CompanyId
WHERE NOT (a.State=0 AND a.Type=0) AND a.StoreId='15'

SELECT * FROM dbo.InventoryRecord WHERE Barcode IN('0503000010','0500431200001','0500431200002')
SELECT * FROM dbo.Inventory WHERE Barcode IN('0503000010','0500431200001','0500431200002')
SELECT * FROM dbo.InventoryBalance WHERE Barcode IN('0503000010','0500431200001','0500431200002')

DELETE dbo.InventoryRecord WHERE Barcode IN('0503000010','0500431200001','0500431200002')
DELETE dbo.Inventory WHERE Barcode IN('0503000010','0500431200001','0500431200002')
DELETE dbo.InventoryBalance WHERE Barcode IN('0503000010','0500431200001','0500431200002')
UPDATE dbo.InboundGoods SET State=0 WHERE InboundGoodsId='P20160329181844510'
UPDATE dbo.OutboundGoods SET State=0 WHERE OutboundId='S20160329181922723'

SELECT * FROM dbo.OrderDistribution WHERE DistributionBatch='2016032800001' AND Barcode='6934055660121'
UPDATE dbo.OrderDistribution SET State=4 WHERE DistributionBatch='2016032800001' AND Barcode='6934055660121'
SELECT * FROM dbo.InventoryBalance 
DECLARE @barcodes VARCHAR(50)
SELECT @barcodes=Barcode FROM dbo.ProductGroup WHERE GroupBarcode='0102010074'
SELECT @barcodes
SELECT * FROM dbo.SysUserInfo

SELECT b.StoreId,b.CreateUID,b.State,b.Type,b.Receive FROM dbo.SaleOrders b
WHERE b.StoreId='10' and b.CreateDT BETWEEN '2016-04-07' AND '2016-04-08' AND b.State=0 and Receive < 0 ORDER BY b.CreateDT DESC

SELECT b.StoreId,b.CreateUID,b.State,b.Type,b.Receive FROM dbo.SaleOrders b
WHERE (b.State=0 AND b.Type=0) and b.StoreId='10' and b.CreateDT BETWEEN '2016-04-07' AND '2016-04-08'   ORDER BY b.CreateDT DESC

SELECT *,CONVERT(VARCHAR(10), CreateDT, 120) FROM dbo.SaleOrders b
WHERE b.StoreId='10' and b.CreateDT BETWEEN '2016-04-07' AND '2016-04-08' AND (State = 1 OR Type = 2) ORDER BY b.CreateDT DESC

SELECT   *
                  FROM      dbo.SaleOrders oo
                  WHERE     CreateUID = 'bbcb0f45642c4bb599c95e81b28e933f'
                            AND CONVERT(VARCHAR(10), CreateDT, 120) = '2016-04-07'
                            AND oo.CompanyId = 1
                            AND ( State = 1
                                  OR Type = 2
                                )
                                

SELECT * FROM dbo.InventoryRecord WHERE Barcode IN('0102010074','6920609801898') ORDER BY CreateDT desc
SELECT * FROM dbo.Inventory WHERE Barcode IN('0102010074','6920609801898')
SELECT * FROM dbo.InventoryBalance WHERE Barcode IN('0101020001','')
SELECT * FROM dbo.StockTaking WHERE CheckBatch='0916033001'
UPDATE dbo.TreasuryLocks SET State=0 WHERE CheckBatch='0916033001'
SELECT * FROM dbo.BreakageList
SELECT * FROM dbo.Vw_StockTaking
SELECT * FROM dbo.InboundList WHERE InboundGoodsId='P20160331133856178'
SELECT * FROM dbo.InboundGoods WHERE InboundGoodsId='P20160331133856178'
SELECT * FROM dbo.InboundList WHERE Barcode='0500062800001'
SELECT * FROM dbo.IndentOrderList
SELECT * FROM (
                    SELECT id,Theme AS Title,NoticeContent Content,State,url,CONVERT(VARCHAR(30),CreateDT,120) CreateDT,CASE WHEN EXISTS(SELECT 1 FROM dbo.Reader WHERE Type=1 AND MainId=a.id AND ReadCode='{0}') THEN 1 ELSE 0 END Flag,a.CompanyId FROM dbo.Notice a
                ) t where State=1 ORDER BY t.Flag,t.CreateDT DESC
                
                
SELECT * FROM(SELECT a.StoreId,a.Title,ISNULL(SUM(d.PurchaseNumber),0) PurchaseNumber,ISNULL(SUM(d.SysPrice*d.PurchaseNumber),0) GiftTotal FROM dbo.Warehouse a
			LEFT JOIN (SELECT c.PurchaseNumber,c.SysPrice,b.StoreId FROM dbo.SaleOrders b ,dbo.SaleDetail c WHERE b.PaySN=c.PaySN AND b.CompanyId=c.CompanyId AND b.CompanyId=1 AND c.SalesClassifyId IN(161,49)
			) AS d ON a.StoreId=d.StoreId
			GROUP BY a.StoreId,a.Title) t ORDER BY t.GiftTotal DESC
			
			UPDATE dbo.SaleOrders SET State=1,IsProcess=0,InInventory=1 WHERE PaySN='S20160331092622061'
			SELECT * FROM dbo.SaleOrders WHERE ReturnOrderUID IS NOT null
			SELECT * FROM dbo.SaleDetail WHERE PaySN='S20160415095834810'--'S20160331203812907'
			EXEC dbo.Auto_OrderToInventory 
			EXEC dbo.AffectInventory_Sale 
			    @CompanyId = 1, -- int
			    @StoreId = 5, -- int
			    @OrderState = 1, -- int
			    @OrderType = 0, -- int
			    @InInventory = 1, -- int
			    @Barcode = '2013070700440', -- varchar(30)
			    @Number = 2, -- money
			    @CreateUID = '24508a822d07422a97a639a8435f72a0', -- varchar(40)
			    @CreateDT = '2016-04-01 02:57:35' -- datetime
			
			SELECT * FROM dbo.InventoryRecord WHERE Barcode='0102010075'
			SELECT * FROM dbo.InventoryBalance WHERE Barcode='0102010074'
			SELECT * FROM dbo.SaleDetail WHERE PaySN='S20160418182600106'
			SELECT * FROM dbo.SaleOrders WHERE CreateDT BETWEEN '2016-04-18' AND '2016-04-19' AND (State=1 OR type=2)
			UPDATE dbo.SaleOrders SET Receive=-Receive  WHERE PaySN='S20160406144740637'
			SELECT * FROM dbo.SysUserInfo WHERE UID='49266f307a614e74bcc20b8b4e2c691f'
			SELECT * FROM dbo.OMS_CompanyAuthorize
			SELECT * FROM dbo.IndentOrderList WHERE Barcode='0103040001'
			SELECT * FROM dbo.IndentOrder WHERE IndentOrderId='20160411130048'
			SELECT * FROM dbo.Supplier WHERE id='582d6734794841c4ab4931dbae35a682'
			
			SELECT * FROM dbo.Attachment
			SELECT * FROM dbo.InboundList WHERE IsGift=1
			SELECT * FROM dbo.SaleDetail a
			INNER join dbo.SaleOrders b ON b.PaySN = a.PaySN
			WHERE b.CompanyId=1 AND b.IsTest=0 and CreateDT BETWEEN '2016-04-26' AND '2016-04-27' AND (State=1 OR type=2)
			SELECT SUM(Receive) FROM SaleOrders
			WHERE CompanyId=1 AND IsTest=0 and CreateDT BETWEEN '2016-04-26' AND '2016-04-27'
			
			SELECT * FROM dbo.InboundList WHERE InboundGoodsId='P20160422155627876'
			SELECT * FROM dbo.InboundGoods ORDER BY id DESC
			
			EXEC dbo.SQLCompareDB @dbname1 = 'Pharos_Dev', -- varchar(250)
			    @dbname2 = 'Dongben_beta', -- varchar(250)
			    @isGenerateSql = NULL, -- bit
			    @ruleOut = '' -- varchar(500)
			
			SELECT c.name,c.max_length,c.is_nullable,b.name [type] FROM sys.columns c
INNER join sys.sysobjects t ON c.object_id=t.id
LEFT JOIN sys.systypes b on c.system_type_id=b.xusertype 
WHERE t.xtype='U' AND t.name='saleorders'

SELECT * FROM sys.systypes
SELECT * FROM dbo.IndentOrder WHERE StoreId='0'
SELECT * FROM OrderDistribution
SELECT * FROM dbo.ProductChangePrice ORDER BY CreateDT DESC
SELECT * FROM dbo.ProductChangePriceList WHERE ChangePriceId='e7f20d561540439fb5b254b60226a76d'

SELECT * FROM ConsumptionPayment WHERE PaySN='S20160427113011918'
SELECT * FROM CommodityReturns WHERE ReturnId='R2016040001'
SELECT * FROM CommodityReturnsDetail WHERE ReturnId='R2016040001'
SELECT * FROM dbo.InboundGoods WHERE InboundGoodsId='P20160427164414068'
UPDATE InboundGoods SET SupplierID='-1',State=0 WHERE InboundGoodsId='P20160427164414068'
SELECT * FROM ProductChangePriceList WHERE ChangePriceId='f6853b53fa914fb69905bdd2637df837';
SELECT * FROM dbo.ProductChangePrice
SELECT * FROM dbo.SaleOrders WHERE CreateDT BETWEEN '2016-05-16' AND '2016-05-17'
SELECT * FROM dbo.SysUserInfo;

SELECT * FROM ProductChangePriceList a
INNER JOIN ProductChangePrice b ON a.ChangePriceId=b.Id
 WHERE a.Barcode='6914204012711';

SELECT * FROM dbo.ProductChangePrice ORDER BY CreateDT desc
UPDATE ProductChangePriceList SET state=1 WHERE Barcode='6914204012711';
EXEC dbo.Auto_ChangePriceState

SELECT * FROM dbo.InboundGoods WHERE InboundType=2
SELECT * FROM dbo.InboundList
SELECT CONVERT(VARCHAR(20),CreateDT,23),* FROM dbo.ProductRecord ORDER BY CreateDT DESC
DELETE ProductRecord WHERE CONVERT(VARCHAR(20),CreateDT,23)='2016-05-20'
SELECT * FROM syslog WHERE Summary LIKE '%组合%' ORDER BY id DESC
SELECT * FROM dbo.ProductCategory WHERE Title='其他'
SELECT * FROM dbo.ProductCategory WHERE CategorySN=3
SELECT barcode FROM dbo.ProductRecord GROUP BY Barcode HAVING COUNT(*)>1

SELECT * FROM dbo.InventoryBalance WHERE Barcode='0101020001' ORDER BY BalanceDate desc
SELECT * FROM dbo.Inventory

SELECT * FROM dbo.ProductRecord WHERE ProductCode='001372'
SELECT * FROM dbo.ProductChangePriceList WHERE Barcode='6941411202994'

ALTER TABLE dbo.ProductRecord ADD CONSTRAINT DF_ProductRecord_State DEFAULT 1 FOR State
ALTER TABLE dbo.ProductRecord DROP CONSTRAINT DF_ProductRecord_State
SELECT InventoryWarning,ValidityWarning FROM dbo.ProductRecord
ALTER TABLE dbo.ProductRecord ADD CONSTRAINT ValidityWarning_Def DEFAULT 5 FOR ValidityWarning

SELECT * FROM dbo.Vw_Product WHERE SupplierId='25273a2ef894460fa644c9deaf4f539f' AND Barcode='0111010003'
SELECT * FROM dbo.Vw_Order

SELECT CONVERT(VARCHAR(30),CAST(SUM(IndentNum) AS DECIMAL(10,3))) AS IndentNums ,
                                SUM(DeliveryNum) AS DeliveryNums ,
                                SUM(AcceptNum) AS AcceptNums 
                         FROM   dbo.IndentOrderList
                         WHERE  Nature = 0 AND IndentOrderId='20160526220006'
                         
                         
                         SELECT CONVERT(VARCHAR(30),CAST(1.523 AS DECIMAL(10,0)))
                         
                         SELECT * FROM dbo.IndentOrder WHERE IndentOrderId='20160527130009'
                         SELECT * FROM dbo.Supplier WHERE id='1899d843a210439298381e6231e9ba6e'
                         
                         
                         SELECT a.*,a.IndentNum InboundNumber,b.SubUnit,b.Title,b.ProductCode,b.CategorySN,b.Expiry,
                STUFF((SELECT '<br/>'+Barcode+' '+dbo.F_ProductNameBybarcode(Barcode,CompanyId)+' '+ dbo.F_NumberAutoStr(IndentNum)+'件' FROM IndentOrderList WHERE Nature=1 AND resbarcode=a.Barcode AND IndentOrderId=a.IndentOrderId FOR XML PATH('')),1,11,'') Detail,
                STUFF((SELECT ','+Barcode+'~'+CAST(IndentNum AS VARCHAR(20)) from IndentOrderList WHERE Nature=1 AND resbarcode=a.Barcode AND IndentOrderId=a.IndentOrderId FOR XML PATH('')),1,1,'') Gift
                FROM dbo.IndentOrderList a 
                INNER JOIN dbo.Vw_Product b ON b.Barcode=a.Barcode
                WHERE a.Nature=0 AND a.IndentOrderId='20160527130009'
                
                SELECT * FROM IndentOrderList WHERE IndentOrderId='20160527130009'
                SELECT * FROM dbo.SaleDetail WHERE PaySN='11201605310003'
                SELECT * FROM Warehouse
                
                SELECT w.StoreId,w.Title,v.CurBuyPrice,ISNULL(v.CurSysPrice,dbo.F_SysPriceByBarcode(w.StoreId,'6955193403180',w.CompanyId)) AS CurSysPrice,v.AuditorDT,v.rn FROM dbo.Warehouse w
                LEFT JOIN(
                SELECT d.Barcode,d.CurBuyPrice,d.CurSysPrice,a.AuditorDT,b.Value,ROW_NUMBER() OVER(PARTITION BY b.Value ORDER BY a.AuditorDT desc) rn FROM dbo.ProductChangePrice a
                OUTER APPLY dbo.SplitString(a.StoreId,',',1) b
                INNER JOIN ProductChangePriceList d ON d.ChangePriceId=a.Id
                WHERE a.CompanyId=1 AND d.State=1 AND a.State=1 AND d.Barcode='6955193403180'
                ) v ON v.Value=w.StoreId AND v.rn=1
                WHERE w.State=1 AND w.Type=1
                
                SELECT w.Title,dbo.F_SysPriceByBarcode(w.StoreId,'6955193409281',w.CompanyId) AS CurSysPrice,
                dbo.F_BuyPriceByBarcode('b83f434fc7f84494a858737bc595d3ca','6955193409281',w.CompanyId) as CurBuyPrice FROM Warehouse w
                
                
                
                SELECT * FROM dbo.Supplier
                SELECT * FROM dbo.Warehouse
                SELECT * FROM dbo.ProductChangePriceList WHERE ChangePriceId='094bf92aa718457cb1298958b6df74fd' and Barcode='6955193409281'
                UPDATE dbo.ProductChangePriceList SET State=1,EndDate='2016-06-02' WHERE id=184
                SELECT * FROM dbo.ProductChangePrice WHERE id IN('094bf92aa718457cb1298958b6df74fd','6d7a58f1697147edbaff4af88daecb12')
                
                SELECT * FROM dbo.ProductMultPrice
                SELECT * FROM dbo.CommodityDiscount
                SELECT * FROM dbo.CommodityPromotion WHERE PromotionType=2
                
                EXEC GetCurrentPrice @companyid=1, @barcode='0305060007',@type=2
                
                SELECT * FROM dbo.BundlingList WHERE Barcode='6955193409281'
                UPDATE dbo.ProductChangePrice SET State=1 WHERE id='6d7a58f1697147edbaff4af88daecb12'
                
                
                SELECT a.StoreId,c.Title,d.Barcode,d.CurBuyPrice,d.CurSysPrice,dbo.F_SupplierNameById(a.SupplierId) SupplierTitle,ROW_NUMBER() OVER(PARTITION BY c.StoreId ORDER BY a.AuditorDT desc) rn FROM dbo.ProductChangePrice a
				OUTER APPLY dbo.SplitString(a.StoreId,',',1) b
				INNER JOIN dbo.Warehouse c ON (b.Value=c.StoreId OR b.Value='-1') AND c.CompanyId = a.CompanyId
				INNER JOIN ProductChangePriceList d ON d.ChangePriceId=a.Id AND d.State=1
				WHERE a.State=1 AND d.Barcode='6955193409281' AND a.CompanyId=1
				
				exec GetCurrentPrice 1,'6901049107153',3
				
				SELECT * FROM dbo.ProductMultSupplier WHERE Barcode='YJSGMT008'
				UPDATE dbo.ProductMultSupplier SET CompanyId=1 WHERE Barcode='YJSGMT008'
				
				'6955193409281',
				'0111010003'
				SELECT * FROM dbo.ProductMultSupplier
				
				SELECT * FROM dbo.ProductChangePrice
				
				SELECT * FROM  dbo.SaleOrders a
				INNER JOIN dbo.SaleDetail b ON a.PaySN = b.PaySN AND b.Barcode='0112030002'
				 WHERE  b.Barcode='0112030002'
				 
				 SELECT * FROM (
				 SELECT a.StoreId,c.Title,d.Barcode,d.CurBuyPrice,d.CurSysPrice,dbo.F_SupplierNameById(a.SupplierId) SupplierTitle,b.Value,ROW_NUMBER() OVER(PARTITION BY c.StoreId ORDER BY a.AuditorDT desc) rn FROM dbo.ProductChangePrice a
				OUTER APPLY dbo.SplitString(a.StoreId,',',1) b
				INNER JOIN dbo.Warehouse c ON (b.Value=c.StoreId OR b.Value='-1') AND c.CompanyId = a.CompanyId
				INNER JOIN ProductChangePriceList d ON d.ChangePriceId=a.Id AND d.State=1
				WHERE a.State=1 AND d.Barcode='0111010003') t WHERE t.rn=1
				
				SELECT * FROM(
				SELECT '' StoreId,c.Title,d.Barcode,d.TradePrice,ROW_NUMBER() OVER(PARTITION BY c.Id ORDER BY a.AuditorDT desc) rn FROM dbo.ProductTradePrice a
				OUTER APPLY dbo.SplitString(a.Wholesaler,',',1) b
				INNER JOIN dbo.Supplier c ON (b.Value=c.Id OR b.Value='-1') AND c.CompanyId = a.CompanyId AND c.BusinessType=2
				INNER JOIN dbo.ProductTradePriceList d ON d.TradePriceId=a.Id
				WHERE a.State=1 AND d.Barcode='6901049107153') t WHERE t.rn=1
				
				SELECT a.*,b.Title FROM dbo.ConsumptionPayment a
				INNER join dbo.ApiLibrary b ON a.ApiCode=b.ApiCode AND b.CompanyId = a.CompanyId
				
				SELECT * FROM dbo.SysLog ORDER BY CreateDT DESC
				SELECT * FROM dbo.WeighingBatch
				DELETE WeighingBatch WHERE id=10
				SELECT * FROM dbo.ProductRecord
				
				SELECT * FROM dbo.ApiLibrary
				
				SELECT * FROM dbo.SysDataDictionary WHERE DicPSN=77
				SELECT * FROM dbo.SysUserInfo WHERE FullName LIKE '%余静好%'
				
				SELECT * FROM dbo.TreasuryLocks WHERE CheckBatch='1716052701'
				SELECT * FROM dbo.StockTaking  WHERE CheckBatch='1716052701'
				SELECT * FROM dbo.StockTakingLog WHERE CheckBatch='1716052701' AND state=1
				DELETE StockTakingLog WHERE CheckBatch='1716052701' AND state=1
				SELECT * FROM dbo.Vw_StockTaking WHERE CheckBatch='1716052701'
				SELECT * FROM dbo.ApiLibrary ORDER BY ApiOrder
				UPDATE dbo.ApiLibrary SET ApiOrder=5 WHERE id=17
				
				SELECT * FROM dbo.InboundGoods ORDER BY CreateDT DESC
				SELECT * FROM dbo.InboundList WHERE InboundGoodsId='P20160615100302443'
				SELECT DISTINCT CreateUID FROM dbo.SaleOrders
				SELECT * FROM BreakageList
				SELECT * FROM dbo.HouseMove
				
				
				ALTER TABLE dbo.InboundGoods ADD VerifyTime DATETIME
				ALTER TABLE dbo.OutboundGoods ADD VerifyTime DATETIME
				ALTER TABLE dbo.CommodityReturns ADD VerifyTime DATETIME
				ALTER TABLE dbo.BreakageGoods ADD VerifyTime DATETIME
				EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'确认时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'InboundGoods', @level2type=N'COLUMN',@level2name=N'VerifyTime'
				EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'确认时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OutboundGoods', @level2type=N'COLUMN',@level2name=N'VerifyTime'
				EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'已完成时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CommodityReturns', @level2type=N'COLUMN',@level2name=N'VerifyTime'
				EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'确认时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BreakageGoods', @level2type=N'COLUMN',@level2name=N'VerifyTime'
				
				
				EXEC OutInNumDetails @TakingTime='2016-06-16 15:10:24',@companyId=1,@barcodes='0213030035,0304040011,10089,6940706333894,10139,10188'
				SELECT * FROM dbo.OutboundList WHERE Barcode='0500214300001'
				SELECT * FROM dbo.OutboundGoods WHERE OutboundId='I20160616100739704'
				SELECT * FROM dbo.Vw_StockTaking WHERE CheckBatch='1816061601' AND Barcode='6940706333894'
				
				SELECT dbo.F_StockLockValidMsg('18',1)
				SELECT * FROM dbo.ProductRecord
				
				SELECT DISTINCT a.StoreId, b.Id, b.Barcode,b.CurBuyPrice BuyPrice,b.CurSysPrice Price FROM dbo.ProductChangePrice a
				INNER JOIN dbo.ProductChangePriceList b ON b.ChangePriceId=a.Id
				CROSS APPLY dbo.SplitString(a.StoreId,',',1) c
				WHERE a.State=1 AND b.State=1 AND a.CompanyId=1 AND b.Barcode='YJSGMT008'
				
				SELECT * FROM dbo.SysLog ORDER BY id DESC
				SELECT * FROM dbo.ApiLibrary
				SELECT * FROM dbo.SysDataDictionary WHERE DicPSN=9
				SELECT * FROM dbo.ConsumptionPayment WHERE PaySN='26893131530d4feeb5c8c0072de9f003'
				SELECT * FROM dbo.SaleOrders ORDER BY CreateDT desc
				SELECT * FROM dbo.SaleDetail
				SELECT * FROM dbo.WipeZero
				SELECT * FROM dbo.SysUserInfo
				SELECT * FROM dbo.ImportSet
				SELECT * FROM dbo.Warehouse
				SELECT * FROM dbo.ConsumptionPayment WHERE paysn IN(
				SELECT PaySN FROM ConsumptionPayment GROUP BY PaySN HAVING(COUNT(*)>1))
				
				DELETE dbo.WipeZero WHERE PaySN IN(
				SELECT PaySN FROM dbo.SaleOrders WHERE CreateDT BETWEEN '2016-06-18' AND '2016-06-19' AND MachineSN='00'
				)
				DELETE dbo.ConsumptionPayment WHERE PaySN IN(
				SELECT PaySN FROM dbo.SaleOrders WHERE CreateDT BETWEEN '2016-06-18' AND '2016-06-19' AND MachineSN='00'
				)
				DELETE dbo.SaleDetail WHERE PaySN IN(
				SELECT PaySN FROM dbo.SaleOrders WHERE CreateDT BETWEEN '2016-06-18' AND '2016-06-19' AND MachineSN='00'
				)
				DELETE dbo.SaleOrders WHERE CreateDT BETWEEN '2016-06-18' AND '2016-06-19' AND MachineSN='00'
				
				SELECT * FROM dbo.SaleOrders a
				INNER JOIN dbo.SaleDetail b ON b.PaySN = a.PaySN
				WHERE CreateDT BETWEEN '2016-07-12' AND '2016-07-13' --AND a.MachineSN='00'--AND a.CustomOrderSn IN('00201606180141','00201606180305','00201606180283')
				ORDER BY CustomOrderSn
				
				SELECT * FROM SaleOrders WHERE PaySN IN(
				SELECT PaySN FROM dbo.ConsumptionPayment WHERE ApiOrderSN='02201606184614')
				
				SELECT ApiCode,SUM(Amount) 实收金额,SUM(Received) 收取金额 FROM dbo.ConsumptionPayment
				WHERE PaySN IN(
				SELECT a.PaySN FROM dbo.SaleOrders a
				WHERE CreateDT BETWEEN '2016-06-18' AND '2016-06-19' AND StoreId='6' )
				GROUP BY ApiCode
				SELECT * FROM dbo.PayNotifyResult
				SELECT * FROM dbo.WipeZero
				SELECT ApiOrderSN, ApiCode,Amount 实收金额,Received 收取金额 FROM dbo.ConsumptionPayment
				WHERE PaySN IN(
				SELECT a.PaySN FROM dbo.SaleOrders a
				WHERE CreateDT BETWEEN '2016-06-18' AND '2016-06-19' AND StoreId='6' )
				AND ApiCode='20'
				
				
				SELECT a.PaySN,a.Receive,a.Type, b.AveragePrice,b.PurchaseNumber FROM dbo.SaleOrders a
				INNER JOIN dbo.SaleDetail b ON b.PaySN=a.PaySN
				WHERE  CreateDT BETWEEN '2016-07-28' AND '2016-07-30' AND StoreId='18' 
				
				  SELECT * FROM dbo.Vw_StockTaking
				SELECT a.Type, b.AveragePrice, FROM dbo.SaleOrders a
				INNER JOIN dbo.SaleDetail b ON b.PaySN=a.PaySN 
				LEFT JOIN dbo.ProductRecord c ON c.Barcode = b.Barcode AND c.CompanyId = a.CompanyId
				WHERE a.CreateDT BETWEEN '2016-07-28' AND '2016-07-30' AND StoreId='18' 
				
				SELECT b.Barcode,b.Title,b.SubUnit,SUM(CASE WHEN c.ValuationType=2 THEN 1 else a.PurchaseNumber END) FROM dbo.SaleDetail a
				INNER JOIN Vw_Product_Bundling b ON a.Barcode = b.Barcode
				LEFT JOIN dbo.ProductRecord c ON c.Barcode = b.Barcode AND c.CompanyId = a.CompanyId
				WHERE EXISTS(SELECT 1 FROM dbo.SaleOrders WHERE PaySN=a.PaySN and CreateDT BETWEEN '2016-07-28' AND '2016-07-30' AND StoreId='18' )
				GROUP BY b.Barcode,b.Title,b.SubUnit
				
				SELECT a.ProductCount,a.Receive,(SELECT SUM(AveragePrice*PurchaseNumber) FROM dbo.SaleDetail WHERE PaySN=a.PaySN) AverageTotal FROM dbo.SaleOrders a
				--INNER JOIN dbo.SaleDetail b ON b.PaySN=a.PaySN 
				WHERE CreateDT BETWEEN '2016-07-28' AND '2016-07-30' AND StoreId='18'  
				
				SELECT SUM(a.ProductCount),SUM(a.Receive) FROM dbo.SaleOrders a
				WHERE CreateDT BETWEEN '2016-07-28' AND '2016-07-30' AND StoreId='18'  
				
				SELECT *,(CASE WHEN b.ValuationType=2 THEN 1 else a.PurchaseNumber END) PurchaseNumbers FROM dbo.SaleDetail
				 
				 
				SELECT * FROM dbo.SaleOrders WHERE PaySN='3b821b5080764fa7984020d3c36520a7'
				SELECT ApiOrderSN FROM dbo.ConsumptionPayment GROUP BY ApiOrderSN HAVING COUNT(*)>1
				SELECT CAST(MAX(sort) AS INT) maxnum,t.prefix FROM(
				 SELECT SUBSTRING(CustomOrderSn,11,4) sort,SUBSTRING(CustomOrderSn,0,11) prefix FROM dbo.SaleOrders WHERE CompanyId=1 and CONVERT(VARCHAR(10),CreateDT,120) IN('2016-06-18','2016-06-16')
				) t GROUP BY prefix
				
				SELECT * FROM dbo.Vw_Product_Bundling v WHERE v.CompanyId=1 --AND(EXISTS(SELECT 1 FROM dbo.SplitString('6940706333900,6940706333887',',',1) WHERE Value=v.Barcode) OR (ISNULL(v.Barcodes,'')<>''))
				AND (','+v.Barcodes+',' LIKE '%,041605250001,%' OR v.Barcode='041605250001')
				
				SELECT * FROM dbo.SysUserInfo
				SELECT * FROM SysCustomMenus
				SELECT * FROM dbo.SysRoles
				SELECT * FROM  SysLimits WHERE PLimitId IN(3,24,25)
				SELECT * FROM dbo.SysMenus m
				WHERE m.CompanyId=1
				and (
				--dbo.Comm_F_NumIsInGroup('4','1,2')=1 OR dbo.Comm_F_NumIsInGroup('4','1,5')=1 or
				EXISTS(SELECT 1 FROM dbo.SplitString((SELECT MenuIds+',' FROM SysCustomMenus WHERE CompanyId=1 AND Type=2 AND dbo.Comm_F_NumIsInGroup(ObjId,'13')=1 FOR XML PATH('')),',',1) WHERE Value=m.MenuId))
				
				SELECT MenuIds+',' FROM SysCustomMenus WHERE CompanyId=1 AND Type=2 AND dbo.Comm_F_NumIsInGroup(ObjId,'1,4')=1 FOR XML PATH('')
				SELECT * FROM  SysLimits WHERE LimitId=198
				SELECT * FROM dbo.SysDataDictionary WHERE DicPSN=103
				UPDATE dbo.SysDataDictionary SET SortOrder=0 WHERE DicPSN=103
				
				SELECT * FROM dbo.SysStoreUserInfo
				SELECT * FROM dbo.Warehouse
				SELECT * FROM dbo.OMS_CompanyAuthorize
				
				SELECT * FROM dbo.SysMenus WHERE MenuId=55
				
				SELECT * FROM dbo.SysUserInfo
				SELECT * FROM web
				UPDATE dbo.ProductRecord SET OldBarcode='CDEF34' WHERE Barcode='0500096514106'
				SELECT * FROM dbo.PayNotifyResult
				
				SELECT * FROM dbo.StockTaking ORDER BY CreateDT desc
				SELECT * FROM dbo.ProductCategory WHERE categorypsn=20
				
				SELECT b.Barcodes,a.* FROM StockTaking a
				INNER JOIN dbo.ProductRecord b ON b.CompanyId = a.CompanyId AND b.Barcode = a.Barcode
				WHERE a.CheckBatch='1516071801' 
				AND (b.Barcode='3333333333' OR ','+b.Barcodes+',' LIKE '%,3333333333,%')
				
				SELECT a.Barcode,b.Title,b.SubUnit,b.CategoryTitle,a.Id,a.SysPrice,a.CreateDT,dbo.F_UserNameById(a.CheckUID) FullName,a.State,a.Number FROM dbo.StockTakingLog a
				INNER JOIN dbo.Vw_Product b ON b.CompanyId = a.CompanyId AND (b.Barcode = a.Barcode OR ','+b.Barcodes+',' LIKE '%,'+a.Barcode+',%')
				WHERE a.CompanyId=1 and a.CheckBatch='1516071801' 
				
				SELECT * FROM Vw_StockTaking WHERE CheckBatch='1516072101' AND Barcode='0104040001'
				SELECT a.Barcode,b.Title,b.SubUnit,b.CategoryTitle,a.Id,a.SysPrice,a.CreateDT,dbo.F_UserNameById(a.CheckUID) FullName,a.CheckUID,a.State,a.Number,a.InitNumber FROM Vw_StockTaking_Log a 
				INNER JOIN dbo.Vw_Product b ON b.CompanyId = a.CompanyId AND (b.Barcode = a.Barcode OR ','+b.Barcodes+',' LIKE '%,'+a.Barcode+',%')
				WHERE a.CompanyId=1 and a.CheckBatch='1516072101'
				
				SELECT * FROM Vw_StockTaking_Log WHERE CheckBatch='1616072101' AND state=0
				SELECT * FROM dbo.StockTakingLog WHERE CheckBatch='1816072501'
				SELECT * FROM dbo.StockTaking WHERE CheckBatch='1816072501'
				SELECT * FROM dbo.InventoryBalance WHERE Barcode='0107040002'
				SELECT * FROM dbo.InventoryRecord ORDER BY Id desc
				SELECT * FROM dbo.StockTakingLog a WHERE CheckBatch='1516072101' and a.Barcode='0102010076'
				AND not EXISTS(SELECT 1 FROM StockTakingLog WHERE CompanyId=a.CompanyId AND CheckBatch=a.CheckBatch AND Barcode=a.Barcode AND State=1) ORDER BY CreateDT
				
				
				SELECT a.Barcode,b.Title,b.SubUnit,b.CategoryTitle,a.Id,a.SysPrice,a.CreateDT,dbo.F_UserNameById(a.CheckUID) FullName,a.CheckUID,a.State,a.Number FROM dbo.StockTakingLog a
				INNER JOIN dbo.Vw_Product b ON b.CompanyId = a.CompanyId AND (b.Barcode = a.Barcode OR ','+b.Barcodes+',' LIKE '%,'+a.Barcode+',%')
				WHERE a.CompanyId=1 and a.CheckBatch='1516071801' and a.state in(0) 
				--AND NOT EXISTS(SELECT 1 FROM StockTakingLog WHERE CompanyId=a.CompanyId AND CheckBatch=a.CheckBatch AND Barcode=a.Barcode AND State=1)  ORDER BY CreateDT
				
				SELECT * FROM dbo.Vw_Product WHERE Barcode='0113050006' OR Barcodes LIKE '%0113050006%'
				
				SELECT TOP 1 a.Number FROM dbo.StockTakingLog a ORDER BY a.CreateDT DESC
				
				DELETE dbo.StockTakingLog WHERE id BETWEEN 1331 AND 1334

				SELECT * FROM dbo.ProductChangePrice a
				INNER JOIN dbo.ProductChangePriceList b ON b.ChangePriceId = a.Id
				 ORDER BY CreateDT DESC
				 
				 SELECT * FROM dbo.SysLimits WHERE PLimitId=271