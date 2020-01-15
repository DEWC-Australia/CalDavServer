--Scaffold-DbContext -Connection "Server=(localdb)\MSSQLLocalDB;Database=DEWC;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Data\CALDAV -Schemas CALDAV -Force

CREATE TABLE CALDAV.FolderType(
	FolderType INT NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(50) NOT NULL,
	[Description] NVARCHAR(100) NOT NULL
);

CREATE TABLE CALDAV.FolderInfo(
	FolderID uniqueidentifier default NEWID() NOT NULL Primary Key,
	[Path] NVARCHAR(200) NOT NULL UNIQUE,
	ParentFolderID uniqueidentifier,

	[Name] NVARCHAR(200) NOT NULL,

	Created	DateTime default GETUTCDate() NOT NULL,
	Modified DateTime default GETUTCDate() NOT NULL,

	FolderType INT NOT NULL,

	Paging BIT NOT NULL,

	FOREIGN KEY (ParentFolderID) REFERENCES CALDAV.FolderInfo(FolderID),
	FOREIGN KEY (FolderType) REFERENCES CALDAV.FolderType(FolderType)
);

CREATE TABLE CALDAV.CalendarFolderInfo(
	FolderID uniqueidentifier default NEWID() NOT NULL Primary Key,
	ContentType NVARCHAR(200) default 'text/calendar; component=vevent' NOT NULL,
	Modified DateTime default GETUTCDate() NOT NULL,
	CTag [ROWVERSION] NOT NULL,
	CalendarColor NVARCHAR(200),
	CalendarDescription NVARCHAR(200),
	AclPrincipalPropSet BIT NOT NULL,
	PrincipalMatch BIT NOT NULL,
	PrincipalPropertySearch BIT NOT NULL,
	CalendarMultiGet BIT NOT NULL,
	CalendarQuery BIT NOT NULL,
	Foreign Key(FolderID) References CALDAV.FolderInfo(FolderID) 

)

CREATE TABLE CALDAV.CalDavServer(
	SystemID uniqueidentifier default NEWID() NOT NULL Primary Key,
	AllowOptions NVARCHAR(200) NOT NULL,
	PublicOptions NVARCHAR(200) NOT NULL,
	ContextPath uniqueidentifier NOT NULL,
	BaseAclPath NVARCHAR(200) NOT NULL,
	BaseCalendarHomeSetPath NVARCHAR(200) NOT NULL,
	DavLevel1 BIT NOT NULL,
	DavLevel2 BIT NOT NULL,
	DavLevel3 BIT NOT NULL,
	Active Bit NOT NULL,
	Foreign Key(ContextPath) References CALDAV.FolderInfo(FolderID)
);

CREATE TABLE CALDAV.UserProfile(
	UserID uniqueidentifier NOT NULL Primary Key,
	UserName NVARCHAR(200) NOT NULL,
	[Password] NVARCHAR(200) NOT NULL,
	Active BIT NOT NULL,
	CalendarHomeSet uniqueidentifier NOT NULL,
	AclFolder uniqueidentifier NOT NULL,
	Foreign Key(CalendarHomeSet) References CALDAV.FolderInfo (FolderID),
	Foreign Key(AclFolder) References CALDAV.FolderInfo (FolderID)
);

CREATE TABLE CALDAV.UserFolderAccess(
	UserID uniqueidentifier NOT NULL,
	FolderID uniqueidentifier NOT NULL,
	[Path] NVARCHAR(200) NOT NULL,

	AccessControl BIT NOT NULL,
	CalendarAccess BIT NOT NULL,
	CalendarServerSharing BIT NOT NULL,

	[Owner] BIT NOT NULL,
	[Read] BIT NOT NULL,
	[Write] BIT NOT NULL,

	PRIMARY KEY (UserID, FolderID),
	Foreign Key (UserID) References CALDAV.UserProfile(UserID),
	Foreign Key (FolderID) References CALDAV.FolderInfo(FolderID)

);

