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
    /// A number and maybe letters that uniquely identifiers property,  facilities, buildings or entrances to building within  an addressable street, road, path, place or area  as registered in the cadastral of a municipality  &lt;br&gt;FREG: Adressenummmer
    /// </summary>
    [DataContract(Name = "StreetAddress_addressNumber")]
    public partial class StreetAddressAddressNumber : IEquatable<StreetAddressAddressNumber>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreetAddressAddressNumber" /> class.
        /// </summary>
        /// <param name="houseNumber">A number that uniquely identifiers property,  facilities, buildings or entrances to building within  an addressable street, road, path, place or area  &lt;br&gt;FREG: Husnummer.</param>
        /// <param name="houseLetter">A letter that together with HouseNumber  uniquely identifiers property, facilities, buildings  or entrances to building within an addressable  street, road, path, place or area  &lt;br&gt;FREG: Husbokstav.</param>
        public StreetAddressAddressNumber(string houseNumber = default(string), string houseLetter = default(string))
        {
            this.HouseNumber = houseNumber;
            this.HouseLetter = houseLetter;
        }

        /// <summary>
        /// A number that uniquely identifiers property,  facilities, buildings or entrances to building within  an addressable street, road, path, place or area  &lt;br&gt;FREG: Husnummer
        /// </summary>
        /// <value>A number that uniquely identifiers property,  facilities, buildings or entrances to building within  an addressable street, road, path, place or area  &lt;br&gt;FREG: Husnummer</value>
        [DataMember(Name = "houseNumber", EmitDefaultValue = true)]
        public string HouseNumber { get; set; }

        /// <summary>
        /// A letter that together with HouseNumber  uniquely identifiers property, facilities, buildings  or entrances to building within an addressable  street, road, path, place or area  &lt;br&gt;FREG: Husbokstav
        /// </summary>
        /// <value>A letter that together with HouseNumber  uniquely identifiers property, facilities, buildings  or entrances to building within an addressable  street, road, path, place or area  &lt;br&gt;FREG: Husbokstav</value>
        [DataMember(Name = "houseLetter", EmitDefaultValue = true)]
        public string HouseLetter { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class StreetAddressAddressNumber {\n");
            sb.Append("  HouseNumber: ").Append(HouseNumber).Append("\n");
            sb.Append("  HouseLetter: ").Append(HouseLetter).Append("\n");
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
            return this.Equals(input as StreetAddressAddressNumber);
        }

        /// <summary>
        /// Returns true if StreetAddressAddressNumber instances are equal
        /// </summary>
        /// <param name="input">Instance of StreetAddressAddressNumber to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(StreetAddressAddressNumber input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.HouseNumber == input.HouseNumber ||
                    (this.HouseNumber != null &&
                    this.HouseNumber.Equals(input.HouseNumber))
                ) && 
                (
                    this.HouseLetter == input.HouseLetter ||
                    (this.HouseLetter != null &&
                    this.HouseLetter.Equals(input.HouseLetter))
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
                if (this.HouseNumber != null)
                {
                    hashCode = (hashCode * 59) + this.HouseNumber.GetHashCode();
                }
                if (this.HouseLetter != null)
                {
                    hashCode = (hashCode * 59) + this.HouseLetter.GetHashCode();
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
