use prueba
create table campos
(
	id int identity(1,1) not null,
	nombre varchar(255) not null,
	PRIMARY KEY (id)
)
create table tiposMuestras
(
	id int identity(1,1) not null,
	nombre varchar(255) not null,
	PRIMARY KEY (id)
)
create table entidades
(
	id varchar(255) not null,
	fechaAlta datetime not null,
	PRIMARY KEY (id)
)
create table nombresDatosEstaticos
(
	id int identity(1,1) not null,
	nombre varchar(255) not null,
	PRIMARY KEY (id)
)
create table valoresDatosEstaticos
(
	id int identity(1,1) not null,
	valor varchar(255) null,
	idEntidad varchar(255) not null,
	idNombreDatoEstatico int not null,
	PRIMARY KEY (id),
	CONSTRAINT FK_nombresDatosEstaticos_valoresDatosEstaticos FOREIGN KEY (idEntidad)
    REFERENCES entidades(id) ON DELETE CASCADE, -- si elimino la entidad, elimino sus valores
	CONSTRAINT FK_entidades_valoresDatosEstaticos FOREIGN KEY (idNombreDatoEstatico)
    REFERENCES nombresDatosEstaticos(id),
	
)
// https://www.w3schools.com/sql/sql_foreignkey.asp



insert into campos values ('campo')
insert into tiposMuestras values ('tiposMuestras')
insert into Entidades values ('e1', getdate())
insert into nombresDatosEstaticos values ('var1')
insert into valoresdatosestaticos (valor, idEntidad, idNombreDatoEstatico) values (1,'e1',1)

select * from valoresdatosestaticos


drop table valoresdatosestaticos

