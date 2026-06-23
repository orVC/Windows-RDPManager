using System.Windows;
using RDPManager.Models;

namespace RDPManager.Views;

public partial class ConnectionDialog : Window
{
    public RdpConnection Result { get; private set; } = new();

    public ConnectionDialog()
    {
        InitializeComponent();
        Owner = Application.Current.MainWindow;
    }

    public ConnectionDialog(RdpConnection existing) : this()
    {
        Title = "编辑 RDP 连接";
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
            MessageBox.Show("请输入服务器地址", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
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
