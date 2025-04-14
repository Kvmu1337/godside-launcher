-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Apr 14, 2025 at 01:20 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `godside`
--

-- --------------------------------------------------------

--
-- Table structure for table `loginlauncher_users`
--

CREATE TABLE `loginlauncher_users` (
  `username` varchar(50) NOT NULL DEFAULT '',
  `password` varchar(50) DEFAULT 'DefaultPass',
  `externalip` varchar(50) DEFAULT '127.0.0.1',
  `IsAdmin` bit(1) DEFAULT b'0',
  `IsAllowed` bit(1) DEFAULT b'0',
  `discordid` varchar(50) DEFAULT NULL,
  `rank` varchar(255) NOT NULL,
  `validLicense` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Dumping data for table `loginlauncher_users`
--

INSERT INTO `loginlauncher_users` (`username`, `password`, `externalip`, `IsAdmin`, `IsAllowed`, `discordid`, `rank`, `validLicense`) VALUES
('test', 'hs', '127.0.0.1', b'1', b'1', '00', 'Admin', '2025-04-30');

-- --------------------------------------------------------

--
-- Table structure for table `settings`
--

CREATE TABLE `settings` (
  `version` varchar(255) NOT NULL,
  `detection` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `settings`
--

INSERT INTO `settings` (`version`, `detection`) VALUES
('1.0.0', 'Undetectable');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `loginlauncher_users`
--
ALTER TABLE `loginlauncher_users`
  ADD PRIMARY KEY (`username`);

--
-- Indexes for table `settings`
--
ALTER TABLE `settings`
  ADD PRIMARY KEY (`version`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
