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
import com.google.gson.annotations.SerializedName;

import java.io.IOException;
import com.google.gson.TypeAdapter;
import com.google.gson.JsonElement;
import com.google.gson.annotations.JsonAdapter;
import com.google.gson.stream.JsonReader;
import com.google.gson.stream.JsonWriter;

/**
 * &lt;br&gt;FREG: Vergemaalsomfang&lt;p&gt;Possible values:&lt;/p&gt;&lt;ul&gt;&lt;li&gt;&lt;i&gt;ImmigrationPersonalAndFinancialInterests&lt;/i&gt; - FREG: UtlendingssakerPersonligeOgOekonomiskeInteresser&lt;/li&gt;&lt;li&gt;&lt;i&gt;PersonalAndFinancialInterests&lt;/i&gt; - FREG: PersonligeOgOekonomiskeInteresser&lt;/li&gt;&lt;li&gt;&lt;i&gt;FinancialInterests&lt;/i&gt; - FREG: OekonomiskeInteresser&lt;/li&gt;&lt;li&gt;&lt;i&gt;PersonalInterests&lt;/i&gt; - FREG: PersonligeInteresser&lt;/li&gt;&lt;/ul&gt;
 */
@JsonAdapter(GuardianshipScope.Adapter.class)
public enum GuardianshipScope {
  
  IMMIGRATION_PERSONAL_AND_FINANCIAL_INTERESTS("ImmigrationPersonalAndFinancialInterests"),
  
  PERSONAL_AND_FINANCIAL_INTERESTS("PersonalAndFinancialInterests"),
  
  FINANCIAL_INTERESTS("FinancialInterests"),
  
  PERSONAL_INTERESTS("PersonalInterests");

  private String value;

  GuardianshipScope(String value) {
    this.value = value;
  }

  public String getValue() {
    return value;
  }

  @Override
  public String toString() {
    return String.valueOf(value);
  }

  public static GuardianshipScope fromValue(String value) {
    for (GuardianshipScope b : GuardianshipScope.values()) {
      if (b.value.equals(value)) {
        return b;
      }
    }
    throw new IllegalArgumentException("Unexpected value '" + value + "'");
  }

  public static class Adapter extends TypeAdapter<GuardianshipScope> {
    @Override
    public void write(final JsonWriter jsonWriter, final GuardianshipScope enumeration) throws IOException {
      jsonWriter.value(enumeration.getValue());
    }

    @Override
    public GuardianshipScope read(final JsonReader jsonReader) throws IOException {
      String value = jsonReader.nextString();
      return GuardianshipScope.fromValue(value);
    }
  }

  public static void validateJsonElement(JsonElement jsonElement) throws IOException {
    String value = jsonElement.getAsString();
    GuardianshipScope.fromValue(value);
  }
}
