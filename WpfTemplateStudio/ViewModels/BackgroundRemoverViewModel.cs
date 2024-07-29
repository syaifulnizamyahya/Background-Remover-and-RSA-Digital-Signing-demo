using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Collections;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Media.Imaging;
using WpfTemplateStudio.Core.Services;

namespace WpfTemplateStudio.ViewModels;

public partial class BackgroundRemoverViewModel : ObservableObject
{
    public BackgroundRemoverViewModel()
    {
    }

    [ObservableProperty]
    private string statusMessage;

    [ObservableProperty]
    private BitmapImage selectedImage;

    [ObservableProperty]
    private BitmapImage removedBackgroundImage1;

    [ObservableProperty]
    private BitmapImage removedBackgroundImage2;

    [ObservableProperty]
    private BitmapImage removedBackgroundImage3;

    [ObservableProperty]
    private BitmapImage removedBackgroundImage4;

    [ObservableProperty]
    private BitmapImage removedBackgroundImage5;

    [ObservableProperty]
    private string filePath;

    [RelayCommand]
    private async Task SelectImageAsync()
    {
        StatusMessage = "Selecting image...";

        await Task.Run(() =>
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png"
            };

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                FilePath = openFileDialog.FileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(FilePath);
                bitmap.EndInit();
                bitmap.Freeze(); 
                SelectedImage = bitmap;
            }
        });

        StatusMessage = "Image selected";
    }

    [RelayCommand]
    private async Task RemoveBackgroundAsync()
    {
        await Task.Run(() =>
        {
            var result = BackgroundRemoverService.RemoveBackground(FilePath);

            if (result != null)
            {
                using (MemoryStream memoryStream = new MemoryStream(result))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = memoryStream;
                    bitmap.EndInit();
                    bitmap.Freeze();
                    RemovedBackgroundImage1 = bitmap;
                }
            }
        });
    }
}

