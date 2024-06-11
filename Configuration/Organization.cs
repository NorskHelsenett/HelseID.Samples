namespace HelseID.Samples.Configuration;

public class Organization
{
    public bool HasChildOrganization => !string.IsNullOrEmpty(OrgNoChild);

    public string Name => HasChildOrganization ? ChildName : ParentName;

    public bool IsEmpty => Id == 0;

    public int Id { get; init; } = 0;

    public string OrgNoParent { get; init; } = string.Empty;

    public string ParentName { get; init; } = string.Empty;
   
    public string OrgNoChild { get; init; } = string.Empty;

    public string ChildName { get; init; } = string.Empty;
}