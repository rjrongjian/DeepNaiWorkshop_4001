/*
Navicat MySQL Data Transfer

Source Server         : 淘宝客订单-测试库
Source Server Version : 50624
Source Host           : 192.168.0.1:3306
Source Database       : test

Target Server Type    : MYSQL
Target Server Version : 50624
File Encoding         : 65001

Date: 2018-01-24 15:32:44
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `tbk_tb_order`
-- ----------------------------
DROP TABLE IF EXISTS `tbk_tb_order`;
CREATE TABLE `tbk_tb_order` (
  `id` bigint(11) NOT NULL AUTO_INCREMENT,
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `clickTime` datetime NOT NULL COMMENT '点击时间',
  `goodInfo` varchar(256) NOT NULL COMMENT '商品信息',
  `goodId` varchar(32) NOT NULL COMMENT '商品id',
  `wangWangName` varchar(128) NOT NULL COMMENT '旺旺名称',
  `shopName` varchar(64) NOT NULL COMMENT '店铺名称',
  `goodCount` int(6) NOT NULL COMMENT '商品数量',
  `goodUnitPrice` double(9,2) NOT NULL COMMENT '商品单价',
  `orderStatus` varchar(32) NOT NULL COMMENT '订单状态',
  `orderType` varchar(16) DEFAULT NULL COMMENT '订单类型',
  `incomeRatio` double(6,2) DEFAULT NULL COMMENT '收入比率（单位：%）',
  `dividedIntoRatio` double(6,2) DEFAULT NULL COMMENT '分成比率 （单位：%）',
  `paymentAmount` double(8,2) DEFAULT NULL COMMENT '付款金额',
  `effect` double(8,2) DEFAULT NULL COMMENT '效果预估',
  `settlementAmount` double(8,2) DEFAULT NULL COMMENT '结算金额',
  `estimatedIncome` double(8,2) DEFAULT NULL COMMENT '预估收入',
  `settlingTime` datetime DEFAULT NULL COMMENT '结算时间',
  `commissionRate` double(6,2) DEFAULT NULL COMMENT '佣金比率（单位：%）',
  `commissionAmount` double(8,2) DEFAULT NULL COMMENT '佣金金额',
  `subsidyRatio` double(5,2) DEFAULT NULL COMMENT '补贴比率（单位：%）',
  `subsidies` double(8,2) DEFAULT NULL COMMENT '补贴金额',
  `subsidiesType` varchar(16) DEFAULT NULL COMMENT '补贴类型',
  `transactionPlatform` varchar(16) DEFAULT NULL COMMENT '成交平台',
  `thirdPartyServiceSource` varchar(16) DEFAULT NULL COMMENT '第三方服务来源',
  `orderId` varchar(32) NOT NULL COMMENT '订单编号',
  `catName` varchar(32) DEFAULT NULL COMMENT '类目名称',
  `mediaSourceId` varchar(32) NOT NULL COMMENT '来源媒体Id',
  `mediaSourceName` varchar(64) DEFAULT NULL COMMENT '来源媒体名称',
  `advertisingId` varchar(32) NOT NULL COMMENT '广告位id',
  `advertisingName` varchar(64) DEFAULT NULL COMMENT '广告位名称',
  `tbUnionAccountId` varchar(64) NOT NULL COMMENT 'pid第一段(订单中没有这个值，所以需要在后台上传时指定这个值)',
  `pid` varchar(64) NOT NULL COMMENT '用于识别此订单是由哪个用户拉来的',
  PRIMARY KEY (`id`),
  KEY `parnterTbOrder_pid` (`pid`) USING BTREE,
  KEY `parnterTbOrder_createTime` (`createTime`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='淘宝联盟订单详情（注意同一个订单编号中可能存在多个商品，所以单靠订单编号不能做唯一索引,注意：购物车中同一商品（颜色不同），会产生相同的订单编号的两条相同的数据）';

-- ----------------------------
-- Records of tbk_tb_order
-- ----------------------------