--##########################################################################################################################

CREATE TABLE CALDAV.CalendarFile(
	FolderID uniqueidentifier NOT NULL,
	CalendarFileId uniqueidentifier default NEWID() NOT NULL,

	Primary Key(FolderID, CalendarFileId),
	Foreign Key(FolderID) References CALDAV.FolderInfo(FolderID),
	Foreign Key(FolderID) References CALDAV.CalendarFolderInfo(FolderID),
	
	[Version] NVARCHAR(50) NULL,
	ProdID NVARCHAR(100) NULL,
	Scale NVARCHAR(100) NULL,
	[ETag] RowVersion NOT NULL,
	Created	DateTime default GETUTCDate() NOT NULL,
	Modified DateTime default GETUTCDate() NOT NULL

);

/*############################################################################################
 * --  Common Tables
  ############################################################################################*/

CREATE TABLE CALDAV.Frequency(
	FrequencyID INTEGER NOT NULL Primary Key,
	[Value] NVARCHAR(15) NOT NULL
);

CREATE TABLE CALDAV.Recurrence(
	RecurrenceID uniqueidentifier default NEWID() NOT NULL Primary Key,

	[Count] INTEGER NULL,
	Interval INTEGER NULL,
	Until DATETIME2 NULL,
	WeekStart NVARCHAR(50) NULL,
	ByMonth INTEGER NULL,
	ByDay NVARCHAR(50) NULL,
	ByMonthDay INTEGER NULL,
	BySetPos NVARCHAR(50) NULL,

	-- has one frequency
	FrequencyID INTEGER,
	FOREIGN KEY (FrequencyID) References CALDAV.Frequency(FrequencyID),
);

CREATE TABLE CALDAV.Category(
	CategoryId uniqueidentifier default NEWID() NOT NULL Primary Key,	
	[Value] NVARCHAR(200) NOT NULL UNIQUE -- need to test that the caterogy doesn't already exist
);

Create TABLE CALDAV.Property(
	PropertyID uniqueidentifier default NEWID() NOT NULL Primary Key,

	[Name] NVARCHAR(255) NOT NULL,
	[Value] NVARCHAR(100) NOT NULL,
	[Parameters] NVARCHAR(max) NOT NULL
);

CREATE TABLE CALDAV.Class(
	ClassId INTEGER NOT NULL Primary Key,
	[Value] NVARCHAR(15) NOT NULL
);

CREATE TABLE CALDAV.Statuses(
	StatusID INTEGER NOT NULL Primary Key,
	[Value] NVARCHAR(15) NOT NULL
);

CREATE TABLE CALDAV.Contact(
	ContactId uniqueidentifier default NEWID() NOT NULL Primary Key,
	[Name] NVARCHAR(200) NOT NULL,
	Email NVARCHAR(200) NOT NULL,
	SentBy NVARCHAR(200) NOT NULL,
	Directory NVARCHAR(200) NOT NULL
);

 CREATE TABLE CALDAV.DateTimeRange(
		DateTimeRangeID uniqueidentifier default NEWID() NOT NULL PRIMARY KEY,
		[From] DateTime2 NULL,
		[TO] DateTime2 NULL
  );

/*############################################################################################
 * --  Event related Tables
 CalendarEvent
  -
  ############################################################################################*/

