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
import java.util.Arrays;
import org.openapitools.client.model.OrganizationAsContactContactPersonName;
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
 * Organization as contact  Used in Residuary estate, if there is an organization that takes care of the estate  &lt;br&gt;FREG: OrganisasjonSomKontakt
 */
@javax.annotation.Generated(value = "org.openapitools.codegen.languages.JavaClientCodegen", date = "2024-05-30T07:50:48.244395087Z[Etc/UTC]", comments = "Generator version: 7.7.0-SNAPSHOT")
public class OrganizationAsContact {
  public static final String SERIALIZED_NAME_ORGANIZATION_NAME = "organizationName";
  @SerializedName(SERIALIZED_NAME_ORGANIZATION_NAME)
  private String organizationName;

  public static final String SERIALIZED_NAME_ORGANIZATION_NUMBER = "organizationNumber";
  @SerializedName(SERIALIZED_NAME_ORGANIZATION_NUMBER)
  private String organizationNumber;

  public static final String SERIALIZED_NAME_CONTACT_PERSON_NAME = "contactPersonName";
  @SerializedName(SERIALIZED_NAME_CONTACT_PERSON_NAME)
  private OrganizationAsContactContactPersonName contactPersonName;

  public OrganizationAsContact() {
  }

  public OrganizationAsContact organizationName(String organizationName) {
    this.organizationName = organizationName;
    return this;
  }

   /**
   * Name of organization, may be foreign  &lt;br&gt;FREG: Organisasjonsnavn
   * @return organizationName
  **/
  @javax.annotation.Nullable
  public String getOrganizationName() {
    return organizationName;
  }

  public void setOrganizationName(String organizationName) {
    this.organizationName = organizationName;
  }


  public OrganizationAsContact organizationNumber(String organizationNumber) {
    this.organizationNumber = organizationNumber;
    return this;
  }

   /**
   * Norwegian organization number, must be 9 digits and start with 8 or 9  &lt;br&gt;FREG: Organisasjonsnummer
   * @return organizationNumber
  **/
  @javax.annotation.Nullable
  public String getOrganizationNumber() {
    return organizationNumber;
  }

  public void setOrganizationNumber(String organizationNumber) {
    this.organizationNumber = organizationNumber;
  }


  public OrganizationAsContact contactPersonName(OrganizationAsContactContactPersonName contactPersonName) {
    this.contactPersonName = contactPersonName;
    return this;
  }

   /**
   * Get contactPersonName
   * @return contactPersonName
  **/
  @javax.annotation.Nullable
  public OrganizationAsContactContactPersonName getContactPersonName() {
    return contactPersonName;
  }

  public void setContactPersonName(OrganizationAsContactContactPersonName contactPersonName) {
    this.contactPersonName = contactPersonName;
  }



  @Override
  public boolean equals(Object o) {
    if (this == o) {
      return true;
    }
    if (o == null || getClass() != o.getClass()) {
      return false;
    }
    OrganizationAsContact organizationAsContact = (OrganizationAsContact) o;
    return Objects.equals(this.organizationName, organizationAsContact.organizationName) &&
        Objects.equals(this.organizationNumber, organizationAsContact.organizationNumber) &&
        Objects.equals(this.contactPersonName, organizationAsContact.contactPersonName);
  }

  private static <T> boolean equalsNullable(JsonNullable<T> a, JsonNullable<T> b) {
    return a == b || (a != null && b != null && a.isPresent() && b.isPresent() && Objects.deepEquals(a.get(), b.get()));
  }

  @Override
  public int hashCode() {
    return Objects.hash(organizationName, organizationNumber, contactPersonName);
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
    sb.append("class OrganizationAsContact {\n");
    sb.append("    organizationName: ").append(toIndentedString(organizationName)).append("\n");
    sb.append("    organizationNumber: ").append(toIndentedString(organizationNumber)).append("\n");
    sb.append("    contactPersonName: ").append(toIndentedString(contactPersonName)).append("\n");
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
    openapiFields.add("organizationName");
    openapiFields.add("organizationNumber");
    openapiFields.add("contactPersonName");

    // a set of required properties/fields (JSON key names)
    openapiRequiredFields = new HashSet<String>();
  }

