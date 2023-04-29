create database BloodBank;
go 

use BloodBank;
create table Roles(
Id int identity primary key,
[Name] nvarchar(max));

create table Doctors(
Id int identity primary key,
[Name] nvarchar(max),
Photo nvarchar(max),
RoleId int references Roles(Id) on delete set null,
Education nvarchar(max),
DateOfBirth nvarchar(max),
Phone nvarchar(20),
Email nvarchar(max),
[Address] nvarchar(max));

create table Donors(
Id int identity primary key,
[Name] nvarchar(max),
Photo nvarchar(max),
BloodGroup int,
DateOfBirth nvarchar(max),
Phone nvarchar(20),
Email nvarchar(max),
[Address] nvarchar(max));

create table Recipients(
Id int identity primary key,
[Name] nvarchar(max),
Photo nvarchar(max),
BloodGroup int,
DateOfBirth nvarchar(max),
Phone nvarchar(20),
Email nvarchar(max),
[Address] nvarchar(max));

create table BloodCollection(
Id int identity primary key,
DonationType int,
DonorId int references Donors(Id) on delete set null,
BloodGroup int,
Amount float,
Unit nvarchar(max),
DateOfCollection nvarchar(max),
DoctorInChargeId int references Doctors(Id) on delete set null);

create table Issue(
Id int identity primary key,
RecipientId int references Recipients(Id) on delete set null,
BloodId int references BloodCollection(Id) on delete cascade,
BloodAmount float,
Unit nvarchar(max),
PricePaid money,
DoctorInChargeId int references Doctors(Id) on delete set null,
DateOfIssue nvarchar(max));

create table [Authorization](
Id int identity primary key,
UserName nvarchar(max),
[Password] nvarchar(max));
go

use BloodBank;
insert into dbo.[Authorization] values
('user', '123'),('admin','12345');

insert into dbo.Roles values 
('Главный врач'),('Заместитель главного врача'),('Врач-трансфузиолог'),
('Врач-методист'),('Врач-эпидемиолог'),('Врач клинической лабораторной диагностики'),
('Биолог'),('Врач-лаборант'),('Врач-методист');

