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
 * &lt;br&gt;KRR: digital_post              Digital Post information for a person
 */
@ApiModel(description = "<br>KRR: digital_post              Digital Post information for a person")
@javax.annotation.Generated(value = "org.openapitools.codegen.languages.JavaClientCodegen", date = "2022-10-21T11:48:55.741350900+02:00[Europe/Oslo]")
public class CommonContactRegisterInformationDigitalPost {
  public static final String SERIALIZED_NAME_DIGITAL_POST_ADDRESS = "digitalPostAddress";
  @SerializedName(SERIALIZED_NAME_DIGITAL_POST_ADDRESS)
  private String digitalPostAddress;

  public static final String SERIALIZED_NAME_DIGITAL_POST_VENDOR_ADDRESS = "digitalPostVendorAddress";
  @SerializedName(SERIALIZED_NAME_DIGITAL_POST_VENDOR_ADDRESS)
  private String digitalPostVendorAddress;

  public CommonContactRegisterInformationDigitalPost() {
  }

  public CommonContactRegisterInformationDigitalPost digitalPostAddress(String digitalPostAddress) {
    
    this.digitalPostAddress = digitalPostAddress;
    return this;
  }

   /**
   * &lt;br&gt;KRR: postkasseadresse              A unique address for a persons post address with a post vendor
   * @return digitalPostAddress
  **/
  @javax.annotation.Nullable
  @ApiModelProperty(example = "ola.nordmann#9YDT", value = "<br>KRR: postkasseadresse              A unique address for a persons post address with a post vendor")

  public String getDigitalPostAddress() {
    return digitalPostAddress;
  }


  public void setDigitalPostAddress(String digitalPostAddress) {
    this.digitalPostAddress = digitalPostAddress;
  }


  public CommonContactRegisterInformationDigitalPost digitalPostVendorAddress(String digitalPostVendorAddress) {
    
    this.digitalPostVendorAddress = digitalPostVendorAddress;
    return this;
  }

   /**
   * &lt;br&gt;KRR: postkasseleverandoeradresse              org number for a post vendor
   * @return digitalPostVendorAddress
  **/
  @javax.annotation.Nullable
  @ApiModelProperty(example = "984661185, 922020175", value = "<br>KRR: postkasseleverandoeradresse              org number for a post vendor")

  public String getDigitalPostVendorAddress() {
    return digitalPostVendorAddress;
  }


  public void setDigitalPostVendorAddress(String digitalPostVendorAddress) {
    this.digitalPostVendorAddress = digitalPostVendorAddress;
  }



  @Override
  public boolean equals(Object o) {
    if (this == o) {
      return true;
    }
    if (o == null || getClass() != o.getClass()) {
      return false;
    }
    CommonContactRegisterInformationDigitalPost commonContactRegisterInformationDigitalPost = (CommonContactRegisterInformationDigitalPost) o;
    return Objects.equals(this.digitalPostAddress, commonContactRegisterInformationDigitalPost.digitalPostAddress) &&
        Objects.equals(this.digitalPostVendorAddress, commonContactRegisterInformationDigitalPost.digitalPostVendorAddress);
  }

  private static <T> boolean equalsNullable(JsonNullable<T> a, JsonNullable<T> b) {
    return a == b || (a != null && b != null && a.isPresent() && b.isPresent() && Objects.deepEquals(a.get(), b.get()));
  }

  @Override
  public int hashCode() {
    return Objects.hash(digitalPostAddress, digitalPostVendorAddress);
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
    sb.append("class CommonContactRegisterInformationDigitalPost {\n");
    sb.append("    digitalPostAddress: ").append(toIndentedString(digitalPostAddress)).append("\n");
    sb.append("    digitalPostVendorAddress: ").append(toIndentedString(digitalPostVendorAddress)).append("\n");
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
    openapiFields.add("digitalPostAddress");
    openapiFields.add("digitalPostVendorAddress");

    // a set of required properties/fields (JSON key names)
    openapiRequiredFields = new HashSet<String>();
  }

