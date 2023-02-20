using System.Net;
using System.Text;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace HelseId.Samples.ApiAccess;

/// <summary>
/// This is an extension to the OpenIdConnectMessage that allows us to append extra
/// resources (a request object or resource indicators) to the request that
/// is sent to the authorization endpoint, if such resources are needed.
/// An instance of this class is created in the OnRedirectToIdentityProvider
/// event handler in the OPenIdConnectOptionsInitializer class. 
/// </summary>
internal class CustomOpenIdConnectMessage : OpenIdConnectMessage
{
    private readonly CustomOpenIdConnectMessageParameters _messageParameters;

    public CustomOpenIdConnectMessage(
        CustomOpenIdConnectMessageParameters messageParameters,
        OpenIdConnectMessage other) : base(other)
    {
        _messageParameters = messageParameters;
    }

    public override string BuildFormPost()
    {
        var strBuilder = new StringBuilder();
        strBuilder.Append("<html><head><title>");
        strBuilder.Append(WebUtility.HtmlEncode(PostTitle));
        strBuilder.Append("</title></head><body><form method=\"POST\" name=\"hiddenform\" action=\"");
        strBuilder.Append(WebUtility.HtmlEncode(IssuerAddress));
        strBuilder.Append("\">");

        foreach (KeyValuePair<string, string> parameter in Parameters)
        {
            strBuilder.Append("<input type=\"hidden\" name=\"");
            strBuilder.Append(WebUtility.HtmlEncode(parameter.Key));
            strBuilder.Append("\" value=\"");
            strBuilder.Append(WebUtility.HtmlEncode(parameter.Value));
            strBuilder.Append("\" />");
        }

        if (_messageParameters.HasRequestObject)
        {
            // Create a request object
            strBuilder.Append("<input type=\"hidden\" name=\"request");
            strBuilder.Append("\" value=\"");
            strBuilder.Append(_messageParameters.RequestObject);
            strBuilder.Append("\" />");
        }

        foreach (var resource in _messageParameters.ResourceIndicators)
        {
            // Add resource indicators (if any)
            strBuilder.Append("<input type=\"hidden\" name=\"resource");
            strBuilder.Append("\" value=\"");
            strBuilder.Append(WebUtility.HtmlEncode(resource));
            strBuilder.Append("\" />");
        }

        strBuilder.Append("<noscript><p>");
        strBuilder.Append(WebUtility.HtmlEncode(ScriptDisabledText));
        strBuilder.Append("</p><input type=\"submit\" value=\"");
        strBuilder.Append(WebUtility.HtmlEncode(ScriptButtonText));
        strBuilder.Append("\" /></noscript>");
        strBuilder.Append("</form>");
        strBuilder.Append(Script);
        strBuilder.Append("</body></html>");
        return strBuilder.ToString();
    }
}