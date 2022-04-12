namespace ChamiUI.BusinessLayer.Mementos
{
    public interface IMemento<out T>
    {
        T State { get; }
    }
}