insert into dbo.Donors values
('Ильин Георгий Глебович','\BloodBankImages\Donors\donor1.jfif',8,'1991-04-12','898(13)039-87-61','sauffohaffafru-2780@yopmail.com','347115, Нижегородская область, город Серпухов, шоссе Славы, 55'),
('Медведев Дмитрий Матвеевич','\BloodBankImages\Donors\donor2.jfif',7,'1991-03-11','898(13)039-87-61','sauffohaffafru-2780@yopmail.com','347115, Нижегородская область, город Серпухов, шоссе Славы, 55'),
('Михеева Варвара Мирославовна','\BloodBankImages\Donors\donor3.jfif',6,'1991-10-11','898(13)039-87-61','sauffohaffafru-2780@yopmail.com','347115, Нижегородская область, город Серпухов, шоссе Славы, 55'),
('Михайлов Дмитрий Владиславович','\BloodBankImages\Donors\donor4.jfif',5,'1991-11-11','898(13)039-87-61','sauffohaffafru-2780@yopmail.com','347115, Нижегородская область, город Серпухов, шоссе Славы, 55'),
('Борисов Максим Алексеевич','\BloodBankImages\Donors\donor5.jfif',4,'1991-04-01','3(84)354-59-12','wuttoubaxamu-1228@yopmail.com','347115, Нижегородская область, город Серпухов, шоссе Славы, 55'),
('Олейников Дмитрий Артёмович','\BloodBankImages\Donors\donor6.jfif',3,'1999-04-11','3(84)354-59-12','wuttoubaxamu-1228@yopmail.com','347115, Нижегородская область, город Серпухов, шоссе Славы, 55'),
('Киселев Лев Егорович','\BloodBankImages\Donors\donor7.jfif',2,'1998-04-11','3(84)354-59-12','wuttoubaxamu-1228@yopmail.com','347115, Нижегородская область, город Серпухов, шоссе Славы, 55'),
('Тихонова Анастасия Данииловна','\BloodBankImages\Donors\donor8.jpg',1,'1995-04-11','3(84)354-59-12','wuttoubaxamu-1228@yopmail.com','347115, Нижегородская область, город Серпухов, шоссе Славы, 55'),
('Смирнова Алёна Никитична','\BloodBankImages\Donors\donor9.jpg',8,'1991-04-21','24(80)746-34-19','nomifoullata-3587@yopmail.com','347115, Нижегородская область, город Серпухов, шоссе Славы, 55'),
('Кузнецова Нина Егоровна','\BloodBankImages\Donors\donor10.jpg',7,'1991-04-23','24(80)746-34-19','nomifoullata-3587@yopmail.com','347115, Нижегородская область, город Серпухов, шоссе Славы, 55'),
('Кудрявцев Дмитрий Иванович','\BloodBankImages\Donors\donor11.jpg',6,'1991-08-11','24(80)746-34-19','nomifoullata-3587@yopmail.com','889147, Ульяновская область, город Сергиев Посад, наб. Чехова, 36'),
('Аксенова Маргарита Львовна','\BloodBankImages\Donors\donor12.jpg',5,'1991-09-11','24(80)746-34-19','nomifoullata-3587@yopmail.com','889147, Ульяновская область, город Сергиев Посад, наб. Чехова, 36'),
('Петрова Камилла Арсентьевна','\BloodBankImages\Donors\donor13.jpg',4,'1991-01-11','81(405)376-06-79','moppapruxupa-4482@yopmail.com','889147, Ульяновская область, город Сергиев Посад, наб. Чехова, 36'),
('Долгов Антон Данилович','\BloodBankImages\Donors\donor14.jpg',3,'1991-04-11','81(405)376-06-79','moppapruxupa-4482@yopmail.com','889147, Ульяновская область, город Сергиев Посад, наб. Чехова, 36');

