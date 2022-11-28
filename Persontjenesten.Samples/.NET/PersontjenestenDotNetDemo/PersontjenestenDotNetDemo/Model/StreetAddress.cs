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
    /// Street address  Describes the address where a person lives, stays or has shared parental residence  &lt;br&gt;FREG: Vegadresse
    /// </summary>
    [DataContract(Name = "StreetAddress")]
    public partial class StreetAddress : IEquatable<StreetAddress>, IValidatableObject
    {

        /// <summary>
        /// Categorization of occupancy unit type  &lt;br&gt;FREG: Bruksenhetstype
        /// </summary>
        /// <value>Categorization of occupancy unit type  &lt;br&gt;FREG: Bruksenhetstype</value>
        [DataMember(Name = "separatelyOccupiedUnitType", EmitDefaultValue = true)]
        public SeparatelyOccupiedUnitType? SeparatelyOccupiedUnitType { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="StreetAddress" /> class.
        /// </summary>
        /// <param name="municipalityNumber">A number identifying a municipality or  municipality-like area  &lt;br&gt;FREG: Kommunenummer.</param>
        /// <param name="separatelyOccupiedUnitNumber">A letter and four digits that uniquely identifies the  sperately occupied unit inside a addressable building  or part of a building  &lt;br&gt;Remarks:   Two first digits represent floor number  Freg: Bruksenhetsnummer.</param>
        /// <param name="separatelyOccupiedUnitType">Categorization of occupancy unit type  &lt;br&gt;FREG: Bruksenhetstype.</param>
        /// <param name="addressName">Name of a street, road, path, place or area  as registered in the cadastral of a municipality  &lt;br&gt;Remarks:   Does not contain street address number (housing number and lettering)  Freg: Adressenavn.</param>
        /// <param name="addressNumber">addressNumber.</param>
        /// <param name="addressCode">Number that uniquely identifies an addressable  street, road, path, place or area  &lt;br&gt;Remarks:   Known as StreetCode (\&quot;gatekode\&quot;) in DSF  Freg: Adressekode.</param>
        /// <param name="addressAdditionalName">Inherited farm name (bruksnavn) or name of a institution or building,  used as a part of the official address  &lt;br&gt;FREG: Addressetilleggsnavn.</param>
        /// <param name="city">city.</param>
        /// <param name="coAddressName">Description of who the recipient is in care of (C/O),  or which recipient in an organization (v/ &#x3D; with, or Att: &#x3D; \&quot;Attention\&quot;)  &lt;br&gt;FREG: CoAdressenavn.</param>
        public StreetAddress(string municipalityNumber = default(string), string separatelyOccupiedUnitNumber = default(string), SeparatelyOccupiedUnitType? separatelyOccupiedUnitType = default(SeparatelyOccupiedUnitType?), string addressName = default(string), StreetAddressAddressNumber addressNumber = default(StreetAddressAddressNumber), string addressCode = default(string), string addressAdditionalName = default(string), CadastralAddressCity city = default(CadastralAddressCity), string coAddressName = default(string))
        {
            this.MunicipalityNumber = municipalityNumber;
            this.SeparatelyOccupiedUnitNumber = separatelyOccupiedUnitNumber;
            this.SeparatelyOccupiedUnitType = separatelyOccupiedUnitType;
            this.AddressName = addressName;
            this.AddressNumber = addressNumber;
            this.AddressCode = addressCode;
            this.AddressAdditionalName = addressAdditionalName;
            this.City = city;
            this.CoAddressName = coAddressName;
        }

        /// <summary>
        /// A number identifying a municipality or  municipality-like area  &lt;br&gt;FREG: Kommunenummer
        /// </summary>
        /// <value>A number identifying a municipality or  municipality-like area  &lt;br&gt;FREG: Kommunenummer</value>
        [DataMember(Name = "municipalityNumber", EmitDefaultValue = true)]
        public string MunicipalityNumber { get; set; }

        /// <summary>
        /// A letter and four digits that uniquely identifies the  sperately occupied unit inside a addressable building  or part of a building  &lt;br&gt;Remarks:   Two first digits represent floor number  Freg: Bruksenhetsnummer
        /// </summary>
        /// <value>A letter and four digits that uniquely identifies the  sperately occupied unit inside a addressable building  or part of a building  &lt;br&gt;Remarks:   Two first digits represent floor number  Freg: Bruksenhetsnummer</value>
        [DataMember(Name = "separatelyOccupiedUnitNumber", EmitDefaultValue = true)]
        public string SeparatelyOccupiedUnitNumber { get; set; }

        /// <summary>
        /// Name of a street, road, path, place or area  as registered in the cadastral of a municipality  &lt;br&gt;Remarks:   Does not contain street address number (housing number and lettering)  Freg: Adressenavn
        /// </summary>
        /// <value>Name of a street, road, path, place or area  as registered in the cadastral of a municipality  &lt;br&gt;Remarks:   Does not contain street address number (housing number and lettering)  Freg: Adressenavn</value>
        [DataMember(Name = "addressName", EmitDefaultValue = true)]
        public string AddressName { get; set; }

        /// <summary>
        /// Gets or Sets AddressNumber
        /// </summary>
        [DataMember(Name = "addressNumber", EmitDefaultValue = true)]
        public StreetAddressAddressNumber AddressNumber { get; set; }

        /// <summary>
        /// Number that uniquely identifies an addressable  street, road, path, place or area  &lt;br&gt;Remarks:   Known as StreetCode (\&quot;gatekode\&quot;) in DSF  Freg: Adressekode
        /// </summary>
        /// <value>Number that uniquely identifies an addressable  street, road, path, place or area  &lt;br&gt;Remarks:   Known as StreetCode (\&quot;gatekode\&quot;) in DSF  Freg: Adressekode</value>
        [DataMember(Name = "addressCode", EmitDefaultValue = true)]
        public string AddressCode { get; set; }

        /// <summary>
        /// Inherited farm name (bruksnavn) or name of a institution or building,  used as a part of the official address  &lt;br&gt;FREG: Addressetilleggsnavn
        /// </summary>
        /// <value>Inherited farm name (bruksnavn) or name of a institution or building,  used as a part of the official address  &lt;br&gt;FREG: Addressetilleggsnavn</value>
        [DataMember(Name = "addressAdditionalName", EmitDefaultValue = true)]
        public string AddressAdditionalName { get; set; }

        /// <summary>
        /// Gets or Sets City
        /// </summary>
        [DataMember(Name = "city", EmitDefaultValue = true)]
        public CadastralAddressCity City { get; set; }

        /// <summary>
        /// Description of who the recipient is in care of (C/O),  or which recipient in an organization (v/ &#x3D; with, or Att: &#x3D; \&quot;Attention\&quot;)  &lt;br&gt;FREG: CoAdressenavn
        /// </summary>
        /// <value>Description of who the recipient is in care of (C/O),  or which recipient in an organization (v/ &#x3D; with, or Att: &#x3D; \&quot;Attention\&quot;)  &lt;br&gt;FREG: CoAdressenavn</value>
        [DataMember(Name = "coAddressName", EmitDefaultValue = true)]
        public string CoAddressName { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class StreetAddress {\n");
            sb.Append("  MunicipalityNumber: ").Append(MunicipalityNumber).Append("\n");
            sb.Append("  SeparatelyOccupiedUnitNumber: ").Append(SeparatelyOccupiedUnitNumber).Append("\n");
            sb.Append("  SeparatelyOccupiedUnitType: ").Append(SeparatelyOccupiedUnitType).Append("\n");
            sb.Append("  AddressName: ").Append(AddressName).Append("\n");
            sb.Append("  AddressNumber: ").Append(AddressNumber).Append("\n");
            sb.Append("  AddressCode: ").Append(AddressCode).Append("\n");
            sb.Append("  AddressAdditionalName: ").Append(AddressAdditionalName).Append("\n");
            sb.Append("  City: ").Append(City).Append("\n");
            sb.Append("  CoAddressName: ").Append(CoAddressName).Append("\n");
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
            return this.Equals(input as StreetAddress);
        }

        /// <summary>
        /// Returns true if StreetAddress instances are equal
        /// </summary>
        /// <param name="input">Instance of StreetAddress to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(StreetAddress input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.MunicipalityNumber == input.MunicipalityNumber ||
                    (this.MunicipalityNumber != null &&
                    this.MunicipalityNumber.Equals(input.MunicipalityNumber))
                ) && 
                (
                    this.SeparatelyOccupiedUnitNumber == input.SeparatelyOccupiedUnitNumber ||
                    (this.SeparatelyOccupiedUnitNumber != null &&
                    this.SeparatelyOccupiedUnitNumber.Equals(input.SeparatelyOccupiedUnitNumber))
                ) && 
                (
                    this.SeparatelyOccupiedUnitType == input.SeparatelyOccupiedUnitType ||
                    this.SeparatelyOccupiedUnitType.Equals(input.SeparatelyOccupiedUnitType)
                ) && 
                (
                    this.AddressName == input.AddressName ||
                    (this.AddressName != null &&
                    this.AddressName.Equals(input.AddressName))
                ) && 
                (
                    this.AddressNumber == input.AddressNumber ||
                    (this.AddressNumber != null &&
                    this.AddressNumber.Equals(input.AddressNumber))
                ) && 
                (
                    this.AddressCode == input.AddressCode ||
                    (this.AddressCode != null &&
                    this.AddressCode.Equals(input.AddressCode))
                ) && 
                (
                    this.AddressAdditionalName == input.AddressAdditionalName ||
                    (this.AddressAdditionalName != null &&
                    this.AddressAdditionalName.Equals(input.AddressAdditionalName))
                ) && 
                (
                    this.City == input.City ||
                    (this.City != null &&
                    this.City.Equals(input.City))
                ) && 
                (
                    this.CoAddressName == input.CoAddressName ||
                    (this.CoAddressName != null &&
                    this.CoAddressName.Equals(input.CoAddressName))
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
                if (this.MunicipalityNumber != null)
                {
                    hashCode = (hashCode * 59) + this.MunicipalityNumber.GetHashCode();
                }
                if (this.SeparatelyOccupiedUnitNumber != null)
                {
                    hashCode = (hashCode * 59) + this.SeparatelyOccupiedUnitNumber.GetHashCode();
                }
                hashCode = (hashCode * 59) + this.SeparatelyOccupiedUnitType.GetHashCode();
                if (this.AddressName != null)
                {
                    hashCode = (hashCode * 59) + this.AddressName.GetHashCode();
                }
                if (this.AddressNumber != null)
                {
                    hashCode = (hashCode * 59) + this.AddressNumber.GetHashCode();
                }
                if (this.AddressCode != null)
                {
                    hashCode = (hashCode * 59) + this.AddressCode.GetHashCode();
                }
                if (this.AddressAdditionalName != null)
                {
                    hashCode = (hashCode * 59) + this.AddressAdditionalName.GetHashCode();
                }
                if (this.City != null)
                {
                    hashCode = (hashCode * 59) + this.City.GetHashCode();
                }
                if (this.CoAddressName != null)
                {
                    hashCode = (hashCode * 59) + this.CoAddressName.GetHashCode();
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