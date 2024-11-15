using BibleWell.App.ViewModels.Pages;

namespace BibleWell.App.ViewModels;

public sealed class MenuItemTemplate(Type _type)
{
    // TODO localization
    public string Label { get; } = _type.Name.Replace("PageViewModel", "");

    public Type ViewModelType { get; } = _type.BaseType == typeof(PageViewModelBase)
        ? _type
        : throw new ArgumentException($"Type must derive from {nameof(PageViewModelBase)}.", nameof(_type));
}