CREATE TABLE CALDAV.CalendarEvent(
	FolderID uniqueidentifier NOT NULL,
	CalendarFileId uniqueidentifier NOT NULL,
	EventId uniqueidentifier default NEWID() NOT NULL,

	Primary Key(FolderID, CalendarFileId, EventId),
	Foreign Key(FolderID, CalendarFileId) References CALDAV.CalendarFile(FolderID, CalendarFileId),
	/*
	public virtual ICollection<Contact> Attendees { get; set; } --
    public virtual ICollection<Alarm> Alarms { get; set; } --
    public virtual ICollection<string> Categories { get; set; } --
    public virtual ICollection<Uri> Attachments { get; set; } --
	public virtual ICollection<Recurrence> Recurrences { get; set; }
	public ICollection<Tuple<string, string, System.Collections.Specialized.NameValueCollection>> Properties { get; set; }
	*/
	
	Modified DateTime default GETUTCDate() NULL,
	Created DateTime default GETUTCDate() NULL,
	[Description] NVARCHAR(max) NULL,
	IsAllDay BIT NOT NULL,
	[Start] DATETIME2 NULL,
	[End] DATETIME2 NULL,
	[Location] NVARCHAR(255) NULL,
	[Priority] INTEGER NULL,
	[Sequence] INTEGER NULL,
	Summary NVARCHAR(max) NULL,
	Transparency NVARCHAR(255) NULL,
	[Url] NVARCHAR(max) NULL,

	ClassId INTEGER NULL,
	Foreign Key(ClassId) References CALDAV.Class(ClassId),
	Organizer uniqueidentifier NULL,
	Foreign Key(Organizer) References CALDAV.Contact(ContactId),
	[StatusID] INTEGER NULL,
	Foreign Key(StatusId) References CALDAV.Statuses(StatusId),
);
/*
 -- Many Attendees for an event
CREATE TABLE CALDAV.CalendarEventAttendee(
	FolderID uniqueidentifier NOT NULL,
	CalendarFileId uniqueidentifier  NOT NULL,
	EventId uniqueidentifier NOT NULL,
	AttendeeId uniqueidentifier default NEWID() NOT NULL,

	Primary Key(FolderID, CalendarFileId, EventId, AttendeeId),
	FOREIGN KEY(FolderID, CalendarFileId,EventId) REFERENCES CALDAV.CalendarEvent(FolderID, CalendarFileId,EventId),

	[Language] NVARCHAR(50) NULL,
	UserType NVARCHAR(15) NULL,
	DelegatedFrom NVARCHAR(50) NULL,
	DelegatedTo NVARCHAR(50) NULL,
	Rsvp BIT NULL,
	ParticipationRole NVARCHAR(15) NULL,
	ParticipationStatus NVARCHAR(15) NULL,

	ContactId uniqueidentifier NULL,
	FOREIGN KEY(ContactId) REFERENCES CALDAV.Contact(ContactId)
);

-- Many Alarms for an event

Create TABLE CALDAV.CalendarEventAlarmTriggerRelated(
	RelatedID INTEGER NOT NULL Primary Key,
	[Values]  NVARCHAR(10) NOT NULL
);

Create TABLE CALDAV.CalendarEventAlarmTrigger(
	TriggerID uniqueidentifier NOT NULL PRIMARY KEY,

	Duration BIGINT, -- equivalent to TimeSpan.Ticks
	[DateTime] DateTime2,

	RelatedID INTEGER NULL,
	Foreign Key (RelatedID) References CALDAV.CalendarEventAlarmTriggerRelated(RelatedID),
);

CREATE TABLE CALDAV.CalendarEventAlarm(
	FolderID uniqueidentifier NOT NULL,
	CalendarFileId uniqueidentifier NOT NULL,
	EventId uniqueidentifier NOT NULL,
	AlarmId uniqueidentifier default NEWID() NOT NULL,

	Primary Key(FolderID, CalendarFileId, EventId, AlarmId),
	Foreign Key(FolderID, CalendarFileId, EventId) References CALDAV.CalendarEvent(FolderID, CalendarFileId, EventId),
	
	[Action] NVARCHAR(200) NOT NULL,
	[Description] NVARCHAR(200) NOT NULL,

	[TriggerID] uniqueidentifier NOT NULL, -- One to One // Could bring trigger details into here
	Foreign Key(TriggerID) References CALDAV.CalendarEventAlarmTrigger(TriggerID),
);

-- Many categories for an event

CREATE TABLE CALDAV.CalendarEventCategory(
	FolderID uniqueidentifier NOT NULL,
	CalendarFileId uniqueidentifier NOT NULL,
	EventId uniqueidentifier NOT NULL,
	CategoryId uniqueidentifier default NEWID() NOT NULL,
	
	Primary Key(FolderID, CalendarFileId, EventId, CategoryId),
	Foreign Key(FolderID, CalendarFileId, EventId) References CALDAV.CalendarEvent(FolderID, CalendarFileId, EventId),
	Foreign Key(CategoryId) References CALDAV.Category(CategoryId)
);

-- Many Attachments for an event

CREATE TABLE CALDAV.CalendarEventAttachment(
	FolderID uniqueidentifier NOT NULL,
	CalendarFileId uniqueidentifier NOT NULL,
	EventId uniqueidentifier NOT NULL,
	AttachmentId uniqueidentifier default NEWID() NOT NULL,

	Primary Key(FolderID, CalendarFileId, EventId, AttachmentId),
	Foreign Key(FolderID, CalendarFileId, EventId) References CALDAV.CalendarEvent(FolderID, CalendarFileId, EventId),

	MediaType NVARCHAR(50) NULL,
	[Url] NVARCHAR(max) NULL,
	Content VARBINARY(max) NULL,
);

-- Many Recurrenes for an event

CREATE TABLE CALDAV.CalendarEventRecurrence(
	FolderID uniqueidentifier NOT NULL,
	CalendarFileId uniqueidentifier NOT NULL,
	EventId uniqueidentifier NOT NULL,
	RecurrenceID uniqueidentifier default NEWID() NOT NULL ,

	Primary Key(FolderID, CalendarFileId, EventId, RecurrenceID),
	Foreign Key(FolderID, CalendarFileId, EventId) References CALDAV.CalendarEvent(FolderID, CalendarFileId, EventId),
	Foreign Key (RecurrenceID) References CALDAV.Recurrence(RecurrenceID)
);

-- Many Properties for a Calendar Event

Create TABLE CALDAV.CalendarEventProperty(
	FolderID uniqueidentifier NOT NULL,
	CalendarFileId uniqueidentifier NOT NULL,
	EventId uniqueidentifier NOT NULL,
	PropertyID uniqueidentifier default NEWID() NOT NULL,

	Primary Key(FolderID, CalendarFileId, EventId, PropertyID),
	Foreign Key(FolderID, CalendarFileId, EventId) References CALDAV.CalendarEvent(FolderID, CalendarFileId, EventId),
	Foreign Key(PropertyID) References CALDAV.Property(PropertyID)
);
*/
/*############################################################################################
 * --  TimeZone related Tables
 CalendarTimeZone
  -
  ############################################################################################*/

