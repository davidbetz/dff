using System;
using System.Linq;
using Squid.Data.Context;
//+
using FeedCreationLINQ = Squid.Data.Entity.FeedCreation;
//+
namespace Squid.Builder
{
    internal static class FeedSecurity
    {
        //- ~CheckAccessById -//
        internal static Boolean CheckAccessById(Int32 id)
        {
            using (SquidLINQDataContext db = new SquidLINQDataContext(Configuration.DatabaseConnectionString))
            {
                FeedCreationLINQ feedCreationLinq = db.FeedCreations.SingleOrDefault(p => p.FeedCreationId == id);
                if (feedCreationLinq != null && !feedCreationLinq.FeedAccessViaGuidOnly)
                {
                    return true;
                }
                //+
                return false;
            }
        }

        //- ~CheckAccessByTitle -//
        internal static Boolean CheckAccessByTitle(String title)
        {
            using (SquidLINQDataContext db = new SquidLINQDataContext(Configuration.DatabaseConnectionString))
            {
                FeedCreationLINQ feedCreationLinq = db.FeedCreations.SingleOrDefault(p => p.FeedCreationTitle == title);
                if (feedCreationLinq != null)
                {
                    if (!feedCreationLinq.FeedAccessViaGuidOnly)
                    {
                        return true;
                    }
                }
                //+
                return false;
            }
        }
    }
}