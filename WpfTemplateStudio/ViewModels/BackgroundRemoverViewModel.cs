using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfTemplateStudio.Core.Services;

namespace WpfTemplateStudio.ViewModels;

public partial class BackgroundRemoverViewModel : ObservableObject
{
    public BackgroundRemoverViewModel()
    {
        Images = new ObservableCollection<BitmapImage>();
    }

    [ObservableProperty]
    private string statusMessage;

    [ObservableProperty]
    private BitmapImage selectedImage;

    [ObservableProperty]
    private ObservableCollection<BitmapImage> images;

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
        for (int i = 0; i < 10; i++)
        {
            await Task.Run(() =>
            {
                byte[] imageBytes = File.ReadAllBytes(FilePath);

                var bitmapImage = new BitmapImage();
                using (var memoryStream = new MemoryStream(imageBytes))
                {
                    memoryStream.Position = 0;
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memoryStream;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();

                }

                App.Current.Dispatcher.Invoke(() =>
                {
                    Images.Add(bitmapImage);
                });
            });
        }
    }
}

