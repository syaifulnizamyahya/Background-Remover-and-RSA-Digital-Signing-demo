using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Imaging;
using WpfTemplateStudio.Core.Services;

namespace WpfTemplateStudio.ViewModels;

public partial class BackgroundRemoverViewModel : ObservableObject
{
    public BackgroundRemoverViewModel()
    {
        Images = [];
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
        if (File.Exists(FilePath) == false)
        {
            StatusMessage = "Image not found";
            return;
        }

        StatusMessage = "Removing background...";
        await Task.Run(() =>
        {
            int availableModels = Enum.GetValues(typeof(BackgroundRemoverService.DeepLearningModel)).Length;
            Parallel.For(0, availableModels, i =>
            {
                byte[] imageBytes = BackgroundRemoverService.RemoveBackground(FilePath, (BackgroundRemoverService.DeepLearningModel)i);

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
                    StatusMessage = $"Background removed using {((BackgroundRemoverService.DeepLearningModel)i).ToString()} model";
                });
            });
        });
        StatusMessage = "Background removed";
    }
}

