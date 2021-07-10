namespace ChamiUI.BusinessLayer.Mementos
{
    public interface IMemento<T>
    {
        T State { get; }
    }
}