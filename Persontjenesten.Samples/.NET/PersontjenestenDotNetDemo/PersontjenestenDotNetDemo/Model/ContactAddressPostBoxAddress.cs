/*
 * Persontjenesten API - ET
 *
 * ## Introduction  The Person API is a copy of the [National Population Register (\"Folkeregisteret\")](https://www.skatteetaten.no/en/person/national-registry/about/) serving the norwegian health sector, maintained by Norsk helsenett. More detailed information on data coming from the National Population Register is also available in Norwegian here: [Information Model](https://skatteetaten.github.io/folkeregisteret-api-dokumentasjon/informasjonsmodell/)  More detailed information about the Person API service, including how to get access, is documented here: [Persontjenesten](https://www.nhn.no/samhandlingsplattform/grunndata/persontjenesten)  ## Disclaimer  The Person API is under continuous development and will be subject to changes without notice. The changes will follow semantic versioning to prevent breaking changes. Legacy versions will be available for 6 months before they are discontinued. We encourage consumers to follow our changelog in order to keep track of any changes. Send feedback and questions to [persontjenesten@nhn.no](mailto:persontjenesten@nhn.no)  ## Changelog  See [Changelog](../static/changelog/index.html)  ## Synthetic test data  Data in our test environment is using synthetic test data coming from the [Synthetic National Register](https://skatteetaten.github.io/testnorge-tenor-dokumentasjon/kilder#syntetisk-folkeregister). To browse the data available, you can log in using ID-porten at [Testnorge](https://testdata.skatteetaten.no/web/testnorge/)  ## Authentication and authorization  This API uses [HelseID](https://www.nhn.no/samhandlingsplattform/helseid) for authentication and authorization. To use the API you will need to have a valid HelseID token with a valid scope.  There are two scopes available to consume resources from the Person API: - **ReadWithLegalBasis** | Scope: `nhn:hgd-persontjenesten-api/read-with-legal-basis`    This scope provides read access to information in the authorization bundle \"public with legal basis\" (aka statutory authority).    For version 0.5 name was `nhn:hgd-persontjenesten-api/read`  - **ReadNoLegalBasis** | Scope: `nhn:hgd-persontjenesten-api/read-no-legal-basis`    This scope provides read access to information in the public bundle \"public with**out** legal basis\". 
 *
 * The version of the OpenAPI document: 1.5
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */


using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using OpenAPIDateConverter = Org.OpenAPITools.Client.OpenAPIDateConverter;

namespace Org.OpenAPITools.Model
{
    /// <summary>
    /// &lt;br&gt;FREG: Postboksadresse
    /// </summary>
    [DataContract(Name = "ContactAddress_postBoxAddress")]
    public partial class ContactAddressPostBoxAddress : IEquatable<ContactAddressPostBoxAddress>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactAddressPostBoxAddress" /> class.
        /// </summary>
        /// <param name="postBoxOwner">Person owning or renting the post box  &lt;br&gt;FREG: Postbokseier.</param>
        /// <param name="postBoxIdentification">Post box identifier consisting of its alphanumeric identification  together with the postbox office name.  &lt;br&gt;FREG: Postboks.</param>
        /// <param name="city">city.</param>
        public ContactAddressPostBoxAddress(string postBoxOwner = default(string), string postBoxIdentification = default(string), CadastralAddressCity city = default(CadastralAddressCity))
        {
            this.PostBoxOwner = postBoxOwner;
            this.PostBoxIdentification = postBoxIdentification;
            this.City = city;
        }

        /// <summary>
        /// Person owning or renting the post box  &lt;br&gt;FREG: Postbokseier
        /// </summary>
        /// <value>Person owning or renting the post box  &lt;br&gt;FREG: Postbokseier</value>
        [DataMember(Name = "postBoxOwner", EmitDefaultValue = true)]
        public string PostBoxOwner { get; set; }

        /// <summary>
        /// Post box identifier consisting of its alphanumeric identification  together with the postbox office name.  &lt;br&gt;FREG: Postboks
        /// </summary>
        /// <value>Post box identifier consisting of its alphanumeric identification  together with the postbox office name.  &lt;br&gt;FREG: Postboks</value>
        [DataMember(Name = "postBoxIdentification", EmitDefaultValue = true)]
        public string PostBoxIdentification { get; set; }

        /// <summary>
        /// Gets or Sets City
        /// </summary>
        [DataMember(Name = "city", EmitDefaultValue = true)]
        public CadastralAddressCity City { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class ContactAddressPostBoxAddress {\n");
            sb.Append("  PostBoxOwner: ").Append(PostBoxOwner).Append("\n");
            sb.Append("  PostBoxIdentification: ").Append(PostBoxIdentification).Append("\n");
            sb.Append("  City: ").Append(City).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as ContactAddressPostBoxAddress);
        }

        /// <summary>
        /// Returns true if ContactAddressPostBoxAddress instances are equal
        /// </summary>
        /// <param name="input">Instance of ContactAddressPostBoxAddress to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ContactAddressPostBoxAddress input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.PostBoxOwner == input.PostBoxOwner ||
                    (this.PostBoxOwner != null &&
                    this.PostBoxOwner.Equals(input.PostBoxOwner))
                ) && 
                (
                    this.PostBoxIdentification == input.PostBoxIdentification ||
                    (this.PostBoxIdentification != null &&
                    this.PostBoxIdentification.Equals(input.PostBoxIdentification))
                ) && 
                (
                    this.City == input.City ||
                    (this.City != null &&
                    this.City.Equals(input.City))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.PostBoxOwner != null)
                {
                    hashCode = (hashCode * 59) + this.PostBoxOwner.GetHashCode();
                }
                if (this.PostBoxIdentification != null)
                {
                    hashCode = (hashCode * 59) + this.PostBoxIdentification.GetHashCode();
                }
                if (this.City != null)
                {
                    hashCode = (hashCode * 59) + this.City.GetHashCode();
                }
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        public IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }

}
