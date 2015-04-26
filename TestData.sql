USE [Kenpro]
GO
/****** Object:  Table [dbo].[Organizations]    Script Date: 4/26/2015 5:43:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Organizations](
	[OrganizationID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[SpaceDisk] [decimal](18, 2) NOT NULL,
	[MaxAssignedUser] [int] NOT NULL,
	[IsGlobalAvailable] [bit] NOT NULL,
	[IsTrainingAvailableForAll] [bit] NOT NULL,
	[CanUserChangeMail] [bit] NOT NULL,
	[CanUserChangeName] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUserID] [int] NOT NULL,
	[ProtectorID] [int] NULL,
	[StatusID] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeletedDate] [datetime] NULL,
	[DeletedUserID] [int] NULL,
 CONSTRAINT [PK_dbo.Organizations] PRIMARY KEY CLUSTERED 
(
	[OrganizationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[People]    Script Date: 4/26/2015 5:43:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[People](
	[PersonID] [int] IDENTITY(1,1) NOT NULL,
	[ProfileID] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Mail] [nvarchar](max) NULL,
	[Login] [nvarchar](max) NULL,
	[Password] [nvarchar](max) NULL,
	[StatusID] [int] NOT NULL,
	[RegistrationDate] [datetime] NOT NULL,
	[RegistrationUserID] [int] NOT NULL,
	[LastActivationDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeletedDate] [datetime] NULL,
	[DeleteUserID] [int] NULL,
	[OrganizationID] [int] NULL,
 CONSTRAINT [PK_dbo.People] PRIMARY KEY CLUSTERED 
(
	[PersonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ProfileGroup2Person]    Script Date: 4/26/2015 5:43:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProfileGroup2Person](
	[ProfileGroup2PersonID] [int] IDENTITY(1,1) NOT NULL,
	[ProfileGroupID] [int] NOT NULL,
	[PersonID] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeletedDate] [datetime] NULL,
	[DeletedUserID] [int] NULL,
 CONSTRAINT [PK_dbo.ProfileGroup2Person] PRIMARY KEY CLUSTERED 
(
	[ProfileGroup2PersonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ProfileGroups]    Script Date: 4/26/2015 5:43:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProfileGroups](
	[ProfileGroupID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeletedDate] [datetime] NULL,
	[DeletedUserID] [int] NULL,
 CONSTRAINT [PK_dbo.ProfileGroups] PRIMARY KEY CLUSTERED 
(
	[ProfileGroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Profiles]    Script Date: 4/26/2015 5:43:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Profiles](
	[ProfileID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Profiles] PRIMARY KEY CLUSTERED 
(
	[ProfileID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Status]    Script Date: 4/26/2015 5:43:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[StatusID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Status] PRIMARY KEY CLUSTERED 
(
	[StatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TrainingResults]    Script Date: 4/26/2015 5:43:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrainingResults](
	[TrainingResultID] [int] IDENTITY(1,1) NOT NULL,
	[TrainingID] [int] NOT NULL,
	[PersonID] [int] NOT NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Rating] [int] NOT NULL,
 CONSTRAINT [PK_dbo.TrainingResults] PRIMARY KEY CLUSTERED 
(
	[TrainingResultID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Trainings]    Script Date: 4/26/2015 5:43:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Trainings](
	[TrainingID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[TrainingTypeID] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUserID] [int] NOT NULL,
	[DeletedDate] [datetime] NULL,
	[DeletedUserID] [int] NULL,
 CONSTRAINT [PK_dbo.Trainings] PRIMARY KEY CLUSTERED 
(
	[TrainingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TrainingTypes]    Script Date: 4/26/2015 5:43:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrainingTypes](
	[TrainingTypeID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.TrainingTypes] PRIMARY KEY CLUSTERED 
(
	[TrainingTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[People] ON 

GO
INSERT [dbo].[People] ([PersonID], [ProfileID], [Name], [Mail], [Login], [Password], [StatusID], [RegistrationDate], [RegistrationUserID], [LastActivationDate], [IsDeleted], [DeletedDate], [DeleteUserID], [OrganizationID]) VALUES (1, 1, N'Admin', N'admin@admin.com.pl', N'a', N'a', 1, CAST(N'2015-04-26 00:00:00.000' AS DateTime), 1, CAST(N'2015-04-26 00:00:00.000' AS DateTime), 0, NULL, NULL, NULL)
GO
INSERT [dbo].[People] ([PersonID], [ProfileID], [Name], [Mail], [Login], [Password], [StatusID], [RegistrationDate], [RegistrationUserID], [LastActivationDate], [IsDeleted], [DeletedDate], [DeleteUserID], [OrganizationID]) VALUES (3, 5, N'Krzysiu Misiu', N'krzys@main.com', N'Krzys', N'a', 1, CAST(N'2015-04-26 00:00:00.000' AS DateTime), 1, CAST(N'2015-04-26 00:00:00.000' AS DateTime), 0, NULL, NULL, NULL)
GO
INSERT [dbo].[People] ([PersonID], [ProfileID], [Name], [Mail], [Login], [Password], [StatusID], [RegistrationDate], [RegistrationUserID], [LastActivationDate], [IsDeleted], [DeletedDate], [DeleteUserID], [OrganizationID]) VALUES (5, 5, N'Zbyszko Zawa', N'zbyh@mail.com', N'Zbyho', N'a', 1, CAST(N'2015-04-26 00:00:00.000' AS DateTime), 1, CAST(N'2015-04-26 00:00:00.000' AS DateTime), 0, NULL, NULL, NULL)
GO
INSERT [dbo].[People] ([PersonID], [ProfileID], [Name], [Mail], [Login], [Password], [StatusID], [RegistrationDate], [RegistrationUserID], [LastActivationDate], [IsDeleted], [DeletedDate], [DeleteUserID], [OrganizationID]) VALUES (6, 5, N'Daniel Danilowy', N'danielowy@mail.com', N'danilo', N'a', 1, CAST(N'2015-04-26 00:00:00.000' AS DateTime), 1, CAST(N'2015-04-26 00:00:00.000' AS DateTime), 1, CAST(N'2015-04-26 15:06:11.817' AS DateTime), 1, NULL)
GO
SET IDENTITY_INSERT [dbo].[People] OFF
GO
SET IDENTITY_INSERT [dbo].[ProfileGroups] ON 

GO
INSERT [dbo].[ProfileGroups] ([ProfileGroupID], [Name], [CreateDate], [IsDeleted], [DeletedDate], [DeletedUserID]) VALUES (1, N'Testowa grupa1', CAST(N'2015-04-26 17:07:00.637' AS DateTime), 0, NULL, -1)
GO
SET IDENTITY_INSERT [dbo].[ProfileGroups] OFF
GO
SET IDENTITY_INSERT [dbo].[Profiles] ON 

GO
INSERT [dbo].[Profiles] ([ProfileID], [Name], [CreateDate]) VALUES (1, N'Administrator', CAST(N'2015-04-26 00:00:00.000' AS DateTime))
GO
INSERT [dbo].[Profiles] ([ProfileID], [Name], [CreateDate]) VALUES (2, N'Tworca', CAST(N'2015-04-26 00:00:00.000' AS DateTime))
GO
INSERT [dbo].[Profiles] ([ProfileID], [Name], [CreateDate]) VALUES (3, N'Manager', CAST(N'2015-04-26 00:00:00.000' AS DateTime))
GO
INSERT [dbo].[Profiles] ([ProfileID], [Name], [CreateDate]) VALUES (4, N'Opiekun', CAST(N'2015-04-26 00:00:00.000' AS DateTime))
GO
INSERT [dbo].[Profiles] ([ProfileID], [Name], [CreateDate]) VALUES (5, N'Uzytkownik', CAST(N'2015-04-26 00:00:00.000' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Profiles] OFF
GO
SET IDENTITY_INSERT [dbo].[Status] ON 

GO
INSERT [dbo].[Status] ([StatusID], [Name]) VALUES (1, N'Aktywny')
GO
INSERT [dbo].[Status] ([StatusID], [Name]) VALUES (2, N'Zablokowany')
GO
SET IDENTITY_INSERT [dbo].[Status] OFF
GO
SET IDENTITY_INSERT [dbo].[TrainingResults] ON 

GO
INSERT [dbo].[TrainingResults] ([TrainingResultID], [TrainingID], [PersonID], [StartDate], [EndDate], [Rating]) VALUES (3, 2, 1, CAST(N'2015-04-26 00:00:00.000' AS DateTime), NULL, 0)
GO
INSERT [dbo].[TrainingResults] ([TrainingResultID], [TrainingID], [PersonID], [StartDate], [EndDate], [Rating]) VALUES (4, 4, 1, CAST(N'2015-04-26 00:00:00.000' AS DateTime), CAST(N'2015-04-26 00:00:00.000' AS DateTime), 5)
GO
SET IDENTITY_INSERT [dbo].[TrainingResults] OFF
GO
SET IDENTITY_INSERT [dbo].[Trainings] ON 

GO
INSERT [dbo].[Trainings] ([TrainingID], [Name], [Description], [IsActive], [TrainingTypeID], [CreateDate], [CreateUserID], [DeletedDate], [DeletedUserID]) VALUES (2, N'Instalacja', N'Pierwsza instalacja', 0, 1, CAST(N'2015-04-26 00:00:00.000' AS DateTime), 1, NULL, NULL)
GO
INSERT [dbo].[Trainings] ([TrainingID], [Name], [Description], [IsActive], [TrainingTypeID], [CreateDate], [CreateUserID], [DeletedDate], [DeletedUserID]) VALUES (3, N'Konfiguracja', N'Pierwsza konfiguracja', 1, 1, CAST(N'2015-04-26 00:00:00.000' AS DateTime), 1, NULL, NULL)
GO
INSERT [dbo].[Trainings] ([TrainingID], [Name], [Description], [IsActive], [TrainingTypeID], [CreateDate], [CreateUserID], [DeletedDate], [DeletedUserID]) VALUES (4, N'Kenpro 1', N'Piersze kenpro', 1, 2, CAST(N'2015-04-26 00:00:00.000' AS DateTime), 1, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Trainings] OFF
GO
SET IDENTITY_INSERT [dbo].[TrainingTypes] ON 

GO
INSERT [dbo].[TrainingTypes] ([TrainingTypeID], [Name]) VALUES (1, N'Wewnetrzne')
GO
INSERT [dbo].[TrainingTypes] ([TrainingTypeID], [Name]) VALUES (2, N'Kenpro')
GO
SET IDENTITY_INSERT [dbo].[TrainingTypes] OFF
GO
ALTER TABLE [dbo].[People]  WITH CHECK ADD  CONSTRAINT [FK_dbo.People_dbo.Organizations_OrganizationID] FOREIGN KEY([OrganizationID])
REFERENCES [dbo].[Organizations] ([OrganizationID])
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_dbo.People_dbo.Organizations_OrganizationID]
GO
ALTER TABLE [dbo].[People]  WITH CHECK ADD  CONSTRAINT [FK_dbo.People_dbo.Profiles_ProfileID] FOREIGN KEY([ProfileID])
REFERENCES [dbo].[Profiles] ([ProfileID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_dbo.People_dbo.Profiles_ProfileID]
GO
ALTER TABLE [dbo].[People]  WITH CHECK ADD  CONSTRAINT [FK_dbo.People_dbo.Status_StatusID] FOREIGN KEY([StatusID])
REFERENCES [dbo].[Status] ([StatusID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_dbo.People_dbo.Status_StatusID]
GO
ALTER TABLE [dbo].[ProfileGroup2Person]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ProfileGroup2Person_dbo.People_PersonID] FOREIGN KEY([PersonID])
REFERENCES [dbo].[People] ([PersonID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProfileGroup2Person] CHECK CONSTRAINT [FK_dbo.ProfileGroup2Person_dbo.People_PersonID]
GO
ALTER TABLE [dbo].[ProfileGroup2Person]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ProfileGroup2Person_dbo.ProfileGroups_ProfileGroupID] FOREIGN KEY([ProfileGroupID])
REFERENCES [dbo].[ProfileGroups] ([ProfileGroupID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProfileGroup2Person] CHECK CONSTRAINT [FK_dbo.ProfileGroup2Person_dbo.ProfileGroups_ProfileGroupID]
GO
ALTER TABLE [dbo].[TrainingResults]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TrainingResults_dbo.People_PersonID] FOREIGN KEY([PersonID])
REFERENCES [dbo].[People] ([PersonID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TrainingResults] CHECK CONSTRAINT [FK_dbo.TrainingResults_dbo.People_PersonID]
GO
ALTER TABLE [dbo].[TrainingResults]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TrainingResults_dbo.Trainings_TrainingID] FOREIGN KEY([TrainingID])
REFERENCES [dbo].[Trainings] ([TrainingID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TrainingResults] CHECK CONSTRAINT [FK_dbo.TrainingResults_dbo.Trainings_TrainingID]
GO
ALTER TABLE [dbo].[Trainings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Trainings_dbo.TrainingTypes_TrainingTypeID] FOREIGN KEY([TrainingTypeID])
REFERENCES [dbo].[TrainingTypes] ([TrainingTypeID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Trainings] CHECK CONSTRAINT [FK_dbo.Trainings_dbo.TrainingTypes_TrainingTypeID]
GO
