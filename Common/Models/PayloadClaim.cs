namespace HelseId.Samples.Common.Models;

// A claim type that can contain an object as the value.
// This is useful for structured claims that are created
// as nested tree structures and not as plain strings.
public class PayloadClaim 
{
    public PayloadClaim(string name, object value)
    {
        Name = name;
        Value = value;
    }
    public string Name { get; }
    public object Value { get; }
}