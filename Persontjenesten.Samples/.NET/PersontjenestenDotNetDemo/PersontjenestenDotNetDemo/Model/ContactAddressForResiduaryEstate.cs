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
    /// Contact information for residuary estate  &lt;br&gt;FREG: KontaktinformasjonForDoedsbo.KontaktAdresseForDoedsbo
    /// </summary>
    [DataContract(Name = "ContactAddressForResiduaryEstate")]
    public partial class ContactAddressForResiduaryEstate : IEquatable<ContactAddressForResiduaryEstate>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactAddressForResiduaryEstate" /> class.
        /// </summary>
        /// <param name="addressLine">Address lines  &lt;br&gt;FREG: Adresselinje.</param>
        /// <param name="city">Post location / city  &lt;br&gt;FREG: Poststedsnavn.</param>
        /// <param name="postalCode">Post code / zip code  &lt;br&gt;FREG: Postnummer.</param>
        /// <param name="countryCode">To letter country code  &lt;br&gt;FREG: Landkode.</param>
        public ContactAddressForResiduaryEstate(List<string> addressLine = default(List<string>), string city = default(string), string postalCode = default(string), string countryCode = default(string))
        {
            this.AddressLine = addressLine;
            this.City = city;
            this.PostalCode = postalCode;
            this.CountryCode = countryCode;
        }

        /// <summary>
        /// Address lines  &lt;br&gt;FREG: Adresselinje
        /// </summary>
        /// <value>Address lines  &lt;br&gt;FREG: Adresselinje</value>
        [DataMember(Name = "addressLine", EmitDefaultValue = true)]
        public List<string> AddressLine { get; set; }

        /// <summary>
        /// Post location / city  &lt;br&gt;FREG: Poststedsnavn
        /// </summary>
        /// <value>Post location / city  &lt;br&gt;FREG: Poststedsnavn</value>
        [DataMember(Name = "city", EmitDefaultValue = true)]
        public string City { get; set; }

        /// <summary>
        /// Post code / zip code  &lt;br&gt;FREG: Postnummer
        /// </summary>
        /// <value>Post code / zip code  &lt;br&gt;FREG: Postnummer</value>
        [DataMember(Name = "postalCode", EmitDefaultValue = true)]
        public string PostalCode { get; set; }

        /// <summary>
        /// To letter country code  &lt;br&gt;FREG: Landkode
        /// </summary>
        /// <value>To letter country code  &lt;br&gt;FREG: Landkode</value>
        [DataMember(Name = "countryCode", EmitDefaultValue = true)]
        public string CountryCode { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class ContactAddressForResiduaryEstate {\n");
            sb.Append("  AddressLine: ").Append(AddressLine).Append("\n");
            sb.Append("  City: ").Append(City).Append("\n");
            sb.Append("  PostalCode: ").Append(PostalCode).Append("\n");
            sb.Append("  CountryCode: ").Append(CountryCode).Append("\n");
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
            return this.Equals(input as ContactAddressForResiduaryEstate);
        }

        /// <summary>
        /// Returns true if ContactAddressForResiduaryEstate instances are equal
        /// </summary>
        /// <param name="input">Instance of ContactAddressForResiduaryEstate to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ContactAddressForResiduaryEstate input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.AddressLine == input.AddressLine ||
                    this.AddressLine != null &&
                    input.AddressLine != null &&
                    this.AddressLine.SequenceEqual(input.AddressLine)
                ) && 
                (
                    this.City == input.City ||
                    (this.City != null &&
                    this.City.Equals(input.City))
                ) && 
                (
                    this.PostalCode == input.PostalCode ||
                    (this.PostalCode != null &&
                    this.PostalCode.Equals(input.PostalCode))
                ) && 
                (
                    this.CountryCode == input.CountryCode ||
                    (this.CountryCode != null &&
                    this.CountryCode.Equals(input.CountryCode))
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
                if (this.AddressLine != null)
                {
                    hashCode = (hashCode * 59) + this.AddressLine.GetHashCode();
                }
                if (this.City != null)
                {
                    hashCode = (hashCode * 59) + this.City.GetHashCode();
                }
                if (this.PostalCode != null)
                {
                    hashCode = (hashCode * 59) + this.PostalCode.GetHashCode();
                }
                if (this.CountryCode != null)
                {
                    hashCode = (hashCode * 59) + this.CountryCode.GetHashCode();
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
