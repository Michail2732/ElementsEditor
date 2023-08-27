using System;

namespace ElementsEditor
{
    public class Observer<T> : IObserver<T>
    {
        private Action<T> _onNext;

        public Observer(Action<T> onCompleate)
        {
            _onNext = onCompleate ?? throw new ArgumentNullException(nameof(onCompleate));
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(T value)
        {
            _onNext(value);
        }
    }
}
