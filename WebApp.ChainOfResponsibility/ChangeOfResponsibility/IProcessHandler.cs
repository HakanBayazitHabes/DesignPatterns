namespace WebApp.ChainOfResponsibility.ChangeOfResponsibility
{
    public interface IProcessHandler
    {
        IProcessHandler SetNext(IProcessHandler handler);
        object handle(object request);
    }
}
