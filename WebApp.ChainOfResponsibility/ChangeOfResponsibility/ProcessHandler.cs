namespace WebApp.ChainOfResponsibility.ChangeOfResponsibility
{
    public abstract class ProcessHandler : IProcessHandler
    {
        private IProcessHandler _nextHandler;
        public virtual object handle(object request)
        {
            if (_nextHandler != null)
            {
                return _nextHandler.handle(request);
            }
            return null;
        }

        public IProcessHandler SetNext(IProcessHandler handler)
        {
            _nextHandler = handler;
            return _nextHandler;
        }
    }
}
