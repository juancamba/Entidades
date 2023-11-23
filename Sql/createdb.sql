create database prueba
use prueba
create table campos
(
	id int not null,
	nombre varchar(255) not null,
	PRIMARY KEY (id)
)
create table tiposMuestras
(
	id int not null,
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
create table muestras(
	id int identity(1,1) not null,
	idEntidad varchar(255) not null,
	idTipoMuestra int not null,
	idCampo int not null,
	fecha datetime not null,
	PRIMARY KEY (id),
	CONSTRAINT FK_entidades_muestras FOREIGN KEY (idEntidad)
	REFERENCES entidades(id) on delete cascade,
	constraint FK_tiposMuestras_muestras FOREIGN KEY (idTipoMuestra)
	REFERENCES tiposMuestras(id),
	constraint FK_campos_muestras FOREIGN KEY (idCampo)
	REFERENCES campos(id)

)
create table nombresVariablesMuestras(
	id int identity(1,1) not null,
	nombre varchar(255) not null,
	idTipoMuestra int not null,
	PRIMARY KEY (id),
	CONSTRAINT FK_tiposMuestras_nombresVariablesMuestras FOREIGN KEY (idTipoMuestra)
	REFERENCES tiposMuestras(id)

)
create table valoresVariablesMuestras(
	id int identity(1,1) not null,
	idNombreVariableMuestra int not null,
	idMuestra int not null,
	valor varchar(1024) null,
	PRIMARY KEY (id),
	CONSTRAINT FK_nombresVariablesMuestra_valoresVariablesMuestras 
	FOREIGN KEY (idNombreVariableMuestra) REFERENCES nombresVariablesMuestras(id),
	CONSTRAINT FK_muestras_valoresVariablesMuestras
	FOREIGN KEY (idMuestra) REFERENCES muestras(id) on delete cascade
	
)

create table valoresReferencia(
	id int identity(1,1) not null,
	idNombreVariableMuestra int not null,
	maximo varchar(255) null,
	minimo varchar(255) null,
	PRIMARY KEY (id),
	CONSTRAINT FK_nombresVariablesMuestra_valoresReferencia 
	FOREIGN KEY (idNombreVariableMuestra)
	REFERENCES nombresVariablesMuestras(id) on delete cascade

)



