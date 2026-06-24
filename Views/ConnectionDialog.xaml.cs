using System.Windows;
using RDPManager.Models;
using RDPManager.Services;

namespace RDPManager.Views;

public partial class ConnectionDialog : Window
{
    public RdpConnection Result { get; private set; } = new();

    public ConnectionDialog()
    {
        InitializeComponent();
        DataContext = TranslationService.Instance;
        Owner = Application.Current.MainWindow;
    }

    public ConnectionDialog(RdpConnection existing) : this()
    {
        Title = TranslationService.Instance.EditTitle;
        TbServer.Text = existing.ServerAddress;
        TbUsername.Text = existing.Username;
        PbPassword.Password = existing.Password;
        TbNotes.Text = existing.Notes;
        Result = existing;
    }

    private void BtnOk_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TbServer.Text))
        {
            MessageBox.Show(
                TranslationService.Instance.ValidationServerRequired,
                TranslationService.Instance.ValidationTitle,
                MessageBoxButton.OK, MessageBoxImage.Warning);
            TbServer.Focus();
            return;
        }

        Result.ServerAddress = TbServer.Text.Trim();
        Result.Username = TbUsername.Text.Trim();
        Result.Password = PbPassword.Password;
        Result.Notes = TbNotes.Text.Trim();
        DialogResult = true;
        Close();
    }
}
