/*
Navicat MySQL Data Transfer

Source Server         : 127
Source Server Version : 50553
Source Host           : localhost:3306
Source Database       : dball

Target Server Type    : MYSQL
Target Server Version : 50553
File Encoding         : 65001

Date: 2019-10-29 19:13:49
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for db_liansai
-- ----------------------------
DROP TABLE IF EXISTS `db_liansai`;
CREATE TABLE `db_liansai` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `lid` int(11) DEFAULT NULL,
  `name` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of db_liansai
-- ----------------------------
INSERT INTO `db_liansai` VALUES ('1', '123', '222');
INSERT INTO `db_liansai` VALUES ('2', '123', '222');
INSERT INTO `db_liansai` VALUES ('3', '123', '222');

-- ----------------------------
-- Table structure for db_odds
-- ----------------------------
DROP TABLE IF EXISTS `db_odds`;
CREATE TABLE `db_odds` (
  `id` int(11) NOT NULL,
  `stime` varchar(50) DEFAULT NULL,
  `Lid` int(11) DEFAULT NULL,
  `Ls` varchar(50) DEFAULT NULL,
  `Q1` varchar(50) DEFAULT NULL,
  `Q2` varchar(50) DEFAULT NULL,
  `HC` varchar(4) DEFAULT NULL,
  `pan` varchar(10) DEFAULT NULL,
  `Q1adds` decimal(3,3) DEFAULT NULL,
  `Q2adds` decimal(3,3) DEFAULT NULL,
  `addtime` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of db_odds
-- ----------------------------
INSERT INTO `db_odds` VALUES ('0', null, null, null, '1', '1', null, null, null, null, null);

-- ----------------------------
-- Table structure for db_team
-- ----------------------------
DROP TABLE IF EXISTS `db_team`;
CREATE TABLE `db_team` (
  `id` int(11) NOT NULL,
  `qid` int(11) DEFAULT NULL,
  `qname` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of db_team
-- ----------------------------
