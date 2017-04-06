
namespace Pharos.Sys
{
    /// <summary>
    /// 权限代码（常用池，与DB权限编码对应）
    /// </summary>
    public class SysConstLimits
    {
        #region 截止2016.02.25

        #region //深度1

        public const int 采购管理 = 3;
        public const int 返利管理 = 4;
        public const int 促销管理 = 5;
        public const int 库存管理 = 6;
        public const int 统计报表 = 7;

        public const int 系统管理 = 8;
        public const int 日常办公 = 155;
        public const int 档案管理 = 156;
        public const int 会员管理 = 183;

        public const int 供应商后台 = 232;
        #endregion

        #region //深度2

        public const int 采购管理_供应合同 = 24;
        public const int 采购管理_采购订单 = 25;
        public const int 返利管理_采购返利 = 53;
        public const int 返利管理_销售返利 = 59;

        public const int 促销管理_查看促销 = 60;
        public const int 促销管理_创建促销 = 199;
        public const int 促销管理_状态设定 = 200;
        public const int 促销管理_移除促销 = 201;

        public const int 库存管理_移除报损 = 202;
        public const int 库存管理_已验报损 = 203;
        public const int 库存管理_查看批发 = 204;
        public const int 库存管理_批发登记 = 205;
        public const int 库存管理_已验批发 = 206;

        public const int 库存管理_打印批发单 = 207;
        public const int 库存管理_采购申请 = 265;
        public const int 库存管理_收货明细 = 266;
        public const int 库存管理_零售退换 = 267;
        public const int 库存管理_调拨管理 = 268;
        public const int 库存管理_采购退货 = 269;

        public const int 库存管理_查看出入库 = 86;
        public const int 库存管理_库存导出 = 87;
        public const int 库存管理_移除出入库 = 88;
        public const int 库存管理_出入库登记 = 89;
        public const int 库存管理_库存查询 = 90;
        public const int 库存管理_已验出入库 = 91;

        public const int 库存管理_打印出入库单 = 92;
        public const int 库存管理_已完成退换货 = 141;
        public const int 库存管理_查看退换货 = 142;
        public const int 库存管理_打印退换货单 = 143;
        public const int 库存管理_查看盘点 = 144;

        public const int 库存管理_库存锁定 = 145;
        public const int 库存管理_初盘录入 = 146;
        public const int 库存管理_导入盘点文件 = 147;
        public const int 库存管理_盘点审核 = 148;
        public const int 库存管理_复盘录入 = 149;

        public const int 库存管理_导出盘点差异 = 150;
        public const int 库存管理_导出空盘报表 = 151;
        public const int 库存管理_查看报损 = 152;
        public const int 库存管理_报损登记 = 153;
        public const int 库存管理_退货审核 = 154;

        public const int 统计报表_销售报表 = 93;
        public const int 统计报表_赠品报表 = 94;
        public const int 统计报表_采购报表 = 208;

        public const int 系统管理_设备管理 = 218;
        public const int 系统管理_票据模板 = 219;
        public const int 系统管理_基本配置 = 220;
        public const int 系统管理_用户管理 = 102;
        public const int 系统管理_角色管理 = 103;

        public const int 系统管理_门店维护 = 104;
        public const int 系统管理_菜单管理 = 105;
        public const int 系统管理_数据字典 = 106;
        public const int 系统管理_支付接口 = 107;
        public const int 系统管理_数据维护 = 108;
        public const int 系统管理_日志管理 = 129;
        public const int 系统管理_组织机构 = 134;

        public const int 日常办公_通知公告 = 1;
        public const int 日常办公_邮件管理 = 2;

        public const int 档案管理_供应商 = 17;
        public const int 档案管理_产品档案 = 39;
        public const int 档案管理_产品种类 = 44;
        public const int 档案管理_品牌管理 = 45;
        public const int 档案管理_批发商 = 160;
        public const int 档案管理_产品变价 = 166;
        public const int 档案管理_批发价 = 167;
        public const int 档案管理_商品传秤 = 168;

        public const int 会员管理_会员档案 = 184;
        public const int 会员管理_积分管理 = 185;
        public const int 会员管理_代金券 = 186;
        public const int 会员管理_活动推送 = 187;

        public const int 供应商后台_订单管理 = 233;
        public const int 供应商后台_统计报表 = 234;
        public const int 供应商后台_商家信息维护 = 235;
        public const int 供应商后台_账号管理 = 236;       

        #endregion

        #region //深度3

        public const int 供应商_查看供应商 = 18;
        public const int 供应商_编辑供应商 = 19;
        public const int 供应商_编辑分类 = 20;
        public const int 供应商_移除供应商 = 22;
        public const int 供应商_下订单 = 23;
        public const int 供应商_主帐号管理 = 157;

