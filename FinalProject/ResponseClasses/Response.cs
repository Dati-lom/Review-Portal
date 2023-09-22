namespace FinalProject.ResponseClasses;

public class Response
{
    public string _message;
    public bool _status;

    public Response(string message,bool statucode)
    {
        _message = message;
        _status = statucode;
    }
    
}