using System;

namespace HelseId.Core.BFF.Sample.WebCommon.Extensions;

public static class ExpectNotNullExtension
{
    public static T ExpectNotNull<T>(this T? value)
    {
        return ExpectNotNull(value, "Expected value to not be null");
    }

    public static T ExpectNotNull<T>(this T? value, string reason)
    {
        return value ?? throw new NotNullExpectedException(reason);
    }

    public static T ExpectNotNull<T>(this T? value)
        where T : struct
    {
        return ExpectNotNull(value, "Expected value to not be null");
    }

    public static T ExpectNotNull<T>(this T? value, string reason)
        where T : struct
    {
        return value ?? throw new NotNullExpectedException(reason);
    }
}

public class NotNullExpectedException : Exception
{
    public NotNullExpectedException(string reason) : base(reason) { }
}
