using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Input;
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

    private IAsyncRelayCommand generateKeyCommand;
    public IAsyncRelayCommand GenerateKeyCommand => generateKeyCommand ??= new AsyncRelayCommand(GenerateKeyAsync);

    private async Task GenerateKeyAsync()
    {
        await Task.Run(() => { RsaDigitalSignService.RsaKeyGenerator(ref _publicKey, ref _privateKey); });

        // Since the update of the properties is not reflected in the UI, we need to force a refresh of the properties
        OnPropertyChanged(nameof(PublicKey));
        OnPropertyChanged(nameof(PrivateKey));
    }

    public RsaDigitalSigningViewModel()
    {
    }


    private IAsyncRelayCommand addDataToSignCommand;
    public IAsyncRelayCommand AddDataToSignCommand => addDataToSignCommand ??= new AsyncRelayCommand(AddDataToSign);

    private Task AddDataToSign()
    {
        MessageBox.Show("AddDataToSign");
        return Task.CompletedTask;
    }

    private IAsyncRelayCommand signDataCommand;
    public IAsyncRelayCommand SignDataCommand => signDataCommand ??= new AsyncRelayCommand(SignData);

    private Task SignData()
    {
        var signData = RsaDigitalSignService.SignData(_dataToSign, _privateKey);
        _digitalSignature = Convert.ToBase64String(signData);
        MessageBox.Show("_digitalSignature");
        return Task.CompletedTask;
    }

    private IAsyncRelayCommand addDataToVerifyCommand;
    public IAsyncRelayCommand AddDataToVerifyCommand => addDataToVerifyCommand ??= new AsyncRelayCommand(AddDataToVerify);

    private Task AddDataToVerify()
    {
        MessageBox.Show("AddDataToVerify");
        return Task.CompletedTask;
    }

    private IAsyncRelayCommand verifyDataCommand;
    public IAsyncRelayCommand VerifyDataCommand => verifyDataCommand ??= new AsyncRelayCommand(VerifyData);

    private Task VerifyData()
    {
        MessageBox.Show("VerifyData");
        return Task.CompletedTask;
    }
}