CREATE TABLE CALDAV.CalendarTimeZone(
	FolderID uniqueidentifier NOT NULL,
	CalendarFileId uniqueidentifier NOT NULL,
	TimeZoneId uniqueidentifier default NEWID() NOT NULL,

	Primary Key(FolderID, CalendarFileId, TimeZoneId),
	Foreign Key(FolderID, CalendarFileId) References CALDAV.CalendarFile(FolderID, CalendarFileId),

	--public virtual ICollection<Recurrence> Recurrences { get; set; }

	[Type] NVARCHAR(255) NOT NULL,
	[Name] NVARCHAR(255) NOT NULL,
	[Start] DateTime2 NULL,
	[End] DateTime2 NULL,
	OffsetFrom BIGINT NULL, -- equivalent to TimeSpan.Ticks
	OffsetTo BIGINT NULL, -- equivalent to TimeSpan.Ticks
);
/*
-- Timezone has many recurrences

CREATE TABLE CALDAV.CalendarTimeZoneRecurrence(
	FolderID uniqueidentifier NOT NULL,
	CalendarFileId uniqueidentifier NOT NULL,
	TimeZoneId uniqueidentifier NOT NULL,
	RecurrenceID uniqueidentifier default NEWID() NOT NULL ,

	Primary Key(FolderID, CalendarFileId, TimeZoneId, RecurrenceID),
	Foreign Key(FolderID, CalendarFileId, TimeZoneId) References CALDAV.CalendarTimeZone(FolderID, CalendarFileId, TimeZoneId),
	Foreign Key (RecurrenceID) References CALDAV.Recurrence(RecurrenceID)
);
*/
/*############################################################################################
 * --  ToDo related Tables
 CalendarToDo
  -
  ############################################################################################*/

