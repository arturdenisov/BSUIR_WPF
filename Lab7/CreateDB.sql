USE [master]
GO
 /************************Создание БД**********************/
CREATE DATABASE [WPF_Lab7_Artur] ON  PRIMARY 
( NAME = N'WPF_LAB7_Artur_Data', FILENAME = N'd:\DENISOV_WPF_LAB7_Data.MDF' , SIZE = 6000KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
 LOG ON 
( NAME = N'WPF_LAB7_Artur_Log', FILENAME = N'd:\DENISOV_WPF_LAB7_Log.LDF' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

USE [WPF_Lab7_Artur]
GO
   /************************Создание таблиц*******************/
 CREATE TABLE Cars (
        ID_car	             int IDENTITY(1,1),
        Model			     varchar(50) NOT NULL,
        Price				 int NULL,
        YearOfProduction     int NULL,
		Quantity			 int Not NULL		
 )
GO
   /************************Наполнение примерами*******************/
INSERT INTO Cars(Model, Price, YearOfProduction, Quantity)
VALUES ('Audi', 8150, 2009, 13)
INSERT INTO Cars(Model, Price, YearOfProduction, Quantity)
VALUES ('Audi', 11000, 2014-1-1, 25)
INSERT INTO Cars(Model, Price, YearOfProduction, Quantity)
VALUES ('Audi', 7200, 2009, 76)
INSERT INTO Cars(Model, Price, YearOfProduction, Quantity)
VALUES ('Opel', 9320, 2012, 8)
INSERT INTO Cars(Model, Price, YearOfProduction, Quantity)
VALUES ('Opel', 9200, 2013-1-1, 14)
INSERT INTO Cars(Model, Price, YearOfProduction, Quantity)
VALUES ('Opel', 11080, 2017-1-1, 21)
INSERT INTO Cars(Model, Price, YearOfProduction, Quantity)
VALUES ('Mercedes', 9700, 2009-1-1, 23)
INSERT INTO Cars(Model, Price, YearOfProduction, Quantity)
VALUES ('Mercedes', 15000, 2012-1-1, 61)
INSERT INTO Cars(Model, Price, YearOfProduction, Quantity)
VALUES ('Mercedes', 18100, 2017-1-1, 108)

