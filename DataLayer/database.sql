CREATE DATABASE  IF NOT EXISTS `cvutdb` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `cvutdb`;
-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: localhost    Database: cvutdb
-- ------------------------------------------------------
-- Server version	5.7.21-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `applications`
--

DROP TABLE IF EXISTS `applications`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `applications` (
  `IdApplication` int(11) NOT NULL AUTO_INCREMENT,
  `IdApplicationType` int(11) NOT NULL,
  `IsRunning` tinyint(1) NOT NULL,
  `Name` varchar(50) CHARACTER SET utf8mb4 DEFAULT NULL,
  PRIMARY KEY (`IdApplication`),
  KEY `FK_Automats_AutomatTypes` (`IdApplicationType`),
  CONSTRAINT `FK_Automats_AutomatTypes` FOREIGN KEY (`IdApplicationType`) REFERENCES `applicationtype` (`IdApplicationType`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `applications`
--

LOCK TABLES `applications` WRITE;
/*!40000 ALTER TABLE `applications` DISABLE KEYS */;
INSERT INTO `applications` VALUES (0,0,1,'V chodbe'),(1,1,1,'Recepcia'),(2,2,0,'Hl. jedalen');
/*!40000 ALTER TABLE `applications` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `applicationtype`
--

DROP TABLE IF EXISTS `applicationtype`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `applicationtype` (
  `IdApplicationType` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
  PRIMARY KEY (`IdApplicationType`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `applicationtype`
--

LOCK TABLES `applicationtype` WRITE;
/*!40000 ALTER TABLE `applicationtype` DISABLE KEYS */;
INSERT INTO `applicationtype` VALUES (0,'Inform'),(1,'Order'),(2,'Serve');
/*!40000 ALTER TABLE `applicationtype` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `clients`
--

DROP TABLE IF EXISTS `clients`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `clients` (
  `IdClient` int(11) NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
  `LastName` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
  `Balance` double NOT NULL,
  `CardNumber` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`IdClient`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clients`
--

LOCK TABLES `clients` WRITE;
/*!40000 ALTER TABLE `clients` DISABLE KEYS */;
INSERT INTO `clients` VALUES (1,'Juraj','Matejka',115.52,'1'),(2,'Mato','Vyskocany',853.11,'2'),(3,'Jozko','Pravy',466.92,'3'),(4,'Kristof','Zeleny',962.63,'4'),(7,'Stefan','Jurosik',180,'0'),(8,'Martin','Lichy',0,'5'),(9,'Juraj','Nejaky',0,'8'),(10,'Filip','Gonda',0,'8'),(11,'Tomas','Zigo',0,'5'),(12,'Martin','Plaffy',0,'123'),(13,'Peter','Podavka',123.85,'74'),(14,'Karol','Kristl',0,'4');
/*!40000 ALTER TABLE `clients` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `deserts`
--

DROP TABLE IF EXISTS `deserts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `deserts` (
  `IdDesert` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `Description` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`IdDesert`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `deserts`
--

LOCK TABLES `deserts` WRITE;
/*!40000 ALTER TABLE `deserts` DISABLE KEYS */;
INSERT INTO `deserts` VALUES (1,'Puding','So slahackou'),(2,'Koláč','Podla vyberu'),(3,'Zmrzlina','Čučoriedková'),(4,'Gríska','S posipanou čokoládou');
/*!40000 ALTER TABLE `deserts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `devices`
--

DROP TABLE IF EXISTS `devices`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `devices` (
  `IdDevice` int(11) NOT NULL AUTO_INCREMENT,
  `IdApplication` int(11) NOT NULL,
  `IP` varchar(50) NOT NULL,
  `Port` int(11) DEFAULT NULL,
  PRIMARY KEY (`IdDevice`),
  KEY `FK_Devices_Automats` (`IdApplication`),
  CONSTRAINT `FK_Devices_Automats` FOREIGN KEY (`IdApplication`) REFERENCES `applications` (`IdApplication`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `devices`
--

LOCK TABLES `devices` WRITE;
/*!40000 ALTER TABLE `devices` DISABLE KEYS */;
INSERT INTO `devices` VALUES (0,0,'10.183.227.131',15000),(1,1,'10.183.227.138',15000),(2,2,'127.0.0.1',15003),(3,2,'127.0.0.1',15004);
/*!40000 ALTER TABLE `devices` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `meals`
--

DROP TABLE IF EXISTS `meals`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `meals` (
  `IdMeal` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `Description` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`IdMeal`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `meals`
--

LOCK TABLES `meals` WRITE;
/*!40000 ALTER TABLE `meals` DISABLE KEYS */;
INSERT INTO `meals` VALUES (1,'Rezeň','Vyprazany s hranolkami'),(2,'Kurca','Bez plnky s kašou'),(3,'Čína','Kuracie mäso so zeleninou'),(4,'Rizoto','Bravčové so zeleninou a uhorkou');
/*!40000 ALTER TABLE `meals` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `menu`
--

DROP TABLE IF EXISTS `menu`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `menu` (
  `ForDate` date NOT NULL,
  `IdMenu` int(11) NOT NULL AUTO_INCREMENT,
  `IdSoup` int(11) DEFAULT NULL,
  `IdMeal` int(11) DEFAULT NULL,
  `IdDesert` int(11) DEFAULT NULL,
  `Price` float DEFAULT NULL,
  PRIMARY KEY (`IdMenu`,`ForDate`),
  KEY `IdDesert` (`IdDesert`),
  KEY `IdMeal` (`IdMeal`),
  KEY `IdSoup` (`IdSoup`),
  KEY `IdMenu` (`IdMenu`),
  KEY `ForDate` (`ForDate`),
  CONSTRAINT `IdDesert` FOREIGN KEY (`IdDesert`) REFERENCES `deserts` (`IdDesert`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `IdMeal` FOREIGN KEY (`IdMeal`) REFERENCES `meals` (`IdMeal`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `IdSoup` FOREIGN KEY (`IdSoup`) REFERENCES `soups` (`IdSoup`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `menu`
--

LOCK TABLES `menu` WRITE;
/*!40000 ALTER TABLE `menu` DISABLE KEYS */;
INSERT INTO `menu` VALUES ('2018-04-22',1,1,1,1,2),('2018-04-22',2,3,2,2,1),('2018-04-23',3,1,1,1,5),('2018-04-23',4,2,2,1,3),('2018-04-23',5,1,1,1,4),('2018-04-23',6,1,2,1,5),('2018-04-23',7,2,1,1,6),('2018-04-23',8,1,1,1,1),('2018-04-23',9,1,2,1,2),('2018-04-23',10,1,1,2,3),('2018-04-23',11,2,1,1,4),('2018-04-23',12,1,1,2,6),('2018-04-24',13,2,1,1,7),('2018-04-24',14,1,1,2,6),('2018-04-23',15,1,1,2,5),('2018-04-23',16,1,1,1,855);
/*!40000 ALTER TABLE `menu` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `orders`
--

DROP TABLE IF EXISTS `orders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `orders` (
  `IdOrder` int(11) NOT NULL AUTO_INCREMENT,
  `ForDate` date NOT NULL,
  `IdMenu` int(11) NOT NULL,
  `IdClient` int(11) NOT NULL,
  `Vydane` datetime DEFAULT NULL,
  PRIMARY KEY (`IdOrder`),
  KEY `FK_ORDERS_CLIENTS` (`IdClient`),
  KEY `FK_ORDERS_MENU_idx` (`ForDate`,`IdMenu`),
  CONSTRAINT `FK_ORDERS_CLIENTS` FOREIGN KEY (`IdClient`) REFERENCES `clients` (`IdClient`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_ORDERS_MENU` FOREIGN KEY (`ForDate`, `IdMenu`) REFERENCES `menu` (`ForDate`, `IdMenu`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orders`
--

LOCK TABLES `orders` WRITE;
/*!40000 ALTER TABLE `orders` DISABLE KEYS */;
INSERT INTO `orders` VALUES (1,'2018-04-22',1,2,NULL),(4,'2018-04-22',2,4,NULL),(5,'2018-04-23',4,3,'2018-04-23 05:12:02'),(10,'2018-04-23',10,2,'2018-04-23 22:49:28'),(11,'2018-04-23',4,1,'2018-04-23 22:49:22'),(12,'2018-04-23',4,1,NULL),(13,'2018-04-23',4,1,NULL),(14,'2018-04-23',4,7,NULL),(15,'2018-04-23',5,7,NULL),(16,'2018-04-23',4,7,NULL);
/*!40000 ALTER TABLE `orders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `soups`
--

DROP TABLE IF EXISTS `soups`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `soups` (
  `IdSoup` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `Description` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`IdSoup`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `soups`
--

LOCK TABLES `soups` WRITE;
/*!40000 ALTER TABLE `soups` DISABLE KEYS */;
INSERT INTO `soups` VALUES (1,'Paradajkova','Cervena s cestovinou'),(2,'Vývar','Kuraci s rezancami'),(3,'Kapusnica','Stredne kyslá'),(4,'Brokolicová','Zelena');
/*!40000 ALTER TABLE `soups` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'cvutdb'
--
/*!50003 DROP PROCEDURE IF EXISTS `GetServedOrders` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetServedOrders`(
IN Count int
)
BEGIN
SELECT Orders.IdOrder, Orders.Vydane, Clients.IdClient, Clients.LastName, Clients.FirstName, Soups.*, Meals.*, Deserts.* 
	FROM Orders
		LEFT JOIN Clients ON Orders.IdClient = Clients.IdClient
		LEFT JOIN Menu ON Orders.IdMenu = Menu.IdMenu
		LEFT JOIN Soups ON Menu.IdSoup = Soups.IdSoup
		LEFT JOIN Meals ON Menu.IdMeal = Meals.IdMeal
		LEFT JOIN Deserts ON Menu.IdDesert = Deserts.IdDesert
		where vydane is not null ORDER BY Vydane DESC
        LIMIT Count;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `ServeOrder` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `ServeOrder`(
  IN ClientId int
)
BEGIN
	DECLARE tmpOrderId int;
	SET tmpOrderId =
      (SELECT IdOrder FROM Orders
        WHERE IdClient = ClientId
		  AND Vydane is null
          AND ForDate = CAST(CURRENT_TIMESTAMP AS DATE)
	  );
	UPDATE Orders SET Vydane = CURRENT_TIMESTAMP WHERE IdOrder = tmpOrderId;
	SELECT Orders.IdMenu, Orders.ForDate, Soups.*, Meals.*, Deserts.*  FROM Orders
		LEFT JOIN Menu ON Orders.IdMenu = Menu.IdMenu
		LEFT JOIN Soups ON Menu.IdSoup = Soups.IdSoup
		LEFT JOIN Meals ON Menu.IdMeal = Meals.IdMeal
		LEFT JOIN Deserts ON Menu.IdDesert = Deserts.IdDesert
		WHERE IdOrder = tmpOrderId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-04-23 23:33:31
