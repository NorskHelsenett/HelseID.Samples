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
import org.openapitools.client.model.InformationPart;
import org.openapitools.client.model.Person;
import org.openapitools.client.model.ProblemDetails;
import org.junit.jupiter.api.Disabled;
import org.junit.jupiter.api.Test;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * API tests for PersonNoLegalBasisApi
 */
@Disabled
public class PersonNoLegalBasisApiTest {

    private final PersonNoLegalBasisApi api = new PersonNoLegalBasisApi();

    /**
     * Get multiple persons from a list of unique person ids, max 100 items.
     *
     * Does not support nin&lt;br /&gt;&lt;br /&gt;&lt;b&gt;Requires HelseId scope:&lt;/b&gt; ReadNoLegalBasis
     *
     * @throws ApiException if the Api call fails
     */
    @Test
    public void apiNoLegalBasisPersonBulkByIdPostTest() throws ApiException {
        List<InformationPart> informationParts = null;
        String apiVersion = null;
        List<String> personIds = null;
        Boolean includeHistory = null;
        List<Person> response = api.apiNoLegalBasisPersonBulkByIdPost(informationParts, apiVersion, personIds, includeHistory);
        // TODO: test validations
    }

    /**
     * Get multiple persons from a list of Norwegian Identification Numbers, max 100 items.
     *
     * Norwegian Identification Number includes national identity numbers (fødselsnummer) and D-numbers (D-nummer).&lt;br /&gt;&lt;br /&gt;&lt;b&gt;Requires HelseId scope:&lt;/b&gt; ReadNoLegalBasis
     *
     * @throws ApiException if the Api call fails
     */
    @Test
    public void apiNoLegalBasisPersonBulkByNinPostTest() throws ApiException {
        List<InformationPart> informationParts = null;
        String apiVersion = null;
        List<String> nins = null;
        Boolean includeHistory = null;
        List<Person> response = api.apiNoLegalBasisPersonBulkByNinPost(informationParts, apiVersion, nins, includeHistory);
        // TODO: test validations
    }

    /**
     * Get a specific Person by Norwegian Identification Number
     *
     * Get a person from a Norwegian Identification Number. This includes&lt;br /&gt;  - National identity numbers (fødselsnummer)&lt;br /&gt;  - D-numbers (D-nummer)&lt;br /&gt;  - Alternate identification numbers (felles hjelpenummer)&lt;br /&gt;  - ImmigrationAuthoritiesIdentificationNumber (DUF)&lt;br /&gt;                Historic NINs are supported. This is a POST request to avoid including NIN as part of the URL.&lt;br /&gt;&lt;br /&gt;&lt;b&gt;Requires HelseId scope:&lt;/b&gt; ReadNoLegalBasis
     *
     * @throws ApiException if the Api call fails
     */
    @Test
    public void apiNoLegalBasisPersonGetByNinPostTest() throws ApiException {
        List<InformationPart> informationParts = null;
        String apiVersion = null;
        String nin = null;
        Boolean includeHistory = null;
        Person response = api.apiNoLegalBasisPersonGetByNinPost(informationParts, apiVersion, nin, includeHistory);
        // TODO: test validations
    }

    /**
     * Get a specific person by unique id
     *
     * Does not support nin&lt;br /&gt;&lt;br /&gt;&lt;b&gt;Requires HelseId scope:&lt;/b&gt; ReadNoLegalBasis
     *
     * @throws ApiException if the Api call fails
     */
    @Test
    public void apiNoLegalBasisPersonIdGetTest() throws ApiException {
        String id = null;
        List<InformationPart> informationParts = null;
        String apiVersion = null;
        Boolean includeHistory = null;
        Person response = api.apiNoLegalBasisPersonIdGet(id, informationParts, apiVersion, includeHistory);
        // TODO: test validations
    }

}