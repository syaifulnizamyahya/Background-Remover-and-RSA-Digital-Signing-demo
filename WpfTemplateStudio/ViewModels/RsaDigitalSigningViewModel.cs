using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.IO;
using System.Windows;
using WpfTemplateStudio.Core.Services;

namespace WpfTemplateStudio.ViewModels;

public partial class RsaDigitalSigningViewModel : ObservableObject
{
    [ObservableProperty]
    private string privateKey;

    [ObservableProperty]
    private string publicKey;

    [ObservableProperty]
    private string dataToSign;

    [ObservableProperty]
    private string dataToVerify;

    [ObservableProperty]
    private string digitalSignature;

    [ObservableProperty]
    private string verificationResult;

    private byte[] signedData;

    public RsaDigitalSigningViewModel()
    {
    }

    [RelayCommand]
    private async Task GenerateKeyAsync()
    {
        await Task.Run(() => { RsaDigitalSignService.RsaKeyGenerator(ref publicKey, ref privateKey); });

        // Since the update of the properties is not reflected in the UI, we need to force a refresh of the properties
        OnPropertyChanged(nameof(PublicKey));
        OnPropertyChanged(nameof(PrivateKey));
    }

    [RelayCommand]
    private async Task AddDataToSign()
    {
        var dialog = new Microsoft.Win32.OpenFileDialog();
        bool? result = dialog.ShowDialog();

        if (result == true)
        {
            DataToSign = dialog.FileName;
            if (DataToVerify == null)
            {
                DataToVerify = dialog.FileName;
            }
        }
        await SignData();
    }

    [RelayCommand]
    private async Task SignData()
    {
        if (PrivateKey == null || DataToSign == null)
        {
            MessageBox.Show("Private key or file to sign cannot be null.", "Getting Digital Signature Error", MessageBoxButton.OK, MessageBoxImage.Error);
            DigitalSignature = null;
            return;
        }
        if (!System.IO.File.Exists(DataToSign))
        {
            MessageBox.Show("File to sign does not exist.", "Getting Digital Signature Error", MessageBoxButton.OK, MessageBoxImage.Error);
            DigitalSignature = null;
            return;
        }

        DigitalSignature = "Getting digital signature...";
        var fileContent = await File.ReadAllBytesAsync(DataToSign);
        await Task.Run(() => { signedData = RsaDigitalSignService.SignData(fileContent, PrivateKey); });

        DigitalSignature = Convert.ToBase64String(signedData);
    }

    [RelayCommand]
    private Task AddDataToVerify()
    {
        var dialog = new Microsoft.Win32.OpenFileDialog();
        bool? result = dialog.ShowDialog();

        if (result == true)
        {
            DataToVerify = dialog.FileName;
        }
        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task VerifyData()
    {
        if (signedData == null || PublicKey == null || DataToVerify == null)
        {
            MessageBox.Show("Public key or file to verify or digital signature cannot be null.", "Verification Error", MessageBoxButton.OK, MessageBoxImage.Error);
            VerificationResult = null;
            return;
        }

        if (!System.IO.File.Exists(DataToVerify))
        {
            MessageBox.Show("File to verify does not exist.", "Verification Error", MessageBoxButton.OK, MessageBoxImage.Error);
            VerificationResult = null;
            return;
        }

        VerificationResult = "Verifying...";
        bool result = false;
        var fileContent = await File.ReadAllBytesAsync(DataToVerify);
        await Task.Run(() => { result = RsaDigitalSignService.VerifyData(fileContent, PublicKey, signedData); });

        if (result)
        {
            VerificationResult = "Signature verified";
        }
        else
        {
            VerificationResult = "Signature not verified";
        }
    }
}
