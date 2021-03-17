namespace ChamiUI.BusinessLayer.Converters
{
    public interface IConverter<TEntity, TViewModel>
    {
        TEntity FromModel(TViewModel model);
        TViewModel FromEntity(TEntity entity);
    }
}