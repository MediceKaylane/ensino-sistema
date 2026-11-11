IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Materias] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(max) NOT NULL,
    [Tipo] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Materias] PRIMARY KEY ([Id])
);

CREATE TABLE [Turmas] (
    [Id] int NOT NULL IDENTITY,
    [Tipo] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Turmas] PRIMARY KEY ([Id])
);

CREATE TABLE [MateriaTurma] (
    [MateriasId] int NOT NULL,
    [TurmaId] int NOT NULL,
    CONSTRAINT [PK_MateriaTurma] PRIMARY KEY ([MateriasId], [TurmaId]),
    CONSTRAINT [FK_MateriaTurma_Materias_MateriasId] FOREIGN KEY ([MateriasId]) REFERENCES [Materias] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_MateriaTurma_Turmas_TurmaId] FOREIGN KEY ([TurmaId]) REFERENCES [Turmas] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Usuario] (
    [Matricula] nvarchar(450) NOT NULL,
    [Nome] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [PrimeiroAcessoConcluido] bit NOT NULL,
    [SenhaHash] nvarchar(max) NOT NULL,
    [Discriminator] nvarchar(13) NOT NULL,
    [Aluno_TurmaId] int NULL,
    [TurmaId] int NULL,
    CONSTRAINT [PK_Usuario] PRIMARY KEY ([Matricula]),
    CONSTRAINT [FK_Usuario_Turmas_Aluno_TurmaId] FOREIGN KEY ([Aluno_TurmaId]) REFERENCES [Turmas] ([Id]),
    CONSTRAINT [FK_Usuario_Turmas_TurmaId] FOREIGN KEY ([TurmaId]) REFERENCES [Turmas] ([Id])
);

CREATE TABLE [Atividades] (
    [Id] int NOT NULL IDENTITY,
    [Titulo] nvarchar(max) NOT NULL,
    [Descricao] nvarchar(max) NOT NULL,
    [ArquivoOuLink] nvarchar(max) NOT NULL,
    [InstrutorMatricula] nvarchar(450) NULL,
    [MateriaId] int NULL,
    CONSTRAINT [PK_Atividades] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Atividades_Materias_MateriaId] FOREIGN KEY ([MateriaId]) REFERENCES [Materias] ([Id]),
    CONSTRAINT [FK_Atividades_Usuario_InstrutorMatricula] FOREIGN KEY ([InstrutorMatricula]) REFERENCES [Usuario] ([Matricula])
);

CREATE INDEX [IX_Atividades_InstrutorMatricula] ON [Atividades] ([InstrutorMatricula]);

CREATE INDEX [IX_Atividades_MateriaId] ON [Atividades] ([MateriaId]);

CREATE INDEX [IX_MateriaTurma_TurmaId] ON [MateriaTurma] ([TurmaId]);

CREATE INDEX [IX_Usuario_Aluno_TurmaId] ON [Usuario] ([Aluno_TurmaId]);

CREATE INDEX [IX_Usuario_TurmaId] ON [Usuario] ([TurmaId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261103185446_CriarBancoInicial', N'10.0.14');

COMMIT;
GO

BEGIN TRANSACTION;
ALTER TABLE [Atividades] ADD [Categoria] int NOT NULL DEFAULT 0;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261106174018_AdicionarCategoriaAtividade', N'10.0.14');

COMMIT;
GO

BEGIN TRANSACTION;
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261106175545_AjustarCategoriaAtividade', N'10.0.14');

COMMIT;
GO

BEGIN TRANSACTION;
CREATE TABLE [CategoriasAtividade] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_CategoriasAtividade] PRIMARY KEY ([Id])
);

SET IDENTITY_INSERT CategoriasAtividade ON; INSERT INTO CategoriasAtividade (Id, Nome) VALUES (1, 'Prova'), (2, 'Trabalho'), (3, 'Exercicio'), (4, 'Seminario'), (5, 'Projeto'); SET IDENTITY_INSERT CategoriasAtividade OFF;

ALTER TABLE [Atividades] ADD [CategoriaAtividadeId] int NULL;

UPDATE Atividades SET CategoriaAtividadeId = CASE Categoria WHEN 0 THEN 1 WHEN 1 THEN 2 WHEN 2 THEN 3 WHEN 3 THEN 4 WHEN 4 THEN 5 ELSE 1 END

DECLARE @var nvarchar(max);
SELECT @var = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Atividades]') AND [c].[name] = N'CategoriaAtividadeId');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Atividades] DROP CONSTRAINT ' + @var + ';');
ALTER TABLE [Atividades] ALTER COLUMN [CategoriaAtividadeId] int NOT NULL;

DECLARE @var1 nvarchar(max);
SELECT @var1 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Atividades]') AND [c].[name] = N'Categoria');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Atividades] DROP CONSTRAINT ' + @var1 + ';');
ALTER TABLE [Atividades] DROP COLUMN [Categoria];

CREATE INDEX [IX_Atividades_CategoriaAtividadeId] ON [Atividades] ([CategoriaAtividadeId]);

ALTER TABLE [Atividades] ADD CONSTRAINT [FK_Atividades_CategoriasAtividade_CategoriaAtividadeId] FOREIGN KEY ([CategoriaAtividadeId]) REFERENCES [CategoriasAtividade] ([Id]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261106180054_CriarTabelaCategoriaAtividade', N'10.0.14');

COMMIT;
GO

BEGIN TRANSACTION;
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261109114538_SincronizarModelo', N'10.0.14');

COMMIT;
GO

BEGIN TRANSACTION;
ALTER TABLE [Atividades] ADD [TipoAtividade] nvarchar(13) NOT NULL DEFAULT N'';

UPDATE Atividades
SET TipoAtividade = CASE CategoriaAtividadeId
    WHEN 1 THEN 'Prova'
    WHEN 2 THEN 'Trabalho'
    WHEN 3 THEN 'Exercicio'
    WHEN 4 THEN 'Seminario'
    WHEN 5 THEN 'Projeto'
    ELSE 'Projeto'
END;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261109115534_AdicionarTiposConcretosAtividade', N'10.0.14');

COMMIT;
GO

BEGIN TRANSACTION;
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261111125855_EstadoAtualBanco', N'10.0.14');

COMMIT;
GO

