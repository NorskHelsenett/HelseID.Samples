/*
 * Persontjenesten API - ET
 * ## Introduction  The Person API is a copy of the [National Population Register (\"Folkeregisteret\")](https://www.skatteetaten.no/en/person/national-registry/about/) serving the norwegian health sector, maintained by Norsk helsenett. More detailed information on data coming from the National Population Register is also available in Norwegian here: [Information Model](https://skatteetaten.github.io/folkeregisteret-api-dokumentasjon/informasjonsmodell/)  More detailed information about the Person API service, including how to get access, is documented here: [Persontjenesten](https://www.nhn.no/samhandlingsplattform/grunndata/persontjenesten)  ## Disclaimer  The Person API is under continuous development and will be subject to changes without notice. The changes will follow semantic versioning to prevent breaking changes. Legacy versions will be available for 6 months before they are discontinued. We encourage consumers to follow our changelog in order to keep track of any changes. Send feedback and questions to [persontjenesten@nhn.no](mailto:persontjenesten@nhn.no)  ## Changelog  See [Changelog](../static/changelog/index.html)  ## Synthetic test data  Data in our test environment is using synthetic test data coming from the [Synthetic National Register](https://skatteetaten.github.io/testnorge-tenor-dokumentasjon/kilder#syntetisk-folkeregister). To browse the data available, you can log in using ID-porten at [Testnorge](https://testdata.skatteetaten.no/web/testnorge/)  ## Authentication and authorization  This API uses [HelseID](https://www.nhn.no/samhandlingsplattform/helseid) for authentication and authorization. To use the API you will need to have a valid HelseID token with a valid scope.  There are two scopes available to consume resources from the Person API: - **ReadWithLegalBasis** | Scope: `nhn:hgd-persontjenesten-api/read-with-legal-basis`    This scope provides read access to information in the authorization bundle \"public with legal basis\" (aka statutory authority).    For version 0.5 name was `nhn:hgd-persontjenesten-api/read`  - **ReadNoLegalBasis** | Scope: `nhn:hgd-persontjenesten-api/read-no-legal-basis`    This scope provides read access to information in the public bundle \"public with**out** legal basis\". 
 *
 * The version of the OpenAPI document: 1.5
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */


package sf.nhn.helseid.demo.persontjenesten.model;

import com.google.gson.*;
import com.google.gson.annotations.SerializedName;
import com.google.gson.reflect.TypeToken;
import com.google.gson.stream.JsonReader;
import com.google.gson.stream.JsonWriter;
import io.swagger.annotations.ApiModel;
import io.swagger.annotations.ApiModelProperty;
import org.openapitools.jackson.nullable.JsonNullable;
import sf.nhn.helseid.demo.persontjenesten.JSON;

import java.io.IOException;
import java.time.OffsetDateTime;
import java.util.Arrays;
import java.util.HashSet;
import java.util.Map.Entry;
import java.util.Objects;
import java.util.Set;

/**
 * Address where person is registered to live.  Only one of StreetAddress, CadastralAddress  or UnknownResidence is in use  &lt;br&gt;FREG: Bostedsadresse
 */
@ApiModel(description = "Address where person is registered to live.  Only one of StreetAddress, CadastralAddress  or UnknownResidence is in use  <br>FREG: Bostedsadresse")
@javax.annotation.Generated(value = "org.openapitools.codegen.languages.JavaClientCodegen", date = "2022-10-21T11:48:55.741350900+02:00[Europe/Oslo]")
public class ResidentialAddress {
  public static final String SERIALIZED_NAME_REGISTERED_AT = "registeredAt";
  @SerializedName(SERIALIZED_NAME_REGISTERED_AT)
  private OffsetDateTime registeredAt;

  public static final String SERIALIZED_NAME_IS_VALID = "isValid";
  @SerializedName(SERIALIZED_NAME_IS_VALID)
  private Boolean isValid;

  public static final String SERIALIZED_NAME_SOURCE = "source";
  @SerializedName(SERIALIZED_NAME_SOURCE)
  private String source;

  public static final String SERIALIZED_NAME_REASON = "reason";
  @SerializedName(SERIALIZED_NAME_REASON)
  private String reason;

  public static final String SERIALIZED_NAME_VALID_FROM = "validFrom";
  @SerializedName(SERIALIZED_NAME_VALID_FROM)
  private OffsetDateTime validFrom;

