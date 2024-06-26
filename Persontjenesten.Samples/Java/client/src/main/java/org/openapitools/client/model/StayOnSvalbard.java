/*
 * Persontjenesten API - ET
 * The Person API provides information on norwegian citizens for the norwegian public health sector, and is maintained by [Norsk helsenett](https://www.nhn.no/).  An API changelog is available [here](../static/changelog/index.html).  For more documentation and a complete integration guide, see the [NHN developer portal](https://utviklerportal.nhn.no/informasjonstjenester/persontjenesten/). 
 *
 * The version of the OpenAPI document: 2
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */


package org.openapitools.client.model;

import java.util.Objects;
import com.google.gson.TypeAdapter;
import com.google.gson.annotations.JsonAdapter;
import com.google.gson.annotations.SerializedName;
import com.google.gson.stream.JsonReader;
import com.google.gson.stream.JsonWriter;
import java.io.IOException;
import java.time.OffsetDateTime;
import java.util.Arrays;
import org.openapitools.client.model.LengthOfStay;
import org.openapitools.jackson.nullable.JsonNullable;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.JsonArray;
import com.google.gson.JsonDeserializationContext;
import com.google.gson.JsonDeserializer;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParseException;
import com.google.gson.TypeAdapterFactory;
import com.google.gson.reflect.TypeToken;
import com.google.gson.TypeAdapter;
import com.google.gson.stream.JsonReader;
import com.google.gson.stream.JsonWriter;
import java.io.IOException;

import java.lang.reflect.Type;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import org.openapitools.client.JSON;

/**
 * Information regarding a persons residency on Svalbard  &lt;br&gt;FREG: OppholdPaaSvalbard
 */
@javax.annotation.Generated(value = "org.openapitools.codegen.languages.JavaClientCodegen", date = "2024-05-30T07:50:48.244395087Z[Etc/UTC]", comments = "Generator version: 7.7.0-SNAPSHOT")
public class StayOnSvalbard {
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

  public static final String SERIALIZED_NAME_START_DATE = "startDate";
  @SerializedName(SERIALIZED_NAME_START_DATE)
  private OffsetDateTime startDate;

  public static final String SERIALIZED_NAME_END_DATE = "endDate";
  @SerializedName(SERIALIZED_NAME_END_DATE)
  private OffsetDateTime endDate;

  public static final String SERIALIZED_NAME_IMMIGRATED_FROM_MUNICIPALITY_NUMBER = "immigratedFromMunicipalityNumber";
  @SerializedName(SERIALIZED_NAME_IMMIGRATED_FROM_MUNICIPALITY_NUMBER)
  private String immigratedFromMunicipalityNumber;

  public static final String SERIALIZED_NAME_IMMIGRATED_FROM_COUNTRY = "immigratedFromCountry";
  @SerializedName(SERIALIZED_NAME_IMMIGRATED_FROM_COUNTRY)
  private String immigratedFromCountry;

  public static final String SERIALIZED_NAME_NUMBER_OF_PREVIOUS_STAYS = "numberOfPreviousStays";
  @SerializedName(SERIALIZED_NAME_NUMBER_OF_PREVIOUS_STAYS)
  private Long numberOfPreviousStays;

  public static final String SERIALIZED_NAME_ESTIMATED_LENGTH_OF_STAY = "estimatedLengthOfStay";
  @SerializedName(SERIALIZED_NAME_ESTIMATED_LENGTH_OF_STAY)
  private LengthOfStay estimatedLengthOfStay;

  public StayOnSvalbard() {
  }

  public StayOnSvalbard registeredAt(OffsetDateTime registeredAt) {
    this.registeredAt = registeredAt;
    return this;
  }

   /**
   * &lt;br&gt;FREG: Ajourholdstidspunkt
   * @return registeredAt
  **/
  @javax.annotation.Nullable
  public OffsetDateTime getRegisteredAt() {
    return registeredAt;
  }

  public void setRegisteredAt(OffsetDateTime registeredAt) {
    this.registeredAt = registeredAt;
  }


  public StayOnSvalbard isValid(Boolean isValid) {
    this.isValid = isValid;
    return this;
  }

   /**
   * &lt;br&gt;FREG: ErGjeldende
   * @return isValid
  **/
  @javax.annotation.Nullable
  public Boolean getIsValid() {
    return isValid;
  }

  public void setIsValid(Boolean isValid) {
    this.isValid = isValid;
  }


