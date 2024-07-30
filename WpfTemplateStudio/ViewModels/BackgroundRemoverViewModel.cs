using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        StatusMessage = WpfTemplateStudio.Properties.Resources.BackgroundRemoverViewModel_SelectImageAsync_SelectingImage;

        await Task.Run(() =>
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = WpfTemplateStudio.Properties.Resources.BackgroundRemoverViewModel_SelectImageAsync_ImageFilter
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

        StatusMessage = WpfTemplateStudio.Properties.Resources.BackgroundRemoverViewModel_SelectImageAsync_ImageSelected;
    }

    [RelayCommand]
    private async Task RemoveBackgroundAsync()
    {
        if (File.Exists(FilePath) == false)
        {
            StatusMessage = WpfTemplateStudio.Properties.Resources.BackgroundRemoverViewModel_RemoveBackgroundAsync_ImageNotFound;
            return;
        }

        StatusMessage = WpfTemplateStudio.Properties.Resources.BackgroundRemoverViewModel_RemoveBackgroundAsync_RemovingBackground;
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
                    if (Images.Count >= availableModels)
                    {
                        Images[i] = bitmapImage;
                    }
                    else
                    {
                        Images.Add(bitmapImage);
                    }
                    StatusMessage = $"Background removed using {((BackgroundRemoverService.DeepLearningModel)i).ToString()} model";
                    Task.Delay(100);
                });
            });
        });
        StatusMessage = WpfTemplateStudio.Properties.Resources.BackgroundRemoverViewModel_RemoveBackgroundAsync_BackgroundRemoved;
    }
}

