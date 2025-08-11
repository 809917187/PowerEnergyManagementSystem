-- MySQL dump 10.13  Distrib 8.0.40, for Win64 (x86_64)
--
-- Host: localhost    Database: gq
-- ------------------------------------------------------
-- Server version	8.0.40

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `device_ems_ps_binding_info`
--

DROP TABLE IF EXISTS `device_ems_ps_binding_info`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `device_ems_ps_binding_info` (
  `device_sn` varchar(45) NOT NULL,
  `ems_sn` varchar(45) DEFAULT NULL,
  `ps_id` int DEFAULT NULL,
  `device_type` int DEFAULT NULL,
  PRIMARY KEY (`device_sn`),
  UNIQUE KEY `devSn_UNIQUE` (`device_sn`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `device_ems_ps_binding_info`
--

LOCK TABLES `device_ems_ps_binding_info` WRITE;
/*!40000 ALTER TABLE `device_ems_ps_binding_info` DISABLE KEYS */;
INSERT INTO `device_ems_ps_binding_info` VALUES ('duikong01','emssn12345678',2,3),('N9_PCS','emssn12345678',2,5),('PCC001','emssn12345678',2,1);
/*!40000 ALTER TABLE `device_ems_ps_binding_info` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `power_station`
--

DROP TABLE IF EXISTS `power_station`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `power_station` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(45) DEFAULT NULL,
  `owner` varchar(45) DEFAULT NULL,
  `phone` varchar(20) DEFAULT NULL,
  `installed_power` float DEFAULT NULL,
  `installed_capacity` float DEFAULT NULL,
  `start_time` datetime DEFAULT NULL,
  `country` varchar(45) DEFAULT NULL,
  `state` varchar(45) DEFAULT NULL,
  `city` varchar(45) DEFAULT NULL,
  `region` varchar(45) DEFAULT NULL,
  `location_details` varchar(255) DEFAULT NULL,
  `longitude` float DEFAULT NULL,
  `latitude` float DEFAULT NULL,
  `transformer_capacity` float DEFAULT NULL,
  `transformer_info` varchar(45) DEFAULT NULL,
  `network_info` varchar(45) DEFAULT NULL,
  `installer` varchar(45) DEFAULT NULL,
  `installer_phone` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `power_station`
--

LOCK TABLES `power_station` WRITE;
/*!40000 ALTER TABLE `power_station` DISABLE KEYS */;
INSERT INTO `power_station` VALUES (2,'dz1','Owner','18888888888',11.11,22.22,'0001-01-01 00:00:00',NULL,NULL,NULL,NULL,NULL,0,0,0,NULL,'1',NULL,NULL),(3,'dz2','Owner','18888888888',11.11,22.22,'0001-01-01 00:00:00','中国','江苏','苏州','江苏','中国江苏省daqiao',0,0,0,NULL,'0',NULL,NULL),(4,'dz3','Owner11','18888888888',11.11,22.22,'2024-12-05 00:00:00','中国','江苏','苏州','江都','DQ',88,66,111,'333','1','zzz','186250xxxxx');
/*!40000 ALTER TABLE `power_station` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `power_station_bind_user`
--

DROP TABLE IF EXISTS `power_station_bind_user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `power_station_bind_user` (
  `id` int NOT NULL AUTO_INCREMENT,
  `power_station_id` int NOT NULL,
  `user_id` int NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `power_station_bind_user`
--

LOCK TABLES `power_station_bind_user` WRITE;
/*!40000 ALTER TABLE `power_station_bind_user` DISABLE KEYS */;
/*!40000 ALTER TABLE `power_station_bind_user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `power_station_images`
--

DROP TABLE IF EXISTS `power_station_images`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `power_station_images` (
  `id` int NOT NULL AUTO_INCREMENT,
  `power_station_id` int NOT NULL,
  `image_path` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `power_station_images`
--

LOCK TABLES `power_station_images` WRITE;
/*!40000 ALTER TABLE `power_station_images` DISABLE KEYS */;
INSERT INTO `power_station_images` VALUES (10,4,'images/stationImages/981f3374-d871-477d-b7cd-b48b46ecdb4c.png'),(11,4,'images/stationImages/c66088a6-0628-404f-bc3d-55dfc768002b.png');
/*!40000 ALTER TABLE `power_station_images` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `power_station_install_images`
--

DROP TABLE IF EXISTS `power_station_install_images`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `power_station_install_images` (
  `id` int NOT NULL AUTO_INCREMENT,
  `power_station_id` varchar(45) NOT NULL,
  `image_path` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `power_station_install_images`
--

LOCK TABLES `power_station_install_images` WRITE;
/*!40000 ALTER TABLE `power_station_install_images` DISABLE KEYS */;
INSERT INTO `power_station_install_images` VALUES (7,'4','images/stationImages/e57d9f20-d98d-4adf-aad6-b0eaf1dda5ab.png'),(8,'4','images/stationImages/46d6b92e-9cee-40c1-bd36-1acef7465acb.png');
/*!40000 ALTER TABLE `power_station_install_images` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `power_station_map_price_template`
--

DROP TABLE IF EXISTS `power_station_map_price_template`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `power_station_map_price_template` (
  `power_station_id` int NOT NULL,
  `price_template_id` int DEFAULT NULL,
  PRIMARY KEY (`power_station_id`),
  UNIQUE KEY `power_station_id_UNIQUE` (`power_station_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `power_station_map_price_template`
--

LOCK TABLES `power_station_map_price_template` WRITE;
/*!40000 ALTER TABLE `power_station_map_price_template` DISABLE KEYS */;
INSERT INTO `power_station_map_price_template` VALUES (2,5),(4,5);
/*!40000 ALTER TABLE `power_station_map_price_template` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `price_template`
--

DROP TABLE IF EXISTS `price_template`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `price_template` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `tag` varchar(45) DEFAULT NULL,
  `create_time` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `creater_id` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `price_template`
--

LOCK TABLES `price_template` WRITE;
/*!40000 ALTER TABLE `price_template` DISABLE KEYS */;
INSERT INTO `price_template` VALUES (5,'Price Template1','Tag1','2024-12-20 05:29:17',3),(6,'Price Template2','','2024-12-20 05:36:01',3),(7,'Price Template3','Tag3','2024-12-20 05:36:53',3),(8,'Price Template4_1_1','Tag444_1_1','2024-12-20 05:56:37',3);
/*!40000 ALTER TABLE `price_template` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `price_template_bug_salse`
--

DROP TABLE IF EXISTS `price_template_bug_salse`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `price_template_bug_salse` (
  `template_id` int NOT NULL,
  `time_frame_type_code` int NOT NULL,
  `buy_price` decimal(10,5) NOT NULL,
  `salse_price` decimal(10,5) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `price_template_bug_salse`
--

LOCK TABLES `price_template_bug_salse` WRITE;
/*!40000 ALTER TABLE `price_template_bug_salse` DISABLE KEYS */;
INSERT INTO `price_template_bug_salse` VALUES (5,1,1.20000,3.40000),(5,2,5.60000,7.80000),(5,3,9.00000,10.50000),(5,4,11.60000,12.90000),(5,5,15.60000,19.54500),(6,1,0.00000,0.00000),(6,2,0.00000,0.00000),(6,3,3.30000,5.60000),(6,4,0.00000,0.00000),(6,5,0.00000,0.00000),(7,1,0.00000,0.00000),(7,2,3.60000,6.60000),(7,3,0.00000,0.00000),(7,4,6.80000,10.95000),(7,5,0.00000,0.00000),(0,1,0.00000,0.00000),(0,2,0.00000,0.00000),(0,3,0.00000,0.00000),(0,4,3.30000,6.30000),(0,5,0.00000,0.00000),(8,1,3.65000,6.68950),(8,2,2.20000,9.60000),(8,3,3.60000,8.80000),(8,4,3.30000,6.30000),(8,5,0.00000,0.00000);
/*!40000 ALTER TABLE `price_template_bug_salse` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `price_template_frame_info`
--

DROP TABLE IF EXISTS `price_template_frame_info`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `price_template_frame_info` (
  `template_id` int NOT NULL,
  `start_time` double NOT NULL,
  `end_time` double NOT NULL,
  `time_frame_type` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `price_template_frame_info`
--

LOCK TABLES `price_template_frame_info` WRITE;
/*!40000 ALTER TABLE `price_template_frame_info` DISABLE KEYS */;
INSERT INTO `price_template_frame_info` VALUES (5,0,60,1),(5,60,120,2),(5,120,180,3),(5,180,255,4),(5,255,1439,5),(6,0,1439,3),(7,0,720,2),(7,720,1439,4),(0,0,1439,4),(8,0,570,1),(8,570,615,2),(8,615,705,3),(8,705,870,1);
/*!40000 ALTER TABLE `price_template_frame_info` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `project_info`
--

DROP TABLE IF EXISTS `project_info`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `project_info` (
  `id` int NOT NULL AUTO_INCREMENT,
  `customer_project_number` varchar(255) NOT NULL,
  `customer_project_name` varchar(255) NOT NULL,
  `my_project_number` varchar(255) NOT NULL,
  `my_project_name` varchar(255) NOT NULL,
  `project_type` varchar(255) NOT NULL,
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `project_info`
--

LOCK TABLES `project_info` WRITE;
/*!40000 ALTER TABLE `project_info` DISABLE KEYS */;
INSERT INTO `project_info` VALUES (1,'11111111','aaaaaaa','222222222','bbbbbbbbb','未分类','2025-02-17 20:10:50'),(2,'22222','cccc','33333','dd','未分类','2025-02-17 20:19:37'),(3,'11111111dd','aaaaaaa','222222222','bbbbbbbbb','未分类','2025-02-17 20:24:24'),(4,'11111111ddcc','aaaaaaa','222222222','bbbbbbbbb','未分类','2025-02-17 20:24:58'),(5,'11111111ee','aaaaaaa','222222222','bbbbbbbbb','未分类','2025-02-17 20:25:52'),(6,'11111111gg','aaaaaaa','22222222233','bbbbbbbbb','未分类','2025-02-17 20:26:34');
/*!40000 ALTER TABLE `project_info` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `role`
--

DROP TABLE IF EXISTS `role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `role` (
  `role_code` int NOT NULL AUTO_INCREMENT,
  `role_name` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`role_code`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `role`
--

LOCK TABLES `role` WRITE;
/*!40000 ALTER TABLE `role` DISABLE KEYS */;
INSERT INTO `role` VALUES (1,'管理员'),(2,'普通用户');
/*!40000 ALTER TABLE `role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user` (
  `id` int NOT NULL AUTO_INCREMENT,
  `email` varchar(320) DEFAULT NULL,
  `password` varchar(255) DEFAULT NULL,
  `name` varchar(45) DEFAULT NULL,
  `phone_number` varchar(45) DEFAULT NULL,
  `role_code` varchar(45) DEFAULT NULL,
  `create_time` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `is_delete` bit(1) DEFAULT b'0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES (2,'admin','AQAAAAEAACcQAAAAEAUWVeUjzTndkWDb7FNmcdC9isiXK4VvBcGb671SVCEIPXXo+LeB6e0Wuruz9Yl7JA==','zw',NULL,'2','2024-11-25 07:17:09',_binary '\0'),(3,'809917187@qq.com','AQAAAAEAACcQAAAAEHPpLsmCIx+4CdCu9qStNFbNLoRkQvlaYalCfTOvvXneQ79o1MAby23gcSrsPco4yw==','zwzw','18688888888','1','2024-11-25 08:01:17',_binary '\0');
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-08-08 17:23:16