  public StayOnSvalbard source(String source) {
    this.source = source;
    return this;
  }

   /**
   * &lt;br&gt;FREG: Kilde
   * @return source
  **/
  @javax.annotation.Nullable
  public String getSource() {
    return source;
  }

  public void setSource(String source) {
    this.source = source;
  }


  public StayOnSvalbard reason(String reason) {
    this.reason = reason;
    return this;
  }

   /**
   * &lt;br&gt;FREG: Aarsak
   * @return reason
  **/
  @javax.annotation.Nullable
  public String getReason() {
    return reason;
  }

  public void setReason(String reason) {
    this.reason = reason;
  }


  public StayOnSvalbard validFrom(OffsetDateTime validFrom) {
    this.validFrom = validFrom;
    return this;
  }

   /**
   * &lt;br&gt;FREG: Gyldighetstidspunkt
   * @return validFrom
  **/
  @javax.annotation.Nullable
  public OffsetDateTime getValidFrom() {
    return validFrom;
  }

  public void setValidFrom(OffsetDateTime validFrom) {
    this.validFrom = validFrom;
  }


  public StayOnSvalbard validTo(OffsetDateTime validTo) {
    this.validTo = validTo;
    return this;
  }

   /**
   * &lt;br&gt;FREG: Opphoerstidspunkt
   * @return validTo
  **/
  @javax.annotation.Nullable
  public OffsetDateTime getValidTo() {
    return validTo;
  }

  public void setValidTo(OffsetDateTime validTo) {
    this.validTo = validTo;
  }


  public StayOnSvalbard startDate(OffsetDateTime startDate) {
    this.startDate = startDate;
    return this;
  }

   /**
   * &lt;br&gt;FREG: Startdato
   * @return startDate
  **/
  @javax.annotation.Nullable
  public OffsetDateTime getStartDate() {
    return startDate;
  }

  public void setStartDate(OffsetDateTime startDate) {
    this.startDate = startDate;
  }


  public StayOnSvalbard endDate(OffsetDateTime endDate) {
    this.endDate = endDate;
    return this;
  }

   /**
   * &lt;br&gt;FREG: Sluttdato
   * @return endDate
  **/
  @javax.annotation.Nullable
  public OffsetDateTime getEndDate() {
    return endDate;
  }

  public void setEndDate(OffsetDateTime endDate) {
    this.endDate = endDate;
  }


  public StayOnSvalbard immigratedFromMunicipalityNumber(String immigratedFromMunicipalityNumber) {
    this.immigratedFromMunicipalityNumber = immigratedFromMunicipalityNumber;
    return this;
  }

   /**
   * &lt;br&gt;FREG: Fraflyttingskommunenummer
   * @return immigratedFromMunicipalityNumber
  **/
  @javax.annotation.Nullable
  public String getImmigratedFromMunicipalityNumber() {
    return immigratedFromMunicipalityNumber;
  }

  public void setImmigratedFromMunicipalityNumber(String immigratedFromMunicipalityNumber) {
    this.immigratedFromMunicipalityNumber = immigratedFromMunicipalityNumber;
  }


  public StayOnSvalbard immigratedFromCountry(String immigratedFromCountry) {
    this.immigratedFromCountry = immigratedFromCountry;
    return this;
  }

   /**
   * &lt;br&gt;FREG: Fraflyttingsland
   * @return immigratedFromCountry
  **/
  @javax.annotation.Nullable
  public String getImmigratedFromCountry() {
    return immigratedFromCountry;
  }

  public void setImmigratedFromCountry(String immigratedFromCountry) {
    this.immigratedFromCountry = immigratedFromCountry;
  }


  public StayOnSvalbard numberOfPreviousStays(Long numberOfPreviousStays) {
    this.numberOfPreviousStays = numberOfPreviousStays;
    return this;
  }

   /**
   * &lt;br&gt;FREG: AntallTidligereOpphold
   * @return numberOfPreviousStays
  **/
  @javax.annotation.Nullable
  public Long getNumberOfPreviousStays() {
    return numberOfPreviousStays;
  }

  public void setNumberOfPreviousStays(Long numberOfPreviousStays) {
    this.numberOfPreviousStays = numberOfPreviousStays;
  }


  public StayOnSvalbard estimatedLengthOfStay(LengthOfStay estimatedLengthOfStay) {
    this.estimatedLengthOfStay = estimatedLengthOfStay;
    return this;
  }

