using HI.SimpleRedirect.Search.SearchTypes;
using Sitecore.ContentSearch;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.HttpRequest;
using System;
using System.Linq;
using System.Web;

namespace HI.SimpleRedirect.Pipelines.HttpRequest
{
    public class ProcessSimpleRedirects : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {

            Assert.ArgumentNotNull(args, "args");

            if (!Sitecore.Context.Site.Name.ToLower().Equals("shell") && args.LocalPath != Constants.Paths.VisitorIdentification && args.LocalPath != Constants.Paths.KeepAlive)
            {
                var indexName = "HI.SimpleRedirect";

                try
                {
                    var simpleRedirectIndex = ContentSearchManager.GetIndex(indexName);
                    if (simpleRedirectIndex != null)
                    {

                        using (var context = simpleRedirectIndex.CreateSearchContext(Sitecore.ContentSearch.Security.SearchSecurityOptions.DisableSecurityCheck))
                        {
                            // Grab the actual requested path for use in both the item and pattern match sections.
                            var requestedUrl = HttpContext.Current.Request.Url.ToString();
                            var requestedUrlwithoutslash = requestedUrl.TrimEnd('/');

                            // Get results
                            var results = context.GetQueryable<SimpleRedirectResultItem>().Where(i => i.Source.Equals(requestedUrl) || i.Source.Equals(requestedUrlwithoutslash)).ToList();

                            if (results.Any())
                            {
                                Log.Info(String.Format("Simple Redirect : match found : {0}", results.FirstOrDefault().Target), this);
                                args.Context.Response.Status = "301 Moved Permanently";
                                args.Context.Response.StatusCode = 301;
                                args.Context.Response.AddHeader("Location", results.FirstOrDefault().Target);
                                args.Context.Response.End();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(String.Format("Simple Redirect Error : {0}", ex.Message), this);
                }
            }
        }
    }   
}
