using System;
using System.ServiceModel.Syndication;
using System.ServiceModel.Web;
using Squid.Builder;
//+
namespace Squid.Service
{
    public class SquidService : ISquidService
    {
        //- @GetFeedById -//
        public Rss20FeedFormatter GetFeedById(String id)
        {
            Int32 idInt32;
            if (Int32.TryParse(id, out idInt32))
            {
                return new Rss20FeedFormatter(FeedGenerator.GetRssFeedText(idInt32, WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri));
            }
            else
            {
                FaultThrower.ThrowArgumentException("Invalid ID.");
                return null;
            }
        }

        //- @GetFeedByTitle -//
        public Rss20FeedFormatter GetFeedByTitle(String title)
        {
            title = title.Replace("+", " ");
            return new Rss20FeedFormatter(FeedGenerator.GetRssFeedText(title, WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri));
        }

        //- @GetFeedBySecretId -//
        public Rss20FeedFormatter GetFeedBySecretId(String secretId)
        {
            Guid guid;
            try
            {
                guid = new Guid(secretId);
            }
            catch
            {
                FaultThrower.ThrowArgumentException("Invalid secret ID.");
                return null;
            }
            //+
            return new Rss20FeedFormatter(FeedGenerator.GetRssFeedText(guid, WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri));
        }
    }
}