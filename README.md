## Overview

This repository contains an example of an API designed to be consumed by project management applications.
The intended workflow consists of three main entity types - Project, Todo and Tag:
- Project - the highest organizational structure in the hierarchy. A project can contain one or more Todo items.
Projects have the following properties (described further): Id, Name, IsArchived.
- Todo - a different name for a task (to avoid confusion in a .NET-based project, I wanted to avoid using the name ‘Task’).
Every Todo item has to be assigned to a single, existing Project. Todo items have the following properties (described further):
Id, Name, ProjectId, IsDone, Date.
- Tag - a label meant for organizing Todo items. One or more Tags can be assigned to a single Todo item.
Also, a single Tag can be assigned to one or more Todo items. Tags have the following properties (described further):
Id, Name. Tags are assigned to Todo items through a designated table called TagAssignments (also described further).

## Data layer

The project is based on MS SQL Server in combination with EF Core. I’ve chosen the Database First approach,
so the database needs to be created before the data model.

Open SQL Server Management Studio and create a new database named TaskManagerDB. Then run the following code:

```
USE [TaskManagerDB]
GO

CREATE TABLE [dbo].[Projects](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[IsArchived] [bit] NOT NULL DEFAULT (0)
)

CREATE TABLE [dbo].[Todos](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[ProjectId] [int] NOT NULL,
	[IsDone] [bit] NOT NULL DEFAULT(0),
	[Date] [date] NULL,
	FOREIGN KEY (ProjectId)
		REFERENCES dbo.Projects(Id)
		ON DELETE CASCADE
)

CREATE TABLE [dbo].[Tags](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Name] [nvarchar](max) NOT NULL
)

CREATE TABLE [dbo].[TagAssignments](
	[TagId] [int] NOT NULL,
	[TodoId] [int] NOT NULL,
	PRIMARY KEY ([TagId], [TodoId]),
	FOREIGN KEY (TagId)
		REFERENCES dbo.Tags(Id)
		ON DELETE CASCADE,
	FOREIGN KEY (TodoId)
		REFERENCES dbo.Todos(Id)
		ON DELETE CASCADE
)
```

## NuGet packages

Packages to be installed:
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.VisualStudio.Web.CodeGeneration.Design (optional)
- Bricelam.EntityFrameworkCore.Pluralizer (optional)
- Microsoft.CodeAnalysis.FxCopAnalyzers (optional)

## Scaffolding the database context

To scaffold the database context, run the following command in the Package Manager Console:

```
Scaffold-DbContext "Server=localhost;Database=TaskManagerDB;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer
-OutputDir Models -Schemas "dbo" -Context "TaskManagerContext"
```
