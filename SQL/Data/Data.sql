/*
INSERT INTO CALDAV.CalendarEventAlarmTriggerRelated(RelatedID, [Values]) VALUES
(1, 'Start'),
(2, 'End');
*/

INSERT INTO CALDAV.Statuses(StatusID, [Value]) VALUES
(1, 'COMPLETED'),
(2, 'CANCELED'),
(3, 'TENTATIVE'),
(4, 'NEEDS_ACTION'),
(5, 'DRAFT');


INSERT INTO CALDAV.Class(ClassId, [Value]) VALUES
(1, 'CONFIDENTIAL'),
(2, 'PRIVATE'),
(3, 'PUBLIC');

INSERT INTO CALDAV.Frequency(FrequencyID, [Value]) VALUES
(1, 'Minutely'),
(2, 'Hourly'),
(3, 'Daily'),
(4, 'Weekly'),
(5, 'Monthly'),
(6, 'Yearly');


INSERT INTO CALDAV.FolderType(FolderType, [Name], [Description]) VALUES
(1, 'WellKnown', 'This is a URL that enables CalDav discovery enabling the CalDav root to be found.' ),
(2,'ContextPath', 'CalDav Servers root folder.'),
(3,'AclFolder', 'Path to access control folders.'),
(4,'CalendarHomeset', 'Contains calendar folders available for the currently logged-in user.'),
(5,'CalendarFolder', 'Folder that contains calendar .ics files.');




INSERT INTO 
CALDAV.CalDavServer(AllowOptions, PublicOptions, ContextPath, BaseAclPath, BaseCalendarHomeSetPath, DavLevel1, DavLevel2, DavLevel3, Active) 
VALUES
(
'OPTIONS, PROPFIND, HEAD, GET, REPORT, PROPPATCH, PUT, DELETE, POST',
'OPTIONS, PROPFIND, HEAD, GET, REPORT, PROPPATCH, PUT, DELETE, POST',
(SELECT FolderID FROM CALDAV.FolderInfo WHERE [Path] = '/CALDAV/'), 
'/CALDAV/acl/users/', 
'/CALDAV/Calendars/', 
1, /*DavLevel1*/
0, /*DavLevel2*/
1, /*DavLevel3*/
1  /*Active*/
);
