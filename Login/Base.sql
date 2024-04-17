-- Active: 1713212878876@@bsbi0h3gvslpn0b4nfid-mysql.services.clever-cloud.com@3306@bsbi0h3gvslpn0b4nfid

create table Employees(
    Id int not null auto_increment,
    Names varchar(45),
    LastNames varchar(45) ,
    Password varchar(45),
    Photo VARCHAR(200),
    TypeIdentification varchar(45),
    Identified varchar(150) unique,
    Phone varchar(50),
    Email varchar(45),
    primary key (Id)           
);

create table Hours(
    Id int auto_increment,
    EntryDate date null,
    OutDate date null,
    EmployeeId int,
    foreign key(EmployeeId) references Employees (Id),
    primary key (Id)
);


drop table Employees;

truncate table Employees;

select * from Employees;

