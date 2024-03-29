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
import java.util.Arrays;
import java.util.HashSet;
import java.util.Map.Entry;
import java.util.Objects;
import java.util.Set;

/**
 * Used if the persons shared residence is unknown  &lt;br&gt;Remarks:   May be missing, only one of the address elements are used. See ResidentialAddress  Freg: UkjentBosted
 */
@ApiModel(description = "Used if the persons shared residence is unknown  <br>Remarks:   May be missing, only one of the address elements are used. See ResidentialAddress  Freg: UkjentBosted")
@javax.annotation.Generated(value = "org.openapitools.codegen.languages.JavaClientCodegen", date = "2022-10-21T11:48:55.741350900+02:00[Europe/Oslo]")
public class SharedResidenceUnknownResidence {
  public static final String SERIALIZED_NAME_MUNICIPALITY_NUMBER = "municipalityNumber";
  @SerializedName(SERIALIZED_NAME_MUNICIPALITY_NUMBER)
  private String municipalityNumber;

  public SharedResidenceUnknownResidence() {
  }

  public SharedResidenceUnknownResidence municipalityNumber(String municipalityNumber) {
    
    this.municipalityNumber = municipalityNumber;
    return this;
  }

   /**
   * Persons adress is unknown, but the Municipality is known.  &lt;br&gt;FREG: Bostedskommune
   * @return municipalityNumber
  **/
  @javax.annotation.Nullable
  @ApiModelProperty(value = "Persons adress is unknown, but the Municipality is known.  <br>FREG: Bostedskommune")

  public String getMunicipalityNumber() {
    return municipalityNumber;
  }


  public void setMunicipalityNumber(String municipalityNumber) {
    this.municipalityNumber = municipalityNumber;
  }



  @Override
  public boolean equals(Object o) {
    if (this == o) {
      return true;
    }
    if (o == null || getClass() != o.getClass()) {
      return false;
    }
    SharedResidenceUnknownResidence sharedResidenceUnknownResidence = (SharedResidenceUnknownResidence) o;
    return Objects.equals(this.municipalityNumber, sharedResidenceUnknownResidence.municipalityNumber);
  }

  private static <T> boolean equalsNullable(JsonNullable<T> a, JsonNullable<T> b) {
    return a == b || (a != null && b != null && a.isPresent() && b.isPresent() && Objects.deepEquals(a.get(), b.get()));
  }

  @Override
  public int hashCode() {
    return Objects.hash(municipalityNumber);
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
    sb.append("class SharedResidenceUnknownResidence {\n");
    sb.append("    municipalityNumber: ").append(toIndentedString(municipalityNumber)).append("\n");
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
    openapiFields.add("municipalityNumber");

    // a set of required properties/fields (JSON key names)
    openapiRequiredFields = new HashSet<String>();
  }

 /**
  * Validates the JSON Object and throws an exception if issues found
  *
  * @param jsonObj JSON Object
  * @throws IOException if the JSON Object is invalid with respect to SharedResidenceUnknownResidence
  */
  public static void validateJsonObject(JsonObject jsonObj) throws IOException {
      if (jsonObj == null) {
        if (SharedResidenceUnknownResidence.openapiRequiredFields.isEmpty()) {
          return;
        } else { // has required fields
          throw new IllegalArgumentException(String.format("The required field(s) %s in SharedResidenceUnknownResidence is not found in the empty JSON string", SharedResidenceUnknownResidence.openapiRequiredFields.toString()));
        }
      }

      Set<Entry<String, JsonElement>> entries = jsonObj.entrySet();
      // check to see if the JSON string contains additional fields
      for (Entry<String, JsonElement> entry : entries) {
        if (!SharedResidenceUnknownResidence.openapiFields.contains(entry.getKey())) {
          throw new IllegalArgumentException(String.format("The field `%s` in the JSON string is not defined in the `SharedResidenceUnknownResidence` properties. JSON: %s", entry.getKey(), jsonObj.toString()));
        }
      }
      if ((jsonObj.get("municipalityNumber") != null && !jsonObj.get("municipalityNumber").isJsonNull()) && !jsonObj.get("municipalityNumber").isJsonPrimitive()) {
        throw new IllegalArgumentException(String.format("Expected the field `municipalityNumber` to be a primitive type in the JSON string but got `%s`", jsonObj.get("municipalityNumber").toString()));
      }
  }

  public static class CustomTypeAdapterFactory implements TypeAdapterFactory {
    @SuppressWarnings("unchecked")
    @Override
    public <T> TypeAdapter<T> create(Gson gson, TypeToken<T> type) {
       if (!SharedResidenceUnknownResidence.class.isAssignableFrom(type.getRawType())) {
         return null; // this class only serializes 'SharedResidenceUnknownResidence' and its subtypes
       }
       final TypeAdapter<JsonElement> elementAdapter = gson.getAdapter(JsonElement.class);
       final TypeAdapter<SharedResidenceUnknownResidence> thisAdapter
                        = gson.getDelegateAdapter(this, TypeToken.get(SharedResidenceUnknownResidence.class));

       return (TypeAdapter<T>) new TypeAdapter<SharedResidenceUnknownResidence>() {
           @Override
           public void write(JsonWriter out, SharedResidenceUnknownResidence value) throws IOException {
             JsonObject obj = thisAdapter.toJsonTree(value).getAsJsonObject();
             elementAdapter.write(out, obj);
           }

           @Override
           public SharedResidenceUnknownResidence read(JsonReader in) throws IOException {
             JsonObject jsonObj = elementAdapter.read(in).getAsJsonObject();
             validateJsonObject(jsonObj);
             return thisAdapter.fromJsonTree(jsonObj);
           }

       }.nullSafe();
    }
  }

 /**
  * Create an instance of SharedResidenceUnknownResidence given an JSON string
  *
  * @param jsonString JSON string
  * @return An instance of SharedResidenceUnknownResidence
  * @throws IOException if the JSON string is invalid with respect to SharedResidenceUnknownResidence
  */
  public static SharedResidenceUnknownResidence fromJson(String jsonString) throws IOException {
    return JSON.getGson().fromJson(jsonString, SharedResidenceUnknownResidence.class);
  }

 /**
  * Convert an instance of SharedResidenceUnknownResidence to an JSON string
  *
  * @return JSON string
  */
  public String toJson() {
    return JSON.getGson().toJson(this);
  }
}

