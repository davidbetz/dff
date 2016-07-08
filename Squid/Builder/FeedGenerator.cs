using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel.Syndication;
using Squid.Data.Context;
using Squid.Validation;
//+
using FeedCreationLINQ = Squid.Data.Entity.FeedCreation;
using SnippetLINQ = Squid.Data.Entity.Snippet;
using SnippetGroupLINQ = Squid.Data.Entity.SnippetGroup;
//+
namespace Squid.Builder
{
    internal static class FeedGenerator
    {
        //- @GetRssFeedText -//
        internal static SyndicationFeed GetRssFeedText(Int32 id, Uri link)
        {
            if (FeedSecurity.CheckAccessById(id))
            {
                return FeedGenerator.GetRssFeedTextById(id, link);
            }
            else
            {
                FaultThrower.ThrowSecurityException("This feed only allows access by SecretId.");
                return null;
            }
        }

        //- @GetRssFeedText -//
        internal static SyndicationFeed GetRssFeedText(String title, Uri link)
        {
            if (FeedSecurity.CheckAccessByTitle(title))
            {
                return FeedGenerator.GetRssFeedTextById(FeedGenerator.GetFeedIdByTitle(title), link);
            }
            else
            {
                FaultThrower.ThrowSecurityException("This feed only allows access by SecretId.");
                return null;
            }
        }

        //- @GetRssFeedText -//
        internal static SyndicationFeed GetRssFeedText(Guid secretId, Uri link)
        {
            return FeedGenerator.GetRssFeedTextById(FeedGenerator.GetFeedIdBySecretId(secretId), link);
        }

        //- @GetRssFeedTextById -//
        private static SyndicationFeed GetRssFeedTextById(Int32 id, Uri target)
        {
            return FeedGenerator.GenerateFeed(id, target);
        }

        //- @GetFeedIdBySecretId -//
        private static Int32 GetFeedIdBySecretId(Guid secretId)
        {
            using (SquidLINQDataContext db = new SquidLINQDataContext(Configuration.DatabaseConnectionString))
            {
                FeedCreationLINQ feedCreationLinq = db.FeedCreations.SingleOrDefault(p => p.FeedGuid == secretId.ToString());
                if (feedCreationLinq == null)
                {
                    FaultThrower.ThrowFeedGenerationException("Invalid title.");
                }
                //+
                return feedCreationLinq.FeedCreationId;
            }
        }

        //- @GetFeedIdByTitle -//
        private static Int32 GetFeedIdByTitle(String title)
        {
            using (SquidLINQDataContext db = new SquidLINQDataContext(Configuration.DatabaseConnectionString))
            {
                FeedCreationLINQ feedCreationLinq = db.FeedCreations.SingleOrDefault(p => p.FeedCreationTitle == title);
                if (feedCreationLinq == null)
                {
                    FaultThrower.ThrowFeedGenerationException("Invalid title.");
                }
                //+
                return feedCreationLinq.FeedCreationId;
            }
        }

        //- @GenerateFeed -//
        private static SyndicationFeed GenerateFeed(Int32 feedId, Uri actualLink)
        {
            using (SquidLINQDataContext db = new SquidLINQDataContext(Configuration.DatabaseConnectionString))
            {
                
                FeedCreationLINQ feed = db.FeedCreations.Single(p => p.FeedCreationId == feedId);
                //+
                return GenerateFeed(feedId, actualLink, feed.FeedCreationTitle, feed.FeedCreationDescription, feed.FeedCreationStatement, feed.FeedCreationDatabase);
            }
        }

        //- @GenerateFeed -//
        private static SyndicationFeed GenerateFeed(Int32 feedId, Uri actualLink, String title, String description, String statement, String database)
        {
            Validator.IsNotBlank(title, "Title is required.");
            Validator.IsNotBlank(statement, "SQL Statement is required.");
            if (description.Length < 1)
            {
                description = " ";
            }
            //+
            SyndicationFeed feed = new SyndicationFeed()
            {
                Title = new TextSyndicationContent(title),
                Description = new TextSyndicationContent(description)
            };
            feed.Links.Add(new SyndicationLink(actualLink));
            using (SqlConnection connection = new SqlConnection(Configuration.DatabaseConnectionString))
            {
                connection.Open();
                if (!String.IsNullOrEmpty(database))
                {
                    connection.ChangeDatabase(database);
                }
                using (SqlCommand command = new SqlCommand(statement, connection))
                {
                    command.CommandType = CommandType.Text;
                    SqlDataReader reader = command.ExecuteReader();
                    List<SyndicationItem> items = new List<SyndicationItem>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            SyndicationItem item = new SyndicationItem();
                            Int32 id = 0;
                            if (reader["Id"] != null)
                            {
                                if (Int32.TryParse(reader["Id"].ToString(), out id))
                                {
                                }
                            }
                            //+
                            item.Title = new TextSyndicationContent((String)reader["Title"]);
                            //+
                            if (reader["Description"] is System.String)
                            {
                                item.Summary = new TextSyndicationContent((String)reader["Description"]);
                            }
                            //+
                            String template = String.Empty;
                            try
                            {
                                template = reader["LinkTemplate"].ToString();
                            }
                            catch
                            {
                            }
                            //+
                            SyndicationLink link = new SyndicationLink(GetEntryLink(id, title, description, template));
                            if (link != null)
                            {
                                item.Links.Add(link);
                            }
                            items.Add(item);
                        }
                        feed.Items = items;
                    }
                    else
                    {
                        SyndicationItem item = new SyndicationItem
                        {
                            Title = new TextSyndicationContent("No entries"),
                            Summary = new TextSyndicationContent(String.Empty)
                        };
                        item.Links.Add(new SyndicationLink(actualLink));
                        //+
                        items.Add(item);
                    }
                    feed.Items = items;
                }
            }
            //+
            return feed;
        }

        //- @GetEntryLink -//
        private static Uri GetEntryLink(Int32 id, String title, String description, String template)
        {
            if (!String.IsNullOrEmpty(template))
            {
                String parsedLink = TemplateParser.ParseLink(template, id, title, description);

                if (parsedLink.StartsWith("http://") || parsedLink.StartsWith("https://"))
                {
                    return new Uri(parsedLink);
                }
                else
                {
                    //+ no parsing codes (i.e. {id}) found
                    return new Uri(ConfigurationFacade.ApplicationSettings("SiteRoot") + template);
                }
            }
            else if (id > 0)
            {
                return new Uri(TemplateParser.ParseLink(ConfigurationFacade.ApplicationSettings("SiteRoot") + Configuration.DefaultEntryLinkTemplate, id, title, description));
            }
            else
            {
                FaultThrower.ThrowInvalidOperationException("LinkTemplate must be set or ID must be greater than 0");
                return null;
            }
        }
    }
}