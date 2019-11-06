namespace Railek.Unibase
{
    public interface IEventListener<in T>
    {
        void OnEventRaised(T value);
    }

    public interface IEventListener
    {
        void OnEventRaised();
    }
}