  public static final String SERIALIZED_NAME_VALID_TO = "validTo";
  @SerializedName(SERIALIZED_NAME_VALID_TO)
  private OffsetDateTime validTo;

  public static final String SERIALIZED_NAME_STREET_ADDRESS = "streetAddress";
  @SerializedName(SERIALIZED_NAME_STREET_ADDRESS)
  private ResidentialAddressStreetAddress streetAddress;

  public static final String SERIALIZED_NAME_CADASTRAL_ADDRESS = "cadastralAddress";
  @SerializedName(SERIALIZED_NAME_CADASTRAL_ADDRESS)
  private ResidentialAddressCadastralAddress cadastralAddress;

  public static final String SERIALIZED_NAME_UNKNOWN_RESIDENCE = "unknownResidence";
  @SerializedName(SERIALIZED_NAME_UNKNOWN_RESIDENCE)
  private ResidentialAddressUnknownResidence unknownResidence;

  public static final String SERIALIZED_NAME_CADASTRAL_IDENTIFIER = "cadastralIdentifier";
  @SerializedName(SERIALIZED_NAME_CADASTRAL_IDENTIFIER)
  private String cadastralIdentifier;

  public static final String SERIALIZED_NAME_CLOSE_CADASTRAL_IDENTIFIER = "closeCadastralIdentifier";
  @SerializedName(SERIALIZED_NAME_CLOSE_CADASTRAL_IDENTIFIER)
  private String closeCadastralIdentifier;

  public static final String SERIALIZED_NAME_ADDRESS_CONFIDENTIALITY = "addressConfidentiality";
  @SerializedName(SERIALIZED_NAME_ADDRESS_CONFIDENTIALITY)
  private AddressConfidentiality addressConfidentiality;

  public static final String SERIALIZED_NAME_MOVE_DATE = "moveDate";
  @SerializedName(SERIALIZED_NAME_MOVE_DATE)
  private OffsetDateTime moveDate;

  public static final String SERIALIZED_NAME_BASIC_STATISTICAL_UNIT = "basicStatisticalUnit";
  @SerializedName(SERIALIZED_NAME_BASIC_STATISTICAL_UNIT)
  private Long basicStatisticalUnit;

  public static final String SERIALIZED_NAME_BASIC_STATISTICAL_UNIT_NAME = "basicStatisticalUnitName";
  @SerializedName(SERIALIZED_NAME_BASIC_STATISTICAL_UNIT_NAME)
  private String basicStatisticalUnitName;

  public static final String SERIALIZED_NAME_CONSTITUENCY = "constituency";
  @SerializedName(SERIALIZED_NAME_CONSTITUENCY)
  private Long constituency;

  public static final String SERIALIZED_NAME_SCHOOL_DISTRICT = "schoolDistrict";
  @SerializedName(SERIALIZED_NAME_SCHOOL_DISTRICT)
  private Long schoolDistrict;

  public static final String SERIALIZED_NAME_CHURCH_DISTRICT = "churchDistrict";
  @SerializedName(SERIALIZED_NAME_CHURCH_DISTRICT)
  private Long churchDistrict;

  public static final String SERIALIZED_NAME_URBAN_DISTRICT_CODE = "urbanDistrictCode";
  @SerializedName(SERIALIZED_NAME_URBAN_DISTRICT_CODE)
  private String urbanDistrictCode;

  public static final String SERIALIZED_NAME_URBAN_DISTRICT_NAME = "urbanDistrictName";
  @SerializedName(SERIALIZED_NAME_URBAN_DISTRICT_NAME)
  private String urbanDistrictName;

  public ResidentialAddress() {
  }

  public ResidentialAddress registeredAt(OffsetDateTime registeredAt) {
    
    this.registeredAt = registeredAt;
    return this;
  }

   /**
   * &lt;br&gt;FREG: Ajourholdstidspunkt
   * @return registeredAt
  **/
  @javax.annotation.Nullable
  @ApiModelProperty(value = "<br>FREG: Ajourholdstidspunkt")

  public OffsetDateTime getRegisteredAt() {
    return registeredAt;
  }


  public void setRegisteredAt(OffsetDateTime registeredAt) {
    this.registeredAt = registeredAt;
  }


  public ResidentialAddress isValid(Boolean isValid) {
    
    this.isValid = isValid;
    return this;
  }