 /**
  * Validates the JSON Object and throws an exception if issues found
  *
  * @param jsonObj JSON Object
  * @throws IOException if the JSON Object is invalid with respect to CommonContactRegisterInformationDigitalPost
  */
  public static void validateJsonObject(JsonObject jsonObj) throws IOException {
      if (jsonObj == null) {
        if (CommonContactRegisterInformationDigitalPost.openapiRequiredFields.isEmpty()) {
          return;
        } else { // has required fields
          throw new IllegalArgumentException(String.format("The required field(s) %s in CommonContactRegisterInformationDigitalPost is not found in the empty JSON string", CommonContactRegisterInformationDigitalPost.openapiRequiredFields.toString()));
        }
      }

      Set<Entry<String, JsonElement>> entries = jsonObj.entrySet();
      // check to see if the JSON string contains additional fields
      for (Entry<String, JsonElement> entry : entries) {
        if (!CommonContactRegisterInformationDigitalPost.openapiFields.contains(entry.getKey())) {
          throw new IllegalArgumentException(String.format("The field `%s` in the JSON string is not defined in the `CommonContactRegisterInformationDigitalPost` properties. JSON: %s", entry.getKey(), jsonObj.toString()));
        }
      }
      if ((jsonObj.get("digitalPostAddress") != null && !jsonObj.get("digitalPostAddress").isJsonNull()) && !jsonObj.get("digitalPostAddress").isJsonPrimitive()) {
        throw new IllegalArgumentException(String.format("Expected the field `digitalPostAddress` to be a primitive type in the JSON string but got `%s`", jsonObj.get("digitalPostAddress").toString()));
      }
      if ((jsonObj.get("digitalPostVendorAddress") != null && !jsonObj.get("digitalPostVendorAddress").isJsonNull()) && !jsonObj.get("digitalPostVendorAddress").isJsonPrimitive()) {
        throw new IllegalArgumentException(String.format("Expected the field `digitalPostVendorAddress` to be a primitive type in the JSON string but got `%s`", jsonObj.get("digitalPostVendorAddress").toString()));
      }
  }

  public static class CustomTypeAdapterFactory implements TypeAdapterFactory {
    @SuppressWarnings("unchecked")
    @Override
    public <T> TypeAdapter<T> create(Gson gson, TypeToken<T> type) {
       if (!CommonContactRegisterInformationDigitalPost.class.isAssignableFrom(type.getRawType())) {
         return null; // this class only serializes 'CommonContactRegisterInformationDigitalPost' and its subtypes
       }
       final TypeAdapter<JsonElement> elementAdapter = gson.getAdapter(JsonElement.class);
       final TypeAdapter<CommonContactRegisterInformationDigitalPost> thisAdapter
                        = gson.getDelegateAdapter(this, TypeToken.get(CommonContactRegisterInformationDigitalPost.class));

       return (TypeAdapter<T>) new TypeAdapter<CommonContactRegisterInformationDigitalPost>() {
           @Override
           public void write(JsonWriter out, CommonContactRegisterInformationDigitalPost value) throws IOException {
             JsonObject obj = thisAdapter.toJsonTree(value).getAsJsonObject();
             elementAdapter.write(out, obj);
           }

           @Override
           public CommonContactRegisterInformationDigitalPost read(JsonReader in) throws IOException {
             JsonObject jsonObj = elementAdapter.read(in).getAsJsonObject();
             validateJsonObject(jsonObj);
             return thisAdapter.fromJsonTree(jsonObj);
           }

       }.nullSafe();
    }
  }

 /**
  * Create an instance of CommonContactRegisterInformationDigitalPost given an JSON string
  *
  * @param jsonString JSON string
  * @return An instance of CommonContactRegisterInformationDigitalPost
  * @throws IOException if the JSON string is invalid with respect to CommonContactRegisterInformationDigitalPost
  */
  public static CommonContactRegisterInformationDigitalPost fromJson(String jsonString) throws IOException {
    return JSON.getGson().fromJson(jsonString, CommonContactRegisterInformationDigitalPost.class);
  }

 /**
  * Convert an instance of CommonContactRegisterInformationDigitalPost to an JSON string
  *
  * @return JSON string
  */
  public String toJson() {
    return JSON.getGson().toJson(this);
  }
}

