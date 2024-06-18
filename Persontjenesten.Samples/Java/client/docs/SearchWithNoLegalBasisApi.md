# SearchWithNoLegalBasisApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**apiNoLegalBasisSearchMatchCountPost**](SearchWithNoLegalBasisApi.md#apiNoLegalBasisSearchMatchCountPost) | **POST** /api/no-legal-basis/search/match-count | Works same way as \&quot;paged-match-list\&quot; regarding search parameters, but returned result is just the total count of matches that would have been returned. |
| [**apiNoLegalBasisSearchMatchListPost**](SearchWithNoLegalBasisApi.md#apiNoLegalBasisSearchMatchListPost) | **POST** /api/no-legal-basis/search/match-list | Search for a set of persons based on current information. Returns a search result with unique person ids, PersonId |
| [**apiNoLegalBasisSearchPagedMatchListPost**](SearchWithNoLegalBasisApi.md#apiNoLegalBasisSearchPagedMatchListPost) | **POST** /api/no-legal-basis/search/paged-match-list | Search for a set of persons based on current information. Returns a search result with unique person ids, see HgdPerson.Out.OpenApi.Model.Person  The search result is limited to the provided page size and shifted by the provided index offset. |
| [**apiNoLegalBasisSearchPersonPost**](SearchWithNoLegalBasisApi.md#apiNoLegalBasisSearchPersonPost) | **POST** /api/no-legal-basis/search/person | Search for a limited set of persons based on current information. Returns a search result with max 100 person documents. |


<a id="apiNoLegalBasisSearchMatchCountPost"></a>
# **apiNoLegalBasisSearchMatchCountPost**
> SearchMatchCountResult apiNoLegalBasisSearchMatchCountPost(apiVersion, includeAinResults, pageSize, indexOffset, fullName, givenName, middleName, familyName, streetAddress, postalCode, municipalityNumber, basicStatisticalUnit, birthDateFrom, birthDateTo, gender, personStatuses)

Works same way as \&quot;paged-match-list\&quot; regarding search parameters, but returned result is just the total count of matches that would have been returned.

See documentation for \&quot;paged-match-list\&quot; for more details.&lt;br /&gt;&lt;br /&gt;&lt;b&gt;Requires HelseId scope:&lt;/b&gt; ReadNoLegalBasis

### Example
```java
// Import classes:
import org.openapitools.client.ApiClient;
import org.openapitools.client.ApiException;
import org.openapitools.client.Configuration;
import org.openapitools.client.auth.*;
import org.openapitools.client.models.*;
import org.openapitools.client.api.SearchWithNoLegalBasisApi;

public class Example {
  public static void main(String[] args) {
    ApiClient defaultClient = Configuration.getDefaultApiClient();
    defaultClient.setBasePath("http://localhost");
    
    // Configure HTTP bearer authorization: HelseID
    HttpBearerAuth HelseID = (HttpBearerAuth) defaultClient.getAuthentication("HelseID");
    HelseID.setBearerToken("BEARER TOKEN");

    SearchWithNoLegalBasisApi apiInstance = new SearchWithNoLegalBasisApi(defaultClient);
    String apiVersion = "2"; // String | The requested API version
    Boolean includeAinResults = false; // Boolean | flag to determine if search results should include persons with alternate identification numbers
    Integer pageSize = 1000; // Integer | The maximum size of the returned result set. Defaults to 1000, max page size is 10000.  When the returned result set is less then the requested page size then you have reached the end of the total search result.
    Integer indexOffset = 0; // Integer | The index offset for the search. A paged-match-list search will return both a startIndex and endIndex in the response.  If the number of search results is larger than the page size, the next portion of the result can be retrieved by setting the  index offset to the end index of the first result, plus one (endIndex + 1).
    String fullName = "fullName_example"; // String | The whole or parts of a person's full name. Only letters are allowed, up to 600 characters.  The search string will be divided into separate search words, and these words can match  the start of any part of the person's full name, regardless of the order of search words.  Only the first three search words will be used.
    String givenName = "givenName_example"; // String | The whole or the first part of a person's given name.  Only letters are allowed, up to 200 characters.
    String middleName = "middleName_example"; // String | The whole or first part of a person's middle name.  Only letters are allowed, up to 200 characters.
    String familyName = "familyName_example"; // String | The whole or first part of a person's family name.  Only letters are allowed, up to 200 characters.
    String streetAddress = "streetAddress_example"; // String | The street address of the street where the person lives.  Contains of letters, house number, and house letter, up to 200 characters.
    String postalCode = "postalCode_example"; // String | The postal code (postnummer) number where the person lives.  If provided, needs to be exactly four digits.
    String municipalityNumber = "municipalityNumber_example"; // String | The municipality number of the municipality the person lives in.  If provided, needs to be exactly four digits.
    String basicStatisticalUnit = "basicStatisticalUnit_example"; // String | The basic statistical unit number of the basic statistical unit the person lives in.  If provided, needs to be exactly eight digits.
    String birthDateFrom = "birthDateFrom_example"; // String | Will return persons with a birthday equal to or higher than the given value.  The person's date of birth provided as a string, formatted as YYYY-MM-DD  Searching only by year (YYYY) or year and month (YYYY-MM) is also allowed.
    String birthDateTo = "birthDateTo_example"; // String | Will return persons with a birthday equal to or lower than the given value.  The person's date of birth provided as a string, formatted as YYYY-MM-DD.  Searching only by year (YYYY) or year and month (YYYY-MM) is also allowed.
    Gender gender = Gender.fromValue("Female"); // Gender | The person's gender
    List<PersonStatusType> personStatuses = Arrays.asList(); // List<PersonStatusType> | The nature of the relationship/connection  to Norway and the National Population Registry
    try {
      SearchMatchCountResult result = apiInstance.apiNoLegalBasisSearchMatchCountPost(apiVersion, includeAinResults, pageSize, indexOffset, fullName, givenName, middleName, familyName, streetAddress, postalCode, municipalityNumber, basicStatisticalUnit, birthDateFrom, birthDateTo, gender, personStatuses);
      System.out.println(result);
    } catch (ApiException e) {
      System.err.println("Exception when calling SearchWithNoLegalBasisApi#apiNoLegalBasisSearchMatchCountPost");
      System.err.println("Status code: " + e.getCode());
      System.err.println("Reason: " + e.getResponseBody());
      System.err.println("Response headers: " + e.getResponseHeaders());
      e.printStackTrace();
    }
  }
}
```

### Parameters

| Name | Type | Description  | Notes |
|------------- | ------------- | ------------- | -------------|
| **apiVersion** | **String**| The requested API version | [default to 2] |
| **includeAinResults** | **Boolean**| flag to determine if search results should include persons with alternate identification numbers | [optional] [default to false] |
| **pageSize** | **Integer**| The maximum size of the returned result set. Defaults to 1000, max page size is 10000.  When the returned result set is less then the requested page size then you have reached the end of the total search result. | [optional] [default to 1000] |
| **indexOffset** | **Integer**| The index offset for the search. A paged-match-list search will return both a startIndex and endIndex in the response.  If the number of search results is larger than the page size, the next portion of the result can be retrieved by setting the  index offset to the end index of the first result, plus one (endIndex + 1). | [optional] [default to 0] |
| **fullName** | **String**| The whole or parts of a person&#39;s full name. Only letters are allowed, up to 600 characters.  The search string will be divided into separate search words, and these words can match  the start of any part of the person&#39;s full name, regardless of the order of search words.  Only the first three search words will be used. | [optional] |
| **givenName** | **String**| The whole or the first part of a person&#39;s given name.  Only letters are allowed, up to 200 characters. | [optional] |
| **middleName** | **String**| The whole or first part of a person&#39;s middle name.  Only letters are allowed, up to 200 characters. | [optional] |
| **familyName** | **String**| The whole or first part of a person&#39;s family name.  Only letters are allowed, up to 200 characters. | [optional] |
| **streetAddress** | **String**| The street address of the street where the person lives.  Contains of letters, house number, and house letter, up to 200 characters. | [optional] |
| **postalCode** | **String**| The postal code (postnummer) number where the person lives.  If provided, needs to be exactly four digits. | [optional] |
| **municipalityNumber** | **String**| The municipality number of the municipality the person lives in.  If provided, needs to be exactly four digits. | [optional] |
| **basicStatisticalUnit** | **String**| The basic statistical unit number of the basic statistical unit the person lives in.  If provided, needs to be exactly eight digits. | [optional] |
| **birthDateFrom** | **String**| Will return persons with a birthday equal to or higher than the given value.  The person&#39;s date of birth provided as a string, formatted as YYYY-MM-DD  Searching only by year (YYYY) or year and month (YYYY-MM) is also allowed. | [optional] |
| **birthDateTo** | **String**| Will return persons with a birthday equal to or lower than the given value.  The person&#39;s date of birth provided as a string, formatted as YYYY-MM-DD.  Searching only by year (YYYY) or year and month (YYYY-MM) is also allowed. | [optional] |
| **gender** | [**Gender**](Gender.md)| The person&#39;s gender | [optional] [enum: Female, Male] |
| **personStatuses** | [**List&lt;PersonStatusType&gt;**](PersonStatusType.md)| The nature of the relationship/connection  to Norway and the National Population Registry | [optional] |

### Return type

[**SearchMatchCountResult**](SearchMatchCountResult.md)

### Authorization

[HelseID](../README.md#HelseID)

### HTTP request headers

 - **Content-Type**: application/x-www-form-urlencoded
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Search result with a list of person ids |  -  |
| **400** | Bad request |  -  |
| **401** | Unauthorized |  -  |

<a id="apiNoLegalBasisSearchMatchListPost"></a>
# **apiNoLegalBasisSearchMatchListPost**
> List&lt;String&gt; apiNoLegalBasisSearchMatchListPost(apiVersion, fullName, givenName, middleName, familyName, streetAddress, postalCode, municipalityNumber, basicStatisticalUnit, birthDateFrom, birthDateTo, gender, personStatuses)

Search for a set of persons based on current information. Returns a search result with unique person ids, PersonId

Search for a set of persons based on current information. Returns a search result with unique person ids, see Person&lt;br /&gt;&lt;br /&gt;&lt;b&gt;Requires HelseId scope:&lt;/b&gt; ReadNoLegalBasis

### Example
```java
// Import classes:
import org.openapitools.client.ApiClient;
import org.openapitools.client.ApiException;
import org.openapitools.client.Configuration;
import org.openapitools.client.auth.*;
import org.openapitools.client.models.*;
import org.openapitools.client.api.SearchWithNoLegalBasisApi;

public class Example {
  public static void main(String[] args) {
    ApiClient defaultClient = Configuration.getDefaultApiClient();
    defaultClient.setBasePath("http://localhost");
    
    // Configure HTTP bearer authorization: HelseID
    HttpBearerAuth HelseID = (HttpBearerAuth) defaultClient.getAuthentication("HelseID");
    HelseID.setBearerToken("BEARER TOKEN");

    SearchWithNoLegalBasisApi apiInstance = new SearchWithNoLegalBasisApi(defaultClient);
    String apiVersion = "2"; // String | The requested API version
    String fullName = "fullName_example"; // String | The whole or parts of a person's full name. Only letters are allowed, up to 600 characters.  The search string will be divided into separate search words, and these words can match  the start of any part of the person's full name, regardless of the order of search words.  Only the first three search words will be used.
    String givenName = "givenName_example"; // String | The whole or the first part of a person's given name.  Only letters are allowed, up to 200 characters.
    String middleName = "middleName_example"; // String | The whole or first part of a person's middle name.  Only letters are allowed, up to 200 characters.
    String familyName = "familyName_example"; // String | The whole or first part of a person's family name.  Only letters are allowed, up to 200 characters.
    String streetAddress = "streetAddress_example"; // String | The street address of the street where the person lives.  Contains of letters, house number, and house letter, up to 200 characters.
    String postalCode = "postalCode_example"; // String | The postal code (postnummer) number where the person lives.  If provided, needs to be exactly four digits.
    String municipalityNumber = "municipalityNumber_example"; // String | The municipality number of the municipality the person lives in.  If provided, needs to be exactly four digits.
    String basicStatisticalUnit = "basicStatisticalUnit_example"; // String | The basic statistical unit number of the basic statistical unit the person lives in.  If provided, needs to be exactly eight digits.
    String birthDateFrom = "birthDateFrom_example"; // String | Will return persons with a birthday equal to or higher than the given value.  The person's date of birth provided as a string, formatted as YYYY-MM-DD  Searching only by year (YYYY) or year and month (YYYY-MM) is also allowed.
    String birthDateTo = "birthDateTo_example"; // String | Will return persons with a birthday equal to or lower than the given value.  The person's date of birth provided as a string, formatted as YYYY-MM-DD.  Searching only by year (YYYY) or year and month (YYYY-MM) is also allowed.
    Gender gender = Gender.fromValue("Female"); // Gender | The person's gender
    List<PersonStatusType> personStatuses = Arrays.asList(); // List<PersonStatusType> | The nature of the relationship/connection  to Norway and the National Population Registry
    try {
      List<String> result = apiInstance.apiNoLegalBasisSearchMatchListPost(apiVersion, fullName, givenName, middleName, familyName, streetAddress, postalCode, municipalityNumber, basicStatisticalUnit, birthDateFrom, birthDateTo, gender, personStatuses);
      System.out.println(result);
    } catch (ApiException e) {
      System.err.println("Exception when calling SearchWithNoLegalBasisApi#apiNoLegalBasisSearchMatchListPost");
      System.err.println("Status code: " + e.getCode());
      System.err.println("Reason: " + e.getResponseBody());
      System.err.println("Response headers: " + e.getResponseHeaders());
      e.printStackTrace();
    }
  }
}
```

### Parameters

| Name | Type | Description  | Notes |
|------------- | ------------- | ------------- | -------------|
| **apiVersion** | **String**| The requested API version | [default to 2] |
| **fullName** | **String**| The whole or parts of a person&#39;s full name. Only letters are allowed, up to 600 characters.  The search string will be divided into separate search words, and these words can match  the start of any part of the person&#39;s full name, regardless of the order of search words.  Only the first three search words will be used. | [optional] |
| **givenName** | **String**| The whole or the first part of a person&#39;s given name.  Only letters are allowed, up to 200 characters. | [optional] |
| **middleName** | **String**| The whole or first part of a person&#39;s middle name.  Only letters are allowed, up to 200 characters. | [optional] |
| **familyName** | **String**| The whole or first part of a person&#39;s family name.  Only letters are allowed, up to 200 characters. | [optional] |
| **streetAddress** | **String**| The street address of the street where the person lives.  Contains of letters, house number, and house letter, up to 200 characters. | [optional] |
| **postalCode** | **String**| The postal code (postnummer) number where the person lives.  If provided, needs to be exactly four digits. | [optional] |
| **municipalityNumber** | **String**| The municipality number of the municipality the person lives in.  If provided, needs to be exactly four digits. | [optional] |
| **basicStatisticalUnit** | **String**| The basic statistical unit number of the basic statistical unit the person lives in.  If provided, needs to be exactly eight digits. | [optional] |
| **birthDateFrom** | **String**| Will return persons with a birthday equal to or higher than the given value.  The person&#39;s date of birth provided as a string, formatted as YYYY-MM-DD  Searching only by year (YYYY) or year and month (YYYY-MM) is also allowed. | [optional] |
| **birthDateTo** | **String**| Will return persons with a birthday equal to or lower than the given value.  The person&#39;s date of birth provided as a string, formatted as YYYY-MM-DD.  Searching only by year (YYYY) or year and month (YYYY-MM) is also allowed. | [optional] |
| **gender** | [**Gender**](Gender.md)| The person&#39;s gender | [optional] [enum: Female, Male] |
| **personStatuses** | [**List&lt;PersonStatusType&gt;**](PersonStatusType.md)| The nature of the relationship/connection  to Norway and the National Population Registry | [optional] |

### Return type

**List&lt;String&gt;**

### Authorization

[HelseID](../README.md#HelseID)

### HTTP request headers

 - **Content-Type**: application/x-www-form-urlencoded
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Search result with a list of person ids |  -  |
| **400** | Bad request |  -  |
| **401** | Unauthorized |  -  |

<a id="apiNoLegalBasisSearchPagedMatchListPost"></a>
# **apiNoLegalBasisSearchPagedMatchListPost**
> StringSearchResult apiNoLegalBasisSearchPagedMatchListPost(apiVersion, includeAinResults, pageSize, indexOffset, fullName, givenName, middleName, familyName, streetAddress, postalCode, municipalityNumber, basicStatisticalUnit, birthDateFrom, birthDateTo, gender, personStatuses)

Search for a set of persons based on current information. Returns a search result with unique person ids, see HgdPerson.Out.OpenApi.Model.Person  The search result is limited to the provided page size and shifted by the provided index offset.

Search for a set of persons based on current information. Returns a search result with unique person ids, see Person  together with the results start and end index with can be further used to paginate the potential remainder of the total search result.&lt;br /&gt;&lt;br /&gt;&lt;b&gt;Requires HelseId scope:&lt;/b&gt; ReadNoLegalBasis

### Example
```java
// Import classes:
import org.openapitools.client.ApiClient;
import org.openapitools.client.ApiException;
import org.openapitools.client.Configuration;
import org.openapitools.client.auth.*;
import org.openapitools.client.models.*;
import org.openapitools.client.api.SearchWithNoLegalBasisApi;

public class Example {
  public static void main(String[] args) {
    ApiClient defaultClient = Configuration.getDefaultApiClient();
    defaultClient.setBasePath("http://localhost");
    
    // Configure HTTP bearer authorization: HelseID
    HttpBearerAuth HelseID = (HttpBearerAuth) defaultClient.getAuthentication("HelseID");
    HelseID.setBearerToken("BEARER TOKEN");

    SearchWithNoLegalBasisApi apiInstance = new SearchWithNoLegalBasisApi(defaultClient);
    String apiVersion = "2"; // String | The requested API version
    Boolean includeAinResults = false; // Boolean | flag to determine if search results should include persons with alternate identification numbers
    Integer pageSize = 1000; // Integer | The maximum size of the returned result set. Defaults to 1000, max page size is 10000.  When the returned result set is less then the requested page size then you have reached the end of the total search result.
    Integer indexOffset = 0; // Integer | The index offset for the search. A paged-match-list search will return both a startIndex and endIndex in the response.  If the number of search results is larger than the page size, the next portion of the result can be retrieved by setting the  index offset to the end index of the first result, plus one (endIndex + 1).
    String fullName = "fullName_example"; // String | The whole or parts of a person's full name. Only letters are allowed, up to 600 characters.  The search string will be divided into separate search words, and these words can match  the start of any part of the person's full name, regardless of the order of search words.  Only the first three search words will be used.
    String givenName = "givenName_example"; // String | The whole or the first part of a person's given name.  Only letters are allowed, up to 200 characters.
    String middleName = "middleName_example"; // String | The whole or first part of a person's middle name.  Only letters are allowed, up to 200 characters.
    String familyName = "familyName_example"; // String | The whole or first part of a person's family name.  Only letters are allowed, up to 200 characters.
    String streetAddress = "streetAddress_example"; // String | The street address of the street where the person lives.  Contains of letters, house number, and house letter, up to 200 characters.
    String postalCode = "postalCode_example"; // String | The postal code (postnummer) number where the person lives.  If provided, needs to be exactly four digits.
    String municipalityNumber = "municipalityNumber_example"; // String | The municipality number of the municipality the person lives in.  If provided, needs to be exactly four digits.
    String basicStatisticalUnit = "basicStatisticalUnit_example"; // String | The basic statistical unit number of the basic statistical unit the person lives in.  If provided, needs to be exactly eight digits.
    String birthDateFrom = "birthDateFrom_example"; // String | Will return persons with a birthday equal to or higher than the given value.  The person's date of birth provided as a string, formatted as YYYY-MM-DD  Searching only by year (YYYY) or year and month (YYYY-MM) is also allowed.
    String birthDateTo = "birthDateTo_example"; // String | Will return persons with a birthday equal to or lower than the given value.  The person's date of birth provided as a string, formatted as YYYY-MM-DD.  Searching only by year (YYYY) or year and month (YYYY-MM) is also allowed.
    Gender gender = Gender.fromValue("Female"); // Gender | The person's gender
    List<PersonStatusType> personStatuses = Arrays.asList(); // List<PersonStatusType> | The nature of the relationship/connection  to Norway and the National Population Registry
    try {
      StringSearchResult result = apiInstance.apiNoLegalBasisSearchPagedMatchListPost(apiVersion, includeAinResults, pageSize, indexOffset, fullName, givenName, middleName, familyName, streetAddress, postalCode, municipalityNumber, basicStatisticalUnit, birthDateFrom, birthDateTo, gender, personStatuses);
      System.out.println(result);
    } catch (ApiException e) {
      System.err.println("Exception when calling SearchWithNoLegalBasisApi#apiNoLegalBasisSearchPagedMatchListPost");
      System.err.println("Status code: " + e.getCode());
      System.err.println("Reason: " + e.getResponseBody());
      System.err.println("Response headers: " + e.getResponseHeaders());
      e.printStackTrace();
    }
  }
}
```

### Parameters

| Name | Type | Description  | Notes |
|------------- | ------------- | ------------- | -------------|
| **apiVersion** | **String**| The requested API version | [default to 2] |
| **includeAinResults** | **Boolean**| flag to determine if search results should include persons with alternate identification numbers | [optional] [default to false] |
| **pageSize** | **Integer**| The maximum size of the returned result set. Defaults to 1000, max page size is 10000.  When the returned result set is less then the requested page size then you have reached the end of the total search result. | [optional] [default to 1000] |
| **indexOffset** | **Integer**| The index offset for the search. A paged-match-list search will return both a startIndex and endIndex in the response.  If the number of search results is larger than the page size, the next portion of the result can be retrieved by setting the  index offset to the end index of the first result, plus one (endIndex + 1). | [optional] [default to 0] |
| **fullName** | **String**| The whole or parts of a person&#39;s full name. Only letters are allowed, up to 600 characters.  The search string will be divided into separate search words, and these words can match  the start of any part of the person&#39;s full name, regardless of the order of search words.  Only the first three search words will be used. | [optional] |
| **givenName** | **String**| The whole or the first part of a person&#39;s given name.  Only letters are allowed, up to 200 characters. | [optional] |
| **middleName** | **String**| The whole or first part of a person&#39;s middle name.  Only letters are allowed, up to 200 characters. | [optional] |
| **familyName** | **String**| The whole or first part of a person&#39;s family name.  Only letters are allowed, up to 200 characters. | [optional] |
| **streetAddress** | **String**| The street address of the street where the person lives.  Contains of letters, house number, and house letter, up to 200 characters. | [optional] |
| **postalCode** | **String**| The postal code (postnummer) number where the person lives.  If provided, needs to be exactly four digits. | [optional] |
| **municipalityNumber** | **String**| The municipality number of the municipality the person lives in.  If provided, needs to be exactly four digits. | [optional] |
| **basicStatisticalUnit** | **String**| The basic statistical unit number of the basic statistical unit the person lives in.  If provided, needs to be exactly eight digits. | [optional] |
| **birthDateFrom** | **String**| Will return persons with a birthday equal to or higher than the given value.  The person&#39;s date of birth provided as a string, formatted as YYYY-MM-DD  Searching only by year (YYYY) or year and month (YYYY-MM) is also allowed. | [optional] |
| **birthDateTo** | **String**| Will return persons with a birthday equal to or lower than the given value.  The person&#39;s date of birth provided as a string, formatted as YYYY-MM-DD.  Searching only by year (YYYY) or year and month (YYYY-MM) is also allowed. | [optional] |
| **gender** | [**Gender**](Gender.md)| The person&#39;s gender | [optional] [enum: Female, Male] |
| **personStatuses** | [**List&lt;PersonStatusType&gt;**](PersonStatusType.md)| The nature of the relationship/connection  to Norway and the National Population Registry | [optional] |

### Return type

[**StringSearchResult**](StringSearchResult.md)

### Authorization

[HelseID](../README.md#HelseID)

### HTTP request headers

 - **Content-Type**: application/x-www-form-urlencoded
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Search result with a list of person ids |  -  |
| **400** | Bad request |  -  |
| **401** | Unauthorized |  -  |

<a id="apiNoLegalBasisSearchPersonPost"></a>
# **apiNoLegalBasisSearchPersonPost**
> PersonSearchResult apiNoLegalBasisSearchPersonPost(informationParts, apiVersion, includeHistory, includeAinResults, birthDate, gender, fullName, givenName, middleName, familyName, streetAddress, postalCode, cityName, municipalityNumber)

Search for a limited set of persons based on current information. Returns a search result with max 100 person documents.

Search parameters will match current information, historic information will not be searchable.  For addresses, it&#39;s possible to search for norwegian residential, present and shared residence addresses.                Search parameters have the following requirements:  &lt;br&gt;1. A value for either FirstName, MiddleName, LastName or FullName&lt;br&gt;2. Combined across all names, there must be at least three letters in total (whitespace is not counted)&lt;br&gt;3. DateOfBirth OR MunicipalityNumber with at least 4 digits OR StreetName with at least three letters (whitespace is not counted)&lt;br&gt;Some search parameters are also validated individually, see the search parameter description for more details.&lt;br /&gt;&lt;br /&gt;&lt;b&gt;Requires HelseId scope:&lt;/b&gt; ReadNoLegalBasis

### Example
```java
// Import classes:
import org.openapitools.client.ApiClient;
import org.openapitools.client.ApiException;
import org.openapitools.client.Configuration;
import org.openapitools.client.auth.*;
import org.openapitools.client.models.*;
import org.openapitools.client.api.SearchWithNoLegalBasisApi;

public class Example {
  public static void main(String[] args) {
    ApiClient defaultClient = Configuration.getDefaultApiClient();
    defaultClient.setBasePath("http://localhost");
    
    // Configure HTTP bearer authorization: HelseID
    HttpBearerAuth HelseID = (HttpBearerAuth) defaultClient.getAuthentication("HelseID");
    HelseID.setBearerToken("BEARER TOKEN");

    SearchWithNoLegalBasisApi apiInstance = new SearchWithNoLegalBasisApi(defaultClient);
    List<InformationPart> informationParts = Arrays.asList(); // List<InformationPart> | Which information parts that should be included in the result
    String apiVersion = "2"; // String | The requested API version
    Boolean includeHistory = false; // Boolean | Flag to indicate if response should include historic information, defaults to false
    Boolean includeAinResults = false; // Boolean | flag to determine if search results should include persons with alternate identification numbers
    String birthDate = "birthDate_example"; // String | The person's date of birth provided as a string, formatted as YYYY-MM-DD.  Searching only by year (YYYY) or year and month (YYYY-MM) is also allowed.
    Gender gender = Gender.fromValue("Female"); // Gender | The person's gender
    String fullName = "fullName_example"; // String | The whole or parts of a person's full name. Only letters are allowed, up to 600 characters.  The search string will be divided into separate search words, and these words can match  the start of any part of the person's full name, regardless of the order of search words.  Only the first three search words will be used.
    String givenName = "givenName_example"; // String | The whole or the first part of a person's given name.  Only letters are allowed, up to 200 characters.
    String middleName = "middleName_example"; // String | The whole or first part of a person's middle name.  Only letters are allowed, up to 200 characters.
    String familyName = "familyName_example"; // String | The whole or first part of a person's family name.  Only letters are allowed, up to 200 characters.
    String streetAddress = "streetAddress_example"; // String | The street address of the street where the person lives.  Contains of letters, housenumber, and houseletter, up to 200 characters.
    String postalCode = "postalCode_example"; // String | The postal code (postnummer) number where the person lives.  If provided, needs to be exactly four digits.
    String cityName = "cityName_example"; // String | The city name (poststed) of the city the person lives in.  Only letters allowed, up to 100 characters
    String municipalityNumber = "municipalityNumber_example"; // String | The municipality number of the municipality the person lives in.  If provided, needs to be exactly four digits.
    try {
      PersonSearchResult result = apiInstance.apiNoLegalBasisSearchPersonPost(informationParts, apiVersion, includeHistory, includeAinResults, birthDate, gender, fullName, givenName, middleName, familyName, streetAddress, postalCode, cityName, municipalityNumber);
      System.out.println(result);
    } catch (ApiException e) {
      System.err.println("Exception when calling SearchWithNoLegalBasisApi#apiNoLegalBasisSearchPersonPost");
      System.err.println("Status code: " + e.getCode());
      System.err.println("Reason: " + e.getResponseBody());
      System.err.println("Response headers: " + e.getResponseHeaders());
      e.printStackTrace();
    }
  }
}
```

### Parameters

| Name | Type | Description  | Notes |
|------------- | ------------- | ------------- | -------------|
| **informationParts** | [**List&lt;InformationPart&gt;**](InformationPart.md)| Which information parts that should be included in the result | |
| **apiVersion** | **String**| The requested API version | [default to 2] |
| **includeHistory** | **Boolean**| Flag to indicate if response should include historic information, defaults to false | [optional] [default to false] |
| **includeAinResults** | **Boolean**| flag to determine if search results should include persons with alternate identification numbers | [optional] [default to false] |
| **birthDate** | **String**| The person&#39;s date of birth provided as a string, formatted as YYYY-MM-DD.  Searching only by year (YYYY) or year and month (YYYY-MM) is also allowed. | [optional] |
| **gender** | [**Gender**](Gender.md)| The person&#39;s gender | [optional] [enum: Female, Male] |
| **fullName** | **String**| The whole or parts of a person&#39;s full name. Only letters are allowed, up to 600 characters.  The search string will be divided into separate search words, and these words can match  the start of any part of the person&#39;s full name, regardless of the order of search words.  Only the first three search words will be used. | [optional] |
| **givenName** | **String**| The whole or the first part of a person&#39;s given name.  Only letters are allowed, up to 200 characters. | [optional] |
| **middleName** | **String**| The whole or first part of a person&#39;s middle name.  Only letters are allowed, up to 200 characters. | [optional] |
| **familyName** | **String**| The whole or first part of a person&#39;s family name.  Only letters are allowed, up to 200 characters. | [optional] |
| **streetAddress** | **String**| The street address of the street where the person lives.  Contains of letters, housenumber, and houseletter, up to 200 characters. | [optional] |
| **postalCode** | **String**| The postal code (postnummer) number where the person lives.  If provided, needs to be exactly four digits. | [optional] |
| **cityName** | **String**| The city name (poststed) of the city the person lives in.  Only letters allowed, up to 100 characters | [optional] |
| **municipalityNumber** | **String**| The municipality number of the municipality the person lives in.  If provided, needs to be exactly four digits. | [optional] |

### Return type

[**PersonSearchResult**](PersonSearchResult.md)

### Authorization

[HelseID](../README.md#HelseID)

### HTTP request headers

 - **Content-Type**: application/x-www-form-urlencoded
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Search result with a list of person documents |  -  |
| **400** | Bad request |  -  |
| **401** | Unauthorized, invalid token |  -  |
| **403** | Forbidden, invalid scope |  -  |

