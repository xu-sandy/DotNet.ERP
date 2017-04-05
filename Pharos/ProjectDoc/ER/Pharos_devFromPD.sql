/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     2015/5/21 17:21:44                           */
/*==============================================================*/


if exists (select 1
            from  sysobjects
           where  id = object_id('ApiLibrary')
            and   type = 'U')
   drop table ApiLibrary
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Attachment')
            and   type = 'U')
   drop table Attachment
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Bundling')
            and   type = 'U')
   drop table Bundling
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BundlingList')
            and   type = 'U')
   drop table BundlingList
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Commodity')
            and   type = 'U')
   drop table Commodity
go

if exists (select 1
            from  sysobjects
           where  id = object_id('CommodityDiscount')
            and   type = 'U')
   drop table CommodityDiscount
go

if exists (select 1
            from  sysobjects
           where  id = object_id('CommodityPromotion')
            and   type = 'U')
   drop table CommodityPromotion
go

if exists (select 1
            from  sysobjects
           where  id = object_id('ConsumptionPayment')
            and   type = 'U')
   drop table ConsumptionPayment
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Contract')
            and   type = 'U')
   drop table Contract
go

if exists (select 1
            from  sysobjects
           where  id = object_id('ContractBoth')
            and   type = 'U')
   drop table ContractBoth
go

if exists (select 1
            from  sysobjects
           where  id = object_id('FreeGiftPurchase')
            and   type = 'U')
   drop table FreeGiftPurchase
go

if exists (select 1
            from  sysobjects
           where  id = object_id('IndentOrder')
            and   type = 'U')
   drop table IndentOrder
go

if exists (select 1
            from  sysobjects
           where  id = object_id('IndentOrderList')
            and   type = 'U')
   drop table IndentOrderList
go

if exists (select 1
            from  sysobjects
           where  id = object_id('MemberIntegral')
            and   type = 'U')
   drop table MemberIntegral
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Members')
            and   type = 'U')
   drop table Members
go

if exists (select 1
            from  sysobjects
           where  id = object_id('OrderDistribution')
            and   type = 'U')
   drop table OrderDistribution
go

if exists (select 1
            from  sysobjects
           where  id = object_id('OrderReturns')
            and   type = 'U')
   drop table OrderReturns
go

if exists (select 1
            from  sysobjects
           where  id = object_id('OutboundGoods')
            and   type = 'U')
   drop table OutboundGoods
go

if exists (select 1
            from  sysobjects
           where  id = object_id('OutboundList')
            and   type = 'U')
   drop table OutboundList
go

if exists (select 1
            from  sysobjects
           where  id = object_id('PosIncomePayout')
            and   type = 'U')
   drop table PosIncomePayout
go

if exists (select 1
            from  sysobjects
           where  id = object_id('ProductBrand')
            and   type = 'U')
   drop table ProductBrand
go

if exists (select 1
            from  sysobjects
           where  id = object_id('ProductCategory')
            and   type = 'U')
   drop table ProductCategory
go

if exists (select 1
            from  sysobjects
           where  id = object_id('ProductRecord')
            and   type = 'U')
   drop table ProductRecord
go

if exists (select 1
            from  sysobjects
           where  id = object_id('PromotionBlend')
            and   type = 'U')
   drop table PromotionBlend
go

if exists (select 1
            from  sysobjects
           where  id = object_id('QuotaPromotion')
            and   type = 'U')
   drop table QuotaPromotion
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Receipts')
            and   type = 'U')
   drop table Receipts
go

if exists (select 1
            from  sysobjects
           where  id = object_id('STHouseMove')
            and   type = 'U')
   drop table STHouseMove
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SaleDetail')
            and   type = 'U')
   drop table SaleDetail
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SaleOrders')
            and   type = 'U')
   drop table SaleOrders
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SalesReturns')
            and   type = 'U')
   drop table SalesReturns
go

if exists (select 1
            from  sysobjects
           where  id = object_id('StockTaking')
            and   type = 'U')
   drop table StockTaking
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Supplier')
            and   type = 'U')
   drop table Supplier
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TreasuryLocks')
            and   type = 'U')
   drop table TreasuryLocks
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Warehouse')
            and   type = 'U')
   drop table Warehouse
