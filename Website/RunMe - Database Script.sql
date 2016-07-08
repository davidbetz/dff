-- create database Squid
use Squid
go

if exists (select * from sysobjects where name = 'FeedCreation' and xtype = 'u')
drop table FeedCreation

if exists (select * from sysobjects where name = 'Snippet' and xtype = 'u')
drop table Snippet

if exists (select * from sysobjects where name = 'SnippetGroup' and xtype = 'u')
drop table SnippetGroup

if exists (select * from sysobjects where name = 'SnippetDated' and xtype = 'v')
drop view SnippetDated
go

create table dbo.SnippetGroup (
SnippetGroupId int primary key identity(1,1) not null,
SnippetGroupTitle varchar(400) Not null,
SnippetGroupModifiedDate datetime not null default(getdate()),
SnippetGroupCreationDate datetime not null default(getdate())
)
go

create table dbo.Snippet(
SnippetId int primary key identity(1,1) not null,
SnippetGroupId int null foreign key references SnippetGroup(SnippetGroupId),
SnippetTitle varchar(200) not null,
SnippetDescription text null,
SnippetExtra varchar(200) null,
SnippetOrder int not null constraint df_informationsnippet_snippetorder  default ((1)),
SnippetValidBegin datetime null,
SnippetValidEnd datetime null,
SnippetModifiedDate datetime not null default (getdate()),
SnippetCreationDate datetime not null default (getdate())
)
go

create table dbo.FeedCreation (
FeedCreationId int primary key identity(1,1) not null,
FeedCreationTitle varchar(200) not null,
FeedCreationDescription varchar(2000) not null,
FeedCreationStatement varchar(4000) not null,
FeedCreationDatabase varchar(200) null,
FeedGuid char(36) not null default (newid()),
FeedAccessViaGuidOnly bit not null default (0),
FeedCreationModifiedDate datetime not null default (getdate()),
FeedCreationCreationDate datetime not null default (getdate())
)
go

create view dbo.SnippetDated
as
select
SnippetId,
SnippetGroupId,
SnippetTitle,
SnippetDescription,
SnippetValidBegin,
SnippetValidEnd
from dbo.Snippet
where (getdate() > SnippetValidBegin) and (getdate() < SnippetValidEnd) or
(getdate() > SnippetValidBegin) and (SnippetValidEnd is null) or
(getdate() < SnippetValidEnd) and (SnippetValidBegin is null) or
(SnippetValidEnd is null) and (SnippetValidBegin is null)
go

-- sample data

insert SnippetGroup select 'Company Announcements', getdate(), getdate()

