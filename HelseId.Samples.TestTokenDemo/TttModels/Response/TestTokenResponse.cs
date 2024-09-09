namespace HelseId.Samples.TestTokenDemo.TttModels.Response;

public class TestTokenResponse
{
    public bool IsError { get; set; }
    public SuccessResponse SuccessResponse { get; set; } = new();
    public ErrorResponse ErrorResponse { get; set; } = new();
}
