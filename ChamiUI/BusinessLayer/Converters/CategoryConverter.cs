using Chami.Db.Entities;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Converters;

public class CategoryConverter : CachedConverter<Category, CategoryViewModel>
{
    public CategoryConverter()
    {
        _brushConverter = new BrushConverter();
    }
    private readonly BrushConverter _brushConverter;
    public override CategoryViewModel ConvertFromEntity(Category entity)
    {
        var viewModel = new CategoryViewModel()
        {
            Id = entity.Id,
            DisplayName = entity.Name,
            IconPath = entity.Icon,
            //BackgroundColor = _brushConverter.Convert(entity.BackgroundColor),
            IsVisible = entity.Visibility
        };
        return viewModel;
    }

    public override Category ConvertFromModel(CategoryViewModel model)
    {
        var entity = new Category()
        {
            Id = model.Id.GetValueOrDefault(),
            Name = model.DisplayName,
            BackgroundColor = model.BackgroundColor?.ToString(),
            Icon = model.IconPath,
            Visibility = model.IsVisible
        };
        return entity;
    }
}