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
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-12-20 17:09:06