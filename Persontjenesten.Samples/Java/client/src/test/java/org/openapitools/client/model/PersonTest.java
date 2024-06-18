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

import com.google.gson.TypeAdapter;
import com.google.gson.annotations.JsonAdapter;
import com.google.gson.annotations.SerializedName;
import com.google.gson.stream.JsonReader;
import com.google.gson.stream.JsonWriter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import org.openapitools.client.model.AddressProtection;
import org.openapitools.client.model.Birth;
import org.openapitools.client.model.BirthInNorway;
import org.openapitools.client.model.Citizenship;
import org.openapitools.client.model.ContactAddress;
import org.openapitools.client.model.DeprivedLegalAuthority;
import org.openapitools.client.model.EmigrationFromNorway;
import org.openapitools.client.model.FamilyRelation;
import org.openapitools.client.model.ForeignContactAddress;
import org.openapitools.client.model.GuardianshipOrFuturePowerOfAttorney;
import org.openapitools.client.model.IdentificationDocument;
import org.openapitools.client.model.IdentityVerification;
import org.openapitools.client.model.ImmigrationToNorway;
import org.openapitools.client.model.LegalAuthority;
import org.openapitools.client.model.MaritalStatus;
import org.openapitools.client.model.NorwegianCitizenshipRetention;
import org.openapitools.client.model.NorwegianIdentificationNumber;
import org.openapitools.client.model.ParentalResponsibility;
import org.openapitools.client.model.PersonCommonContactRegisterInformation;
import org.openapitools.client.model.PersonDeath;
import org.openapitools.client.model.PersonFalseIdentity;
import org.openapitools.client.model.PersonGender;
import org.openapitools.client.model.PersonIdentification;
import org.openapitools.client.model.PersonName;
import org.openapitools.client.model.PersonStatus;
import org.openapitools.client.model.PreferredContactAddress;
import org.openapitools.client.model.PresentAddress;
import org.openapitools.client.model.ResidencePermit;
import org.openapitools.client.model.ResidentialAddress;
import org.openapitools.client.model.ResiduaryEstateContactInformation;
import org.openapitools.client.model.SamiLanguage;
import org.openapitools.client.model.SamiParliamentElectoralRegistry;
import org.openapitools.client.model.SharedResidence;
import org.openapitools.client.model.StayOnSvalbard;
import org.openapitools.jackson.nullable.JsonNullable;
import org.junit.jupiter.api.Disabled;
import org.junit.jupiter.api.Test;

/**
 * Model tests for Person
 */
public class PersonTest {
    private final Person model = new Person();

    /**
     * Model tests for Person
     */
    @Test
    public void testPerson() {
        // TODO: test Person
    }

    /**
     * Test the property 'falseIdentity'
     */
    @Test
    public void falseIdentityTest() {
        // TODO: test falseIdentity
    }

    /**
     * Test the property 'norwegianIdentificationNumber'
     */
    @Test
    public void norwegianIdentificationNumberTest() {
        // TODO: test norwegianIdentificationNumber
    }

    /**
     * Test the property 'identityVerification'
     */
    @Test
    public void identityVerificationTest() {
        // TODO: test identityVerification
    }

    /**
     * Test the property 'residuaryEstateContactInformation'
     */
    @Test
    public void residuaryEstateContactInformationTest() {
        // TODO: test residuaryEstateContactInformation
    }

    /**
     * Test the property 'identificationDocument'
     */
    @Test
    public void identificationDocumentTest() {
        // TODO: test identificationDocument
    }

    /**
     * Test the property 'status'
     */
    @Test
    public void statusTest() {
        // TODO: test status
    }

    /**
     * Test the property 'immigrationAuthoritiesIdentificationNumber'
     */
    @Test
    public void immigrationAuthoritiesIdentificationNumberTest() {
        // TODO: test immigrationAuthoritiesIdentificationNumber
    }

    /**
     * Test the property 'foreignPersonIdentificationNumber'
     */
    @Test
    public void foreignPersonIdentificationNumberTest() {
        // TODO: test foreignPersonIdentificationNumber
    }

    /**
     * Test the property 'sharedResidence'
     */
    @Test
    public void sharedResidenceTest() {
        // TODO: test sharedResidence
    }

    /**
     * Test the property 'gender'
     */
    @Test
    public void genderTest() {
        // TODO: test gender
    }

