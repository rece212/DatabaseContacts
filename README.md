# prjDatabaseContacts
Database Contacts app 


Create database Contacts ;
use Contacts;
create table tblContacts(
	[PersonID] int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[FirstName] varchar(255) not null,
	[LastName] varchar(255) not null,
	PhoneNumber varchar(10) not null,
	EmailAddress varchar(255) not null,
	Username varchar(255) not null
);

create table tblUsers(
	Username varchar(255) not null,
	[Password] varchar(255) not null
	);
