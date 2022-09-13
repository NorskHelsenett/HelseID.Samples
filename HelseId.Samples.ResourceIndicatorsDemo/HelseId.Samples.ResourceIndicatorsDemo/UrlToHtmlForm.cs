using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Text;

namespace HelseId.ResourceIndicatorsDemo
{
    /// <summary>
    /// Converts a url into a html form that posts the same data
    /// Quick and dirty workaround to handle long url's
    /// This should probably not be used in a prodution environment :-)
    /// </summary>
    static class UrlToHtmlForm
    {
        public static string Parse(string urlWithQueryString)
        {
            var uri = new Uri(urlWithQueryString);

            var action = uri.GetLeftPart(UriPartial.Path);

            var parameters = QueryHelpers.ParseQuery(uri.Query);

            var html = new StringBuilder();
            html.Append("<html><body>");
            html.AppendFormat("<form id='helseid-form' method='post' action='{0}'>", action);

            foreach (var parameter in parameters)
            {
                foreach (var value in parameter.Value)
                {
                    html.AppendFormat("<input type='hidden' name='{0}' value='{1}'/>", parameter.Key, value);
                }
            }

            html.Append("</form>");
            html.Append("<script type='text/javascript'>window.onload=function () { document.getElementById('helseid-form').submit(); }; </script>");
            html.Append("</body>");
            html.Append("</html>");

            return html.ToString();
        }
    }
}