   /**
   * &lt;br&gt;FREG: AntattOppholdsvarighet
   * @return estimatedLengthOfStay
  **/
  @javax.annotation.Nullable
  public LengthOfStay getEstimatedLengthOfStay() {
    return estimatedLengthOfStay;
  }

  public void setEstimatedLengthOfStay(LengthOfStay estimatedLengthOfStay) {
    this.estimatedLengthOfStay = estimatedLengthOfStay;
  }



  @Override
  public boolean equals(Object o) {
    if (this == o) {
      return true;
    }
    if (o == null || getClass() != o.getClass()) {
      return false;
    }
    StayOnSvalbard stayOnSvalbard = (StayOnSvalbard) o;
    return Objects.equals(this.registeredAt, stayOnSvalbard.registeredAt) &&
        Objects.equals(this.isValid, stayOnSvalbard.isValid) &&
        Objects.equals(this.source, stayOnSvalbard.source) &&
        Objects.equals(this.reason, stayOnSvalbard.reason) &&
        Objects.equals(this.validFrom, stayOnSvalbard.validFrom) &&
        Objects.equals(this.validTo, stayOnSvalbard.validTo) &&
        Objects.equals(this.startDate, stayOnSvalbard.startDate) &&
        Objects.equals(this.endDate, stayOnSvalbard.endDate) &&
        Objects.equals(this.immigratedFromMunicipalityNumber, stayOnSvalbard.immigratedFromMunicipalityNumber) &&
        Objects.equals(this.immigratedFromCountry, stayOnSvalbard.immigratedFromCountry) &&
        Objects.equals(this.numberOfPreviousStays, stayOnSvalbard.numberOfPreviousStays) &&
        Objects.equals(this.estimatedLengthOfStay, stayOnSvalbard.estimatedLengthOfStay);
  }

  private static <T> boolean equalsNullable(JsonNullable<T> a, JsonNullable<T> b) {
    return a == b || (a != null && b != null && a.isPresent() && b.isPresent() && Objects.deepEquals(a.get(), b.get()));
  }

  @Override
  public int hashCode() {
    return Objects.hash(registeredAt, isValid, source, reason, validFrom, validTo, startDate, endDate, immigratedFromMunicipalityNumber, immigratedFromCountry, numberOfPreviousStays, estimatedLengthOfStay);
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
    sb.append("class StayOnSvalbard {\n");
    sb.append("    registeredAt: ").append(toIndentedString(registeredAt)).append("\n");
    sb.append("    isValid: ").append(toIndentedString(isValid)).append("\n");
    sb.append("    source: ").append(toIndentedString(source)).append("\n");
    sb.append("    reason: ").append(toIndentedString(reason)).append("\n");
    sb.append("    validFrom: ").append(toIndentedString(validFrom)).append("\n");
    sb.append("    validTo: ").append(toIndentedString(validTo)).append("\n");
    sb.append("    startDate: ").append(toIndentedString(startDate)).append("\n");
    sb.append("    endDate: ").append(toIndentedString(endDate)).append("\n");
    sb.append("    immigratedFromMunicipalityNumber: ").append(toIndentedString(immigratedFromMunicipalityNumber)).append("\n");
    sb.append("    immigratedFromCountry: ").append(toIndentedString(immigratedFromCountry)).append("\n");
    sb.append("    numberOfPreviousStays: ").append(toIndentedString(numberOfPreviousStays)).append("\n");
    sb.append("    estimatedLengthOfStay: ").append(toIndentedString(estimatedLengthOfStay)).append("\n");
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
    openapiFields.add("startDate");
    openapiFields.add("endDate");
    openapiFields.add("immigratedFromMunicipalityNumber");
    openapiFields.add("immigratedFromCountry");
    openapiFields.add("numberOfPreviousStays");
    openapiFields.add("estimatedLengthOfStay");

    // a set of required properties/fields (JSON key names)
    openapiRequiredFields = new HashSet<String>();
  }