        public const int 供应合同_查看合同 = 26;
        public const int 供应合同_创建合同 = 27;
        public const int 供应合同_审批合同 = 29;
        public const int 供应合同_续签合同 = 30;
        public const int 供应合同_中止合同 = 31;
        public const int 供应合同_删除合同 = 32;

        public const int 采购订单_查看订单 = 33;
        public const int 采购订单_创建订单 = 34;
        public const int 采购订单_审批订单 = 36;
        public const int 采购订单_中止订货 = 37;
        public const int 采购订单_提醒配送 = 38;

        public const int 产品档案_查看信息 = 40;
        public const int 产品档案_新品建档 = 41;
        public const int 产品档案_删除商品 = 43;
        public const int 产品档案_状态设定 = 158;
        public const int 产品档案_导出商品 = 159;

        public const int 产品种类_查看种类 = 46;
        public const int 产品种类_编辑种类 = 47;
        public const int 产品种类_移除种类 = 48;

        public const int 品牌管理_查看品牌 = 49;
        public const int 品牌管理_编辑品牌 = 50;
        public const int 品牌管理_移除品牌 = 51;

        public const int 采购返利_查看返利 = 212;
        public const int 采购返利_创建返利方案 = 213;
        public const int 采购返利_移除返利方案 = 214;
        public const int 采购返利_状态设定 = 215;

        public const int 销售报表_商品销售明细月报表 = 95;
        public const int 销售报表_前台商品销售流水数据 = 96;
        public const int 销售报表_销售员日结报表 = 97;
        public const int 销售报表_门店日结报表 = 98;
        public const int 销售报表_进销存综合报表 = 99;

        public const int 赠品报表_年度赠品统计报表 = 100;
        public const int 赠品报表_月度赠品统计报表 = 101;
        public const int 用户管理_查看用户 = 109;
        public const int 用户管理_创建用户 = 110;
        public const int 用户管理_门店角色设置 = 111;
        public const int 用户管理_物理删除用户 = 139;

        public const int 角色管理_查看角色 = 112;
        public const int 角色管理_创建角色 = 113;
        public const int 角色管理_菜单设置 = 114;
        public const int 角色管理_权限设置 = 115;
        public const int 角色管理_状态设定 = 116;
        public const int 角色管理_物理删除角色 = 117;

        public const int 门店维护_查看门店 = 118;
        public const int 门店维护_创建门店 = 119;
        public const int 门店维护_营业状态 = 120;
        public const int 门店维护_后台状态 = 217;

        public const int 菜单管理_查看菜单信息 = 121;
        public const int 菜单管理_新增菜单信息 = 122;
        public const int 菜单管理_设为显示或隐藏 = 123;
        public const int 菜单管理_删除菜单信息 = 124;

        public const int 数据字典_查看字典 = 125;
        public const int 数据字典_创建字典 = 126;
        public const int 数据字典_状态设定 = 127;

        public const int 支付接口_查看接口 = 222;
        public const int 支付接口_支付宝配置 = 223;
        public const int 支付接口_微信支付配置 = 224;

        public const int 数据维护_初始数据 = 228;
        public const int 数据维护_数据迁移 = 229;
        public const int 数据维护_数据库收缩 = 230;
        public const int 数据维护_数据库备份 = 231;

        public const int 日志管理_查看日志 = 130;
        public const int 日志管理_删除日志 = 131;

        public const int 组织机构_查看机构或部门 = 136;
        public const int 组织机构_创建机构或部门 = 137;
        public const int 组织机构_删除机构或部门 = 138;
        public const int 组织机构_关闭或显示机构部门 = 216;

        public const int 批发商_查看批发商 = 161;
        public const int 批发商_编辑批发商 = 162;
        public const int 批发商_编辑分类 = 163;
        public const int 批发商_移除批发商 = 164;
        public const int 批发商_主帐号管理 = 165;

        public const int 产品变价_查看变价 = 169;
        public const int 产品变价_变价申请 = 170;
        public const int 产品变价_变价审批 = 171;
        public const int 产品变价_变价失效 = 172;
        public const int 产品变价_移除变价 = 173;

        public const int 批发价_查看批发价 = 174;
        public const int 批发价_批发价设定 = 175;
        public const int 批发价_批发价审批 = 176;
        public const int 批发价_批发价失效 = 177;
        public const int 批发价_移除批发价 = 178;

        public const int 商品传秤_查看传秤 = 179;
        public const int 商品传秤_电子秤配置 = 180;
        public const int 商品传秤_导出下发到电子秤 = 181;
        public const int 商品传秤_联机下发到电子秤 = 182;

