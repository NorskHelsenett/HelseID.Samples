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
    /// Family relation between Person and a related person.  The related person has either a valid identification number (nin or d-number) or RelatedBiPerson object  &lt;br&gt;FREG: Familierelasjon
    /// </summary>
    [DataContract(Name = "FamilyRelation")]
    public partial class FamilyRelation : IEquatable<FamilyRelation>, IValidatableObject
    {

        /// <summary>
        /// The relation type seen from the RelatedPerson or RelatedBiPersons view  Freg: RelatertPersonsRolle
        /// </summary>
        /// <value>The relation type seen from the RelatedPerson or RelatedBiPersons view  Freg: RelatertPersonsRolle</value>
        [DataMember(Name = "relatedPersonsRole", EmitDefaultValue = false)]
        public FamilyRelationType? RelatedPersonsRole { get; set; }

        /// <summary>
        /// The relation type seen from the Persons view  Freg: MinRolleForPerson
        /// </summary>
        /// <value>The relation type seen from the Persons view  Freg: MinRolleForPerson</value>
        [DataMember(Name = "myRoleForRelatedPerson", EmitDefaultValue = true)]
        public FamilyRelationType? MyRoleForRelatedPerson { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="FamilyRelation" /> class.
        /// </summary>
        /// <param name="registeredAt">&lt;br&gt;FREG: Ajourholdstidspunkt.</param>
        /// <param name="isValid">&lt;br&gt;FREG: ErGjeldende.</param>
        /// <param name="source">&lt;br&gt;FREG: Kilde.</param>
        /// <param name="reason">&lt;br&gt;FREG: Aarsak.</param>
        /// <param name="validFrom">&lt;br&gt;FREG: Gyldighetstidspunkt.</param>
        /// <param name="validTo">&lt;br&gt;FREG: Opphoerstidspunkt.</param>
        /// <param name="relatedPerson">The identification number (nin or d-number) for the related person  &lt;br&gt;Remarks: Is null if RelatedPersonWithoutIdentifier is used.</param>
        /// <param name="relatedPersonWithoutIdentifier">relatedPersonWithoutIdentifier.</param>
        /// <param name="relatedPersonsRole">The relation type seen from the RelatedPerson or RelatedBiPersons view  Freg: RelatertPersonsRolle.</param>
        /// <param name="myRoleForRelatedPerson">The relation type seen from the Persons view  Freg: MinRolleForPerson.</param>
        public FamilyRelation(DateTime? registeredAt = default(DateTime?), bool? isValid = default(bool?), string source = default(string), string reason = default(string), DateTime? validFrom = default(DateTime?), DateTime? validTo = default(DateTime?), string relatedPerson = default(string), FamilyRelationRelatedPersonWithoutIdentifier relatedPersonWithoutIdentifier = default(FamilyRelationRelatedPersonWithoutIdentifier), FamilyRelationType? relatedPersonsRole = default(FamilyRelationType?), FamilyRelationType? myRoleForRelatedPerson = default(FamilyRelationType?))
        {
            this.RegisteredAt = registeredAt;
            this.IsValid = isValid;
            this.Source = source;
            this.Reason = reason;
            this.ValidFrom = validFrom;
            this.ValidTo = validTo;
            this.RelatedPerson = relatedPerson;
            this.RelatedPersonWithoutIdentifier = relatedPersonWithoutIdentifier;
            this.RelatedPersonsRole = relatedPersonsRole;
            this.MyRoleForRelatedPerson = myRoleForRelatedPerson;
        }

        /// <summary>
        /// &lt;br&gt;FREG: Ajourholdstidspunkt
        /// </summary>
        /// <value>&lt;br&gt;FREG: Ajourholdstidspunkt</value>
        [DataMember(Name = "registeredAt", EmitDefaultValue = true)]
        public DateTime? RegisteredAt { get; set; }

        /// <summary>
        /// &lt;br&gt;FREG: ErGjeldende
        /// </summary>
        /// <value>&lt;br&gt;FREG: ErGjeldende</value>
        [DataMember(Name = "isValid", EmitDefaultValue = true)]
        public bool? IsValid { get; set; }

        /// <summary>
        /// &lt;br&gt;FREG: Kilde
        /// </summary>
        /// <value>&lt;br&gt;FREG: Kilde</value>
        [DataMember(Name = "source", EmitDefaultValue = true)]
        public string Source { get; set; }

        /// <summary>
        /// &lt;br&gt;FREG: Aarsak
        /// </summary>
        /// <value>&lt;br&gt;FREG: Aarsak</value>
        [DataMember(Name = "reason", EmitDefaultValue = true)]
        public string Reason { get; set; }

        /// <summary>
        /// &lt;br&gt;FREG: Gyldighetstidspunkt
        /// </summary>
        /// <value>&lt;br&gt;FREG: Gyldighetstidspunkt</value>
        [DataMember(Name = "validFrom", EmitDefaultValue = true)]
        public DateTime? ValidFrom { get; set; }

        /// <summary>
        /// &lt;br&gt;FREG: Opphoerstidspunkt
        /// </summary>
        /// <value>&lt;br&gt;FREG: Opphoerstidspunkt</value>
        [DataMember(Name = "validTo", EmitDefaultValue = true)]
        public DateTime? ValidTo { get; set; }

        /// <summary>
        /// The identification number (nin or d-number) for the related person  &lt;br&gt;Remarks: Is null if RelatedPersonWithoutIdentifier is used
        /// </summary>
        /// <value>The identification number (nin or d-number) for the related person  &lt;br&gt;Remarks: Is null if RelatedPersonWithoutIdentifier is used</value>
        [DataMember(Name = "relatedPerson", EmitDefaultValue = true)]
        public string RelatedPerson { get; set; }

        /// <summary>
        /// Gets or Sets RelatedPersonWithoutIdentifier
        /// </summary>
        [DataMember(Name = "relatedPersonWithoutIdentifier", EmitDefaultValue = true)]
        public FamilyRelationRelatedPersonWithoutIdentifier RelatedPersonWithoutIdentifier { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class FamilyRelation {\n");
            sb.Append("  RegisteredAt: ").Append(RegisteredAt).Append("\n");
            sb.Append("  IsValid: ").Append(IsValid).Append("\n");
            sb.Append("  Source: ").Append(Source).Append("\n");
            sb.Append("  Reason: ").Append(Reason).Append("\n");
            sb.Append("  ValidFrom: ").Append(ValidFrom).Append("\n");
            sb.Append("  ValidTo: ").Append(ValidTo).Append("\n");
            sb.Append("  RelatedPerson: ").Append(RelatedPerson).Append("\n");
            sb.Append("  RelatedPersonWithoutIdentifier: ").Append(RelatedPersonWithoutIdentifier).Append("\n");
            sb.Append("  RelatedPersonsRole: ").Append(RelatedPersonsRole).Append("\n");
            sb.Append("  MyRoleForRelatedPerson: ").Append(MyRoleForRelatedPerson).Append("\n");
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
            return this.Equals(input as FamilyRelation);
        }

        /// <summary>
        /// Returns true if FamilyRelation instances are equal
        /// </summary>
        /// <param name="input">Instance of FamilyRelation to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(FamilyRelation input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.RegisteredAt == input.RegisteredAt ||
                    (this.RegisteredAt != null &&
                    this.RegisteredAt.Equals(input.RegisteredAt))
                ) && 
                (
                    this.IsValid == input.IsValid ||
                    (this.IsValid != null &&
                    this.IsValid.Equals(input.IsValid))
                ) && 
                (
                    this.Source == input.Source ||
                    (this.Source != null &&
                    this.Source.Equals(input.Source))
                ) && 
                (
                    this.Reason == input.Reason ||
                    (this.Reason != null &&
                    this.Reason.Equals(input.Reason))
                ) && 
                (
                    this.ValidFrom == input.ValidFrom ||
                    (this.ValidFrom != null &&
                    this.ValidFrom.Equals(input.ValidFrom))
                ) && 
                (
                    this.ValidTo == input.ValidTo ||
                    (this.ValidTo != null &&
                    this.ValidTo.Equals(input.ValidTo))
                ) && 
                (
                    this.RelatedPerson == input.RelatedPerson ||
                    (this.RelatedPerson != null &&
                    this.RelatedPerson.Equals(input.RelatedPerson))
                ) && 
                (
                    this.RelatedPersonWithoutIdentifier == input.RelatedPersonWithoutIdentifier ||
                    (this.RelatedPersonWithoutIdentifier != null &&
                    this.RelatedPersonWithoutIdentifier.Equals(input.RelatedPersonWithoutIdentifier))
                ) && 
                (
                    this.RelatedPersonsRole == input.RelatedPersonsRole ||
                    this.RelatedPersonsRole.Equals(input.RelatedPersonsRole)
                ) && 
                (
                    this.MyRoleForRelatedPerson == input.MyRoleForRelatedPerson ||
                    this.MyRoleForRelatedPerson.Equals(input.MyRoleForRelatedPerson)
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
                if (this.RegisteredAt != null)
                {
                    hashCode = (hashCode * 59) + this.RegisteredAt.GetHashCode();
                }
                if (this.IsValid != null)
                {
                    hashCode = (hashCode * 59) + this.IsValid.GetHashCode();
                }
                if (this.Source != null)
                {
                    hashCode = (hashCode * 59) + this.Source.GetHashCode();
                }
                if (this.Reason != null)
                {
                    hashCode = (hashCode * 59) + this.Reason.GetHashCode();
                }
                if (this.ValidFrom != null)
                {
                    hashCode = (hashCode * 59) + this.ValidFrom.GetHashCode();
                }
                if (this.ValidTo != null)
                {
                    hashCode = (hashCode * 59) + this.ValidTo.GetHashCode();
                }
                if (this.RelatedPerson != null)
                {
                    hashCode = (hashCode * 59) + this.RelatedPerson.GetHashCode();
                }
                if (this.RelatedPersonWithoutIdentifier != null)
                {
                    hashCode = (hashCode * 59) + this.RelatedPersonWithoutIdentifier.GetHashCode();
                }
                hashCode = (hashCode * 59) + this.RelatedPersonsRole.GetHashCode();
                hashCode = (hashCode * 59) + this.MyRoleForRelatedPerson.GetHashCode();
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
