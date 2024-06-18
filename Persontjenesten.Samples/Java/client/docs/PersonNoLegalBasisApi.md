# PersonNoLegalBasisApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**apiNoLegalBasisPersonBulkByIdPost**](PersonNoLegalBasisApi.md#apiNoLegalBasisPersonBulkByIdPost) | **POST** /api/no-legal-basis/person/bulk-by-id | Get multiple persons from a list of unique person ids, max 100 items. |
| [**apiNoLegalBasisPersonBulkByNinPost**](PersonNoLegalBasisApi.md#apiNoLegalBasisPersonBulkByNinPost) | **POST** /api/no-legal-basis/person/bulk-by-nin | Get multiple persons from a list of Norwegian Identification Numbers, max 100 items. |
| [**apiNoLegalBasisPersonGetByNinPost**](PersonNoLegalBasisApi.md#apiNoLegalBasisPersonGetByNinPost) | **POST** /api/no-legal-basis/person/get-by-nin | Get a specific Person by Norwegian Identification Number |
| [**apiNoLegalBasisPersonIdGet**](PersonNoLegalBasisApi.md#apiNoLegalBasisPersonIdGet) | **GET** /api/no-legal-basis/person/{id} | Get a specific person by unique id |


<a id="apiNoLegalBasisPersonBulkByIdPost"></a>
# **apiNoLegalBasisPersonBulkByIdPost**
> List&lt;Person&gt; apiNoLegalBasisPersonBulkByIdPost(informationParts, apiVersion, personIds, includeHistory)

Get multiple persons from a list of unique person ids, max 100 items.

Does not support nin&lt;br /&gt;&lt;br /&gt;&lt;b&gt;Requires HelseId scope:&lt;/b&gt; ReadNoLegalBasis

### Example
```java
// Import classes:
import org.openapitools.client.ApiClient;
import org.openapitools.client.ApiException;
import org.openapitools.client.Configuration;
import org.openapitools.client.auth.*;
import org.openapitools.client.models.*;
import org.openapitools.client.api.PersonNoLegalBasisApi;

public class Example {
  public static void main(String[] args) {
    ApiClient defaultClient = Configuration.getDefaultApiClient();
    defaultClient.setBasePath("http://localhost");
    
    // Configure HTTP bearer authorization: HelseID
    HttpBearerAuth HelseID = (HttpBearerAuth) defaultClient.getAuthentication("HelseID");
    HelseID.setBearerToken("BEARER TOKEN");

    PersonNoLegalBasisApi apiInstance = new PersonNoLegalBasisApi(defaultClient);
    List<InformationPart> informationParts = Arrays.asList(); // List<InformationPart> | Which information parts that should be included in the result
    String apiVersion = "2"; // String | The requested API version
    List<String> personIds = Arrays.asList(); // List<String> | 
    Boolean includeHistory = false; // Boolean | Flag to indicate if response should include historic information, defaults to false
    try {
      List<Person> result = apiInstance.apiNoLegalBasisPersonBulkByIdPost(informationParts, apiVersion, personIds, includeHistory);
      System.out.println(result);
    } catch (ApiException e) {
      System.err.println("Exception when calling PersonNoLegalBasisApi#apiNoLegalBasisPersonBulkByIdPost");
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
| **personIds** | [**List&lt;String&gt;**](String.md)|  | |
| **includeHistory** | **Boolean**| Flag to indicate if response should include historic information, defaults to false | [optional] [default to false] |

### Return type

[**List&lt;Person&gt;**](Person.md)

### Authorization

[HelseID](../README.md#HelseID)

### HTTP request headers

 - **Content-Type**: application/x-www-form-urlencoded
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Person documents returned |  -  |
| **400** | Bad request |  -  |
| **401** | Unauthorized, invalid token |  -  |
| **403** | Forbidden, invalid scope |  -  |

<a id="apiNoLegalBasisPersonBulkByNinPost"></a>
# **apiNoLegalBasisPersonBulkByNinPost**
> List&lt;Person&gt; apiNoLegalBasisPersonBulkByNinPost(informationParts, apiVersion, nins, includeHistory)

Get multiple persons from a list of Norwegian Identification Numbers, max 100 items.

Norwegian Identification Number includes national identity numbers (fødselsnummer) and D-numbers (D-nummer).&lt;br /&gt;&lt;br /&gt;&lt;b&gt;Requires HelseId scope:&lt;/b&gt; ReadNoLegalBasis

### Example
```java
// Import classes:
import org.openapitools.client.ApiClient;
import org.openapitools.client.ApiException;
import org.openapitools.client.Configuration;
import org.openapitools.client.auth.*;
import org.openapitools.client.models.*;
import org.openapitools.client.api.PersonNoLegalBasisApi;

public class Example {
  public static void main(String[] args) {
    ApiClient defaultClient = Configuration.getDefaultApiClient();
    defaultClient.setBasePath("http://localhost");
    
    // Configure HTTP bearer authorization: HelseID
    HttpBearerAuth HelseID = (HttpBearerAuth) defaultClient.getAuthentication("HelseID");
    HelseID.setBearerToken("BEARER TOKEN");

    PersonNoLegalBasisApi apiInstance = new PersonNoLegalBasisApi(defaultClient);
    List<InformationPart> informationParts = Arrays.asList(); // List<InformationPart> | Which information parts that should be included in the result
    String apiVersion = "2"; // String | The requested API version
    List<String> nins = Arrays.asList(); // List<String> | 
    Boolean includeHistory = false; // Boolean | Flag to indicate if response should include historic information, defaults to false
    try {
      List<Person> result = apiInstance.apiNoLegalBasisPersonBulkByNinPost(informationParts, apiVersion, nins, includeHistory);
      System.out.println(result);
    } catch (ApiException e) {
      System.err.println("Exception when calling PersonNoLegalBasisApi#apiNoLegalBasisPersonBulkByNinPost");
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
| **nins** | [**List&lt;String&gt;**](String.md)|  | |
| **includeHistory** | **Boolean**| Flag to indicate if response should include historic information, defaults to false | [optional] [default to false] |

### Return type

[**List&lt;Person&gt;**](Person.md)

### Authorization

[HelseID](../README.md#HelseID)

### HTTP request headers

 - **Content-Type**: application/x-www-form-urlencoded
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Person documents returned |  -  |
| **400** | Bad request |  -  |
| **401** | Unauthorized, invalid token |  -  |
| **403** | Forbidden, invalid scope |  -  |

<a id="apiNoLegalBasisPersonGetByNinPost"></a>
# **apiNoLegalBasisPersonGetByNinPost**
> Person apiNoLegalBasisPersonGetByNinPost(informationParts, apiVersion, nin, includeHistory)

Get a specific Person by Norwegian Identification Number

Get a person from a Norwegian Identification Number. This includes&lt;br /&gt;  - National identity numbers (fødselsnummer)&lt;br /&gt;  - D-numbers (D-nummer)&lt;br /&gt;  - Alternate identification numbers (felles hjelpenummer)&lt;br /&gt;  - ImmigrationAuthoritiesIdentificationNumber (DUF)&lt;br /&gt;                Historic NINs are supported. This is a POST request to avoid including NIN as part of the URL.&lt;br /&gt;&lt;br /&gt;&lt;b&gt;Requires HelseId scope:&lt;/b&gt; ReadNoLegalBasis

### Example
```java
// Import classes:
import org.openapitools.client.ApiClient;
import org.openapitools.client.ApiException;
import org.openapitools.client.Configuration;
import org.openapitools.client.auth.*;
import org.openapitools.client.models.*;
import org.openapitools.client.api.PersonNoLegalBasisApi;

public class Example {
  public static void main(String[] args) {
    ApiClient defaultClient = Configuration.getDefaultApiClient();
    defaultClient.setBasePath("http://localhost");
    
    // Configure HTTP bearer authorization: HelseID
    HttpBearerAuth HelseID = (HttpBearerAuth) defaultClient.getAuthentication("HelseID");
    HelseID.setBearerToken("BEARER TOKEN");

    PersonNoLegalBasisApi apiInstance = new PersonNoLegalBasisApi(defaultClient);
    List<InformationPart> informationParts = Arrays.asList(); // List<InformationPart> | Which information parts that should be included in the result
    String apiVersion = "2"; // String | The requested API version
    String nin = "nin_example"; // String | 
    Boolean includeHistory = false; // Boolean | Flag to indicate if response should include historic information, defaults to false
    try {
      Person result = apiInstance.apiNoLegalBasisPersonGetByNinPost(informationParts, apiVersion, nin, includeHistory);
      System.out.println(result);
    } catch (ApiException e) {
      System.err.println("Exception when calling PersonNoLegalBasisApi#apiNoLegalBasisPersonGetByNinPost");
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
| **nin** | **String**|  | |
| **includeHistory** | **Boolean**| Flag to indicate if response should include historic information, defaults to false | [optional] [default to false] |

### Return type

[**Person**](Person.md)

### Authorization

[HelseID](../README.md#HelseID)

### HTTP request headers

 - **Content-Type**: application/x-www-form-urlencoded
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Person returned |  -  |
| **400** | Bad request |  -  |
| **404** | Person not found |  -  |
| **401** | Unauthorized, invalid token |  -  |
| **403** | Forbidden, invalid scope |  -  |

<a id="apiNoLegalBasisPersonIdGet"></a>
# **apiNoLegalBasisPersonIdGet**
> Person apiNoLegalBasisPersonIdGet(id, informationParts, apiVersion, includeHistory)

Get a specific person by unique id

Does not support nin&lt;br /&gt;&lt;br /&gt;&lt;b&gt;Requires HelseId scope:&lt;/b&gt; ReadNoLegalBasis

### Example
```java
// Import classes:
import org.openapitools.client.ApiClient;
import org.openapitools.client.ApiException;
import org.openapitools.client.Configuration;
import org.openapitools.client.auth.*;
import org.openapitools.client.models.*;
import org.openapitools.client.api.PersonNoLegalBasisApi;

public class Example {
  public static void main(String[] args) {
    ApiClient defaultClient = Configuration.getDefaultApiClient();
    defaultClient.setBasePath("http://localhost");
    
    // Configure HTTP bearer authorization: HelseID
    HttpBearerAuth HelseID = (HttpBearerAuth) defaultClient.getAuthentication("HelseID");
    HelseID.setBearerToken("BEARER TOKEN");

    PersonNoLegalBasisApi apiInstance = new PersonNoLegalBasisApi(defaultClient);
    String id = "3f9c5e318c9497770e37e0ff925034848f9bb1899ce1e8047589fba1c322b278"; // String | The Person id
    List<InformationPart> informationParts = Arrays.asList(); // List<InformationPart> | Which information parts that should be included in the result
    String apiVersion = "2"; // String | The requested API version
    Boolean includeHistory = false; // Boolean | Flag to indicate if response should include historic information, defaults to false
    try {
      Person result = apiInstance.apiNoLegalBasisPersonIdGet(id, informationParts, apiVersion, includeHistory);
      System.out.println(result);
    } catch (ApiException e) {
      System.err.println("Exception when calling PersonNoLegalBasisApi#apiNoLegalBasisPersonIdGet");
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
| **id** | **String**| The Person id | |
| **informationParts** | [**List&lt;InformationPart&gt;**](InformationPart.md)| Which information parts that should be included in the result | |
| **apiVersion** | **String**| The requested API version | [default to 2] |
| **includeHistory** | **Boolean**| Flag to indicate if response should include historic information, defaults to false | [optional] [default to false] |

### Return type

[**Person**](Person.md)

### Authorization

[HelseID](../README.md#HelseID)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Person returned |  -  |
| **400** | Bad request |  -  |
| **404** | Person not found |  -  |
| **401** | Unauthorized, invalid token |  -  |
| **403** | Forbidden, invalid scope |  -  |