CREATE TABLE CALDAV.CalendarToDo(
	FolderID uniqueidentifier NOT NULL,
	CalendarFileId uniqueidentifier NOT NULL,
	ToDoId uniqueidentifier default NEWID() NOT NULL,

	Primary Key(FolderID, CalendarFileId, ToDoId),
	Foreign Key(FolderID, CalendarFileId) References CALDAV.CalendarFile(FolderID, CalendarFileId),

	DateTimeStamp DateTime2 NULL,
	[Start] DateTime2 NULL,
	Due DateTime2 Null,
	Summary NVARCHAR(max) NULL,
	[Priority] INTEGER NULL,
	[Sequence] INTEGER NULL,
	Modified DateTime default GETUTCDate() NULL,
	Completed DateTime default GETUTCDate() NULL,

	--public ICollection<Tuple<string, string, System.Collections.Specialized.NameValueCollection>> Properties { get; set; }
	--public virtual ICollection<string> Categories { get; set; }

	StatusID INTEGER NULL,
	FOREIGN KEY (StatusID) References CALDAV.Statuses(StatusID),
	ClassID INTEGER NULL,
	FOREIGN KEY (ClassID) References CALDAV.Class(ClassID)
);
/*
-- Many propertiesies for a ToDo

Create TABLE CALDAV.CalendarToDoProperty(
	FolderID uniqueidentifier NOT NULL,
	CalendarFileId uniqueidentifier NOT NULL,
	ToDoId uniqueidentifier NOT NULL,
	PropertyID uniqueidentifier default NEWID() NOT NULL,

	Primary Key(FolderID, CalendarFileId, ToDoId, PropertyID),
	Foreign Key(FolderID, CalendarFileId, ToDoId) References CALDAV.CalendarToDo(FolderID, CalendarFileId, ToDoId),
	Foreign Key(PropertyID) References CALDAV.Property(PropertyID)
);

-- Many categories for a ToDo

CREATE TABLE CALDAV.CalendarToDoCategory(
	FolderID uniqueidentifier NOT NULL,
	CalendarFileId uniqueidentifier NOT NULL,
	ToDoId uniqueidentifier NOT NULL,
	CategoryId uniqueidentifier default NEWID() NOT NULL,
	
	Primary Key(FolderID, CalendarFileId, ToDoId, CategoryId),
	Foreign Key(FolderID, CalendarFileId, ToDoId) References CALDAV.CalendarToDo(FolderID, CalendarFileId, ToDoId),
	Foreign Key(CategoryId) References CALDAV.Category(CategoryId)
);
*/
/*############################################################################################
 * --  Journal related Tables
 CalendarJournal
  -
  ############################################################################################*/

