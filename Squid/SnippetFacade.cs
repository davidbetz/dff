using System;
using System.Linq;
using Squid.Data.Context;
using Squid.Data.Entity;
//+
using FeedCreationLINQ = Squid.Data.Entity.FeedCreation;
using SnippetLINQ = Squid.Data.Entity.Snippet;
using SnippetGroupLINQ = Squid.Data.Entity.SnippetGroup;
//+
namespace Squid
{
    public class SnippetFacade
    {
        //- @GetSnippetData -//
        public static Snippet GetSnippetData(Int32 snippetId)
        {
            using (SquidLINQDataContext db = new SquidLINQDataContext(Configuration.DatabaseConnectionString))
            {
                SnippetLINQ snippetLinq = db.Snippets.SingleOrDefault(p => p.SnippetId == snippetId);
                if (snippetLinq != null)
                {
                    return snippetLinq;
                }
                //+
                return null;
            }
        }
    }
}