 /**
  * Validates the JSON Element and throws an exception if issues found
  *
  * @param jsonElement JSON Element
  * @throws IOException if the JSON Element is invalid with respect to OrganizationAsContact
  */
  public static void validateJsonElement(JsonElement jsonElement) throws IOException {
      if (jsonElement == null) {
        if (!OrganizationAsContact.openapiRequiredFields.isEmpty()) { // has required fields but JSON element is null
          throw new IllegalArgumentException(String.format("The required field(s) %s in OrganizationAsContact is not found in the empty JSON string", OrganizationAsContact.openapiRequiredFields.toString()));
        }
      }

      Set<Map.Entry<String, JsonElement>> entries = jsonElement.getAsJsonObject().entrySet();
      // check to see if the JSON string contains additional fields
      for (Map.Entry<String, JsonElement> entry : entries) {
        if (!OrganizationAsContact.openapiFields.contains(entry.getKey())) {
          throw new IllegalArgumentException(String.format("The field `%s` in the JSON string is not defined in the `OrganizationAsContact` properties. JSON: %s", entry.getKey(), jsonElement.toString()));
        }
      }
        JsonObject jsonObj = jsonElement.getAsJsonObject();
      if ((jsonObj.get("organizationName") != null && !jsonObj.get("organizationName").isJsonNull()) && !jsonObj.get("organizationName").isJsonPrimitive()) {
        throw new IllegalArgumentException(String.format("Expected the field `organizationName` to be a primitive type in the JSON string but got `%s`", jsonObj.get("organizationName").toString()));
      }
      if ((jsonObj.get("organizationNumber") != null && !jsonObj.get("organizationNumber").isJsonNull()) && !jsonObj.get("organizationNumber").isJsonPrimitive()) {
        throw new IllegalArgumentException(String.format("Expected the field `organizationNumber` to be a primitive type in the JSON string but got `%s`", jsonObj.get("organizationNumber").toString()));
      }
      // validate the optional field `contactPersonName`
      if (jsonObj.get("contactPersonName") != null && !jsonObj.get("contactPersonName").isJsonNull()) {
        OrganizationAsContactContactPersonName.validateJsonElement(jsonObj.get("contactPersonName"));
      }
  }

  public static class CustomTypeAdapterFactory implements TypeAdapterFactory {
    @SuppressWarnings("unchecked")
    @Override
    public <T> TypeAdapter<T> create(Gson gson, TypeToken<T> type) {
       if (!OrganizationAsContact.class.isAssignableFrom(type.getRawType())) {
         return null; // this class only serializes 'OrganizationAsContact' and its subtypes
       }
       final TypeAdapter<JsonElement> elementAdapter = gson.getAdapter(JsonElement.class);
       final TypeAdapter<OrganizationAsContact> thisAdapter
                        = gson.getDelegateAdapter(this, TypeToken.get(OrganizationAsContact.class));

       return (TypeAdapter<T>) new TypeAdapter<OrganizationAsContact>() {
           @Override
           public void write(JsonWriter out, OrganizationAsContact value) throws IOException {
             JsonObject obj = thisAdapter.toJsonTree(value).getAsJsonObject();
             elementAdapter.write(out, obj);
           }

           @Override
           public OrganizationAsContact read(JsonReader in) throws IOException {
             JsonElement jsonElement = elementAdapter.read(in);
             validateJsonElement(jsonElement);
             return thisAdapter.fromJsonTree(jsonElement);
           }

       }.nullSafe();
    }
  }

 /**
  * Create an instance of OrganizationAsContact given an JSON string
  *
  * @param jsonString JSON string
  * @return An instance of OrganizationAsContact
  * @throws IOException if the JSON string is invalid with respect to OrganizationAsContact
  */
  public static OrganizationAsContact fromJson(String jsonString) throws IOException {
    return JSON.getGson().fromJson(jsonString, OrganizationAsContact.class);
  }

 /**
  * Convert an instance of OrganizationAsContact to an JSON string
  *
  * @return JSON string
  */
  public String toJson() {
    return JSON.getGson().toJson(this);
  }
}
