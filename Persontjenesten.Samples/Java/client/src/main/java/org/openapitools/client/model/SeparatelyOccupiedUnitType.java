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
 * Categorization of occupancy unit type  &lt;br&gt;FREG: Bruksenhetstype&lt;p&gt;Possible values:&lt;/p&gt;&lt;ul&gt;&lt;li&gt;&lt;i&gt;Housing&lt;/i&gt; - Approved as housing              FREG: Bolig&lt;/li&gt;&lt;li&gt;&lt;i&gt;OtherThanHousing&lt;/i&gt; - Mostly commercial usage             FREG: AnnetEnnBolig&lt;/li&gt;&lt;li&gt;&lt;i&gt;HolidayHome&lt;/i&gt; - Approved as holiday home, but not housing             FREG: Fritidsbolig&lt;/li&gt;&lt;li&gt;&lt;i&gt;NotApprovedAsHousing&lt;/i&gt; - Unit most likely not approved as housing             FREG: IkkeGodkjentBolig&lt;/li&gt;&lt;li&gt;&lt;i&gt;UnnumberedSeparatelyOccupiedUnit&lt;/i&gt; - Used to connect building-, cadastral- and address             FREG: UnummerertBruksenhet&lt;/li&gt;&lt;/ul&gt;
 */
@JsonAdapter(SeparatelyOccupiedUnitType.Adapter.class)
public enum SeparatelyOccupiedUnitType {
  
  HOUSING("Housing"),
  
  OTHER_THAN_HOUSING("OtherThanHousing"),
  
  HOLIDAY_HOME("HolidayHome"),
  
  NOT_APPROVED_AS_HOUSING("NotApprovedAsHousing"),
  
  UNNUMBERED_SEPARATELY_OCCUPIED_UNIT("UnnumberedSeparatelyOccupiedUnit");

  private String value;

  SeparatelyOccupiedUnitType(String value) {
    this.value = value;
  }

  public String getValue() {
    return value;
  }

  @Override
  public String toString() {
    return String.valueOf(value);
  }

  public static SeparatelyOccupiedUnitType fromValue(String value) {
    for (SeparatelyOccupiedUnitType b : SeparatelyOccupiedUnitType.values()) {
      if (b.value.equals(value)) {
        return b;
      }
    }
    throw new IllegalArgumentException("Unexpected value '" + value + "'");
  }

  public static class Adapter extends TypeAdapter<SeparatelyOccupiedUnitType> {
    @Override
    public void write(final JsonWriter jsonWriter, final SeparatelyOccupiedUnitType enumeration) throws IOException {
      jsonWriter.value(enumeration.getValue());
    }

    @Override
    public SeparatelyOccupiedUnitType read(final JsonReader jsonReader) throws IOException {
      String value = jsonReader.nextString();
      return SeparatelyOccupiedUnitType.fromValue(value);
    }
  }

  public static void validateJsonElement(JsonElement jsonElement) throws IOException {
    String value = jsonElement.getAsString();
    SeparatelyOccupiedUnitType.fromValue(value);
  }
}

