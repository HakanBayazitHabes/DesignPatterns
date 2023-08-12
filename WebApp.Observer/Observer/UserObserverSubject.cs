using BaseProject.Models;

namespace WebApp.Observer.Observer
{
    public class UserObserverSubject
    {
        private List<IUserObserver> _observers;

        public UserObserverSubject(List<IUserObserver> observers)
        {
            _observers = observers;
        }

        public void Attach(IUserObserver observer)
        {
            _observers.Add(observer);
        }
        public void Detach(IUserObserver observer)
        {
            _observers.Remove(observer);
        }
        public void Notify(AppUser appUser)
        {
            foreach (var observer in _observers)
            {
                observer.UserCreatedAsync(appUser);
            }
        }
    }
}
