USE master
GO

-- Create database
CREATE DATABASE MeetingDatabase
GO

USE MeetingDatabase
GO

-- Create tables
CREATE TABLE Meeting
(
	IDMeeting int IDENTITY PRIMARY KEY NOT NULL,
	[Date] datetime2 NOT NULL,
	Place nvarchar(20) NOT NULL,
	[Description] nvarchar(120) NOT NULL,
	Title nvarchar(40) NOT NULL,
	ClientID int NOT NULL
)
GO

CREATE TABLE Client
(
	IDClient int IDENTITY PRIMARY KEY NOT NULL,
	Name nvarchar(40) NOT NULL,
	Surname nvarchar(40) NOT NULL,
	Email nvarchar(80) NOT NULL,
	Phone nvarchar(20) NOT NULL
)
GO

-- Helper Function
CREATE FUNCTION GenerateDate(@numDays AS int, @hour AS int)
RETURNS DATETIME2
AS
BEGIN
	DECLARE @date AS date
	DECLARE @dateTime AS datetime2

	SET @date = DATEADD(DAY, @numDays, GETDATE())

	IF DATENAME(DW, @date) = 'Sunday'
	BEGIN
		SET @date = DATEADD(DAY, 1, @date)
	END
	IF DATENAME(DW, @date) = 'Saturday'
	BEGIN
		SET @date = DATEADD(DAY, 2, @date)
	END

	SET @dateTime = TRY_CONVERT(datetime2, @date, 104)
	RETURN DATEADD(HOUR, @hour, @dateTime)
END
GO

-- Insert data
INSERT INTO Client VALUES
('Randle', 'McMurphy', 'randle.mcmurphy@example.com', '+44 20 7946 0062'),
('Norman', 'Bates', 'norman.bates@example.com', '+44 20 7946 0708'),
('Wednesday', 'Addams', 'wednesday.addams@example.com', '+44 20 7946 0795'),
('Jacques', 'Clouseau', 'jacques.clouseau@example.com', '+44 20 7946 0778'),
('Inigo', 'Montoya', 'inigo.montoya@example.com', '+44 20 7946 0594'),
('Ethan', 'Hunt', 'ethan.hunt@example.com', '+44 20 7946 0105'),
('Corporal', 'Hicks', 'corporal.hicks@example.com', '+1 202 555 0107'),
('Roy', 'Batty', 'roy.batty@example.com', '+1 202 555 0179'),
('Lisbeth', 'Salander', 'lisbeth.salander@example.com', '+1 202 555 0106'),
('Ellen', 'Ripley', 'ellen.ripley@example.com', '+1 202 555 0131'),
('Frank', 'Drebin', 'frank.drebin@example.com', '+1 202 555 0189')
GO

INSERT INTO Meeting ([Date], Place, [Description], Title, ClientID) VALUES
(dbo.GenerateDate(0,12), 'Room 1', 'Client interested in our POS software', 'POS Software', 1),
(dbo.GenerateDate(2,10), 'Room 7', 'Second meeting for website design', 'Website design', 2),
(dbo.GenerateDate(4,9), 'Room 4', 'Student from MIT, job interview, Java', 'Job interview', 3),
(dbo.GenerateDate(6,15), 'Room 3', 'Client interested in our POS software', 'POS Software', 4),
(dbo.GenerateDate(8,18), 'Room 4', 'Second meeting about implementation', 'POS Software', 5),
(dbo.GenerateDate(10,18), 'Room 5', 'Second meeting for website about dogs', 'Website', 6),
(dbo.GenerateDate(12,9), 'Room 1', 'Student from MIT, job interview, C#', 'Job interview', 7),
(dbo.GenerateDate(14,9), 'Room 7', 'Student from MIT, job interview, Java', 'Job interview', 8),
(dbo.GenerateDate(16,20), 'Room 6', 'Website for hotel in Istria', 'Website', 9),
(dbo.GenerateDate(18,19), 'Room 1', 'Client interested in our POS software', 'POS Software', 10),
(dbo.GenerateDate(20,8), 'Room 4', 'Android application for bike store', 'Mobile app development', 11)
GO

-- Drop helper function
DROP FUNCTION dbo.GenerateDate
GO

-- Procedures
CREATE PROCEDURE GetAllMeetings
AS
BEGIN
	SELECT * FROM Meeting
END
GO

CREATE PROCEDURE GetMeetings
	@weekNumber int,
	@year int
AS
BEGIN
	SELECT * FROM Meeting WHERE DATEPART(WEEK, [Date]) = @weekNumber AND DATEPART(YEAR, [Date]) = @year
END
GO

CREATE PROCEDURE GetMeeting
	@iDMeeting int
AS
BEGIN
	SELECT * FROM Meeting WHERE IDMeeting = @iDMeeting
END
GO

CREATE PROCEDURE GetClients
AS
BEGIN
	SELECT * FROM Client
END
GO

CREATE PROCEDURE RemoveMeeting
	@iDMeeting int
AS
BEGIN
	DELETE FROM Meeting WHERE IDMeeting = @iDMeeting
END
GO

CREATE PROCEDURE AddMeeting
	@date datetime2,
	@place varchar(20),
	@description nvarchar(120),
	@title nvarchar(40),
	@clientID int
AS
BEGIN
	INSERT INTO Meeting ([Date], Place, [Description], Title, ClientID) VALUES
	(@date, @place, @description, @title, @clientID)
	SELECT SCOPE_IDENTITY()
END
GO

CREATE PROCEDURE ChangeMeeting
	@iDMeeting int,
	@date datetime2,
	@place varchar(20),
	@description nvarchar(120),
	@title nvarchar(40),
	@clientID int
AS
BEGIN
	UPDATE Meeting SET [Date] = @date, Place = @place, [Description] = @description,
	Title = @title, ClientID = @clientID WHERE IDMeeting = @iDMeeting
END
GO