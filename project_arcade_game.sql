-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Oct 25, 2021 at 12:49 PM
-- Server version: 10.4.21-MariaDB
-- PHP Version: 8.0.11

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `project_arcade_game`
--

-- --------------------------------------------------------

--
-- Table structure for table `multiplayer`
--

CREATE TABLE IF NOT EXISTS `multiplayer` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `name_player_one` varchar(255) NOT NULL,
  `name_player_two` varchar(255) NOT NULL,
  `score` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `multiplayer`
--

INSERT INTO `multiplayer` (`ID`, `name_player_one`, `name_player_two`, `score`) VALUES
(1, 'Khalid', 'Teshale', 150),
(2, 'Steven', 'Marietta', 149),
(3, 'Roan', 'Martin', 149),
(4, 'Jan', 'Bauke', 145);

-- --------------------------------------------------------

--
-- Table structure for table `singleplayer`
--

CREATE TABLE IF NOT EXISTS `singleplayer` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `score` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `singleplayer`
--

INSERT INTO `singleplayer` (`ID`, `name`, `score`) VALUES
(1, 'Khalido', 180),
(2, 'Steven', 179),
(3, 'Roan', 178),
(4, 'Teshale', 150),
(5, 'Marietta', 150),
(6, 'Jos', 130),
(7, 'Sterre', 120),
(8, 'Liban', 115),
(9, 'Jorn', 101),
(10, 'Anna', 100);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