        public const int 会员档案_查看会员 = 188;
        public const int 会员档案_创建会员 = 189;
        public const int 会员档案_记录回访 = 190;
        public const int 会员档案_状态设定 = 191;
        public const int 会员档案_导出会员 = 192;

        public const int 积分管理_查看积分 = 193;
        public const int 积分管理_编辑规则 = 194;
        public const int 积分管理_移除规则 = 195;

        public const int 活动推送_编辑推送 = 196;
        public const int 活动推送_移除推送 = 197;
        public const int 活动推送_开始推送 = 198;

        public const int 采购报表_商品采购明细汇总表 = 209;

        public const int 设备管理_查看设备 = 225;
        public const int 设备管理_状态设定 = 226;

        public const int 基本配置_编辑配置信息 = 227;
        public const int 基本配置_查看基本信息 = 221;

        public const int 订单管理_查看订单 = 237;
        public const int 订单管理_订单配送 = 238;
        public const int 订单管理_退换处理 = 239;

        #endregion

        #endregion

        #region 截止2015.10.20

        /*
        //深度1
        public const int 通知公告 = 1;
        public const int 邮件管理 = 2;
        public const int 采购管理 = 3;
        public const int 返利管理 = 4;

        public const int 市场营销 = 5;
        public const int 库存管理 = 6;
        public const int 统计报表 = 7;
        public const int 系统管理 = 8;

        //深度2
        public const int 通知公告_发布公告 = 9;
        public const int 通知公告_移除公告 = 10;
        public const int 通知公告_公告详情 = 11;

        public const int 邮件管理_写信 = 12;
        public const int 邮件管理_收邮件 = 13;
        public const int 邮件管理_已发送 = 14;
        public const int 邮件管理_转发 = 15;
        public const int 邮件管理_删除邮件 = 16;

        public const int 采购管理_供应商 = 17;
        public const int 采购管理_供应合同 = 24;
        public const int 采购管理_采购订单 = 25;
        public const int 采购管理_产品档案 = 39;
        public const int 采购管理_产品分类 = 44;
        public const int 采购管理_品牌管理 = 45;

        public const int 返利管理_查看采购返利方案 = 53;
        public const int 返利管理_新增采购返利方案 = 54;
        public const int 返利管理_删除采购返利方案 = 55;
        public const int 返利管理_查看返利计算 = 56;
        public const int 返利管理_新增返利计算 = 57;
        public const int 返利管理_设为已返利 = 58;
        public const int 返利管理_删除返利计算 = 59;

        public const int 市场营销_单品折扣 = 60;
        public const int 市场营销_捆绑销售 = 61;
        public const int 市场营销_组合促销 = 62;
        public const int 市场营销_买赠促销 = 63;
        public const int 市场营销_满元促销 = 64;

        public const int 库存管理_入库登记 = 85;
        public const int 库存管理_查看入库信息 = 86;
        public const int 库存管理_设为已验入库 = 87;
        public const int 库存管理_删除入库信息 = 88;
        public const int 库存管理_出库登记 = 89;
        public const int 库存管理_查看出库信息 = 90;
        public const int 库存管理_设为已审出库 = 91;
        public const int 库存管理_删除出库信息 = 92;
        public const int 库存管理_退货登记 = 141;
        public const int 库存管理_查看退货信息 = 142;
        public const int 库存管理_删除退货信息 = 143;
        public const int 库存管理_查看盘点信息 = 144;
        public const int 库存管理_盘点锁定 = 145;
        public const int 库存管理_初盘录入 = 146;
        public const int 库存管理_导入盘点文件 = 147;
        public const int 库存管理_盘点审核通过 = 148;
        public const int 库存管理_复盘修改 = 149;
        public const int 库存管理_盘点差异报表 = 150;
        public const int 库存管理_导出盘点差异报表 = 151;
        public const int 库存管理_入库审核 = 152;
        public const int 库存管理_出库审核 = 153;
        public const int 库存管理_退货审核 = 154;

        public const int 统计报表_销售报表 = 93;
        public const int 统计报表_赠品报表 = 94;

        public const int 系统管理_用户管理 = 102;
        public const int 系统管理_角色管理 = 103;
        public const int 系统管理_门店维护 = 104;
        public const int 系统管理_菜单管理 = 105;
        public const int 系统管理_数据字典 = 106;
        public const int 系统管理_支付接口 = 107;
        public const int 系统管理_数据维护 = 108;
        public const int 系统管理_日志管理 = 129;
        public const int 系统管理_组织机构 = 134;
        
        //深度3
        public const int 供应商_查看供应商信息 = 18;
        public const int 供应商_新增供应商信息 = 19;
        public const int 供应商_编辑商家分类 = 20;
        public const int 供应商_批量修改商家分类 = 21;
        public const int 供应商_删除供应商 = 22;
        public const int 供应商_下订单 = 23;

        public const int 供应合同_查看合同 = 26;
        public const int 供应合同_新增合同 = 27;
        public const int 供应合同_提交审批 = 28;
        public const int 供应合同_审批通过 = 29;
        public const int 供应合同_续签合同 = 30;
        public const int 供应合同_中止合同 = 31;
        public const int 供应合同_删除合同 = 32;

        public const int 采购订单_查看订单 = 33;
        public const int 采购订单_新增订单 = 34;
        public const int 采购订单_提交审批 = 35;
        public const int 采购订单_审批通过 = 36;
        public const int 采购订单_中止订货 = 37;
        public const int 采购订单_提醒商家配送 = 38;

        public const int 产品档案_查看产品信息 = 40;
        public const int 产品档案_新品建档 = 41;
        public const int 产品档案_组合新商品 = 42;
        public const int 产品档案_删除商品 = 43;
        public const int 产品档案_拆分新商品 = 140;

        public const int 产品分类_查看分类 = 46;
        public const int 产品分类_新增分类 = 47;
        public const int 产品分类_设为可用或停用 = 48;

        public const int 品牌管理_查看品牌 = 49;
        public const int 品牌管理_新增品牌 = 50;
        public const int 品牌管理_移除品牌 = 51;
        public const int 品牌管理_设为可用或停用 = 52;

        public const int 单品折扣_查看折扣信息 = 65;
        public const int 单品折扣_新增折扣信息 = 66;
        public const int 单品折扣_设为已过期 = 67;
        public const int 单品折扣_删除折扣信息 = 68;

        public const int 捆绑销售_查看捆绑信息 = 69;
        public const int 捆绑销售_新增捆绑销售 = 70;
        public const int 捆绑销售_设为已过期 = 71;
        public const int 捆绑销售_删除捆绑信息 = 72;

        public const int 组合促销_查看组合信息 = 73;
        public const int 组合促销_新增组合促销 = 74;
        public const int 组合促销_设为已过期 = 75;
        public const int 组合促销_删除组合信息 = 76;

        public const int 买赠促销_查看买赠信息 = 77;
        public const int 买赠促销_新增买赠促销 = 78;
        public const int 买赠促销_设为已过期 = 79;
        public const int 买赠促销_删除买赠信息 = 80;

        public const int 满元促销_查看满元信息 = 81;
        public const int 满元促销_新增满元促销 = 82;
        public const int 满元促销_设为已过期 = 83;
        public const int 满元促销_删除满元信息 = 84;

        public const int 销售报表_商品销售明细月报表 = 95;
        public const int 销售报表_品牌销售明细月报表 = 96;
        public const int 销售报表_供应商销售明细月报表 = 97;
        public const int 销售报表_供应商汇总月报 = 98;
        public const int 销售报表_大类销售月报 = 99;

        public const int 赠品报表_年度赠品统计报表 = 100;
        public const int 赠品报表_月度赠品统计报表 = 101;

        public const int 用户管理_查看用户信息 = 109;
        public const int 用户管理_新增用户信息 = 110;
        public const int 用户管理_设置门店角色 = 111;
        public const int 用户管理_物理删除用户 = 139;

        public const int 角色管理_查看角色信息 = 112;
        public const int 角色管理_新增角色信息 = 113;
        public const int 角色管理_菜单设置 = 114;
        public const int 角色管理_权限设置 = 115;
        public const int 角色管理_设为启用或关闭 = 116;
        public const int 角色管理_删除角色信息 = 117;

        public const int 门店维护_查看门店信息 = 118;
        public const int 门店维护_新增门店信息 = 119;
        public const int 门店维护_设为可用或停用 = 120;

        public const int 菜单管理_查看菜单信息 = 121;
        public const int 菜单管理_新增菜单信息 = 122;
        public const int 菜单管理_设为显示或隐藏 = 123;
        public const int 菜单管理_删除菜单信息 = 124;

        public const int 数据字典_查看字典信息 = 125;
        public const int 数据字典_编辑子项字典 = 126;
        public const int 数据字典_设为显示或关闭 = 127;

        public const int 日志管理_查看日志信息 = 130;
        public const int 日志管理_删除所选日志 = 131;
        public const int 日志管理_清除全部日志 = 132;

        public const int 组织机构_新增机构或部门 = 135;
        public const int 组织机构_查看机构或部门 = 136;
        public const int 组织机构_设为显示或关闭 = 137;
        public const int 组织机构_删除机构或部门 = 138;

         * */
        #endregion
    }
}
