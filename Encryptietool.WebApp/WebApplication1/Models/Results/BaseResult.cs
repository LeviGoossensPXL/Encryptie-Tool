namespace WebApplication1.Models.Results;


public abstract class BaseResult
{
    private readonly List<string> _errors = new();
    public bool Succeeded { get; set; } = true;
    public IEnumerable<string> Errors => _errors;
    
    public void Failed(string errorMessage)
    {
        Succeeded = false;
        _errors.Add(errorMessage);
    }
}