CREATE TABLE CALDAV.CalendarJournal(
	FolderID uniqueidentifier NOT NULL,
	CalendarFileId uniqueidentifier NOT NULL,
	JournalId uniqueidentifier default NEWID() NOT NULL,

	Primary Key(FolderID, CalendarFileId, JournalId),
	Foreign Key(FolderID, CalendarFileId) References CALDAV.CalendarFile(FolderID, CalendarFileId),

	[Description] NVARCHAR(max) NULL,
	[Sequence] INTEGER NULL,
	Modified DateTime default GETUTCDate() NULL,
	DateTimeStamp DateTime2 NULL,

	Organizer uniqueidentifier NULL,
	Foreign Key(Organizer) References CALDAV.Contact(ContactId),
	StatusID INTEGER NULL,
	FOREIGN KEY (StatusID) References CALDAV.Statuses(StatusID),
	ClassID INTEGER NULL,
	FOREIGN KEY (ClassID) References CALDAV.Class(ClassID)
);
/*
-- Many categories for a Journal

CREATE TABLE CALDAV.CalendarJournalCategory(
	FolderID uniqueidentifier NOT NULL,
	CalendarFileId uniqueidentifier NOT NULL,
	JournalId uniqueidentifier NOT NULL,
	CategoryId uniqueidentifier default NEWID() NOT NULL,
	
	Primary Key(FolderID, CalendarFileId, JournalId, CategoryId),
	Foreign Key(FolderID, CalendarFileId, JournalId) References CALDAV.CalendarJournal(FolderID, CalendarFileId, JournalId),
	Foreign Key(CategoryId) References CALDAV.Category(CategoryId)
);

-- Many properties for a Journal

Create TABLE CALDAV.CalendarJournalProperty(
	FolderID uniqueidentifier NOT NULL,
	CalendarFileId uniqueidentifier NOT NULL,
	JournalId uniqueidentifier NOT NULL,
	PropertyID uniqueidentifier default NEWID() NOT NULL,

	Primary Key(FolderID, CalendarFileId, JournalId, PropertyID),
	Foreign Key(FolderID, CalendarFileId, JournalId) References CALDAV.CalendarJournal(FolderID, CalendarFileId, JournalId),
	Foreign Key(PropertyID) References CALDAV.Property(PropertyID)
);
*/
/*############################################################################################
 * --  FreeBusy related Tables
 CalendarFreeBusy
  -
  ############################################################################################*/


CREATE TABLE CALDAV.CalendarFreeBusy(
	FolderID uniqueidentifier NOT NULL,
	CalendarFileId uniqueidentifier NOT NULL,
	FreeBusyId uniqueidentifier default NEWID() NOT NULL,

	Primary Key(FolderID, CalendarFileId, FreeBusyId),
	Foreign Key(FolderID, CalendarFileId) References CALDAV.CalendarFile(FolderID, CalendarFileId),

	DateTimeStamp DateTime2 NULL,
	[Url] NVARCHAR(max) NULL,
	[Sequence] INTEGER NULL,
	Modified DateTime default GETUTCDate() NULL,
	[Start] DateTime default GETUTCDate() NULL,
	[End] DateTime default GETUTCDate() NULL,

	
	Organizer uniqueidentifier NULL,
	Foreign Key(Organizer) References CALDAV.Contact(ContactId)
);
/*
-- Many properties for a FreeBusy

Create TABLE CALDAV.CalendarFreeBusyProperty(
	FolderID uniqueidentifier NOT NULL,
	CalendarFileId uniqueidentifier NOT NULL,
	FreeBusyId uniqueidentifier NOT NULL,
	PropertyID uniqueidentifier default NEWID() NOT NULL,

	Primary Key(FolderID, CalendarFileId, FreeBusyId, PropertyID),
	Foreign Key(FolderID, CalendarFileId, FreeBusyId) References CALDAV.CalendarFreeBusy(FolderID, CalendarFileId, FreeBusyId),
	Foreign Key(PropertyID) References CALDAV.Property(PropertyID)
);

-- Many DateTimeRange for a FreeBusy

Create TABLE CALDAV.CalendarFreeBusyDateTimeRange(
	FolderID uniqueidentifier NOT NULL,
	CalendarFileId uniqueidentifier NOT NULL,
	FreeBusyId uniqueidentifier NOT NULL,
	DateTimeRangeID uniqueidentifier default NEWID() NOT NULL,

	Primary Key(FolderID, CalendarFileId, FreeBusyId, DateTimeRangeID),
	Foreign Key(FolderID, CalendarFileId, FreeBusyId) References CALDAV.CalendarFreeBusy(FolderID, CalendarFileId, FreeBusyId),
	Foreign Key(DateTimeRangeID) References CALDAV.DateTimeRange(DateTimeRangeID)
);

*/