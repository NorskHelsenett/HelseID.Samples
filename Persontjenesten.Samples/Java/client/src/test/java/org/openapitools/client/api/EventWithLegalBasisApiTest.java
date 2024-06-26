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


package org.openapitools.client.api;

import org.openapitools.client.ApiException;
import org.openapitools.client.model.EventDocument;
import org.openapitools.client.model.EventDocumentSearchResult;
import org.openapitools.client.model.EventType;
import org.openapitools.client.model.ProblemDetails;
import org.junit.jupiter.api.Disabled;
import org.junit.jupiter.api.Test;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * API tests for EventWithLegalBasisApi
 */
@Disabled
public class EventWithLegalBasisApiTest {

    private final EventWithLegalBasisApi api = new EventWithLegalBasisApi();

    /**
     * Get the latest event. Returns a search result with the latest available event document
     *
     * &lt;b&gt;Requires HelseId scope:&lt;/b&gt; ReadWithLegalBasis
     *
     * @throws ApiException if the Api call fails
     */
    @Test
    public void apiLegalBasisEventLatestGetTest() throws ApiException {
        String apiVersion = null;
        EventDocument response = api.apiLegalBasisEventLatestGet(apiVersion);
        // TODO: test validations
    }

    /**
     * Get list of specific events by filter options. Returns a search result with max 1000 event documents  starting with the specified sequenceNumber
     *
     * Get a list of up to 1000 events from a given sequenceNumber as a starting point. The result list may be  further filtered by specifying which event types that should be included in the result.  The result list will contain the event for the given sequenceNumber if it exists.&lt;br /&gt;&lt;br /&gt;&lt;b&gt;Requires HelseId scope:&lt;/b&gt; ReadWithLegalBasis
     *
     * @throws ApiException if the Api call fails
     */
    @Test
    public void eventWithLegalBasisTest() throws ApiException {
        String apiVersion = null;
        Long sequenceNumber = null;
        List<EventType> eventTypes = null;
        EventDocumentSearchResult response = api.eventWithLegalBasis(apiVersion, sequenceNumber, eventTypes);
        // TODO: test validations
    }

}
