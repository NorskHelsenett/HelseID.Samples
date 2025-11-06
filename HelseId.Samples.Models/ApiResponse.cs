namespace HelseId.Samples.Common.Models;

// A class that contains the response data that is sent
// from the Sample API with a 200 response
public class ApiResponse
{
    public string? Greeting { get; set; }
    
    public string? ParentOrganizationNumber { get; set; }

    public string? ChildOrganizationNumber { get; set; }
    
    public string? SupplierOrganizationNumber { get; set; }
}
