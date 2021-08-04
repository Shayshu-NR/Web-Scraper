using Nager.PublicSuffix;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;

namespace WebScraper
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Either read the query string or make a UI...

            try
            {
                string requestURL = HttpUtility.UrlDecode(Request.QueryString["link"]);
                string saveLocation = HttpUtility.UrlDecode(Request.QueryString["path"]);
                string webPage = getPage(requestURL);
                savePage(webPage, saveLocation, requestURL);
            }
            catch(Exception ex)
            {
                Response.Write(ex.Message);
            }

            StringBuilder webScraperUI = new StringBuilder();

            webScraperUI.Append(@"
                <br>
                <div class=""container"">
                   <div class=""row justify-content-center"">
                      <div class=""col-auto"">
                         <div class=""card"">
                            <div class=""card-body"">
                               <h3 class=""card-title text-center"">Web Scraper Tool</h3>
                               <div class=""input-group mb-3"">
                                  <input type=""text"" id=""requestURL"" class=""form-control"" aria-label=""Request URL"" placeholder=""www.example.com"" data-toggle=""tooltip"" data-placement=""auto"" title=""Website to be scraped"">
                                  <input type=""text"" id=""downloadLocation"" class=""form-control"" aria-label=""Save Location"" placeholder=""C:/Users/Downloads"" data-toggle=""tooltip"" data-placement=""auto"" title=""Where to Save website"">
                               </div>
                               <div class=""row text-center"">
                                  <div class=""col-md-12"">
                                    <button type = ""button"" class=""btn btn-primary"">Get Web Page</button>
                                  </div>
                               </div>
                            </div>
                         </div>
                      </div>
                   </div>
                </div>");

            scrapeUI.InnerHtml = webScraperUI.ToString();

        }

        public static string getPage(string requestURL)
        {
            try
            {

                if (!string.IsNullOrEmpty(requestURL))
                {
                    // Make a quick get request
                    Uri uriResult;
                    bool validURL = Uri.TryCreate(requestURL, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                    if (validURL)
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestURL);
                        request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                        StringBuilder webPage = new StringBuilder();

                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        using (Stream stream = response.GetResponseStream())
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            webPage.Append(reader.ReadToEnd());
                        }

                        return webPage.ToString();
                    }
                }
                return "Failed to get web page";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static bool savePage(string webPage, string saveLocation, string requestURL)
        {
            if (!string.IsNullOrEmpty(saveLocation))
            {
                try
                {
                    // Save the data to the specified path...
                    var domainParser = new DomainParser(new WebTldRuleProvider());
                    var domainInfo = domainParser.Parse(requestURL);

                    File.WriteAllText(saveLocation + domainInfo.RegistrableDomain + ".html", webPage.ToString());
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        [WebMethod]
        public static string download(List<string> requestInfo)
        {
            if (requestInfo.Count > 1)
            {
                string requestURL = requestInfo[0];
                string saveLocation = requestInfo[1];
                string webPage = getPage(requestURL);
                return savePage(webPage, saveLocation, requestURL) ? "{ \"Success\" : \"Web Page Saved Successfully\" }" : "{ \"Error\" : \"Failed To Save Web Page\" }";
            }
            else
            {
                return "{ \"Error\" : \"Not Enough Arguments Supplied\" }";
            }
        }
    }

}