   /**
   * &lt;br&gt;FREG: ErGjeldende
   * @return isValid
  **/
  @javax.annotation.Nullable
  @ApiModelProperty(value = "<br>FREG: ErGjeldende")

  public Boolean getIsValid() {
    return isValid;
  }


  public void setIsValid(Boolean isValid) {
    this.isValid = isValid;
  }


  public ResidentialAddress source(String source) {
    
    this.source = source;
    return this;
  }

   /**
   * &lt;br&gt;FREG: Kilde
   * @return source
  **/
  @javax.annotation.Nullable
  @ApiModelProperty(value = "<br>FREG: Kilde")

  public String getSource() {
    return source;
  }


  public void setSource(String source) {
    this.source = source;
  }


  public ResidentialAddress reason(String reason) {
    
    this.reason = reason;
    return this;
  }

   /**
   * &lt;br&gt;FREG: Aarsak
   * @return reason
  **/
  @javax.annotation.Nullable
  @ApiModelProperty(value = "<br>FREG: Aarsak")

  public String getReason() {
    return reason;
  }


  public void setReason(String reason) {
    this.reason = reason;
  }


  public ResidentialAddress validFrom(OffsetDateTime validFrom) {
    
    this.validFrom = validFrom;
    return this;
  }

   /**
   * &lt;br&gt;FREG: Gyldighetstidspunkt
   * @return validFrom
  **/
  @javax.annotation.Nullable
  @ApiModelProperty(value = "<br>FREG: Gyldighetstidspunkt")

  public OffsetDateTime getValidFrom() {
    return validFrom;
  }


  public void setValidFrom(OffsetDateTime validFrom) {
    this.validFrom = validFrom;
  }


  public ResidentialAddress validTo(OffsetDateTime validTo) {
    
    this.validTo = validTo;
    return this;
  }

   /**
   * &lt;br&gt;FREG: Opphoerstidspunkt
   * @return validTo
  **/
  @javax.annotation.Nullable
  @ApiModelProperty(value = "<br>FREG: Opphoerstidspunkt")

  public OffsetDateTime getValidTo() {
    return validTo;
  }


  public void setValidTo(OffsetDateTime validTo) {
    this.validTo = validTo;
  }


  public ResidentialAddress streetAddress(ResidentialAddressStreetAddress streetAddress) {
    
    this.streetAddress = streetAddress;
    return this;
  }

   /**
   * Get streetAddress
   * @return streetAddress
  **/
  @javax.annotation.Nullable
  @ApiModelProperty(value = "")

  public ResidentialAddressStreetAddress getStreetAddress() {
    return streetAddress;
  }


  public void setStreetAddress(ResidentialAddressStreetAddress streetAddress) {
    this.streetAddress = streetAddress;
  }


  public ResidentialAddress cadastralAddress(ResidentialAddressCadastralAddress cadastralAddress) {
    
    this.cadastralAddress = cadastralAddress;
    return this;
  }

   /**
   * Get cadastralAddress
   * @return cadastralAddress
  **/
  @javax.annotation.Nullable
  @ApiModelProperty(value = "")

  public ResidentialAddressCadastralAddress getCadastralAddress() {
    return cadastralAddress;
  }


  public void setCadastralAddress(ResidentialAddressCadastralAddress cadastralAddress) {
    this.cadastralAddress = cadastralAddress;
  }


  public ResidentialAddress unknownResidence(ResidentialAddressUnknownResidence unknownResidence) {
    
    this.unknownResidence = unknownResidence;
    return this;
  }

   /**
   * Get unknownResidence
   * @return unknownResidence
  **/
  @javax.annotation.Nullable
  @ApiModelProperty(value = "")

  public ResidentialAddressUnknownResidence getUnknownResidence() {
    return unknownResidence;
  }


  public void setUnknownResidence(ResidentialAddressUnknownResidence unknownResidence) {
    this.unknownResidence = unknownResidence;
  }


  public ResidentialAddress cadastralIdentifier(String cadastralIdentifier) {
    
    this.cadastralIdentifier = cadastralIdentifier;
    return this;
  }

   /**
   * Unique identifier from the Norwegian Mapping Authority.  &lt;br&gt;FREG: AdresseIdentifikatorFraMatrikkelen
   * @return cadastralIdentifier
  **/
  @javax.annotation.Nullable
  @ApiModelProperty(example = "14464824", value = "Unique identifier from the Norwegian Mapping Authority.  <br>FREG: AdresseIdentifikatorFraMatrikkelen")