    /**
     * Test the property 'birth'
     */
    @Test
    public void birthTest() {
        // TODO: test birth
    }

    /**
     * Test the property 'birthInNorway'
     */
    @Test
    public void birthInNorwayTest() {
        // TODO: test birthInNorway
    }

    /**
     * Test the property 'familyRelation'
     */
    @Test
    public void familyRelationTest() {
        // TODO: test familyRelation
    }

    /**
     * Test the property 'maritalStatus'
     */
    @Test
    public void maritalStatusTest() {
        // TODO: test maritalStatus
    }

    /**
     * Test the property 'death'
     */
    @Test
    public void deathTest() {
        // TODO: test death
    }

    /**
     * Test the property 'name'
     */
    @Test
    public void nameTest() {
        // TODO: test name
    }

    /**
     * Test the property 'addressProtection'
     */
    @Test
    public void addressProtectionTest() {
        // TODO: test addressProtection
    }

    /**
     * Test the property 'residentialAddress'
     */
    @Test
    public void residentialAddressTest() {
        // TODO: test residentialAddress
    }

    /**
     * Test the property 'presentAddress'
     */
    @Test
    public void presentAddressTest() {
        // TODO: test presentAddress
    }

    /**
     * Test the property 'immigrationToNorway'
     */
    @Test
    public void immigrationToNorwayTest() {
        // TODO: test immigrationToNorway
    }

    /**
     * Test the property 'emigrationFromNorway'
     */
    @Test
    public void emigrationFromNorwayTest() {
        // TODO: test emigrationFromNorway
    }

    /**
     * Test the property 'useOfSamiLanguage'
     */
    @Test
    public void useOfSamiLanguageTest() {
        // TODO: test useOfSamiLanguage
    }

    /**
     * Test the property 'samiParliamentElectoralRegistryStatus'
     */
    @Test
    public void samiParliamentElectoralRegistryStatusTest() {
        // TODO: test samiParliamentElectoralRegistryStatus
    }

    /**
     * Test the property 'preferredContactAddress'
     */
    @Test
    public void preferredContactAddressTest() {
        // TODO: test preferredContactAddress
    }

    /**
     * Test the property 'postalAddress'
     */
    @Test
    public void postalAddressTest() {
        // TODO: test postalAddress
    }

    /**
     * Test the property 'foreignPostalAddress'
     */
    @Test
    public void foreignPostalAddressTest() {
        // TODO: test foreignPostalAddress
    }

    /**
     * Test the property 'parentalResponsibility'
     */
    @Test
    public void parentalResponsibilityTest() {
        // TODO: test parentalResponsibility
    }

    /**
     * Test the property 'citizenship'
     */
    @Test
    public void citizenshipTest() {
        // TODO: test citizenship
    }

    /**
     * Test the property 'citizenshipRetention'
     */
    @Test
    public void citizenshipRetentionTest() {
        // TODO: test citizenshipRetention
    }

    /**
     * Test the property 'residencePermit'
     */
    @Test
    public void residencePermitTest() {
        // TODO: test residencePermit
    }

    /**
     * Test the property 'stayOnSvalbard'
     */
    @Test
    public void stayOnSvalbardTest() {
        // TODO: test stayOnSvalbard
    }

    /**
     * Test the property 'guardianshipOrFuturePowerOfAttorney'
     */
    @Test
    public void guardianshipOrFuturePowerOfAttorneyTest() {
        // TODO: test guardianshipOrFuturePowerOfAttorney
    }

    /**
     * Test the property 'deprivedLegalAuthority'
     */
    @Test
    public void deprivedLegalAuthorityTest() {
        // TODO: test deprivedLegalAuthority
    }

    /**
     * Test the property 'legalAuthority'
     */
    @Test
    public void legalAuthorityTest() {
        // TODO: test legalAuthority
    }

    /**
     * Test the property 'commonContactRegisterInformation'
     */
    @Test
    public void commonContactRegisterInformationTest() {
        // TODO: test commonContactRegisterInformation
    }

    /**
     * Test the property 'id'
     */
    @Test
    public void idTest() {
        // TODO: test id
    }

    /**
     * Test the property 'sequenceNumber'
     */
    @Test
    public void sequenceNumberTest() {
        // TODO: test sequenceNumber
    }

}
