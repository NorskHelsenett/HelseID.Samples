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
    /// &lt;br&gt;FREG: AnsvarssubjektUtenIdentifikator
    /// </summary>
    [DataContract(Name = "ParentalResponsibility_subjectOfResponsibilityWithoutIdentifier")]
    public partial class ParentalResponsibilitySubjectOfResponsibilityWithoutIdentifier : IEquatable<ParentalResponsibilitySubjectOfResponsibilityWithoutIdentifier>, IValidatableObject
    {

        /// <summary>
        /// &lt;br&gt;FREG: Kjoenn
        /// </summary>
        /// <value>&lt;br&gt;FREG: Kjoenn</value>
        [DataMember(Name = "gender", EmitDefaultValue = true)]
        public Gender? Gender { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ParentalResponsibilitySubjectOfResponsibilityWithoutIdentifier" /> class.
        /// </summary>
        /// <param name="name">name.</param>
        /// <param name="birthDate">&lt;br&gt;FREG: Foedselsdato.</param>
        /// <param name="citizenship">Name of country RelatedBiPerson has a Citizenship&lt;br&gt;FREG: Statsborgerskap.</param>
        /// <param name="gender">&lt;br&gt;FREG: Kjoenn.</param>
        public ParentalResponsibilitySubjectOfResponsibilityWithoutIdentifier(GuardianOrProxyName name = default(GuardianOrProxyName), DateTime? birthDate = default(DateTime?), string citizenship = default(string), Gender? gender = default(Gender?))
        {
            this.Name = name;
            this.BirthDate = birthDate;
            this.Citizenship = citizenship;
            this.Gender = gender;
        }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = true)]
        public GuardianOrProxyName Name { get; set; }

        /// <summary>
        /// &lt;br&gt;FREG: Foedselsdato
        /// </summary>
        /// <value>&lt;br&gt;FREG: Foedselsdato</value>
        [DataMember(Name = "birthDate", EmitDefaultValue = true)]
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Name of country RelatedBiPerson has a Citizenship&lt;br&gt;FREG: Statsborgerskap
        /// </summary>
        /// <value>Name of country RelatedBiPerson has a Citizenship&lt;br&gt;FREG: Statsborgerskap</value>
        [DataMember(Name = "citizenship", EmitDefaultValue = true)]
        public string Citizenship { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class ParentalResponsibilitySubjectOfResponsibilityWithoutIdentifier {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  BirthDate: ").Append(BirthDate).Append("\n");
            sb.Append("  Citizenship: ").Append(Citizenship).Append("\n");
            sb.Append("  Gender: ").Append(Gender).Append("\n");
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
            return this.Equals(input as ParentalResponsibilitySubjectOfResponsibilityWithoutIdentifier);
        }

        /// <summary>
        /// Returns true if ParentalResponsibilitySubjectOfResponsibilityWithoutIdentifier instances are equal
        /// </summary>
        /// <param name="input">Instance of ParentalResponsibilitySubjectOfResponsibilityWithoutIdentifier to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ParentalResponsibilitySubjectOfResponsibilityWithoutIdentifier input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.Name == input.Name ||
                    (this.Name != null &&
                    this.Name.Equals(input.Name))
                ) && 
                (
                    this.BirthDate == input.BirthDate ||
                    (this.BirthDate != null &&
                    this.BirthDate.Equals(input.BirthDate))
                ) && 
                (
                    this.Citizenship == input.Citizenship ||
                    (this.Citizenship != null &&
                    this.Citizenship.Equals(input.Citizenship))
                ) && 
                (
                    this.Gender == input.Gender ||
                    this.Gender.Equals(input.Gender)
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
                if (this.Name != null)
                {
                    hashCode = (hashCode * 59) + this.Name.GetHashCode();
                }
                if (this.BirthDate != null)
                {
                    hashCode = (hashCode * 59) + this.BirthDate.GetHashCode();
                }
                if (this.Citizenship != null)
                {
                    hashCode = (hashCode * 59) + this.Citizenship.GetHashCode();
                }
                hashCode = (hashCode * 59) + this.Gender.GetHashCode();
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