  public String getCadastralIdentifier() {
    return cadastralIdentifier;
  }


  public void setCadastralIdentifier(String cadastralIdentifier) {
    this.cadastralIdentifier = cadastralIdentifier;
  }


  public ResidentialAddress closeCadastralIdentifier(String closeCadastralIdentifier) {
    
    this.closeCadastralIdentifier = closeCadastralIdentifier;
    return this;
  }

   /**
   * Unique identifier from the Mapping Authority.  If the Cadastral information used in CadastralIdentifier  is inaccurate, CloseCadastralIdentifier will contain the  unqiue identifier for a near by house.  I.e. If the house letter is not known, the CloseCadastralIdentifier  could point to the house with letter \&quot;A\&quot;, while CadastralIdentifier  would point to all the house number, without letters (which could be multiple houses)  Used to catch changes to the Cadastral to a nearby  &lt;br&gt;Remarks:   (07.02.2020) Not in use  Freg: NaerAdresseIdentifikatorFraMatrikkelen
   * @return closeCadastralIdentifier
  **/
  @javax.annotation.Nullable
  @ApiModelProperty(value = "Unique identifier from the Mapping Authority.  If the Cadastral information used in CadastralIdentifier  is inaccurate, CloseCadastralIdentifier will contain the  unqiue identifier for a near by house.  I.e. If the house letter is not known, the CloseCadastralIdentifier  could point to the house with letter \"A\", while CadastralIdentifier  would point to all the house number, without letters (which could be multiple houses)  Used to catch changes to the Cadastral to a nearby  <br>Remarks:   (07.02.2020) Not in use  Freg: NaerAdresseIdentifikatorFraMatrikkelen")

  public String getCloseCadastralIdentifier() {
    return closeCadastralIdentifier;
  }


  public void setCloseCadastralIdentifier(String closeCadastralIdentifier) {
    this.closeCadastralIdentifier = closeCadastralIdentifier;
  }


  public ResidentialAddress addressConfidentiality(AddressConfidentiality addressConfidentiality) {
    
    this.addressConfidentiality = addressConfidentiality;
    return this;
  }

   /**
   * Describes with which confidentiality the address  should be handled  &lt;br&gt;FREG: Adressegradering
   * @return addressConfidentiality
  **/
  @javax.annotation.Nullable
  @ApiModelProperty(value = "Describes with which confidentiality the address  should be handled  <br>FREG: Adressegradering")

  public AddressConfidentiality getAddressConfidentiality() {
    return addressConfidentiality;
  }


  public void setAddressConfidentiality(AddressConfidentiality addressConfidentiality) {
    this.addressConfidentiality = addressConfidentiality;
  }


  public ResidentialAddress moveDate(OffsetDateTime moveDate) {
    
    this.moveDate = moveDate;
    return this;
  }

   /**
   * Reported as move date by person  &lt;br&gt;FREG: Flyttedato
   * @return moveDate
  **/
  @javax.annotation.Nullable
  @ApiModelProperty(value = "Reported as move date by person  <br>FREG: Flyttedato")

  public OffsetDateTime getMoveDate() {
    return moveDate;
  }


  public void setMoveDate(OffsetDateTime moveDate) {
    this.moveDate = moveDate;
  }


  public ResidentialAddress basicStatisticalUnit(Long basicStatisticalUnit) {
    
    this.basicStatisticalUnit = basicStatisticalUnit;
    return this;
  }

   /**
   * Unique code by the municipality used to  divide small areas of similar nature/commercial/building structure  used as grounds for detailed regional statistic analysis.  &lt;br&gt;FREG: Grunnkrets
   * @return basicStatisticalUnit
  **/
  @javax.annotation.Nullable
  @ApiModelProperty(example = "404", value = "Unique code by the municipality used to  divide small areas of similar nature/commercial/building structure  used as grounds for detailed regional statistic analysis.  <br>FREG: Grunnkrets")

  public Long getBasicStatisticalUnit() {
    return basicStatisticalUnit;
  }


  public void setBasicStatisticalUnit(Long basicStatisticalUnit) {
    this.basicStatisticalUnit = basicStatisticalUnit;
  }


  public ResidentialAddress basicStatisticalUnitName(String basicStatisticalUnitName) {
    
    this.basicStatisticalUnitName = basicStatisticalUnitName;
    return this;
  }

