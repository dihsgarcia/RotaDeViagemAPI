--Crio uma database generica, para não impactar em outros possiveis projetos de quem executar o script
if DB_ID ('GenericProjects_db') is null
begin
	create database GenericProjects_db
	print 'database GenericProjects_db criada!'
end
else
begin
	print 'database GenericProjects_db já existe!'
end

go

--------------------------------------------------------------------------------------------------------
--Crio uma tabela para armazenar as rotas
use GenericProjects_db
go

if not exists ( select 1
				from sys.objects
				where name = 'RotaDeViagem')
begin
	create table RotaDeViagem ( id				int	identity(1,1)				,
								origem			varchar(10)			not null	,
								destino			varchar(10)			not null	,								
								valorViagemInt	int					not null	,
						   							
								constraint [RotaDeViagem_PK] primary key (id)		)	
							
	print 'tabela RotaDeViagem criada!'
end
else
begin
	print 'table RotaDeViagem ja existe!'
end

go

--------------------------------------------------------------------------------------------------------
--populo a tabela com os dados fornecidos
if not exists (select 1 from RotaDeViagem)
begin
	insert into RotaDeViagem (origem, destino, valorViagemInt)
		values 
			('GRU', 'BRC', 10),
			('BRC', 'SCL', 5 ),
			('GRU', 'CDG', 75),
			('GRU', 'SCL', 20),
			('GRU', 'ORL', 56),
			('ORL', 'CDG', 5 ),
			('SCL', 'ORL', 20)

	print 'Pré Carga RotaDeViagem feita com sucesso!'
end
else
begin
	print 'Pré Carga RotaDeViagem já feita!'
end
