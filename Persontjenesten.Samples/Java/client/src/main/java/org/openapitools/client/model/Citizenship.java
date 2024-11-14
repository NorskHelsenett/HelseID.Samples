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
 * Information on citizenships held by the person.                Country codes are given in ISO 3166-1 alpha 3, with the following  extensions, following rules from UDI:                XXX &#x3D; No citizenship  XUK &#x3D; Unknown citizenship  XXK &#x3D; Kosovo  &lt;br&gt;FREG: Statsborgerskap
 */
@javax.annotation.Generated(value = "org.openapitools.codegen.languages.JavaClientCodegen", date = "2024-05-30T07:50:48.244395087Z[Etc/UTC]", comments = "Generator version: 7.7.0-SNAPSHOT")
public class Citizenship {
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

  public static final String SERIALIZED_NAME_COUNTRY_CODE = "countryCode";
  @SerializedName(SERIALIZED_NAME_COUNTRY_CODE)
  private String countryCode;

  public static final String SERIALIZED_NAME_AQUIRED_DATE = "aquiredDate";
  @SerializedName(SERIALIZED_NAME_AQUIRED_DATE)
  private OffsetDateTime aquiredDate;

  public Citizenship() {
  }

  public Citizenship registeredAt(OffsetDateTime registeredAt) {
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


  public Citizenship isValid(Boolean isValid) {
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


  public Citizenship source(String source) {
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


  public Citizenship reason(String reason) {
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


  public Citizenship validFrom(OffsetDateTime validFrom) {
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


  public Citizenship validTo(OffsetDateTime validTo) {
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


  public Citizenship countryCode(String countryCode) {
    this.countryCode = countryCode;
    return this;
  }

   /**
   * The country code, given in ISO 3166-1 alpha 3  &lt;br&gt;FREG: Statsborgerskap
   * @return countryCode
  **/
  @javax.annotation.Nullable
  public String getCountryCode() {
    return countryCode;
  }

  public void setCountryCode(String countryCode) {
    this.countryCode = countryCode;
  }


  public Citizenship aquiredDate(OffsetDateTime aquiredDate) {
    this.aquiredDate = aquiredDate;
    return this;
  }

   /**
   * &lt;br&gt;FREG: Ervervsdato
   * @return aquiredDate
  **/
  @javax.annotation.Nullable
  public OffsetDateTime getAquiredDate() {
    return aquiredDate;
  }

  public void setAquiredDate(OffsetDateTime aquiredDate) {
    this.aquiredDate = aquiredDate;
  }



  @Override
  public boolean equals(Object o) {
    if (this == o) {
      return true;
    }
    if (o == null || getClass() != o.getClass()) {
      return false;
    }
    Citizenship citizenship = (Citizenship) o;
    return Objects.equals(this.registeredAt, citizenship.registeredAt) &&
        Objects.equals(this.isValid, citizenship.isValid) &&
        Objects.equals(this.source, citizenship.source) &&
        Objects.equals(this.reason, citizenship.reason) &&
        Objects.equals(this.validFrom, citizenship.validFrom) &&
        Objects.equals(this.validTo, citizenship.validTo) &&
        Objects.equals(this.countryCode, citizenship.countryCode) &&
        Objects.equals(this.aquiredDate, citizenship.aquiredDate);
  }

  private static <T> boolean equalsNullable(JsonNullable<T> a, JsonNullable<T> b) {
    return a == b || (a != null && b != null && a.isPresent() && b.isPresent() && Objects.deepEquals(a.get(), b.get()));
  }

  @Override
  public int hashCode() {
    return Objects.hash(registeredAt, isValid, source, reason, validFrom, validTo, countryCode, aquiredDate);
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
    sb.append("class Citizenship {\n");
    sb.append("    registeredAt: ").append(toIndentedString(registeredAt)).append("\n");
    sb.append("    isValid: ").append(toIndentedString(isValid)).append("\n");
    sb.append("    source: ").append(toIndentedString(source)).append("\n");
    sb.append("    reason: ").append(toIndentedString(reason)).append("\n");
    sb.append("    validFrom: ").append(toIndentedString(validFrom)).append("\n");
    sb.append("    validTo: ").append(toIndentedString(validTo)).append("\n");
    sb.append("    countryCode: ").append(toIndentedString(countryCode)).append("\n");
    sb.append("    aquiredDate: ").append(toIndentedString(aquiredDate)).append("\n");
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
    openapiFields.add("countryCode");
    openapiFields.add("aquiredDate");

    // a set of required properties/fields (JSON key names)
    openapiRequiredFields = new HashSet<String>();
  }

 /**
  * Validates the JSON Element and throws an exception if issues found
  *
  * @param jsonElement JSON Element
  * @throws IOException if the JSON Element is invalid with respect to Citizenship
  */
  public static void validateJsonElement(JsonElement jsonElement) throws IOException {
      if (jsonElement == null) {
        if (!Citizenship.openapiRequiredFields.isEmpty()) { // has required fields but JSON element is null
          throw new IllegalArgumentException(String.format("The required field(s) %s in Citizenship is not found in the empty JSON string", Citizenship.openapiRequiredFields.toString()));
        }
      }

      Set<Map.Entry<String, JsonElement>> entries = jsonElement.getAsJsonObject().entrySet();
      // check to see if the JSON string contains additional fields
      for (Map.Entry<String, JsonElement> entry : entries) {
        if (!Citizenship.openapiFields.contains(entry.getKey())) {
          throw new IllegalArgumentException(String.format("The field `%s` in the JSON string is not defined in the `Citizenship` properties. JSON: %s", entry.getKey(), jsonElement.toString()));
        }
      }
        JsonObject jsonObj = jsonElement.getAsJsonObject();
      if ((jsonObj.get("source") != null && !jsonObj.get("source").isJsonNull()) && !jsonObj.get("source").isJsonPrimitive()) {
        throw new IllegalArgumentException(String.format("Expected the field `source` to be a primitive type in the JSON string but got `%s`", jsonObj.get("source").toString()));
      }
      if ((jsonObj.get("reason") != null && !jsonObj.get("reason").isJsonNull()) && !jsonObj.get("reason").isJsonPrimitive()) {
        throw new IllegalArgumentException(String.format("Expected the field `reason` to be a primitive type in the JSON string but got `%s`", jsonObj.get("reason").toString()));
      }
      if ((jsonObj.get("countryCode") != null && !jsonObj.get("countryCode").isJsonNull()) && !jsonObj.get("countryCode").isJsonPrimitive()) {
        throw new IllegalArgumentException(String.format("Expected the field `countryCode` to be a primitive type in the JSON string but got `%s`", jsonObj.get("countryCode").toString()));
      }
  }

  public static class CustomTypeAdapterFactory implements TypeAdapterFactory {
    @SuppressWarnings("unchecked")
    @Override
    public <T> TypeAdapter<T> create(Gson gson, TypeToken<T> type) {
       if (!Citizenship.class.isAssignableFrom(type.getRawType())) {
         return null; // this class only serializes 'Citizenship' and its subtypes
       }
       final TypeAdapter<JsonElement> elementAdapter = gson.getAdapter(JsonElement.class);
       final TypeAdapter<Citizenship> thisAdapter
                        = gson.getDelegateAdapter(this, TypeToken.get(Citizenship.class));

       return (TypeAdapter<T>) new TypeAdapter<Citizenship>() {
           @Override
           public void write(JsonWriter out, Citizenship value) throws IOException {
             JsonObject obj = thisAdapter.toJsonTree(value).getAsJsonObject();
             elementAdapter.write(out, obj);
           }

           @Override
           public Citizenship read(JsonReader in) throws IOException {
             JsonElement jsonElement = elementAdapter.read(in);
             validateJsonElement(jsonElement);
             return thisAdapter.fromJsonTree(jsonElement);
           }

       }.nullSafe();
    }
  }

 /**
  * Create an instance of Citizenship given an JSON string
  *
  * @param jsonString JSON string
  * @return An instance of Citizenship
  * @throws IOException if the JSON string is invalid with respect to Citizenship
  */
  public static Citizenship fromJson(String jsonString) throws IOException {
    return JSON.getGson().fromJson(jsonString, Citizenship.class);
  }

 /**
  * Convert an instance of Citizenship to an JSON string
  *
  * @return JSON string
  */
  public String toJson() {
    return JSON.getGson().toJson(this);
  }
}