   /**
   * Name corresponding to BasicStatisticalUnit code  &lt;br&gt;SSB: BasicStatisticalUnitName
   * @return basicStatisticalUnitName
  **/
  @javax.annotation.Nullable
  @ApiModelProperty(example = "Sentrum 1 - Rode 1", value = "Name corresponding to BasicStatisticalUnit code  <br>SSB: BasicStatisticalUnitName")

  public String getBasicStatisticalUnitName() {
    return basicStatisticalUnitName;
  }


  public void setBasicStatisticalUnitName(String basicStatisticalUnitName) {
    this.basicStatisticalUnitName = basicStatisticalUnitName;
  }


  public ResidentialAddress constituency(Long constituency) {
    
    this.constituency = constituency;
    return this;
  }

   /**
   * Unique code by the municipality used as  geographical division of the municipality set by the  electoral committee  &lt;br&gt;Remarks:   Also knows as in the voting constituency Cadastral  Freg: Stemmekrets (Valgkrets i matrikkelen)
   * @return constituency
  **/
  @javax.annotation.Nullable
  @ApiModelProperty(example = "7", value = "Unique code by the municipality used as  geographical division of the municipality set by the  electoral committee  <br>Remarks:   Also knows as in the voting constituency Cadastral  Freg: Stemmekrets (Valgkrets i matrikkelen)")

  public Long getConstituency() {
    return constituency;
  }


  public void setConstituency(Long constituency) {
    this.constituency = constituency;
  }


  public ResidentialAddress schoolDistrict(Long schoolDistrict) {
    
    this.schoolDistrict = schoolDistrict;
    return this;
  }

   /**
   * Unique code by the municipality used to describe  a geographical division, used as a non-binding school  affiliation for kids in the area  &lt;br&gt;FREG: Skolekrets
   * @return schoolDistrict
  **/
  @javax.annotation.Nullable
  @ApiModelProperty(example = "5", value = "Unique code by the municipality used to describe  a geographical division, used as a non-binding school  affiliation for kids in the area  <br>FREG: Skolekrets")

  public Long getSchoolDistrict() {
    return schoolDistrict;
  }


  public void setSchoolDistrict(Long schoolDistrict) {
    this.schoolDistrict = schoolDistrict;
  }


  public ResidentialAddress churchDistrict(Long churchDistrict) {
    
    this.churchDistrict = churchDistrict;
    return this;
  }

   /**
   * Unique code for the parish (kirke sogn)  Parish is the basic unit of the Norwegian Church.  &lt;br&gt;Remarks:   The church district can extend over several municipalities  Freg: Kirkekrets
   * @return churchDistrict
  **/
  @javax.annotation.Nullable
  @ApiModelProperty(example = "3", value = "Unique code for the parish (kirke sogn)  Parish is the basic unit of the Norwegian Church.  <br>Remarks:   The church district can extend over several municipalities  Freg: Kirkekrets")

  public Long getChurchDistrict() {
    return churchDistrict;
  }


  public void setChurchDistrict(Long churchDistrict) {
    this.churchDistrict = churchDistrict;
  }


  public ResidentialAddress urbanDistrictCode(String urbanDistrictCode) {
    
    this.urbanDistrictCode = urbanDistrictCode;
    return this;
  }

   /**
   * Unique code for the urban district (bydel kode)  &lt;br&gt;SSB: Bydel id
   * @return urbanDistrictCode
  **/
  @javax.annotation.Nullable
  @ApiModelProperty(example = "030116", value = "Unique code for the urban district (bydel kode)  <br>SSB: Bydel id")

  public String getUrbanDistrictCode() {
    return urbanDistrictCode;
  }


  public void setUrbanDistrictCode(String urbanDistrictCode) {
    this.urbanDistrictCode = urbanDistrictCode;
  }


  public ResidentialAddress urbanDistrictName(String urbanDistrictName) {
    
    this.urbanDistrictName = urbanDistrictName;
    return this;
  }

   /**
   * The Urban district name (bydel)  &lt;br&gt;SSB: Bydel 
   * @return urbanDistrictName
  **/
  @javax.annotation.Nullable
  @ApiModelProperty(value = "The Urban district name (bydel)  <br>SSB: Bydel ")

  public String getUrbanDistrictName() {
    return urbanDistrictName;
  }


