﻿SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE DATABASE [EventStore]
GO

ALTER DATABASE EventStore SET READ_COMMITTED_SNAPSHOT ON
GO

ALTER DATABASE EventStore SET ALLOW_SNAPSHOT_ISOLATION ON
GO

USE [EventStore]
GO

CREATE TABLE [dbo].[Events](
	[EventNumber] [bigint] IDENTITY(1,1) NOT NULL,
	[EventId] [uniqueidentifier] NOT NULL,
	[EventType] [nvarchar](450) NULL,
	[AggregateId] [uniqueidentifier] NOT NULL,
	[AggregateType] [nvarchar](450) NULL,
	[AggregateVersion] [bigint] NOT NULL,
	[SerializedData] [nvarchar](max) NULL,
	[SerializedMetadata] [nvarchar](max) NULL,
	[Recorded] [datetimeoffset](7) NOT NULL,
 CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED 
(
	[EventNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [AK_Events_AggregateId_AggregateVersion] UNIQUE NONCLUSTERED 
(
	[AggregateId] ASC,
	[AggregateVersion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [AK_Events_EventId] UNIQUE NONCLUSTERED 
(
	[EventId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Snapshots](
	[AggregateId] [uniqueidentifier] NOT NULL,
	[AggregateType] [nvarchar](450) NULL,
	[AggregateVersion] [bigint] NOT NULL,
	[SerializedData] [nvarchar](max) NULL,
	[SerializedMetadata] [nvarchar](max) NULL,
	[Recorded] [datetimeoffset](7) NOT NULL,
 CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED 
(
	[AggregateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO