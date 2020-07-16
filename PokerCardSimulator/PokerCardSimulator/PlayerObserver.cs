using System;

namespace PokerCardSimulator
{
    public class PlayerObserver : IObserver<PlayerObserverEvent>
    {
        IDisposable _unsubscriber;

        public void Subscribe(IObservable<PlayerObserverEvent> observable)
        {
            if (observable != null)
                _unsubscriber = observable.Subscribe(this);
        }

        public void Unsubscribe()
        {
            _unsubscriber.Dispose();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(PlayerObserverEvent value)
        {
            throw new NotImplementedException();
        }
    }
}
