namespace HelseId.Samples.ApiAccess.Models;

public enum OrganizationIds
{
    HansensLegekontor = 3,
    FlaksvaagoeyKommune = 4,
    FlaksvaagoeyKommuneBoOgOmsorgssenter = 5,    
}


/*
 *  Flyt: 1) initiell pålogging, uten virksomhet (inkluder resuurs-indikatorer for de ressursene du vil velge) [ingen over- eller underenhet]
 *  2) Rediriger til side der brukeren må velge virksomhet (Hansens legekontor) [kun hovedenhet] (Trondheim kommune/Moholdt sykehjem) [med underenhet]
 *  3) Sett opp mulighet for å kalle 2 ressurser, hver med sin ressursindikator
 *  4) Bruk refreshtoken til å hente accesstoken, kall API
 * 
 */