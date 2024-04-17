-- Active: 1713312108031@@bsbi0h3gvslpn0b4nfid-mysql.services.clever-cloud.com@3306@bsbi0h3gvslpn0b4nfid
create table Employees(
  Id int not null auto_increment,
  Names varchar(45),
  LastNames varchar(45) ,
  TypeIdentification varchar(45),
  Identified varchar(150) unique,
  Phone varchar(50),
  Email varchar(45),
  Password varchar(45),  
  EntryDate date,
  OutDate date,
  primary key (Id) 
);


drop table Employees;
truncate table Employees;

select * from Employees;