  public void setUrbanDistrictName(String urbanDistrictName) {
    this.urbanDistrictName = urbanDistrictName;
  }



  @Override
  public boolean equals(Object o) {
    if (this == o) {
      return true;
    }
    if (o == null || getClass() != o.getClass()) {
      return false;
    }
    ResidentialAddress residentialAddress = (ResidentialAddress) o;
    return Objects.equals(this.registeredAt, residentialAddress.registeredAt) &&
        Objects.equals(this.isValid, residentialAddress.isValid) &&
        Objects.equals(this.source, residentialAddress.source) &&
        Objects.equals(this.reason, residentialAddress.reason) &&
        Objects.equals(this.validFrom, residentialAddress.validFrom) &&
        Objects.equals(this.validTo, residentialAddress.validTo) &&
        Objects.equals(this.streetAddress, residentialAddress.streetAddress) &&
        Objects.equals(this.cadastralAddress, residentialAddress.cadastralAddress) &&
        Objects.equals(this.unknownResidence, residentialAddress.unknownResidence) &&
        Objects.equals(this.cadastralIdentifier, residentialAddress.cadastralIdentifier) &&
        Objects.equals(this.closeCadastralIdentifier, residentialAddress.closeCadastralIdentifier) &&
        Objects.equals(this.addressConfidentiality, residentialAddress.addressConfidentiality) &&
        Objects.equals(this.moveDate, residentialAddress.moveDate) &&
        Objects.equals(this.basicStatisticalUnit, residentialAddress.basicStatisticalUnit) &&
        Objects.equals(this.basicStatisticalUnitName, residentialAddress.basicStatisticalUnitName) &&
        Objects.equals(this.constituency, residentialAddress.constituency) &&
        Objects.equals(this.schoolDistrict, residentialAddress.schoolDistrict) &&
        Objects.equals(this.churchDistrict, residentialAddress.churchDistrict) &&
        Objects.equals(this.urbanDistrictCode, residentialAddress.urbanDistrictCode) &&
        Objects.equals(this.urbanDistrictName, residentialAddress.urbanDistrictName);
  }

  private static <T> boolean equalsNullable(JsonNullable<T> a, JsonNullable<T> b) {
    return a == b || (a != null && b != null && a.isPresent() && b.isPresent() && Objects.deepEquals(a.get(), b.get()));
  }

  @Override
  public int hashCode() {
    return Objects.hash(registeredAt, isValid, source, reason, validFrom, validTo, streetAddress, cadastralAddress, unknownResidence, cadastralIdentifier, closeCadastralIdentifier, addressConfidentiality, moveDate, basicStatisticalUnit, basicStatisticalUnitName, constituency, schoolDistrict, churchDistrict, urbanDistrictCode, urbanDistrictName);
  }

  private static <T> int hashCodeNullable(JsonNullable<T> a) {
    if (a == null) {
      return 1;
    }
    return a.isPresent() ? Arrays.deepHashCode(new Object[]{a.get()}) : 31;
  }

  @Override
  public String toString() {
    StringBuilder sb = new StringBuilder();
    sb.append("class ResidentialAddress {\n");
    sb.append("    registeredAt: ").append(toIndentedString(registeredAt)).append("\n");
    sb.append("    isValid: ").append(toIndentedString(isValid)).append("\n");
    sb.append("    source: ").append(toIndentedString(source)).append("\n");
    sb.append("    reason: ").append(toIndentedString(reason)).append("\n");
    sb.append("    validFrom: ").append(toIndentedString(validFrom)).append("\n");
    sb.append("    validTo: ").append(toIndentedString(validTo)).append("\n");
    sb.append("    streetAddress: ").append(toIndentedString(streetAddress)).append("\n");
    sb.append("    cadastralAddress: ").append(toIndentedString(cadastralAddress)).append("\n");
    sb.append("    unknownResidence: ").append(toIndentedString(unknownResidence)).append("\n");
    sb.append("    cadastralIdentifier: ").append(toIndentedString(cadastralIdentifier)).append("\n");
    sb.append("    closeCadastralIdentifier: ").append(toIndentedString(closeCadastralIdentifier)).append("\n");
    sb.append("    addressConfidentiality: ").append(toIndentedString(addressConfidentiality)).append("\n");
    sb.append("    moveDate: ").append(toIndentedString(moveDate)).append("\n");
    sb.append("    basicStatisticalUnit: ").append(toIndentedString(basicStatisticalUnit)).append("\n");
    sb.append("    basicStatisticalUnitName: ").append(toIndentedString(basicStatisticalUnitName)).append("\n");
    sb.append("    constituency: ").append(toIndentedString(constituency)).append("\n");
    sb.append("    schoolDistrict: ").append(toIndentedString(schoolDistrict)).append("\n");
    sb.append("    churchDistrict: ").append(toIndentedString(churchDistrict)).append("\n");
    sb.append("    urbanDistrictCode: ").append(toIndentedString(urbanDistrictCode)).append("\n");
    sb.append("    urbanDistrictName: ").append(toIndentedString(urbanDistrictName)).append("\n");
    sb.append("}");
    return sb.toString();
  }

