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
    /// Residuary estate contact information  Will be either a Person, a Lawyer or an Organization.  &lt;br&gt;FREG: KontaktinformasjonForDoedsbo
    /// </summary>
    [DataContract(Name = "ResiduaryEstateContactInformation")]
    public partial class ResiduaryEstateContactInformation : IEquatable<ResiduaryEstateContactInformation>, IValidatableObject
    {

        /// <summary>
        /// Probate certificate type  &lt;br&gt;FREG: Skifteform
        /// </summary>
        /// <value>Probate certificate type  &lt;br&gt;FREG: Skifteform</value>
        [DataMember(Name = "probateCertificateType", EmitDefaultValue = false)]
        public ProbateCertificateType? ProbateCertificateType { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ResiduaryEstateContactInformation" /> class.
        /// </summary>
        /// <param name="registeredAt">&lt;br&gt;FREG: Ajourholdstidspunkt.</param>
        /// <param name="isValid">&lt;br&gt;FREG: ErGjeldende.</param>
        /// <param name="source">&lt;br&gt;FREG: Kilde.</param>
        /// <param name="reason">&lt;br&gt;FREG: Aarsak.</param>
        /// <param name="validFrom">&lt;br&gt;FREG: Gyldighetstidspunkt.</param>
        /// <param name="validTo">&lt;br&gt;FREG: Opphoerstidspunkt.</param>
        /// <param name="person">person.</param>
        /// <param name="lawyer">lawyer.</param>
        /// <param name="organization">organization.</param>
        /// <param name="probateCertificateType">Probate certificate type  &lt;br&gt;FREG: Skifteform.</param>
        /// <param name="probateCertificateIssueDate">Date of the probate certificate issuance  &lt;br&gt;FREG: Attestutstedelsesdato.</param>
        /// <param name="contactAddress">contactAddress.</param>
        public ResiduaryEstateContactInformation(DateTime? registeredAt = default(DateTime?), bool? isValid = default(bool?), string source = default(string), string reason = default(string), DateTime? validFrom = default(DateTime?), DateTime? validTo = default(DateTime?), ResiduaryEstateContactInformationPerson person = default(ResiduaryEstateContactInformationPerson), ResiduaryEstateContactInformationLawyer lawyer = default(ResiduaryEstateContactInformationLawyer), ResiduaryEstateContactInformationOrganization organization = default(ResiduaryEstateContactInformationOrganization), ProbateCertificateType? probateCertificateType = default(ProbateCertificateType?), DateTime probateCertificateIssueDate = default(DateTime), ResiduaryEstateContactInformationContactAddress contactAddress = default(ResiduaryEstateContactInformationContactAddress))
        {
            this.RegisteredAt = registeredAt;
            this.IsValid = isValid;
            this.Source = source;
            this.Reason = reason;
            this.ValidFrom = validFrom;
            this.ValidTo = validTo;
            this.Person = person;
            this.Lawyer = lawyer;
            this.Organization = organization;
            this.ProbateCertificateType = probateCertificateType;
            this.ProbateCertificateIssueDate = probateCertificateIssueDate;
            this.ContactAddress = contactAddress;
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
        /// Gets or Sets Person
        /// </summary>
        [DataMember(Name = "person", EmitDefaultValue = true)]
        public ResiduaryEstateContactInformationPerson Person { get; set; }

        /// <summary>
        /// Gets or Sets Lawyer
        /// </summary>
        [DataMember(Name = "lawyer", EmitDefaultValue = true)]
        public ResiduaryEstateContactInformationLawyer Lawyer { get; set; }

        /// <summary>
        /// Gets or Sets Organization
        /// </summary>
        [DataMember(Name = "organization", EmitDefaultValue = true)]
        public ResiduaryEstateContactInformationOrganization Organization { get; set; }

        /// <summary>
        /// Date of the probate certificate issuance  &lt;br&gt;FREG: Attestutstedelsesdato
        /// </summary>
        /// <value>Date of the probate certificate issuance  &lt;br&gt;FREG: Attestutstedelsesdato</value>
        [DataMember(Name = "probateCertificateIssueDate", EmitDefaultValue = false)]
        public DateTime ProbateCertificateIssueDate { get; set; }

        /// <summary>
        /// Gets or Sets ContactAddress
        /// </summary>
        [DataMember(Name = "contactAddress", EmitDefaultValue = true)]
        public ResiduaryEstateContactInformationContactAddress ContactAddress { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class ResiduaryEstateContactInformation {\n");
            sb.Append("  RegisteredAt: ").Append(RegisteredAt).Append("\n");
            sb.Append("  IsValid: ").Append(IsValid).Append("\n");
            sb.Append("  Source: ").Append(Source).Append("\n");
            sb.Append("  Reason: ").Append(Reason).Append("\n");
            sb.Append("  ValidFrom: ").Append(ValidFrom).Append("\n");
            sb.Append("  ValidTo: ").Append(ValidTo).Append("\n");
            sb.Append("  Person: ").Append(Person).Append("\n");
            sb.Append("  Lawyer: ").Append(Lawyer).Append("\n");
            sb.Append("  Organization: ").Append(Organization).Append("\n");
            sb.Append("  ProbateCertificateType: ").Append(ProbateCertificateType).Append("\n");
            sb.Append("  ProbateCertificateIssueDate: ").Append(ProbateCertificateIssueDate).Append("\n");
            sb.Append("  ContactAddress: ").Append(ContactAddress).Append("\n");
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
            return this.Equals(input as ResiduaryEstateContactInformation);
        }

        /// <summary>
        /// Returns true if ResiduaryEstateContactInformation instances are equal
        /// </summary>
        /// <param name="input">Instance of ResiduaryEstateContactInformation to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ResiduaryEstateContactInformation input)
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
                    this.Person == input.Person ||
                    (this.Person != null &&
                    this.Person.Equals(input.Person))
                ) && 
                (
                    this.Lawyer == input.Lawyer ||
                    (this.Lawyer != null &&
                    this.Lawyer.Equals(input.Lawyer))
                ) && 
                (
                    this.Organization == input.Organization ||
                    (this.Organization != null &&
                    this.Organization.Equals(input.Organization))
                ) && 
                (
                    this.ProbateCertificateType == input.ProbateCertificateType ||
                    this.ProbateCertificateType.Equals(input.ProbateCertificateType)
                ) && 
                (
                    this.ProbateCertificateIssueDate == input.ProbateCertificateIssueDate ||
                    (this.ProbateCertificateIssueDate != null &&
                    this.ProbateCertificateIssueDate.Equals(input.ProbateCertificateIssueDate))
                ) && 
                (
                    this.ContactAddress == input.ContactAddress ||
                    (this.ContactAddress != null &&
                    this.ContactAddress.Equals(input.ContactAddress))
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
                if (this.Person != null)
                {
                    hashCode = (hashCode * 59) + this.Person.GetHashCode();
                }
                if (this.Lawyer != null)
                {
                    hashCode = (hashCode * 59) + this.Lawyer.GetHashCode();
                }
                if (this.Organization != null)
                {
                    hashCode = (hashCode * 59) + this.Organization.GetHashCode();
                }
                hashCode = (hashCode * 59) + this.ProbateCertificateType.GetHashCode();
                if (this.ProbateCertificateIssueDate != null)
                {
                    hashCode = (hashCode * 59) + this.ProbateCertificateIssueDate.GetHashCode();
                }
                if (this.ContactAddress != null)
                {
                    hashCode = (hashCode * 59) + this.ContactAddress.GetHashCode();
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
