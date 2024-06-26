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
 * &lt;br&gt;FREG: Identifikatorstatuskode&lt;p&gt;Possible values:&lt;/p&gt;&lt;ul&gt;&lt;li&gt;&lt;i&gt;InUse&lt;/i&gt; - FREG: IBruk&lt;/li&gt;&lt;li&gt;&lt;i&gt;Discontinued&lt;/i&gt; - FREG: Opphoert&lt;/li&gt;&lt;/ul&gt;
 */
@JsonAdapter(IdentificationStatus.Adapter.class)
public enum IdentificationStatus {
  
  IN_USE("InUse"),
  
  DISCONTINUED("Discontinued");

  private String value;

  IdentificationStatus(String value) {
    this.value = value;
  }

  public String getValue() {
    return value;
  }

  @Override
  public String toString() {
    return String.valueOf(value);
  }

  public static IdentificationStatus fromValue(String value) {
    for (IdentificationStatus b : IdentificationStatus.values()) {
      if (b.value.equals(value)) {
        return b;
      }
    }
    throw new IllegalArgumentException("Unexpected value '" + value + "'");
  }

  public static class Adapter extends TypeAdapter<IdentificationStatus> {
    @Override
    public void write(final JsonWriter jsonWriter, final IdentificationStatus enumeration) throws IOException {
      jsonWriter.value(enumeration.getValue());
    }

    @Override
    public IdentificationStatus read(final JsonReader jsonReader) throws IOException {
      String value = jsonReader.nextString();
      return IdentificationStatus.fromValue(value);
    }
  }

  public static void validateJsonElement(JsonElement jsonElement) throws IOException {
    String value = jsonElement.getAsString();
    IdentificationStatus.fromValue(value);
  }
}

