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
 * The nature of the relationship/connection  to Norway and the National Population Registry&lt;p&gt;Possible values:&lt;/p&gt;&lt;ul&gt;&lt;li&gt;&lt;i&gt;Resident&lt;/i&gt; - FREG: Bosatt&lt;/li&gt;&lt;li&gt;&lt;i&gt;Emigrated&lt;/i&gt; - FREG: Utflyttet&lt;/li&gt;&lt;li&gt;&lt;i&gt;Disappeared&lt;/i&gt; - FREG: Forsvunnet&lt;/li&gt;&lt;li&gt;&lt;i&gt;Deceased&lt;/i&gt; - FREG: Doed&lt;/li&gt;&lt;li&gt;&lt;i&gt;Discontinued&lt;/i&gt; - FREG: Opphoert&lt;/li&gt;&lt;li&gt;&lt;i&gt;RegisteredAtBirth&lt;/i&gt; - FREG: Foedselsregistrert&lt;/li&gt;&lt;li&gt;&lt;i&gt;Temporary&lt;/i&gt; - FREG: Midlertidig&lt;/li&gt;&lt;li&gt;&lt;i&gt;Inactive&lt;/i&gt; - FREG: Inaktiv&lt;/li&gt;&lt;li&gt;&lt;i&gt;NotResident&lt;/i&gt; - FREG: IkkeBosatt&lt;/li&gt;&lt;li&gt;&lt;i&gt;Active&lt;/i&gt; - FREG: Aktiv&lt;/li&gt;&lt;/ul&gt;
 */
@JsonAdapter(PersonStatusType.Adapter.class)
public enum PersonStatusType {
  
  RESIDENT("Resident"),
  
  EMIGRATED("Emigrated"),
  
  DISAPPEARED("Disappeared"),
  
  DECEASED("Deceased"),
  
  DISCONTINUED("Discontinued"),
  
  REGISTERED_AT_BIRTH("RegisteredAtBirth"),
  
  TEMPORARY("Temporary"),
  
  INACTIVE("Inactive"),
  
  NOT_RESIDENT("NotResident"),
  
  ACTIVE("Active");

  private String value;

  PersonStatusType(String value) {
    this.value = value;
  }

  public String getValue() {
    return value;
  }

  @Override
  public String toString() {
    return String.valueOf(value);
  }

  public static PersonStatusType fromValue(String value) {
    for (PersonStatusType b : PersonStatusType.values()) {
      if (b.value.equals(value)) {
        return b;
      }
    }
    throw new IllegalArgumentException("Unexpected value '" + value + "'");
  }

  public static class Adapter extends TypeAdapter<PersonStatusType> {
    @Override
    public void write(final JsonWriter jsonWriter, final PersonStatusType enumeration) throws IOException {
      jsonWriter.value(enumeration.getValue());
    }

    @Override
    public PersonStatusType read(final JsonReader jsonReader) throws IOException {
      String value = jsonReader.nextString();
      return PersonStatusType.fromValue(value);
    }
  }

  public static void validateJsonElement(JsonElement jsonElement) throws IOException {
    String value = jsonElement.getAsString();
    PersonStatusType.fromValue(value);
  }
}

