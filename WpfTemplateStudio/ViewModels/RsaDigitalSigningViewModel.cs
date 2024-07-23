using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Input;
using Windows.ApplicationModel.Background;
using WpfTemplateStudio.Core.Services;

namespace WpfTemplateStudio.ViewModels;

public class RsaDigitalSigningViewModel : ObservableObject
{
    private string _privateKey;
    public string PrivateKey
    {
        get => _privateKey;
        set => SetProperty(ref _privateKey, value);
    }

    private string _publicKey;
    public string PublicKey
    {
        get => _publicKey;
        set => SetProperty(ref _publicKey, value);
    }

    private string _dataToSign;
    public string DataToSign
    {
        get => _dataToSign;
        set => SetProperty(ref _dataToSign, value);
    }

    private string _dataToVerify;
    public string DataToVerify
    {
        get => _dataToVerify;
        set => SetProperty(ref _dataToVerify, value);
    }

    private string _digitalSignature;
    public string DigitalSignature
    {
        get => _digitalSignature;
        set => SetProperty(ref _digitalSignature, value);
    }

    private string _verificationResult;
    public string VerificationResult
    {
        get => _verificationResult;
        set => SetProperty(ref _verificationResult, value);
    }

    private byte[] _signedData;

    public RsaDigitalSigningViewModel()
    {
    }

    private IAsyncRelayCommand generateKeyCommand;
    public IAsyncRelayCommand GenerateKeyCommand => generateKeyCommand ??= new AsyncRelayCommand(GenerateKeyAsync);

    private async Task GenerateKeyAsync()
    {
        await Task.Run(() => { RsaDigitalSignService.RsaKeyGenerator(ref _publicKey, ref _privateKey); });

        // Since the update of the properties is not reflected in the UI, we need to force a refresh of the properties
        OnPropertyChanged(nameof(PublicKey));
        OnPropertyChanged(nameof(PrivateKey));
    }

    private IAsyncRelayCommand addDataToSignCommand;
    public IAsyncRelayCommand AddDataToSignCommand => addDataToSignCommand ??= new AsyncRelayCommand(AddDataToSign);

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

        //return Task.CompletedTask;
    }

    private IAsyncRelayCommand signDataCommand;
    public IAsyncRelayCommand SignDataCommand => signDataCommand ??= new AsyncRelayCommand(SignData);

    private async Task SignData()
    {
        if (_privateKey == null || _dataToSign == null)
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
        await Task.Run(() => { _signedData = RsaDigitalSignService.SignData(fileContent, _privateKey); });

        DigitalSignature = Convert.ToBase64String(_signedData);
    }

    private IAsyncRelayCommand addDataToVerifyCommand;
    public IAsyncRelayCommand AddDataToVerifyCommand => addDataToVerifyCommand ??= new AsyncRelayCommand(AddDataToVerify);

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

    private IAsyncRelayCommand verifyDataCommand;
    public IAsyncRelayCommand VerifyDataCommand => verifyDataCommand ??= new AsyncRelayCommand(VerifyData);

    private async Task VerifyData()
    {
        if (_signedData == null || _publicKey == null || _dataToVerify == null)
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

        //var fileContent = System.IO.File.ReadAllBytes(DataToSign);
        //bool result = RsaDigitalSignService.VerifyData(fileContent, _publicKey, _signedData);

        VerificationResult = "Verifying...";
        bool result = false;
        var fileContent = await File.ReadAllBytesAsync(DataToVerify);
        await Task.Run(() => { result = RsaDigitalSignService.VerifyData(fileContent, _publicKey, _signedData); });

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
