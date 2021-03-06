USE [Speedy]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 04/25/2010 21:42:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[ID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Basic]    Script Date: 04/25/2010 21:42:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Basic](
	[id] [char](24) NOT NULL,
	[TheData] [nvarchar](500) NOT NULL,
	[TheTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Basic] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 04/25/2010 21:42:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[ID] [uniqueidentifier] NOT NULL,
	[Quantity] [int] NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerToOrders]    Script Date: 04/25/2010 21:42:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerToOrders](
	[ID] [uniqueidentifier] NOT NULL,
	[OrderId] [uniqueidentifier] NOT NULL,
	[CustomerID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CustomerToOrders] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_CustomerToOrders_Customer]    Script Date: 04/25/2010 21:42:06 ******/
ALTER TABLE [dbo].[CustomerToOrders]  WITH CHECK ADD  CONSTRAINT [FK_CustomerToOrders_Customer] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([ID])
GO
ALTER TABLE [dbo].[CustomerToOrders] CHECK CONSTRAINT [FK_CustomerToOrders_Customer]
GO
/****** Object:  ForeignKey [FK_CustomerToOrders_Orders]    Script Date: 04/25/2010 21:42:06 ******/
ALTER TABLE [dbo].[CustomerToOrders]  WITH CHECK ADD  CONSTRAINT [FK_CustomerToOrders_Orders] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([ID])
GO
ALTER TABLE [dbo].[CustomerToOrders] CHECK CONSTRAINT [FK_CustomerToOrders_Orders]
GO
