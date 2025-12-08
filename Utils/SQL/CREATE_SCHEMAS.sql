-- // BANCO DE DADOS DO MATHEUS \\ --

CREATE DATABASE [DevLearningStudentAPI]
GO

USE [DevLearningStudentAPI]
GO

CREATE TABLE [Student]
(
    [Id] uniqueidentifier NOT NULL,
    [Name] NVARCHAR(120) NOT NULL,
    [Email] NVARCHAR(180) NOT NULL,
    [Document] NVARCHAR(20) NULL,
    [Phone] NVARCHAR(20) NULL,
    [Birthdate] DATETIME NULL,
    [CreateDate] DATETIME NOT NULL DEFAULT(GETDATE()),
    CONSTRAINT [PK_Student] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [StudentCourse]
(
    [CourseId] CHAR(24) NOT NULL,
    [StudentId] uniqueidentifier NOT NULL,
    [Progress] TINYINT NOT NULL,
    [Favorite] BIT NOT NULL,
    [StartDate] DATETIME NOT NULL,
    [LastUpdateDate] DATETIME NULL DEFAULT(GETDATE()),
    CONSTRAINT [PK_StudentCourse] PRIMARY KEY ([CourseId], [StudentId]),
    CONSTRAINT [FK_StudentCourse_Student_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Student] ([Id])
);
GO


-- // BANCO DE DADOS DO HENRIQUE \\ --

CREATE DATABASE [DevLearningAuthorAPI]
GO

USE [DevLearningAuthorAPI]
GO

CREATE TABLE [Author]
(
    [Id] uniqueidentifier NOT NULL,
    [Name] NVARCHAR(80) NOT NULL,
    [Title] NVARCHAR(80) NOT NULL,
    [Image] NVARCHAR(1024) NOT NULL,
    [Bio] NVARCHAR(2000) NOT NULL,
    [Url] nvarchar(450) NULL,
    [Email] NVARCHAR(160) NOT NULL,
    [Type] TINYINT NOT NULL,
    CONSTRAINT [PK_Author] PRIMARY KEY ([Id])
);
GO

CREATE DATABASE [DevLearningCategoryAPI]
GO

USE [DevLearningCategoryAPI]
GO

CREATE TABLE [Category]
(
    [Id] uniqueidentifier NOT NULL,
    [Title] NVARCHAR(160) NOT NULL,
    [Url] NVARCHAR(1024) NOT NULL,
    [Summary] NVARCHAR(2000) NOT NULL,
    [Order] int NOT NULL,
    [Description] TEXT NOT NULL,
    [Featured] BIT NOT NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY ([Id])
);
GO


-- // BANCO DE DADOS DO CHRISTIAN \\ --

CREATE DATABASE [DevLearningCareerAPI]
GO

USE [DevLearningCareerAPI]
GO

CREATE TABLE [Career]
(
    [Id] uniqueidentifier NOT NULL,
    [Title] NVARCHAR(160) NOT NULL,
    [Summary] NVARCHAR(2000) NOT NULL,
    [Url] NVARCHAR(1024) NOT NULL,
    [DurationInMinutes] INT NOT NULL,
    [Active] BIT NOT NULL,
    [Featured] BIT NOT NULL,
    [Tags] NVARCHAR(160) NOT NULL,
    CONSTRAINT [PK_Career] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [CareerItem]
(
    [CareerId] uniqueidentifier NOT NULL,
    [CourseId] CHAR(24) NOT NULL,
    [Title] NVARCHAR(160) NOT NULL,
    [Description] TEXT NOT NULL,
    [Order] TINYINT NOT NULL,    
    CONSTRAINT [PK_CareerItem] PRIMARY KEY ([CourseId], [CareerId]),
    CONSTRAINT [FK_CareerItem_Career_CareerId] FOREIGN KEY ([CareerId]) REFERENCES [Career] ([Id])
);
GO
