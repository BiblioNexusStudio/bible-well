using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;

namespace BibleWell.App.ViewModels;

public partial class OpenFileViewModel : ViewModelBase
{

    [ObservableProperty]
    private string? _fileText;

    [ObservableProperty]
    private Bitmap? _image;

    [ObservableProperty]
    private bool _isImageVisible;

    [RelayCommand]
    private async Task OpenFileWithMauiFilePicker(CancellationToken ct)
    {
        ErrorMessages?.Clear();
        try
        {
            var file = await FilePicker.PickAsync();

            if (file is null)
            {
                return;
            }

            await using var readStream = await file.OpenReadAsync();

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (fileExtension is ".png" or ".jpg" or ".jpeg")
            {
                Image = Bitmap.DecodeToWidth(readStream, 800);
                IsImageVisible = true;
            }
            else
            {
                using var reader = new StreamReader(readStream);
                FileText = await reader.ReadToEndAsync(ct);
                IsImageVisible = false;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}

public class InverseBooleanConverter : IValueConverter
{
    public static readonly InverseBooleanConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b && targetType.IsAssignableTo(typeof(bool)))
        {
            return !b;
        }

        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error, "The value must be a boolean");
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}