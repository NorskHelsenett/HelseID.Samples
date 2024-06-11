using System;
using System.Diagnostics.CodeAnalysis;

namespace HelseId.Core.BFF.Sample.Api.Validation;

public readonly record struct ValidationResult<T>(bool IsSuccess, T? ErrorValue)
{
    public bool IsError => !IsSuccess;

    public bool TryError([NotNullWhen(true)] out T? error)
    {
        if (IsError)
        {
            error = ErrorValue == null ? throw new NotNullExpectedException("Expected ErrorValue to not be null") : ErrorValue;
            return true;
        }
        else
        {
            error = default;
            return false;
        }
    }

    public static ValidationResult<T> Success()
    {
        return new ValidationResult<T>(true, default);
    }

    public static implicit operator ValidationResult<T>(T error) => new(false, error);
}

public readonly struct ValidationResult
{
    public readonly bool IsSuccess;
    public readonly string ErrorMessage;

    public static ValidationResult Error(string error)
    {
        return new ValidationResult(false, error);
    }

    public static ValidationResult Success()
    {
        return new ValidationResult(true, string.Empty);
    }

    private ValidationResult(bool isSuccess, string errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }
}

public class NotNullExpectedException : Exception
{
    public NotNullExpectedException(string reason) : base(reason)
    {
    }
}