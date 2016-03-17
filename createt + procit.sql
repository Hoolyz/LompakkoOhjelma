
create table Pelaaja (
nimi varchar(50) not null,
pisteet smallint not null,
primary key (nimi)
)

create table Lompakko (
nimi varchar(50) not null,
lompakkotunnus smallint,
saldo decimal(10,2)
primary key (lompakkotunnus,nimi)
foreign key (nimi) references Pelaaja )


insert into Pelaaja (nimi,pisteet)
values('Esa','20')

insert into Pelaaja (nimi,pisteet)
values('Simo','30')

insert into Pelaaja (nimi,pisteet)
values ('Kari','13')

insert into Lompakko (nimi,lompakkotunnus,saldo)
values ('Simo',1,'10')

insert into Lompakko (nimi,lompakkotunnus,saldo)
values ('Esa',1,'100')

insert into Lompakko (nimi,lompakkotunnus,saldo)
values ('Simo',2,'200')

insert into Lompakko (nimi,lompakkotunnus,saldo)
values ('Kari',1,'2')

insert into Lompakko (nimi,lompakkotunnus,saldo)
values ('Kari',2,'133')

insert into Lompakko (nimi,lompakkotunnus,saldo)
values ('Kari',3,'400')

insert into Lompakko (nimi,lompakkotunnus,saldo)
values ('Kari',4,'500')

drop table Pelaaja
drop table Lompakko

create proc PoistaPelaaja
@poistettavaPelaaja varchar(50)
as
delete 
from Lompakko
where nimi = @poistettavaPelaaja
delete 
from Pelaaja
where nimi = @poistettavaPelaaja

Create proc PoistaLompakko
@poistettavaLompakko smallint,
@lompakonPelaaja varchar(50)
as
delete
from Lompakko
where lompakkotunnus = @poistettavaLompakko
AND
nimi = @LompakonPelaaja
WHILE (Select MAX(Lompakkotunnus) From Lompakko) >= @poistettavaLompakko
BEGIN
update Lompakko 
SET Lompakkotunnus = @poistettavaLompakko
Where lompakkotunnus = (@poistettavaLompakko+1) AND nimi = @LompakonPelaaja
SET @poistettavalompakko = (@poistettavalompakko+1)
END


