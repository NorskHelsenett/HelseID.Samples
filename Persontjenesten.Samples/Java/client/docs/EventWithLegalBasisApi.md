# EventWithLegalBasisApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**apiLegalBasisEventLatestGet**](EventWithLegalBasisApi.md#apiLegalBasisEventLatestGet) | **GET** /api/legal-basis/event/latest | Get the latest event. Returns a search result with the latest available event document |
| [**eventWithLegalBasis**](EventWithLegalBasisApi.md#eventWithLegalBasis) | **GET** /api/legal-basis/event | Get list of specific events by filter options. Returns a search result with max 1000 event documents  starting with the specified sequenceNumber |


<a id="apiLegalBasisEventLatestGet"></a>
# **apiLegalBasisEventLatestGet**
> EventDocument apiLegalBasisEventLatestGet(apiVersion)

Get the latest event. Returns a search result with the latest available event document

&lt;b&gt;Requires HelseId scope:&lt;/b&gt; ReadWithLegalBasis

### Example
```java
// Import classes:
import org.openapitools.client.ApiClient;
import org.openapitools.client.ApiException;
import org.openapitools.client.Configuration;
import org.openapitools.client.auth.*;
import org.openapitools.client.models.*;
import org.openapitools.client.api.EventWithLegalBasisApi;

public class Example {
  public static void main(String[] args) {
    ApiClient defaultClient = Configuration.getDefaultApiClient();
    defaultClient.setBasePath("http://localhost");
    
    // Configure HTTP bearer authorization: HelseID
    HttpBearerAuth HelseID = (HttpBearerAuth) defaultClient.getAuthentication("HelseID");
    HelseID.setBearerToken("BEARER TOKEN");

    EventWithLegalBasisApi apiInstance = new EventWithLegalBasisApi(defaultClient);
    String apiVersion = "2"; // String | The requested API version
    try {
      EventDocument result = apiInstance.apiLegalBasisEventLatestGet(apiVersion);
      System.out.println(result);
    } catch (ApiException e) {
      System.err.println("Exception when calling EventWithLegalBasisApi#apiLegalBasisEventLatestGet");
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

### Return type

[**EventDocument**](EventDocument.md)

### Authorization

[HelseID](../README.md#HelseID)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Event document returned |  -  |
| **400** | Bad request |  -  |
| **404** | Event document not found |  -  |
| **401** | Unauthorized, invalid token |  -  |
| **403** | Forbidden, invalid scope |  -  |

<a id="eventWithLegalBasis"></a>
# **eventWithLegalBasis**
> EventDocumentSearchResult eventWithLegalBasis(apiVersion, sequenceNumber, eventTypes)

Get list of specific events by filter options. Returns a search result with max 1000 event documents  starting with the specified sequenceNumber

Get a list of up to 1000 events from a given sequenceNumber as a starting point. The result list may be  further filtered by specifying which event types that should be included in the result.  The result list will contain the event for the given sequenceNumber if it exists.&lt;br /&gt;&lt;br /&gt;&lt;b&gt;Requires HelseId scope:&lt;/b&gt; ReadWithLegalBasis

### Example
```java
// Import classes:
import org.openapitools.client.ApiClient;
import org.openapitools.client.ApiException;
import org.openapitools.client.Configuration;
import org.openapitools.client.auth.*;
import org.openapitools.client.models.*;
import org.openapitools.client.api.EventWithLegalBasisApi;

public class Example {
  public static void main(String[] args) {
    ApiClient defaultClient = Configuration.getDefaultApiClient();
    defaultClient.setBasePath("http://localhost");
    
    // Configure HTTP bearer authorization: HelseID
    HttpBearerAuth HelseID = (HttpBearerAuth) defaultClient.getAuthentication("HelseID");
    HelseID.setBearerToken("BEARER TOKEN");

    EventWithLegalBasisApi apiInstance = new EventWithLegalBasisApi(defaultClient);
    String apiVersion = "2"; // String | The requested API version
    Long sequenceNumber = 56L; // Long | The lowest sequence number that should be included in the result
    List<EventType> eventTypes = Arrays.asList(); // List<EventType> | Which event types that should be included in the result.              All event types will be returned if no event types are specified
    try {
      EventDocumentSearchResult result = apiInstance.eventWithLegalBasis(apiVersion, sequenceNumber, eventTypes);
      System.out.println(result);
    } catch (ApiException e) {
      System.err.println("Exception when calling EventWithLegalBasisApi#eventWithLegalBasis");
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
| **sequenceNumber** | **Long**| The lowest sequence number that should be included in the result | [optional] |
| **eventTypes** | [**List&lt;EventType&gt;**](EventType.md)| Which event types that should be included in the result.              All event types will be returned if no event types are specified | [optional] |

### Return type

[**EventDocumentSearchResult**](EventDocumentSearchResult.md)

### Authorization

[HelseID](../README.md#HelseID)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Event document returned |  -  |
| **400** | Bad request |  -  |
| **404** | Event not found |  -  |
| **401** | Unauthorized, invalid token |  -  |
| **403** | Forbidden, invalid scope |  -  |