insert Snippet select 1, 'Client in Office - Week of March 12', '<p>Morbi quis tellus. Duis eget ligula. Duis ligula. Vestibulum nec felis. Cras nulla erat, fringilla vitae, placerat vel, sagittis quis, elit. Suspendisse non dolor. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Donec vulputate lorem a quam. Donec libero nibh, porttitor sed, pharetra dignissim, venenatis in, elit. Cras non nibh. Mauris fringilla sapien quis ante. Praesent vel tortor a leo semper pretium. Curabitur vehicula augue sit amet quam. In rhoncus.</p>', NULL, 1, '2/10/2006 12:00:00 AM', '3/30/2006 12:00:00 AM', '2/23/2007 9:55:37 AM', '2/23/2007 9:55:14 AM'
insert Snippet select 1, 'Happy Independence Day!', '<p>Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Integer urna ligula, dictum vel, fermentum et, congue eu, turpis. Fusce massa dolor, pretium at, ultricies sed, varius a, enim. Quisque aliquam, ipsum hendrerit tincidunt molestie, quam nunc vestibulum ipsum, nec ultrices massa lorem eu ipsum. Duis in pede. Nullam vulputate. Phasellus condimentum, dui pellentesque tempus imperdiet.</p><p>Mi tellus sodales ligula, ut tincidunt augue nisi id sapien. Curabitur placerat elit non lectus. Nulla sollicitudin, lectus eu pharetra vestibulum, odio neque egestas risus, eget varius enim mi nec libero. Integer lectus leo, consectetuer eu, congue ac, luctus lobortis, libero. Duis commodo. Curabitur et tortor tristique diam rhoncus ultrices. Pellentesque tempor nulla id mi. In hac habitasse platea dictumst. Morbi non arcu.</p>', NULL, 1, '6/27/2007 12:00:00 AM', '7/8/2007 12:00:00 AM', '2/23/2007 9:55:37 AM', '2/23/2007 9:55:14 AM'
insert Snippet select 1, 'February Birthdays', '<dl class="Dictionary"><dt>2</dt><dd>Jane Smith<dd><dt>21</dt><dd>John Doe<dd><dt>27</dt><dd>James Johnson<dd></dl>', NULL, 1, '2/1/2007 12:00:00 AM', '3/1/2007 12:00:00 AM', '2/23/2007 9:55:37 AM', '2/23/2007 9:55:14 AM'
insert Snippet select 1, 'March Birthdays', '<dl class="Dictionary"><dt>5</dt><dd>Jim MacArthur<dd><dt>12</dt><dd>Bob Dudley<dd><dt>24</dt><dd>Larry Falu<dd></dl>', NULL, 1, '3/1/2007 12:00:00 AM', '4/1/2007 12:00:00 AM', '2/23/2007 9:55:37 AM', '2/23/2007 9:55:14 AM'
insert Snippet select 1, 'Jeans Day Every Friday', '<p>Donec massa. Aenean tempus purus id arcu. Fusce id arcu dignissim dui euismod dignissim. Nullam non lorem quis nisl congue rhoncus. Donec eu libero. Morbi hendrerit libero. Maecenas risus tellus, bibendum sed, vestibulum ut, molestie ac, magna. Nullam ornare volutpat urna. Suspendisse ultricies, odio quis consequat tempus, est metus dignissim est, nec bibendum felis odio eget urna. Phasellus eget leo quis nibh sagittis scelerisque.</p><p>Aliquam luctus egestas metus. Duis ligula. Nunc ullamcorper, ante vel dignissim tempus, sem dolor blandit magna, id mollis urna quam id justo. Praesent mi. Donec odio sapien, blandit eu, gravida ut, accumsan in, ipsum.  Mauris commodo. </p>', NULL, 1, NULL, NULL, '2/24/2007 6:41:07 PM', '2/24/2007 6:41:07 PM'

insert FeedCreation select 'Company Announcements', '', 'select Id = SnippetId, Title = SnippetTitle, Description = SnippetDescription from SnippetDated where SnippetGroupId = 1', NULL, '75DCDDFF-E457-4CF3-95EC-951B1B648AE7', 0, '2/23/2007 9:56:35 AM', '2/23/2007 9:52:09 AM'
insert FeedCreation select 'SQL Database Size', '', 'select Id=0, Title=filename, Description=convert(varchar(20), size) + ''MB'', LinkTemplate=''http://MyWebSite/SQLProcesses/SqlDatabaseDetails.aspx?File={Title}'' from sysfiles', NULL, '94905906-50ED-4F17-AEAA-0B24857E717B', 1, '2/23/2007 9:56:35 AM', '2/23/2007 9:52:09 AM'
insert FeedCreation select 'SQL Jobs', '', 'select Id=0, Title=name, Description=description from msdb.dbo.sysjobs where enabled = 1', NULL, '9BE0FBBB-FAC4-464A-B183-88B5B5DEB582', 1, '2/23/2007 9:56:35 AM', '2/23/2007 9:52:09 AM'
insert FeedCreation select 'New Database Objects', 'This feed lists database objects created in the past 30 days.', 'select Id=id, Title=name + '' ('' + convert(varchar(100), datediff(dd, crdate, getdate())) + '')'', Description='''' from sysobjects where datediff(dd, crdate, getdate()) < 30 order by datediff(dd, crdate, getdate())', NULL, '23FD44E7-5C93-462D-A2EB-E7CF80597D80', 0, '2/24/2007 2:57:30 PM', '2/24/2007 2:57:30 PM'
insert FeedCreation select 'Top Sales Persons', '', 'select top 10 Id=pc.ContactID, Title=pc.FirstName + '' '' + pc.LastName + '': $'' + convert(varchar(20), convert(numeric(10,2), sum(LineTotal))), Description='''', LinkTemplate = ''ShowContactInformation.aspx?ContactID={id}'' from Sales.SalesOrderDetail sod inner join Sales.SalesOrderHeader soh on soh.SalesOrderID = sod.SalesOrderID inner join Person.Contact pc on pc.ContactID = soh.SalesPersonID group by pc.FirstName, pc.LastName, pc.ContactID order by sum(LineTotal) desc', 'AdventureWorks', '0E0028FD-62E7-4CFA-A2ED-7C415A5C2555', 1, '2/24/2007 5:41:38 PM', '2/24/2007 5:41:38 PM'