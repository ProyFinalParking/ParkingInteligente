BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "clientes" (
	"id_cliente"	INTEGER,
	"nombre"	TEXT,
	"documento"	TEXT NOT NULL,
	"foto"	TEXT,
	"edad"	INTEGER,
	"genero"	TEXT,
	"telefono"	TEXT,
	PRIMARY KEY("id_cliente")
);
CREATE TABLE IF NOT EXISTS "estacionamientos" (
	"id_estacionamiento"	INTEGER,
	"id_vehiculo"	INTEGER,
	"matricula"	TEXT NOT NULL,
	"entrada"	TEXT NOT NULL,
	"salida"	TEXT,
	"importe"	REAL,
	"tipo"	TEXT NOT NULL,
	FOREIGN KEY("id_vehiculo") REFERENCES "vehiculos"("id_vehiculo"),
	PRIMARY KEY("id_estacionamiento")
);
CREATE TABLE IF NOT EXISTS "marcas" (
	"id_marca"	INTEGER,
	"marca"	TEXT NOT NULL,
	PRIMARY KEY("id_marca")
);
CREATE TABLE IF NOT EXISTS "vehiculos" (
	"id_vehiculo"	INTEGER,
	"id_cliente"	INTEGER NOT NULL,
	"matricula"	TEXT NOT NULL,
	"id_marca"	INTEGER,
	"modelo"	TEXT,
	"tipo"	TEXT NOT NULL,
	FOREIGN KEY("id_marca") REFERENCES "marcas"("id_marca"),
	FOREIGN KEY("id_cliente") REFERENCES "clientes"("id_cliente"),
	PRIMARY KEY("id_vehiculo")
);
INSERT INTO "clientes" ("id_cliente","nombre","documento","foto","edad","genero","telefono") VALUES (1,'Acevedo Manríquez María Mireya','12358496A','URL Imagen 1',25,'Femenino','956124578'),
 (2,'Aguilar Lorantes Irma','32659815B','URL 2',33,'Femenino','965234589'),
 (3,'Alcoverde Martínez Roberto Antonio','95123678C','URL 3',49,'Masculino','654128935'),
 (4,'Alvarado Mendoza Oscar','56489520D','URL4',65,'Masculino','784629514'),
 (5,'Serina Byrd','65841523H','URL 5',62,'Femenino','645127895'),
 (6,'Ursa Mcdowell','62845951K','URL 6',33,'Femenino','964513498'),
 (7,'Channing Melton','98156343O','URL 7',54,'Femenino','651284231'),
 (8,'Chantale Barrera','83492154Y','URL 8',24,'Femenino','679512354'),
 (9,'Jonah Quinn','65124578T','URL 9',45,'Masculino','635951555'),
 (10,'John Daniels','95152648H','URL 10',25,'Masculino','632145791'),
 (11,'Alexander Lancaster','01324807G','URL 11',34,'Masculino','654123951'),
 (12,'Hollee Pratt','96234584R','URL 12',41,'Femenino','653753159'),
 (13,'Fernando Alonso','95123648J','URL',45,'Masculino','654789512'),
 (14,'Juan Antonio Gonzalez Rodriguez','26154859G','Foto',44,'Masculino','651234895');
INSERT INTO "marcas" ("id_marca","marca") VALUES (1,'BMW'),
 (2,'Citroen'),
 (3,'Renault'),
 (4,'Mercedez'),
 (5,'Peugeot'),
 (6,'Audi'),
 (7,'Range Rover'),
 (8,'Opel'),
 (9,'Hyundai'),
 (10,'Ford'),
 (11,'Fiat'),
 (12,'Jeep'),
 (13,'Lexus'),
 (14,'MINI'),
 (15,'SEAT'),
 (16,'Subaru'),
 (17,'Mitsubishi'),
 (18,'Nissan'),
 (19,'Skoda'),
 (20,'Porsche'),
 (21,'Smart'),
 (22,'Tesla'),
 (23,'Toyota'),
 (24,'Volvo'),
 (25,'Volkswagen'),
 (26,'SsangYong'),
 (27,'MG'),
 (28,'Mazda'),
 (29,'Land Rover'),
 (30,'Jaguar'),
 (31,'Dacia'),
 (32,'Ferrari'),
 (33,'Bentley'),
 (34,'Aston Martin'),
 (35,'Bugatti'),
 (36,'Yamaha'),
 (37,'Suzuki'),
 (38,'Honda'),
 (39,'Aprilia'),
 (40,'KTM'),
 (41,'');
INSERT INTO "vehiculos" ("id_vehiculo","id_cliente","matricula","id_marca","modelo","tipo") VALUES (1,1,'2648KHY',1,'Serie 3','Coche'),
 (2,2,'45778KYB',2,'C3','Coche'),
 (3,11,'4595HHY',36,'R1','Moto'),
 (4,14,'9543YAC',19,'Fabia','Coche'),
 (5,8,'3215KPE',37,'Hayabusa','Moto'),
 (6,6,'9435ODS',22,'Model S','Coche');
COMMIT;