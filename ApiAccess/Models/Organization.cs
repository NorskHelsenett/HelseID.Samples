namespace HelseId.Samples.ApiAccess.Models;

public class Organization
{
    public string OrgNoParent { get; set; } = string.Empty;
    public string OrgNoChild { get; set; } = string.Empty;
}
/*
 *  Flyt: 1) initiell pålogging, uten virksomhet (inkluder resuurs-indikatorer for de ressursene du vil velge) [ingen over- eller underenhet]
 *  2) Rediriger til side der brukeren må velge virksomhet (Hansens legekontor) [kun hovedenhet] (Trondheim kommune/Moholdt sykehjem) [med underenhet]
 *  3) Sett opp mulighet for å kalle 2 ressurser, hver med sin ressursindikator
 *  4) Bruk refreshtoken til å hente accesstoken, kall API
 * 
 */