 /**
  * Validates the JSON Element and throws an exception if issues found
  *
  * @param jsonElement JSON Element
  * @throws IOException if the JSON Element is invalid with respect to StayOnSvalbard
  */
  public static void validateJsonElement(JsonElement jsonElement) throws IOException {
      if (jsonElement == null) {
        if (!StayOnSvalbard.openapiRequiredFields.isEmpty()) { // has required fields but JSON element is null
          throw new IllegalArgumentException(String.format("The required field(s) %s in StayOnSvalbard is not found in the empty JSON string", StayOnSvalbard.openapiRequiredFields.toString()));
        }
      }

      Set<Map.Entry<String, JsonElement>> entries = jsonElement.getAsJsonObject().entrySet();
      // check to see if the JSON string contains additional fields
      for (Map.Entry<String, JsonElement> entry : entries) {
        if (!StayOnSvalbard.openapiFields.contains(entry.getKey())) {
          throw new IllegalArgumentException(String.format("The field `%s` in the JSON string is not defined in the `StayOnSvalbard` properties. JSON: %s", entry.getKey(), jsonElement.toString()));
        }
      }
        JsonObject jsonObj = jsonElement.getAsJsonObject();
      if ((jsonObj.get("source") != null && !jsonObj.get("source").isJsonNull()) && !jsonObj.get("source").isJsonPrimitive()) {
        throw new IllegalArgumentException(String.format("Expected the field `source` to be a primitive type in the JSON string but got `%s`", jsonObj.get("source").toString()));
      }
      if ((jsonObj.get("reason") != null && !jsonObj.get("reason").isJsonNull()) && !jsonObj.get("reason").isJsonPrimitive()) {
        throw new IllegalArgumentException(String.format("Expected the field `reason` to be a primitive type in the JSON string but got `%s`", jsonObj.get("reason").toString()));
      }
      if ((jsonObj.get("immigratedFromMunicipalityNumber") != null && !jsonObj.get("immigratedFromMunicipalityNumber").isJsonNull()) && !jsonObj.get("immigratedFromMunicipalityNumber").isJsonPrimitive()) {
        throw new IllegalArgumentException(String.format("Expected the field `immigratedFromMunicipalityNumber` to be a primitive type in the JSON string but got `%s`", jsonObj.get("immigratedFromMunicipalityNumber").toString()));
      }
      if ((jsonObj.get("immigratedFromCountry") != null && !jsonObj.get("immigratedFromCountry").isJsonNull()) && !jsonObj.get("immigratedFromCountry").isJsonPrimitive()) {
        throw new IllegalArgumentException(String.format("Expected the field `immigratedFromCountry` to be a primitive type in the JSON string but got `%s`", jsonObj.get("immigratedFromCountry").toString()));
      }
      // validate the optional field `estimatedLengthOfStay`
      if (jsonObj.get("estimatedLengthOfStay") != null && !jsonObj.get("estimatedLengthOfStay").isJsonNull()) {
        LengthOfStay.validateJsonElement(jsonObj.get("estimatedLengthOfStay"));
      }
  }

  public static class CustomTypeAdapterFactory implements TypeAdapterFactory {
    @SuppressWarnings("unchecked")
    @Override
    public <T> TypeAdapter<T> create(Gson gson, TypeToken<T> type) {
       if (!StayOnSvalbard.class.isAssignableFrom(type.getRawType())) {
         return null; // this class only serializes 'StayOnSvalbard' and its subtypes
       }
       final TypeAdapter<JsonElement> elementAdapter = gson.getAdapter(JsonElement.class);
       final TypeAdapter<StayOnSvalbard> thisAdapter
                        = gson.getDelegateAdapter(this, TypeToken.get(StayOnSvalbard.class));

       return (TypeAdapter<T>) new TypeAdapter<StayOnSvalbard>() {
           @Override
           public void write(JsonWriter out, StayOnSvalbard value) throws IOException {
             JsonObject obj = thisAdapter.toJsonTree(value).getAsJsonObject();
             elementAdapter.write(out, obj);
           }

           @Override
           public StayOnSvalbard read(JsonReader in) throws IOException {
             JsonElement jsonElement = elementAdapter.read(in);
             validateJsonElement(jsonElement);
             return thisAdapter.fromJsonTree(jsonElement);
           }

       }.nullSafe();
    }
  }

 /**
  * Create an instance of StayOnSvalbard given an JSON string
  *
  * @param jsonString JSON string
  * @return An instance of StayOnSvalbard
  * @throws IOException if the JSON string is invalid with respect to StayOnSvalbard
  */
  public static StayOnSvalbard fromJson(String jsonString) throws IOException {
    return JSON.getGson().fromJson(jsonString, StayOnSvalbard.class);
  }

 /**
  * Convert an instance of StayOnSvalbard to an JSON string
  *
  * @return JSON string
  */
  public String toJson() {
    return JSON.getGson().toJson(this);
  }
}

