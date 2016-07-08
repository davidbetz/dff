using System;
using System.ServiceModel;
using System.ServiceModel.Syndication;
using System.ServiceModel.Web;
//+
namespace Squid.Service
{
    [ServiceContract(Namespace = "http://www.netfxharmonics.com/services/squid/2008/03/")]
    public interface ISquidService
    {
        //- GetFeedById -//
        [OperationContract]
        [WebGet(UriTemplate = "GetFeedById/{id}")]
        Rss20FeedFormatter GetFeedById(String id);

        //- GetFeedByTitle -//
        [OperationContract]
        [WebGet(UriTemplate = "GetFeedByTitle/{title}")]
        Rss20FeedFormatter GetFeedByTitle(String title);

        //- GetFeedBySecretId -//
        [OperationContract]
        [WebGet(UriTemplate = "GetFeedBySecretId/{secretId}")]
        Rss20FeedFormatter GetFeedBySecretId(String secretId);
    }
}