  /**
   * Convert the given object to string with each line indented by 4 spaces
   * (except the first line).
   */
  private String toIndentedString(Object o) {
    if (o == null) {
      return "null";
    }
    return o.toString().replace("\n", "\n    ");
  }


  public static HashSet<String> openapiFields;
  public static HashSet<String> openapiRequiredFields;

  static {
    // a set of all properties/fields (JSON key names)
    openapiFields = new HashSet<String>();
    openapiFields.add("registeredAt");
    openapiFields.add("isValid");
    openapiFields.add("source");
    openapiFields.add("reason");
    openapiFields.add("validFrom");
    openapiFields.add("validTo");
    openapiFields.add("streetAddress");
    openapiFields.add("cadastralAddress");
    openapiFields.add("unknownResidence");
    openapiFields.add("cadastralIdentifier");
    openapiFields.add("closeCadastralIdentifier");
    openapiFields.add("addressConfidentiality");
    openapiFields.add("moveDate");
    openapiFields.add("basicStatisticalUnit");
    openapiFields.add("basicStatisticalUnitName");
    openapiFields.add("constituency");
    openapiFields.add("schoolDistrict");
    openapiFields.add("churchDistrict");
    openapiFields.add("urbanDistrictCode");
    openapiFields.add("urbanDistrictName");

    // a set of required properties/fields (JSON key names)
    openapiRequiredFields = new HashSet<String>();
  }

