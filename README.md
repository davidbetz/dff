# Data Feed Framework

**Copyright (c) 2005-2008 David Betz <dfb@davidbetz.net>**

Back in late 2005 I created a system which I released in 2007 under the title of Data Feed Framework (DFF) which I later started to call Sql Feed Framework. Now I'm releasing a newly refactored edition, which relies on WCF for RSS creation and aggregation in place of RSS.NET, called simply Squid. This library exists to greatly simplify the creation of RSS feeds and the aggregation of RSS and Atom feeds on websites. The goal of the feed creation portion of the system was to allow developers and IT professionals to create RSS feeds with little to no work at all thereby allowing them to use RSS for purposes other than blogging or news. The goal of the aggrgator portion of DFF was to the integration of information into ASP.NET 2.0 pages.

Original 2005 version was based on LLBLGen and RSS.NET, see project on Codeplex to download that version. https://squid.codeplex.com/releases/view/12519

![](https://github.com/davidbetz/dff/raw/master/readme.png)

### The Data Feed Framework has two primary components:

*   FeedCreation
*   InfoBlocks

Using the FeedCreation mechanism, developers and IT professionals can quickly create RSS feeds to monitor internal SQL databases, create sales RSS feeds for management, or monitor whatever else they wanted. The aggregator portion of the system, called InfoBlocks, allows web developers to declare a control in ASP.NET 2.0, give it an RSS or Atom feed and walk away having an InfoBlock on their page containing a list of feed entries from the RSS feed.

The two portions of the system work together, but can also be used separately.

To make explaining this a bit easier, below is a link to my documentation for the system. Below that are the links for the source of the system as well as screen shots. I'll soon be creating a video to go over the basic concepts of this system.

I can't overemphasis how simply this system makes RSS creation and RSS/Atom aggregation. It's literally a matter writing a SQL statement. Absolutely no more work than that is required. The simplicity is very much like that simplicity in WCF. Where WCF relied primarily on attributes and a config file, I rely simply on a SQL Server 2005 table called "FeedCreation". You literally write a SQL statement and give it a name and you have a complete RSS feed ready for corporate or public distribution. I'm planning on incorporating this system into the core of Minima in the next CTP.

The following documentation explains step-by-step how the FeedCreator and InfoBlocks work. *Please read this completely*.

### Simply Feed Creation

To create a feed, go to the FeedCreation table put a Title, Description (which can be blank), and a SQL statement. That's all that's required to create a feed. The feed is then accessible via the LoadFeed.aspx page. For example, go to the FeedCreation table and put the following information in:

<pre>FeedCreationTitle: SQL Jobs
FeedCreationStatement: select Id=0, Title=name, Description=description from msdb.dbo.sysjobs where enabled = 1</pre>

Leave all others as defaults, then you can access the feed with:

    LoadFeed.aspx?title=SQL+Jobs

Or, if the **FeedCreationId** is 29, you can use this:

    LoadFeed.aspx?id=29

However, this may be something you don't want people to guess. If they know they can type in LoadFeed.aspx?id=# to get a feed, they can start guessing numbers.

To get around this...

### Secret Feed Creation

... simply set **FeedAccessViaGuidOnly** to true and then you may only access it with the Guid in FeedGuid like this:

    LoadFeed.aspx?secretId=9BE0FBBB-FAC4-464A-B183-88B5B5DEB582

Now you have a feed that isn't publicly advertised. There's really no way anyone is going to access that feed unless they are given the Guid.

### ...as an InfoBlock

Now, if you were to use an **InfoBlock** to access this feed you simply have to register the InfoBlock control assembly with the page and declare an InfoBlock.

For example, to register the assembly with the page, the use the following syntax:

    <%@ Register Assembly="InformationAccessPointControls" Namespace="Iap.Controls" TagPrefix="iap" %>

Now, to declare an InfoBlock, use the following syntax, setting the **DataSource** to the feed you want the **InfoBlock** to use:

    <iap:InfoBlock ID="ibBlock3" runat="server" **DataSource="http://MyHost/LoadFeed.aspx?secretId=9BE0FBBB-FAC4-464A-B183-88B5B5DEB582"**></iap:InfoBlock>

...which would show a list of entries in the feed with each entry's link set whatever is in the **DefaultEntryLinkTemplate** item in the web configuration file.

For this scenario it may not matter where the link goes as you may not even need a description. Perhaps all you care about are the titles. However, if you did care about the description you can rely on the **DefaultEntryLinkTemplate** to have a custom page display the description for you. Furthermore, the **DefaultEntryLinkTemplate** is actually a link template allowing you more flexibility than a mere link would.

For example, if **"SnippetViewer.aspx?id={id}"** was the **DefaultEntryLinkTemplate**, then the **{id}** would be replaced with the ID of the entry. So, clicking on item 28 in the feed would take you to "**SnippetViewer.aspx?id=28**". You could also use **{title}** or **{description}** if you needed to, though **{id}** will probably be used more than anything else.

**SnippetViewer.aspx** is provided as an example of what could be used to display snippets.  You could either use this page (with some rather extensive styling) or you could create your own page and completely change the value of **DefaultEntryLinkTemplate**.

### Custom Link Templates

Now, say you wanted this, and only this, feed to have links going to a page you created, but you didn't want to change the **DefaultEntryLinkTemplate** as that would affect all feeds.  Perhaps you setup a SQLMonitor.aspx page that will help you monitor your system. Assuming that you can send paramaters to the page, you can use your own custom template by altering the feed creation statement:

    select Id=0, Title=name, Description=description, LinkTemplate='SQLMonitor.aspx?SqlJobId={id}' from msdb.dbo.sysjobs where enabled = 1

Now, the InfoBlock will show your own link.

### Schedules Entries

Say you want to setup a company feed to advertise company announcements, however you don't want to micromanage the announcements. Using this system, you can schedule your annoucements to automatically show at a certain date and time and then automatically disappear at a different date and time. To see this, lets first setup a **SnippetGroup** called "Company Announcements". You do this simply by putting a record in the **SnippetGroup** table.

Now, in the **FeedCreation** table, use this **FeedCreationStatment**:

<pre>  select Id = SnippetId, Title = SnippetTitle, Description = SnippetDescription from **SnippetDated**where SnippetGroupId = 12</pre>

Please note that this statement uses the **SnippetDated** view, not <u>not</u> the Snippet table directly. This is important when working with scheduling.

Lastly, in the **Snippet** table start putting in some snippets, setting each snippet's **SnippetGroupId** to the **SnippetGroupId** obtained from the **SnippetGroup**.

#### Snippet Titles:

*   Client in Office - Week of March 12
*   Happy Independence Day!
*   February Birthdays
*   March Birthdays
*   Jeans Day Every Friday

You'll also probably want to put in some descriptions for each of these. Samples are given in the provided SQL Server 2005 database.

Now, say you want to set the February Birthdays entry to only show for the month of February and the March Birthdays entry to only show up for the month of march. To do this just put in the start and end dates in the **SnippetValidBegin** and **SnippetValidEnd** columns, respectively.

When someone views the feed by any of the following means, they will see only the entries appropriate for that moment. If the **SnippetValidBegin** and **SnippetValidEnd** columns are null, the entries will always show up:

#### Possibly ways to access this feed:

    LoadFeed.aspx?id=30
    LoadFeed.aspx?title=Company+Announcements
    LoadFeed.aspx?secretId=75DCDDFF-E457-4CF3-95EC-951B1B648AE7

### More Complex SQL

As you saw in the SQL Server jobs example, you do not have to use the **Snippet** and **SnippetGroup** tables. You can use anything you want. But that's not all. You don't even have to stay in the same database and you don't have to use simple SQL statements.

Here's an example of creating a feed to get the top 10 sales persons with the best sales from the AdventureWorks database.

Use this as your FeedCreationStatement:

<pre>select
Id=pc.ContactID,
Title=pc.FirstName + ' ' + pc.LastName + ': ' + convert(varchar(20), convert(numeric(10,2), sum(LineTotal))),
Description='',
LinkTemplate = 'ShowContactInformation.aspx?ContactID={id}'
from Sales.SalesOrderDetail sod
inner join Sales.SalesOrderHeader soh on soh.SalesOrderID = sod.SalesOrderID
inner join Person.Contact pc on pc.ContactID = soh.SalesPersonID
group by pc.FirstName, pc.LastName, pc.ContactID order by sum(LineTotal) desc</pre>

Then, you need to put AdventureWorks in the **FeedCreationDatabase** column of the same record.

This SQL Server statement will refresh every time the feed is accessed, unless you have some sort of caching mechanism to prevent this.  Also notice that this statement has **LinkTemplate** set.

For something like this, you probably won't need to schedule it, though you could, but you may want to set **FeedAccessViaGuidOnly** to true to prevent just anyone from accessing it.

In addition to more complicated SQL statements, you could also use stored procedures, but I won't show an example of that as it should be fairly self explanatory at this point.

### Registering a Feed with a Web Browser

Both Mozilla Firefox (1.0 and beyond) and Internet Explorer 7 allow for the ability to register a feed with a web browser via the use of a <link /> element. When you place an **InfoBlock** on a screen, you can tell it to register the specified feed with the web browser by setting the **InfoBlock** property of **RegisterFeedWithBrowser** to True. This will tell Firefox and IE about the link and they will in turn show the feed icon (usually the orange feed icon) in the address bar.

For example, to register the company announcement feed with Firefox or IE7, you will have this:

    <iap:InfoBlock ID="ibBlock3" runat="server" **RegisterFeedWithBrowser="True"** DataSource="http://MyHost/LoadFeed.aspx?secretId=75DCDDFF-E457-4CF3-95EC-951B1B648AE7"></iap:InfoBlock>

Now the user will see the feed icon in his or her address bar. In Firefox, the user could click the icon and set it as a Firefox Live Bookmark, which looks and feels like a bookmark, but automatically updates as the feed changes.

### Other Feeds

The InfoBlocks could also used to show other RSS feeds.

For example, the following creates and InfoBlock for my NetFX Harmonics feed:

    <iap:InfoBlock ID="ibBlock9" runat="server" DataSource="http://feeds.feedburner.com/FXHarmonics/" Visible="true"></iap:InfoBlock>

For something like this, you probably wouldn't register with the browser, but you may want to have as a different size, either taller or wider

### Block Sizes

To set this, simply change the **BlockSize** property of the **InfoBlock** to either **Wide** or **Tall**, depending on what you want.

For example, this sets a wide **InfoBlock**...

    <iap:InfoBlock ID="ibBlock9" runat="server" **BlockSize="Wide"** DataSource="http://feeds.feedburner.com/FXHarmonics/" Visible="true"></iap:InfoBlock></pre>

... and this sets a tall InfoBlock.

    <iap:InfoBlock ID="ibBlock3" runat="server" **BlockSize="Tall"** RegisterFeedWithBrowser="True" DataSource="http://MyHost/LoadFeed.aspx?secretId=9BE0FBBB-FAC4-464A-B183-88B5B5DEB582"></iap:InfoBlock>
