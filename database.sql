CREATE DATABASE VehicleDB;
GO

USE VehicleDB;
GO

CREATE TABLE Brands (
    Id INT PRIMARY KEY IDENTITY(1,1),
    BrandName NVARCHAR(100) NOT NULL
);

CREATE TABLE Models (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ModelName NVARCHAR(100) NOT NULL,
    BrandId INT NOT NULL,
    FOREIGN KEY (BrandId) REFERENCES Brands(Id)
);

CREATE TABLE Submissions (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    BrandId INT NOT NULL,
    ModelId INT NOT NULL,
    CivilId NVARCHAR(50) NOT NULL,
    Phone NVARCHAR(20),
    Email NVARCHAR(100) NOT NULL,
    CivilIdImagePath NVARCHAR(300) NOT NULL,
    SubmittedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (BrandId) REFERENCES Brands(Id),
    FOREIGN KEY (ModelId) REFERENCES Models(Id)
);

INSERT INTO Brands VALUES ('Toyota'), ('KIA'), ('Nissan');

INSERT INTO Models VALUES 
('Yaris', 1), ('Corolla', 1),
('Rio', 2),
('Altima', 3), ('Patrol', 3);