insert into dbo.Recipients values
('Васильева Дарья Алексеевна','\BloodBankImages\Recipients\recipient1.jpg',1,'1991-04-11','81(405)376-06-79','moppapruxupa-4482@yopmail.com','889147, Ульяновская область, город Сергиев Посад, наб. Чехова, 36'),
('Акимова Эмилия Михайловна','\BloodBankImages\Recipients\recipient2.jpg',2,'1991-04-11','81(405)376-06-79','moppapruxupa-4482@yopmail.com','889147, Ульяновская область, город Сергиев Посад, наб. Чехова, 36'),
('Сахарова Дарья Ивановна','\BloodBankImages\Recipients\recipient3.jpg',3,'1991-04-11','48(628)150-56-51','kupeceijoidda-4521@yopmail.com','889147, Ульяновская область, город Сергиев Посад, наб. Чехова, 36'),
('Морозова Эмилия Георгиевна','\BloodBankImages\Recipients\recipient4.jpg',4,'1991-04-11','48(628)150-56-51','kupeceijoidda-4521@yopmail.com','889147, Ульяновская область, город Сергиев Посад, наб. Чехова, 36'),
('Лебедев Фёдор Иванович','\BloodBankImages\Recipients\recipient5.jpg',5,'1991-04-11','48(628)150-56-51','kupeceijoidda-4521@yopmail.com','889147, Ульяновская область, город Сергиев Посад, наб. Чехова, 36'),
('Жукова Елизавета Андреевна','\BloodBankImages\Recipients\recipient6.jpg',6,'1991-04-11','48(628)150-56-51','kupeceijoidda-4521@yopmail.com','889147, Ульяновская область, город Сергиев Посад, наб. Чехова, 36'),
('Быкова Маргарита Владимировна','\BloodBankImages\Recipients\recipient7.jpg',7,'1991-04-11','291(897)136-91-97','teuleissotrule-2882@yopmail.com','058114, Тверская область, город Дмитров, проезд 1905 года, 33'),
('Григорьев Роман Платонович','\BloodBankImages\Recipients\recipient8.jpg',8,'2001-04-11','291(897)136-91-97','teuleissotrule-2882@yopmail.com','058114, Тверская область, город Дмитров, проезд 1905 года, 33'),
('Боброва Кристина Максимовна','\BloodBankImages\Recipients\recipient9.jpg',1,'2003-04-11','291(897)136-91-97','teuleissotrule-2882@yopmail.com','058114, Тверская область, город Дмитров, проезд 1905 года, 33'),
('Рыбаков Дмитрий Глебович','\BloodBankImages\Recipients\recipient10.jpg',2,'2002-04-11','291(897)136-91-97','teuleissotrule-2882@yopmail.com','058114, Тверская область, город Дмитров, проезд 1905 года, 33'),
('Спиридонова Вероника Максимовна','\BloodBankImages\Recipients\recipient11.jpg',3,'2003-04-11','1(32)966-30-56','heiffejukucru-3439@yopmail.com','058114, Тверская область, город Дмитров, проезд 1905 года, 33'),
('Воронова Полина Тимофеевна','\BloodBankImages\Recipients\recipient12.jpg',4,'2003-04-11','1(32)966-30-56','heiffejukucru-3439@yopmail.com','058114, Тверская область, город Дмитров, проезд 1905 года, 33'),
('Старостина Николь Денисовна','\BloodBankImages\Recipients\recipient13.jpg',5,'2003-04-11','1(32)966-30-56','heiffejukucru-3439@yopmail.com','058114, Тверская область, город Дмитров, проезд 1905 года, 33'),
('Смирнов Лев Тимофеевич','\BloodBankImages\Recipients\recipient14.jpg',6,'2005-04-11','1(32)966-30-56','heiffejukucru-3439@yopmail.com','058114, Тверская область, город Дмитров, проезд 1905 года, 33'),
('Горбунов Михаил Владимирович','\BloodBankImages\Recipients\recipient15.jpg',7,'2000-04-11','97(41)387-37-35','quepresezeitru-9361@yopmail.com','058114, Тверская область, город Дмитров, проезд 1905 года, 33'),
('Новикова Елизавета Давидовна','\BloodBankImages\Recipients\recipient16.jpg',8,'2000-04-11','97(41)387-37-35','quepresezeitru-9361@yopmail.com','058114, Тверская область, город Дмитров, проезд 1905 года, 33');

