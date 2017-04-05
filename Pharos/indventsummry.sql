USE [Dongben_Test]
GO
/****** Object:  StoredProcedure [dbo].[Rpt_InvoicingSummary]    Script Date: 04/15/2016 14:52:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		linbl
-- Create date: <2015-12-16>
-- Description:	<进销存统计报表>
-- =============================================
ALTER PROCEDURE [dbo].[Rpt_InvoicingSummary]
	@companyId INT,
	@startDate VARCHAR(20)='', 
	@endDate VARCHAR(20)='', 
	@storeId VARCHAR(100)='',
	@supplierId VARCHAR(300)='',
	@categorySn	VARCHAR(2000)='',
	@title VARCHAR(50)='',
	@CurrentPage	INT=1,			--当前页	@PageSize		INT=20,				--页大小,
	@ispage SMALLINT=1  --是否分页	
AS
BEGIN
	SET NOCOUNT ON;
	--IF OBJECT_ID('tempdb.dbo.#t1','U') IS NOT NULL DROP TABLE #t1;
	
	DECLARE @RecordStart INT;
    DECLARE @RecordEnd INT;
    IF ( @CurrentPage <= 1 )
        BEGIN
            SET @RecordStart = 1;
            SET @RecordEnd = @PageSize;
        END
    ELSE
        BEGIN
            SET @RecordStart = ( ( @CurrentPage - 1 ) * @PageSize ) + 1;
            SET @RecordEnd = @CurrentPage * @PageSize;
        END
            
	--采购量
	SELECT t.Barcode,SUM(t.ReceivedNum) 采购入库数,SUM(t.ReceivedNum*t.Price) 采购入库金额 INTO #t1 FROM (
	SELECT a.Barcode,a.ReceivedNum,c.Price from dbo.OrderDistribution a 
	INNER join dbo.IndentOrder b ON b.IndentOrderId=a.IndentOrderId
	INNER join IndentOrderList c ON a.IndentOrderId=c.IndentOrderId AND a.Barcode=c.Barcode AND c.Nature=0
	WHERE a.State=5
	AND (@startDate='' OR @endDate='' OR (a.ReceivedDT BETWEEN @startDate AND @endDate))
	AND (@storeId='' OR EXISTS(SELECT * FROM dbo.SplitString(@storeId,',',1) WHERE Value=b.StoreId))
	AND (@supplierId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@supplierId,',',1) WHERE Value=b.SupplierID))
	AND a.companyid=@companyId
	UNION all
	SELECT b.Barcode,b.InboundNumber,b.BuyPrice FROM dbo.InboundGoods a, dbo.InboundList b 
	WHERE a.InboundGoodsId=b.InboundGoodsId AND a.State=1 AND b.IsGift=0 AND a.InboundType=1
	and (@startDate='' OR @endDate='' OR (CreateDT BETWEEN @startDate AND @endDate))
	AND (@storeId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@storeId,',',1) WHERE Value=a.StoreId))
	AND (@supplierId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@supplierId,',',1) WHERE Value=a.SupplierID))
	AND a.companyid=@companyId
	) t GROUP BY Barcode
	
	--采购赠送量
	SELECT SUM(赠送入库数量) 赠送入库数量,SUM(赠送入库金额) 赠送入库金额,Barcode INTO #t2  FROM(
	SELECT SUM(b.InboundNumber) 赠送入库数量,SUM(b.InboundNumber*b.BuyPrice) 赠送入库金额,b.Barcode FROM dbo.InboundGoods a ,dbo.InboundList b WHERE a.InboundGoodsId=b.InboundGoodsId AND b.IsGift=1 AND a.State=1
	AND (@startDate='' OR @endDate='' OR (a.CreateDT BETWEEN @startDate AND @endDate))
	AND (@storeId='' OR EXISTS(SELECT * FROM dbo.SplitString(@storeId,',',1) WHERE Value=a.StoreId))
	AND (@supplierId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@supplierId,',',1) WHERE Value=a.SupplierID))
	AND a.companyid=@companyId
	GROUP BY b.Barcode
	UNION all
	SELECT SUM(a.ReceivedNum) 赠送入库数量,SUM(a.ReceivedNum*c.Price) 赠送入库金额,a.Barcode FROM dbo.OrderDistributionGift a 
	INNER JOIN dbo.OrderDistribution d ON d.Id=a.OrderDistributionId
	INNER join dbo.IndentOrder b ON b.IndentOrderId=d.IndentOrderId
	INNER join IndentOrderList c ON b.IndentOrderId=c.IndentOrderId AND a.Barcode=c.Barcode AND c.Nature=1 AND d.Barcode=c.ResBarcode
	WHERE d.State=5
	AND (@startDate='' OR @endDate='' OR (d.ReceivedDT BETWEEN @startDate AND @endDate))
	AND (@storeId='' OR EXISTS(SELECT * FROM dbo.SplitString(@storeId,',',1) WHERE Value=b.StoreId))
	AND (@supplierId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@supplierId,',',1) WHERE Value=b.SupplierID))
	AND d.companyid=@companyId
	GROUP BY a.Barcode) giftdt GROUP BY giftdt.Barcode
	
	--采购退换
	SELECT retdt.Barcode,SUM(retdt.采购退货数量) 采购退货数量,SUM(retdt.采购退货金额) 采购退货金额 INTO #t3 FROM(
	SELECT a.Barcode,SUM(ReturnNum) 采购退货数量,SUM(b.Price*ReturnNum) as 采购退货金额 FROM OrderReturns a,dbo.IndentOrderList b WHERE a.IndentOrderId=b.IndentOrderId and a.Barcode=b.Barcode and ReturnType=0 
	AND (@startDate='' OR @endDate='' OR (CreateDT BETWEEN @startDate AND @endDate))
	AND (@storeId='' OR EXISTS(SELECT 1 FROM dbo.IndentOrder,dbo.SplitString(@storeId,',',1) WHERE IndentOrderId=b.IndentOrderId AND StoreId=Value))
	AND (@supplierId='' OR EXISTS(SELECT 1 FROM dbo.IndentOrder,dbo.SplitString(@supplierId,',',1) WHERE IndentOrderId=b.IndentOrderId AND SupplierID=Value))
	AND a.companyid=@companyId
	GROUP BY a.Barcode
	UNION all
	SELECT b.Barcode,SUM(b.ReturnNum) 退货登记数量,SUM(b.BuyPrice*b.ReturnNum) 退货登记金额 FROM dbo.CommodityReturns a,dbo.CommodityReturnsDetail b WHERE b.ReturnId=a.ReturnId AND a.State=2
	AND (@startDate='' OR @endDate='' OR (CreateDT BETWEEN @startDate AND @endDate))
	AND (@storeId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@storeId,',',1) WHERE Value=a.StoreId))
	AND (@supplierId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@supplierId,',',1) WHERE Value=a.SupplierID))
	AND a.companyid=@companyId
	GROUP BY b.Barcode
	) retdt GROUP BY retdt.Barcode
	
	--调拨调出
	--SELECT a.Barcode,SUM(a.DeliveryQuantity) 店内调出数量,SUM(dbo.F_SysPriceByBarcode('',a.Barcode)*a.DeliveryQuantity) as 店内调出金额 INTO #t4 FROM STHouseMove a 
	--WHERE a.State=4 AND( @startDate='' OR @endDate='' OR (CreateDT BETWEEN @startDate AND @endDate))
	--AND(@storeId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@storeId,',',1) WHERE Value=a.OutStoreId))
	--GROUP BY a.Barcode
	SELECT b.Barcode,SUM(b.DeliveryQuantity) 店内调出数量,SUM(b.DeliveryQuantity*b.SysPrice) as 店内调出金额 INTO #t4 FROM dbo.HouseMove a,dbo.HouseMoveList b WHERE a.MoveId=b.MoveId and a.State=4 
	AND( @startDate='' OR @endDate='' OR (a.ActualDT BETWEEN @startDate AND @endDate))
	AND(@storeId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@storeId,',',1) WHERE Value=a.OutStoreId))
	AND a.companyid=@companyId
	GROUP BY b.Barcode
	
	--调拨调入
	--SELECT a.Barcode,SUM(a.ActualQuantity) 店内调入数量,SUM(dbo.F_SysPriceByBarcode('',a.Barcode)*a.ActualQuantity) as 店内调入金额 INTO #t12 FROM STHouseMove a 
	--WHERE a.State=4 AND( @startDate='' OR @endDate='' OR (CreateDT BETWEEN @startDate AND @endDate))
	--AND(@storeId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@storeId,',',1) WHERE Value=a.InStoreId))
	--GROUP BY a.Barcode
	SELECT b.Barcode,SUM(b.ActualQuantity) 店内调入数量,SUM(b.ActualQuantity*b.SysPrice) as 店内调入金额 INTO #t12 FROM dbo.HouseMove a,dbo.HouseMoveList b WHERE a.MoveId=b.MoveId and a.State=4 
	AND( @startDate='' OR @endDate='' OR (a.ActualDT BETWEEN @startDate AND @endDate))
	AND(@storeId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@storeId,',',1) WHERE Value=a.InStoreId))
	AND a.companyid=@companyId
	GROUP BY b.Barcode
	
	--批发
	SELECT b.Barcode,SUM(b.OutboundNumber) 批发销售数,SUM(b.SysPrice*b.OutboundNumber) 批发销售金额,
	SUM(b.OutboundNumber*b.BuyPrice) 批发销售成本 INTO #t5
	FROM OutboundGoods a,dbo.OutboundList b WHERE a.OutboundId=b.OutboundId AND a.Channel=1 AND a.State=1
	and (@startDate='' OR @endDate='' OR (CreateDT BETWEEN @startDate AND @endDate))
	AND (@storeId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@storeId,',',1) WHERE Value=a.StoreId))
	AND a.companyid=@companyId
	GROUP BY b.Barcode
	--其它出库
	SELECT b.Barcode,	SUM(b.OutboundNumber) 其它出库数量,SUM(b.SysPrice*b.OutboundNumber) 其它出库金额 INTO #t6
	FROM OutboundGoods a,dbo.OutboundList b WHERE a.OutboundId=b.OutboundId AND a.Channel=0 AND a.State=1 AND a.OutboundType=2
	and (@startDate='' OR @endDate='' OR (CreateDT BETWEEN @startDate AND @endDate))
	AND (@storeId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@storeId,',',1) WHERE Value=a.StoreId))
	AND (@supplierId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@supplierId,',',1) WHERE Value=a.ApplyOrgId))
	AND a.companyid=@companyId
	GROUP BY b.Barcode
	--销售
	SELECT b.Barcode,SUM(b.PurchaseNumber) 零售数量,SUM(b.ActualPrice*b.PurchaseNumber) 零售金额,SUM(b.BuyPrice*b.PurchaseNumber) 零售成本 INTO #t7
	FROM dbo.SaleOrders a,dbo.SaleDetail b WHERE a.PaySN=b.PaySN and a.Type=0 AND a.State=0 AND a.IsTest=0
	and (@startDate='' OR @endDate='' OR (CreateDT BETWEEN @startDate AND @endDate))
	AND (@storeId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@storeId,',',1) WHERE Value=a.StoreId))
	AND a.companyid=@companyId
	GROUP BY b.Barcode
	--其它入库
	SELECT Barcode,SUM(InboundNumber) 其它入库数量,SUM(InboundNumber*BuyPrice) 其它入库金额 INTO #t8 FROM dbo.InboundGoods a, dbo.InboundList b 
	WHERE a.InboundGoodsId=b.InboundGoodsId AND a.State=1  AND a.InboundType=2 --AND b.IsGift=0
	and (@startDate='' OR @endDate='' OR (CreateDT BETWEEN @startDate AND @endDate))
	AND (@storeId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@storeId,',',1) WHERE Value=a.StoreId))
	AND (@supplierId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@supplierId,',',1) WHERE Value=a.SupplierID))
	AND a.companyid=@companyId
	GROUP BY Barcode
	--报损
	SELECT b.Barcode,SUM(b.BreakageNumber) 报损数量,SUM(b.BreakageNumber*b.BreakagePrice) 报损金额 INTO #t9 FROM dbo.BreakageGoods a ,dbo.BreakageList b WHERE a.BreakageGoodsId=b.BreakageGoodsId AND a.State=1
	and (@startDate='' OR @endDate='' OR (CreateDT BETWEEN @startDate AND @endDate))
	AND (@storeId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@storeId,',',1) WHERE Value=a.StoreId))
	AND a.companyid=@companyId
	GROUP BY b.Barcode
	--组合
	SELECT groupdt.Barcode,SUM(组合数量) 组合数量,sum(组合金额) 组合金额 INTO #t10 FROM (
		--SELECT  b.Barcode,SUM(b.PurchaseNumber) 组合数量,sum(b.PurchaseNumber*b.ActualPrice) 组合金额 FROM dbo.SaleOrders a,dbo.SaleDetail b WHERE a.PaySN=b.PaySN AND a.Type=0 AND a.State=0
		--AND EXISTS(SELECT 1 FROM dbo.ProductRecord WHERE Barcode=b.Barcode AND Nature=1)
		--and (@startDate='' OR @endDate='' OR (CreateDT BETWEEN @startDate AND @endDate))
		--AND (@storeId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@storeId,',',1) WHERE Value=a.StoreId))
		--GROUP BY b.Barcode
		--UNION all
		--SELECT b.Barcode,SUM(a.Number) 组合数量,sum(a.Number*b.ActualPrice) 组合金额 FROM dbo.SaleInventoryHistory a,dbo.SaleDetail b WHERE a.PaySN=b.PaySN AND a.SaleBarcode=b.Barcode AND a.Mode=3
		--and (@startDate='' OR @endDate='' OR EXISTS(SELECT 1 FROM dbo.SaleOrders WHERE PaySN=b.PaySN and CreateDT BETWEEN @startDate AND @endDate))
		--AND (@storeId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@storeId,',',1) WHERE Value=a.StoreId))
		--GROUP BY b.Barcode
		SELECT b.Barcode,SUM(b.PurchaseNumber) 组合数量,sum(b.PurchaseNumber*b.ActualPrice) 组合金额 FROM dbo.SaleOrders a, dbo.SaleDetail b 
		WHERE a.PaySN=b.PaySN and EXISTS(SELECT 1 FROM SaleInventoryHistory where PaySN=b.PaySN AND SaleBarcode=b.Barcode AND Mode=3)
		and (@startDate='' OR @endDate='' OR EXISTS(SELECT 1 FROM dbo.SaleOrders WHERE PaySN=b.PaySN and CreateDT BETWEEN @startDate AND @endDate))
		AND (@storeId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@storeId,',',1) WHERE Value=a.StoreId))
		AND a.companyid=@companyId
		GROUP BY b.Barcode
	) groupdt GROUP BY groupdt.Barcode
	--盘点盈亏
	SELECT Barcode,SUM(SubstractNum) 盘点盈亏数量,SUM(SubstractTotal) 盘点盈亏金额 INTO #t11 FROM dbo.Vw_StockTaking WHERE state=1
	and (@startDate='' OR @endDate='' OR (LockDate BETWEEN @startDate AND @endDate))
	AND (@storeId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@storeId,',',1) WHERE Value=LockStoreID))
	AND companyid=@companyId
	GROUP BY Barcode
	--拆分
	SELECT ROW_NUMBER() OVER(PARTITION BY Barcode ORDER BY BalanceDate desc) row,Barcode,Number 拆分数量,dbo.F_BuyPriceByBarcode('',Barcode,a.CompanyId)*number 拆分金额 INTO #t13 FROM dbo.InventoryBalance a WHERE 1=1
	AND EXISTS(SELECT 1 FROM dbo.ProductRecord WHERE Barcode=a.Barcode AND Nature=2)
	and (@startDate='' OR @endDate='' OR (BalanceDate BETWEEN @startDate AND @endDate))
	AND (@storeId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@storeId,',',1) WHERE Value=StoreId))
	AND a.companyid=@companyId
	
	--SELECT  SUM(Number) 结存数量,Barcode,SUM(StockAmount) 结存金额 INTO #t14 FROM dbo.InventoryBalance WHERE 1=1
	--AND (@startDate='' OR @endDate='' OR (BalanceDate BETWEEN @startDate AND @endDate))
	--AND (@storeId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@storeId,',',1) WHERE Value=StoreId))
	--AND companyid=@companyId
	--GROUP BY Barcode
	
	--结存
	SELECT Barcode,SUM(t.Number) 期初库存,SUM(StockAmount) 期初金额 INTO #t14 FROM(
		SELECT  ROW_NUMBER() OVER(PARTITION BY StoreId,Barcode ORDER BY BalanceDate DESC) row,* FROM dbo.InventoryBalance
		WHERE BalanceDate<@startDate
		AND (@storeId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@storeId,',',1) WHERE Value=StoreId))
		AND CompanyId=@companyId 
	) t WHERE row=1 GROUP BY Barcode
		
	SELECT Barcode,SUM(t.Number) 结存数量,SUM(StockAmount) 结存金额 INTO #t15 FROM(
		SELECT  ROW_NUMBER() OVER(PARTITION BY StoreId,Barcode ORDER BY BalanceDate DESC) row,* FROM dbo.InventoryBalance
		WHERE BalanceDate<@endDate
		AND (@storeId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@storeId,',',1) WHERE Value=StoreId))
		AND CompanyId=@companyId 
	) t WHERE row=1 GROUP BY Barcode
	
	;WITH RecordList 
	AS(
		SELECT a.SupplierTitle 主供应商,a.ProductCode 商品编码,a.Barcode 商品条码,a.Title 品名,a.SubUnit 单位,a.CategoryTitle 商品类别,a.StockRate,
		m.期初库存,m.期初金额,b.采购入库数,b.采购入库金额,c.赠送入库数量,c.赠送入库金额,d.采购退货数量,d.采购退货金额,
		e.店内调出数量,e.店内调出金额,o.店内调入数量,o.店内调入金额,f.批发销售数,f.批发销售金额,f.批发销售成本,(f.批发销售金额-f.批发销售成本) 批发毛利,
		CAST(CASE f.批发销售金额 WHEN 0 THEN 0 ELSE (f.批发销售金额-f.批发销售成本)/f.批发销售金额*100 END AS VARCHAR(20))+'%' 批发毛利率,
		g.其它出库数量,g.其它出库金额,i.其它入库数量,i.其它入库金额,h.零售数量,h.零售金额,h.零售成本,ROUND(h.零售成本/(100+ISNULL(a.SaleRate,0))*100,2) 未税零售成本,h.零售金额-h.零售成本 零售毛利,
		CAST(CASE h.零售金额 WHEN 0 THEN 0 ELSE (h.零售金额-h.零售成本)/h.零售金额*100 END AS VARCHAR(20))+'%' 零售毛利率,
		h.零售成本 销售成本,h.零售金额-h.零售成本 销售毛利,CAST(CASE h.零售金额 WHEN 0 THEN 0 ELSE (h.零售金额-h.零售成本)/h.零售金额*100 END AS VARCHAR(20))+'%' 销售毛利率,
		j.报损数量,j.报损金额,n.盘点盈亏数量,n.盘点盈亏金额,k.组合数量,k.组合金额,l.拆分数量,l.拆分金额,p.结存数量,p.结存金额,
		dbo.F_BuyPriceByBarcode(@supplierId,a.Barcode,a.CompanyId) BuyPrice,ROW_NUMBER() OVER(ORDER BY a.Id) AS RSNO
		FROM dbo.Vw_Product a
		LEFT JOIN  #t1 b ON a.Barcode=b.Barcode
		LEFT JOIN  #t2 c ON a.Barcode=c.Barcode
		LEFT JOIN  #t3 d ON a.Barcode=d.Barcode
		LEFT JOIN  #t4 e ON a.Barcode=e.Barcode
		LEFT JOIN  #t5 f ON a.Barcode=f.Barcode
		LEFT JOIN  #t6 g ON a.Barcode=g.Barcode
		LEFT JOIN  #t7 h ON a.Barcode=h.Barcode
		LEFT JOIN  #t8 i ON a.Barcode=i.Barcode
		LEFT JOIN  #t9 j ON a.Barcode=j.Barcode
		LEFT JOIN  #t10 k ON a.Barcode=k.Barcode
		LEFT JOIN #t13 l ON a.Barcode=l.Barcode AND l.row=1
		LEFT JOIN #t14 m ON a.Barcode=m.Barcode
		LEFT JOIN  #t11 n ON a.Barcode=n.Barcode
		LEFT JOIN  #t12 o ON a.Barcode=o.Barcode
		LEFT JOIN #t15 p ON a.Barcode=p.Barcode
		WHERE (@title='' OR a.Barcode LIKE '%'+@title+'%' OR a.Title LIKE '%'+@title+'%')
		AND (@categorySn='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@categorySn,',',1) s WHERE s.Value=CAST(a.CategorySN AS VARCHAR(20))))
		AND (@supplierId='' OR EXISTS(SELECT 1 FROM dbo.SplitString(@supplierId,',',1) WHERE Value=a.SupplierId))
		AND a.companyid=@companyId
		),
		RecordPage
		  AS ( SELECT   MAX(RSNO) AS [RecordTotal] ,
						( CASE WHEN @CurrentPage > CEILING(CONVERT(DECIMAL(18,
													  2), MAX(RSNO))
													  / @PageSize)
							   THEN ( CEILING(CONVERT(DECIMAL(18, 2), MAX(RSNO))
											  / @PageSize) - 1 )
									* @PageSize + 1
							   ELSE @RecordStart
						  END ) AS [RecordStart] ,
						( CASE WHEN @CurrentPage > ( CONVERT(DECIMAL(18,
													  2), MAX(RSNO))
													 / @PageSize )
							   THEN CEILING(CONVERT(DECIMAL(18, 2), MAX(RSNO))
											/ @PageSize) * @PageSize
							   ELSE @RecordEnd
						  END ) AS [RecordEnd]
			   FROM     RecordList
			 )
	SELECT  *,ROUND(结存金额/(100+ISNULL(RL.StockRate,0))*100,2) 未税结存金额
	FROM    RecordList AS RL ,
			RecordPage AS RP
	WHERE   ( (RL.RSNO BETWEEN RP.RecordStart AND RP.RecordEnd AND @ispage=1) OR @ispage=0 )
END


