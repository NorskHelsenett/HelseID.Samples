namespace HelseID.Samples.Configuration;

public static class OrganizationStore
{
    private static readonly List<Organization> Organizations = new()
    {
        new Organization
        {
            Id = 1,
            OrgNoParent = ConfigurationValues.GranfjelldalKommuneOrganizationNumber,
            ParentName = ConfigurationValues.GranfjelldalKommuneOrganizationName
        },
        new Organization
        {
            Id = 2,
            OrgNoParent = ConfigurationValues.GranfjelldalKommuneOrganizationNumber,
            ParentName = ConfigurationValues.GranfjelldalKommuneOrganizationName,
            OrgNoChild = ConfigurationValues.GranfjelldalKommuneChildOrganizationNumber1,
            ChildName = ConfigurationValues.GranfjelldalKommuneChildOrganizationName1        },
        new Organization
        {
            Id = 3,
            OrgNoParent = ConfigurationValues.GranfjelldalKommuneOrganizationNumber,
            ParentName = ConfigurationValues.GranfjelldalKommuneOrganizationName,
            OrgNoChild = ConfigurationValues.GranfjelldalKommuneChildOrganizationNumber2,
            ChildName = ConfigurationValues.GranfjelldalKommuneChildOrganizationName2
        },
        new Organization
        {
            Id = 4,
            OrgNoParent = ConfigurationValues.SupplierOrganizationNumber,
            ParentName = ConfigurationValues.SupplierOrganizationName
        },
        new Organization
        {
            Id = 5,
            OrgNoParent = ConfigurationValues.HansensLegekontorOrganizationNumber,
            ParentName = ConfigurationValues.HansensLegekontorOrganizationName
        },
        new Organization
        {
            Id = 6,
            OrgNoParent = ConfigurationValues.FlaksvaagoeyKommuneOrganizationNumber,
            ParentName = ConfigurationValues.FlaksvaagoeyKommuneOrganizationName
        },
        new Organization
        {
            Id = 7,
            OrgNoParent = ConfigurationValues.FlaksvaagoeyKommuneOrganizationNumber,
            ParentName = ConfigurationValues.FlaksvaagoeyKommuneOrganizationName,
            OrgNoChild = ConfigurationValues.FlaksvaagoeyKommuneChildOrganizationNumber,
            ChildName = ConfigurationValues.FlaksvaagoeyKommuneChildOrganizationName
        },
        new Organization
        {
            Id = 8,
            OrgNoParent = ConfigurationValues.FlaksvaagoeyKommuneOrganizationNumber,
            ParentName = ConfigurationValues.FlaksvaagoeyKommuneOrganizationName,
            OrgNoChild = ConfigurationValues.FlaksvaagoeyKommuneChildOrganizationNumber,
            ChildName = ConfigurationValues.FlaksvaagoeyKommuneChildOrganizationName
        },
    };

    public static Organization? GetOrganization(int id)
    {
        return Organizations.SingleOrDefault(o =>
            o.Id == id);
    }

    public static Organization? GetOrganization(string? parentOrganizationNumber)
    {
        if (parentOrganizationNumber == null)
        {
            return null;
        }

        return Organizations.FirstOrDefault(o =>
            o.OrgNoParent == parentOrganizationNumber);
    }

    public static Organization? GetOrganizationWithChild(string? childOrganizationNumber)
    {
        if (childOrganizationNumber == null)
        {
            return null;
        }

        return Organizations.FirstOrDefault(
            o => o.OrgNoChild == childOrganizationNumber);
    }
}
