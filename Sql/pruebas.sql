-- generamos un campo, tipo muestra, entidad, nombre dato estatico, valor dato estatico
insert into campos values ('Medicina')
insert into tiposMuestras values ('Analisis de sangre')
insert into Entidades values ('e1', getdate())
insert into nombresDatosEstaticos values ('var1')
insert into valoresdatosestaticos (valor, idEntidad, idNombreDatoEstatico) values (1,'e1',1)

-- generamos una muestra
insert into muestras values ('e1',1,1,getdate())
insert into nombresVariablesMuestras values ('var1',1)
insert into nombresVariablesMuestras values ('var2',1)
insert into valoresVariablesMuestras values (1,1,100)
insert into valoresVariablesMuestras values (2,1,100)
insert into valoresReferencia values (1,200,100)

-- comprobaciones
select * from entidades
select * from nombresDatosEstaticos
select * from valoresDatosEstaticos

select * from muestras
-- variables que tiene una muestra
select * from nombresVariablesMuestras
-- valores de las variables de una muestra
select * from valoresVariablesMuestras

select * from valoresReferencia

-- pruebas
/*
1 al borrar una entitdad se borran sus muestras
2 al borrar una entidad se borran sus valores de datos estaticos
3 al borrar una entidad se borran sus valores de variables de muestras
4 al borrar una entidad NO se borran sus nombres de variables de muestras
5 al borrar una muestras se borran sus valores de variables de muestras
6 al borrar un nombre de variable no se borran sus valores de variables de muestras
7 al borrar un nombre de variable, si no tiene muestras se borra su valor de referencia
*/