insert into dbo.Doctors values
('Кузьмина Ева Степановна','\BloodBankImages\Doctors\doctor1.jfif',1,'Ставропольский государственный медицинский институт Лечебное дело','1986-05-21','97(41)387-37-35','quepresezeitru-9361@yopmail.com','964683, Владимирская область, город Щёлково, бульвар Гагарина, 75'),
('Белов Михаил Дмитриевич','\BloodBankImages\Doctors\doctor2.jfif',2,'Ростовский ордена Дружбы народов медицинский институт, Гигиена, санитария, эпидемиология','1990-05-21','97(41)387-37-35','quepresezeitru-9361@yopmail.com','964683, Владимирская область, город Щёлково, бульвар Гагарина, 75'),
('Широков Захар Александрович','\BloodBankImages\Doctors\doctor3.jfif',3,'Ростовский ордена Дружбы народов медицинский институт Педиатрия','1991-05-21','30(548)393-77-24','gekojatteisse-8600@yopmail.com','964683, Владимирская область, город Щёлково, бульвар Гагарина, 75'),
('Николаев Никита Павлович','\BloodBankImages\Doctors\doctor4.jfif',3,'Ростовский государственный медицинский институт Сестринское дело','1992-05-21','30(548)393-77-24','gekojatteisse-8600@yopmail.com','964683, Владимирская область, город Щёлково, бульвар Гагарина, 75'),
('Ефремов Артемий Ярославович','\BloodBankImages\Doctors\doctor5.jfif',4,'ГБОУ ВПО «Ростовский государственный медицинский университет» МЗ РФ Медико-профилактическое дело','1993-05-21','30(548)393-77-24','gekojatteisse-8600@yopmail.com','964683, Владимирская область, город Щёлково, бульвар Гагарина, 75'),
('Кузьмин Богдан Эминович','\BloodBankImages\Doctors\doctor6.jfif',5,'Ростовский государственный медицинский институт Санитария','1994-05-21','30(548)393-77-24','gekojatteisse-8600@yopmail.com','964683, Владимирская область, город Щёлково, бульвар Гагарина, 75'),
('Литвинова Таисия Владимировна','\BloodBankImages\Doctors\doctor7.jfif',6,'Ростовский государственный университет им. М. А. Суслова Биология','1999-05-21','37(348)109-52-28','kitratreillaunna-7192@yopmail.com','964683, Владимирская область, город Щёлково, бульвар Гагарина, 75'),
('Павлова Елизавета Романовна','\BloodBankImages\Doctors\doctor8.jfif',7,'Ростовский ордена Дружбы народов медицинский институт Педиатрия','1998-05-21','37(348)109-52-28','kitratreillaunna-7192@yopmail.com','964683, Владимирская область, город Щёлково, бульвар Гагарина, 75'),
('Лаврова Екатерина Фёдоровна','\BloodBankImages\Doctors\doctor9.jfif',8,'Ростовский государственный медицинский университет Педиатрия','1997-05-21','37(348)109-52-28','kitratreillaunna-7192@yopmail.com','964683, Владимирская область, город Щёлково, бульвар Гагарина, 75'),
('Константинова Алёна Ильинична','\BloodBankImages\Doctors\doctor10.jfif',9,'Военно-медицинская ордена Ленина Краснознаменная академия им.С.М.Кирова Лечебное дело','1995-05-21','37(348)109-52-28','kitratreillaunna-7192@yopmail.com','964683, Владимирская область, город Щёлково, бульвар Гагарина, 75');

insert into dbo.BloodCollection values
(1,1,8,450,'мл','2023-03-10',3),(2,2,7,450,'мл','2023-03-10',3),
(3,3,6,1,'(0,6*10^11) тромбоцитов','2023-03-10',3),(4,4,5,450,'мл','2023-03-10',3),
(1,5,4,450,'мл','2023-03-10',3),(2,6,3,450,'мл','2023-03-10',3),
(3,7,2,1,'(0,6*10^11) тромбоцитов','2023-03-10',3),(4,8,1,450,'мл','2023-03-10',4),
(1,9,8,450,'мл','2023-03-10',4),(2,10,7,450,'мл','2023-03-10',4),
(3,11,6,1,'(0,6*10^11) тромбоцитов','2023-03-10',4),(4,12,5,450,'мл','2023-03-10',4),
(1,13,4,450,'мл','2023-03-10',4),(2,14,3,450,'мл','2023-03-10',4);

insert into dbo.Issue values
(16,1,450,'мл',4000,2,'2023-03-11'),
(15,2,450,'мл',3000,2,'2023-03-11'),
(14,3,1,'(0,6*10^11) тромбоцитов',5800,2,'2023-03-11'),
(13,4,450,'мл',6000,2,'2023-03-11'),
(12,5,450,'мл',4000,2,'2023-03-11'),
(11,6,450,'мл',3000,2,'2023-03-11'),
(10,7,1,'(0,6*10^11) тромбоцитов',5800,2,'2023-03-11'),
(9,8,450,'мл',6000,2,'2023-03-11'),
(8,9,450,'мл',4000,2,'2023-03-11'),
(7,10,450,'мл',3000,2,'2023-03-11'),
(6,11,1,'(0,6*10^11) тромбоцитов',5800,2,'2023-03-11'),
(5,12,450,'мл',6000,2,'2023-03-11'),
(4,13,450,'мл',4000,2,'2023-03-11'),
(3,14,450,'мл',3000,2,'2023-03-11');