 /**
  * Validates the JSON Object and throws an exception if issues found
  *
  * @param jsonObj JSON Object
  * @throws IOException if the JSON Object is invalid with respect to ResidentialAddress
  */
  public static void validateJsonObject(JsonObject jsonObj) throws IOException {
      if (jsonObj == null) {
        if (ResidentialAddress.openapiRequiredFields.isEmpty()) {
          return;
        } else { // has required fields
          throw new IllegalArgumentException(String.format("The required field(s) %s in ResidentialAddress is not found in the empty JSON string", ResidentialAddress.openapiRequiredFields.toString()));
        }
      }

      Set<Entry<String, JsonElement>> entries = jsonObj.entrySet();
      // check to see if the JSON string contains additional fields
      for (Entry<String, JsonElement> entry : entries) {
        if (!ResidentialAddress.openapiFields.contains(entry.getKey())) {
          throw new IllegalArgumentException(String.format("The field `%s` in the JSON string is not defined in the `ResidentialAddress` properties. JSON: %s", entry.getKey(), jsonObj.toString()));
        }
      }
      if ((jsonObj.get("source") != null && !jsonObj.get("source").isJsonNull()) && !jsonObj.get("source").isJsonPrimitive()) {
        throw new IllegalArgumentException(String.format("Expected the field `source` to be a primitive type in the JSON string but got `%s`", jsonObj.get("source").toString()));
      }
      if ((jsonObj.get("reason") != null && !jsonObj.get("reason").isJsonNull()) && !jsonObj.get("reason").isJsonPrimitive()) {
        throw new IllegalArgumentException(String.format("Expected the field `reason` to be a primitive type in the JSON string but got `%s`", jsonObj.get("reason").toString()));
      }
      // validate the optional field `streetAddress`
      if (jsonObj.get("streetAddress") != null && !jsonObj.get("streetAddress").isJsonNull()) {
        ResidentialAddressStreetAddress.validateJsonObject(jsonObj.getAsJsonObject("streetAddress"));
      }
      // validate the optional field `cadastralAddress`
      if (jsonObj.get("cadastralAddress") != null && !jsonObj.get("cadastralAddress").isJsonNull()) {
        ResidentialAddressCadastralAddress.validateJsonObject(jsonObj.getAsJsonObject("cadastralAddress"));
      }
      // validate the optional field `unknownResidence`
      if (jsonObj.get("unknownResidence") != null && !jsonObj.get("unknownResidence").isJsonNull()) {
        ResidentialAddressUnknownResidence.validateJsonObject(jsonObj.getAsJsonObject("unknownResidence"));
      }
      if ((jsonObj.get("cadastralIdentifier") != null && !jsonObj.get("cadastralIdentifier").isJsonNull()) && !jsonObj.get("cadastralIdentifier").isJsonPrimitive()) {
        throw new IllegalArgumentException(String.format("Expected the field `cadastralIdentifier` to be a primitive type in the JSON string but got `%s`", jsonObj.get("cadastralIdentifier").toString()));
      }
      if ((jsonObj.get("closeCadastralIdentifier") != null && !jsonObj.get("closeCadastralIdentifier").isJsonNull()) && !jsonObj.get("closeCadastralIdentifier").isJsonPrimitive()) {
        throw new IllegalArgumentException(String.format("Expected the field `closeCadastralIdentifier` to be a primitive type in the JSON string but got `%s`", jsonObj.get("closeCadastralIdentifier").toString()));
      }
      if ((jsonObj.get("basicStatisticalUnitName") != null && !jsonObj.get("basicStatisticalUnitName").isJsonNull()) && !jsonObj.get("basicStatisticalUnitName").isJsonPrimitive()) {
        throw new IllegalArgumentException(String.format("Expected the field `basicStatisticalUnitName` to be a primitive type in the JSON string but got `%s`", jsonObj.get("basicStatisticalUnitName").toString()));
      }
      if ((jsonObj.get("urbanDistrictCode") != null && !jsonObj.get("urbanDistrictCode").isJsonNull()) && !jsonObj.get("urbanDistrictCode").isJsonPrimitive()) {
        throw new IllegalArgumentException(String.format("Expected the field `urbanDistrictCode` to be a primitive type in the JSON string but got `%s`", jsonObj.get("urbanDistrictCode").toString()));
      }
      if ((jsonObj.get("urbanDistrictName") != null && !jsonObj.get("urbanDistrictName").isJsonNull()) && !jsonObj.get("urbanDistrictName").isJsonPrimitive()) {
        throw new IllegalArgumentException(String.format("Expected the field `urbanDistrictName` to be a primitive type in the JSON string but got `%s`", jsonObj.get("urbanDistrictName").toString()));
      }
  }

  public static class CustomTypeAdapterFactory implements TypeAdapterFactory {
    @SuppressWarnings("unchecked")
    @Override
    public <T> TypeAdapter<T> create(Gson gson, TypeToken<T> type) {
       if (!ResidentialAddress.class.isAssignableFrom(type.getRawType())) {
         return null; // this class only serializes 'ResidentialAddress' and its subtypes
       }
       final TypeAdapter<JsonElement> elementAdapter = gson.getAdapter(JsonElement.class);
       final TypeAdapter<ResidentialAddress> thisAdapter
                        = gson.getDelegateAdapter(this, TypeToken.get(ResidentialAddress.class));

       return (TypeAdapter<T>) new TypeAdapter<ResidentialAddress>() {
           @Override
           public void write(JsonWriter out, ResidentialAddress value) throws IOException {
             JsonObject obj = thisAdapter.toJsonTree(value).getAsJsonObject();
             elementAdapter.write(out, obj);
           }

           @Override
           public ResidentialAddress read(JsonReader in) throws IOException {
             JsonObject jsonObj = elementAdapter.read(in).getAsJsonObject();
             validateJsonObject(jsonObj);
             return thisAdapter.fromJsonTree(jsonObj);
           }

       }.nullSafe();
    }
  }

 /**
  * Create an instance of ResidentialAddress given an JSON string
  *
  * @param jsonString JSON string
  * @return An instance of ResidentialAddress
  * @throws IOException if the JSON string is invalid with respect to ResidentialAddress
  */
  public static ResidentialAddress fromJson(String jsonString) throws IOException {
    return JSON.getGson().fromJson(jsonString, ResidentialAddress.class);
  }

 /**
  * Convert an instance of ResidentialAddress to an JSON string
  *
  * @return JSON string
  */
  public String toJson() {
    return JSON.getGson().toJson(this);
  }
}

