using System.ComponentModel.DataAnnotations;

namespace HelseId.Core.BFF.Sample.Api.Options
{
    public class HelseIdOptions
    {
        /// <summary>
        /// OIDC authority for HelseID
        /// </summary>
        [Required]
        public string Authority { get; set; } = null!;

        /// <summary>
        /// Name of the API provided by this application
        /// </summary>
        [Required]
        public string ApiName { get; set; } = null!;

        /// <summary>
        /// Scope for access to the API by a person
        /// </summary>
        [Required]
        public string ApiScope { get; set; } = null!;
    }
}