namespace Api.Errors;

public class APIExceptions(int statuscode, string message, string? details)
{
    public int Statuscode { get; set; } = statuscode;
    public String Message { get; set; } = message;
    public String? Details { get; set; } = details;
}