go

/*==============================================================*/
/* Table: ApiLibrary                                            */
/*==============================================================*/
create table ApiLibrary (
   Id                   char(10)             not null,
   ApiType              char(10)             not null,
   ApiCode              char(10)             not null,
   ApiUrl               char(10)             not null,
   Memo                 char(10)             null,
   State                char(10)             not null,
   constraint PK_APILIBRARY primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Table: Attachment                                            */
/*==============================================================*/
create table Attachment (
   Id                   char(10)             not null,
   SourceClassify       char(10)             not null,
   ItemId               char(10)             not null,
   Title                char(10)             not null,
   SaveUrl              char(10)             not null,
   Size                 char(10)             not null,
   constraint PK_ATTACHMENT primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Table: Bundling                                              */
/*==============================================================*/
create table Bundling (
   Id                   char(10)             not null,
   NewBarcode           char(10)             null,
   BundledPrice         char(10)             not null,
   TotalBundled         char(10)             not null
)
go

/*==============================================================*/
/* Table: BundlingList                                          */
/*==============================================================*/
create table BundlingList (
   Id                   char(10)             not null,
   Barcode              char(10)             null,
   Number               char(10)             not null
)
go

/*==============================================================*/
/* Table: Commodity                                             */
/*==============================================================*/
create table Commodity (
   Id                   char(10)             not null,
   StoreId              char(10)             null,
   Barcode              char(10)             not null,
   ProducedDate         char(10)             null,
   ExpiryDate           char(10)             null,
   ExpirationDate       char(10)             null,
   ProductionBatch      char(10)             null,
   StockNumber          char(10)             not null,
   constraint PK_COMMODITY primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Table: CommodityDiscount                                     */
/*==============================================================*/
create table CommodityDiscount (
   Id                   char(10)             not null,
   Barcode              char(10)             null,
   CategorySN           char(10)             null,
   DiscountRate         char(10)             not null,
   DiscountPrice        char(10)             not null,
   MinPurchaseNum       char(10)             not null,
   constraint PK_COMMODITYDISCOUNT primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Table: CommodityPromotion                                    */
/*==============================================================*/
create table CommodityPromotion (
   Id                   char(10)             not null,
   StoreId              char(10)             not null,
   CustomerObj          char(10)             not null,
   StartDate            char(10)             not null,
   EndDate              char(10)             not null,
   Timeliness           char(10)             not null,
   StartAging1          char(10)             null,
   EndAging1            char(10)             null,
   StartAging2          char(10)             null,
   EndAging2            char(10)             null,
   StartAging3          char(10)             null,
   EndAging3            char(10)             null,
   PromotionType        char(10)             not null,
   RestrictionBuyNum    char(10)             null,
   State                char(10)             not null,
   CreateDT             char(10)             not null,
   CreateUID            char(10)             not null,
   constraint PK_COMMODITYPROMOTION primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Table: ConsumptionPayment                                    */
/*==============================================================*/
create table ConsumptionPayment (
   Id                   char(10)             not null,
   PaySN                char(10)             not null,
   ApiCode              char(10)             not null,
   ApiOrderSN           char(10)             null,
   Amount               char(10)             not null,
   Memo                 char(10)             null,
   State                char(10)             not null,
   constraint PK_CONSUMPTIONPAYMENT primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Table: Contract                                              */
/*==============================================================*/
create table Contract (
   Id                   char(10)             not null,
   SupplierId           char(10)             null,
   ClassifyId           char(10)             not null,
   ContractSN           char(10)             not null,
   Title                char(10)             not null,
   PId                  char(10)             null,
   Version              char(10)             not null,
   SigningDate          char(10)             null,
   StartDate            char(10)             null,
   EndDate              char(10)             null,
   CreateUID            char(10)             not null,
   CreateDT             char(10)             not null,
   State                char(10)             not null,
   constraint PK_CONTRACT primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Table: ContractBoth                                          */
/*==============================================================*/
create table ContractBoth (
   ContractId           char(10)             not null,
   Signatory            char(10)             not null,
   CompanyName          char(10)             not null,
   Representative       char(10)             not null,
   Tel                  char(10)             null,
   Fax                  char(10)             null,
   TaxNumber            char(10)             null,
   Url                  char(10)             null,
   Address              char(10)             null,
   PostCode             char(10)             null,
   PayNumber            char(10)             null,
   BankName             char(10)             null,
   BankAccount          char(10)             null,
   --constraint PK_CONTRACTBOTH primary key nonclustered ()
)
go

/*==============================================================*/
/* Table: FreeGiftPurchase                                      */
/*==============================================================*/
create table FreeGiftPurchase (
   Id                   char(10)             not null,
   MinPurchaseNum       char(10)             not null,
   RestrictionBuyNum    char(10)             null,
   Barcode              char(10)             null,
   CategorySN           char(10)             null,
   GiftBarcode          char(10)             null,
   GiftCategorySN       char(10)             null,
   GiftNumber           char(10)             null
)
go

/*==============================================================*/
/* Table: IndentOrder                                           */
/*==============================================================*/
create table IndentOrder (
   Id                   char(10)             not null,
   StoreId              char(10)             null,
   OrdererUID           char(10)             null,
   SupplierID           char(10)             not null,
   OrderTotal           char(10)             not null,
   ShippingAddress      char(10)             null,
   DeliveryDate         char(10)             not null,
   CreateDT             char(10)             not null,
   CreateUID            char(10)             not null,
   State                char(10)             not null,
   constraint PK_INDENTORDER primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Table: IndentOrderList                                       */
/*==============================================================*/
create table IndentOrderList (
   Id                   char(10)             not null,
   IndentOrderId        char(10)             not null,
   Barcode              char(10)             not null,
   IndentNum            char(10)             not null,
   Price                char(10)             not null,
   Subtotal             char(10)             null,
   Memo                 char(10)             null,
   DeliveryNum          char(10)             null,
   AcceptNum            char(10)             null,
   State                char(10)             not null,
   constraint PK_INDENTORDERLIST primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Table: MemberIntegral                                        */
/*==============================================================*/
create table MemberIntegral (
   Id                   char(10)             not null,
   PaySN                char(10)             not null,
   MemberCardNum        char(10)             not null,
   ActualPrice          char(10)             not null,
   Integral             char(10)             not null,
   CreateDT             char(10)             not null,
   constraint PK_MEMBERINTEGRAL primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Table: Members                                               */
/*==============================================================*/
create table Members (
   Id                   char(10)             not null,
   StoreId              char(10)             not null,
   MemberCardNum        char(10)             not null,
   RealName             char(10)             null,
   MobilePhone          char(10)             null,
   Email                char(10)             null,
   Birthday             char(10)             null,
   UsableIntegral       char(10)             null,
   UsedIntegral         char(10)             null,
   CurrentCityId        char(10)             null,
   InsiderUID           char(10)             null,
   CreateDT             char(10)             not null,
   CreateUID            char(10)             not null,
   constraint PK_MEMBERS primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Table: OrderDistribution                                     */
/*==============================================================*/
create table OrderDistribution (
   Id                   char(10)             not null,
   IndentOrderId        char(10)             not null,
   Barcode              char(10)             not null,
   ProducedDate         char(10)             null,
   ExpiryDate           char(10)             null,
   ExpirationDate       char(10)             null,
   ProductionBatch      char(10)             null,
   DeliveryNum          char(10)             null,
   DeliveryDT           char(10)             null,
   State                char(10)             not null,
   constraint PK_ORDERDISTRIBUTION primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Table: OrderReturns                                          */
/*==============================================================*/
create table OrderReturns (
   Id                   char(10)             not null,
   IndentOrderId        char(10)             not null,
   DistributionId       char(10)             null,
   ReturnType           char(10)             not null,
   ReturnNum            char(10)             not null,
   ReasonId             char(10)             not null,
   CreateDT             char(10)             not null,
   CreateUID            char(10)             not null,
   constraint PK_ORDERRETURNS primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Table: OutboundGoods                                         */
/*==============================================================*/
create table OutboundGoods (
   Id                   char(10)             not null,
   StoreId              char(10)             not null,
   ApplyOrgId           char(10)             null,
   ApplyUID             char(10)             null,
   OperatorUID          char(10)             not null,
   CreateDT             char(10)             not null,
   constraint PK_OUTBOUNDGOODS primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Table: OutboundList                                          */
/*==============================================================*/
create table OutboundList (
   Id                   char(10)             not null,
   Barcode              char(10)             not null,
   OutboundNumber       char(10)             not null,
   BuyPrice             char(10)             not null,
   SysPrice             char(10)             not null,
   Memo                 char(10)             null
)
go

/*==============================================================*/
/* Table: PosIncomePayout                                       */
/*==============================================================*/
create table PosIncomePayout (
   Id                   char(10)             not null,
   StoreId              char(10)             not null,
   MachineSN            char(10)             not null,
   CreateUID            char(10)             not null,
   Type                 char(10)             not null,
   Amount               char(10)             not null,
   CreateDT             char(10)             not null,
   constraint PK_POSINCOMEPAYOUT primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Table: ProductBrand                                          */
/*==============================================================*/
create table ProductBrand (
   Id                   char(10)             not null,
   ClassifyId           char(10)             not null,
   BrandSN              char(10)             not null,
   Title                char(10)             not null,
   State                char(10)             not null,
   constraint PK_PRODUCTBRAND primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Table: ProductCategory                                       */
/*==============================================================*/
create table ProductCategory (
   Id                   char(10)             not null,
   CategorySN           char(10)             not null,
   CategoryPSN          char(10)             not null,
   Grade                char(10)             not null,
   Title                char(10)             not null,
   OrderNum             char(10)             null,
   State                char(10)             not null,
   constraint PK_PRODUCTCATEGORY primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Table: ProductRecord                                         */
/*==============================================================*/
create table ProductRecord (
   Id                   char(10)             not null,
   ProductCode          char(10)             not null,
   Barcode              char(10)             not null,
   Title                char(10)             not null,
   Size                 char(10)             null,
   BrandSN              char(10)             null,
   CityId               char(10)             null,
   BigCategorySN        char(10)             null,
   SubCategorySN        char(10)             null,
   BigUnitId            char(10)             null,
   SubUnitId            char(10)             null,
   BuyPrice             char(10)             not null,
   SysPrice             char(10)             not null,
   Nature               char(10)             not null,
   BarcodeGroup         char(10)             null,
   RaterUID             char(10)             null,
   State                char(10)             not null,
   constraint PK_PRODUCTRECORD primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Table: PromotionBlend                                        */
/*==============================================================*/
create table PromotionBlend (
   Id                   char(10)             not null,
   FullNumber           char(10)             not null,
   PromotionType        char(10)             not null,
   Barcode              char(10)             null,
   CategorySN           char(10)             null,
   GiftBarcode          char(10)             null,
   GiftCategorySN       char(10)             null,
   AllowedAccumulate    char(10)             null
)
go

/*==============================================================*/
/* Table: QuotaPromotion                                        */
/*==============================================================*/
create table QuotaPromotion (
   Id                   char(10)             not null,
   OrderAmount          char(10)             not null,
   PromotionType        char(10)             not null,
   Discount             char(10)             not null,
   Barcode              char(10)             null,
   CategorySN           char(10)             null,
   ExclBarcode          char(10)             null,
   ExclCategorySN       char(10)             null,
   AllowedAccumulate    char(10)             null
)
go

/*==============================================================*/
/* Table: Receipts                                              */
/*==============================================================*/
create table Receipts (
   Id                   char(10)             not null,
   StoreId              char(10)             null,
   CategoryId           char(10)             not null,
   Number               char(10)             not null,
   Amount               char(10)             not null,
   Pages                char(10)             not null,
   Title                char(10)             not null,
   Memo                 char(10)             null,
   CreateUID            char(10)             not null,
   CreateDT             char(10)             not null,
   State                char(10)             not null,
   constraint PK_RECEIPTS primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Table: STHouseMove                                           */
/*==============================================================*/
create table STHouseMove (
   Id                   char(10)             not null,
   OutStoreId           char(10)             not null,
   InStoreId            char(10)             not null,
   Barcode              char(10)             not null,
   OrderQuantity        char(10)             not null,
   CreateDT             char(10)             not null,
   CreateUID            char(10)             not null,
   DeliveryQuantity     char(10)             null,
   DeliveryUID          char(10)             null,
   ActualQuantity       char(10)             null,
   ActualUID            char(10)             null,
   Memo                 char(10)             null,
   State                char(10)             not null,
   ReasonId             char(10)             null,
   constraint PK_STHOUSEMOVE primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Table: SaleDetail                                            */
/*==============================================================*/
create table SaleDetail (
   Id                   char(10)             not null,
   PaySN                char(10)             not null,
   Barcode              char(10)             not null,
   PurchaseNumber       char(10)             not null,
   BuyPrice             char(10)             not null,
   SysPrice             char(10)             not null,
   ActualPrice          char(10)             not null,
   SalesClassifyId      char(10)             not null,
   Memo                 char(10)             null,
   constraint PK_SALEDETAIL primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Table: SaleOrders                                            */
/*==============================================================*/
create table SaleOrders (
   Id                   char(10)             not null,
   StoreId              char(10)             not null,
   MachineSN            char(10)             not null,
   PaySN                char(10)             not null,
   PurchaseNumber       char(10)             not null,
   TotalAmount          char(10)             not null,
   PreferentialPrice    char(10)             not null,
   ApiCode              char(10)             not null,
   CreateDT             char(10)             not null,
   CreateUID            char(10)             null,
   Salesman             char(10)             null,
   Memo                 char(10)             null,
   constraint PK_SALEORDERS primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Table: SalesReturns                                          */
/*==============================================================*/
create table SalesReturns (
   Id                   char(10)             not null,
   StoreId              char(10)             not null,
   Barcode              char(10)             not null,
   ReceiptsNumber       char(10)             not null,
   ReturnType           char(10)             not null,
   NewBarcode           char(10)             not null,
   ReturnNumber         char(10)             not null,
   ReasonId             char(10)             not null,
   ReturnPrice          char(10)             not null,
   ReturnAmountType     char(10)             not null,
   CardNumber           char(10)             not null,
   Cardholder           char(10)             not null,
   CreateDT             char(10)             not null,
   CreateUID            char(10)             not null,
   constraint PK_SALESRETURNS primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Table: StockTaking                                           */
/*==============================================================*/
create table StockTaking (
   Id                   char(10)             not null,
   CheckBatch           char(10)             not null,
   Barcode              char(10)             not null,
   ActualNumber         char(10)             not null,
   CheckUID             char(10)             not null,
   CreateDT             char(10)             not null,
   CreateUID            char(10)             not null,
   constraint PK_STOCKTAKING primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Table: Supplier                                              */
/*==============================================================*/
create table Supplier (
   Id                   char(10)             not null,
   ClassifyId           char(10)             not null,
   Title                char(10)             not null,
   Jianpin              char(10)             null,
   FullTitle            char(10)             not null,
   Linkman              char(10)             not null,
   MobilePhone          char(10)             null,
   Tel                  char(10)             null,
   "E-mail"             char(10)             null,
   Address              char(10)             null,
   Designee             char(10)             not null,
   MasterAccount        char(10)             null,
   MasterPwd            char(10)             null,
   MasterState          char(10)             not null,
   constraint PK_SUPPLIER primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Table: TreasuryLocks                                         */
/*==============================================================*/
create table TreasuryLocks (
   Id                   char(10)             not null,
   LockStoreID          char(10)             not null,
   LockCategorySN       char(10)             not null,
   CheckBatch           char(10)             not null,
   LockDate             char(10)             not null,
   LockUID              char(10)             not null,
   constraint PK_TREASURYLOCKS primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Table: Warehouse                                             */
/*==============================================================*/
create table Warehouse (
   Id                   char(10)             not null,
   StoreId              char(10)             not null,
   Title                char(10)             not null,
   CategorySN           char(10)             not null,
   State                char(10)             not null,
   CreateDT             char(10)             not null,
   CreateUID            char(10)             not null,
   constraint PK_WAREHOUSE primary key nonclustered (Id)
)
go

