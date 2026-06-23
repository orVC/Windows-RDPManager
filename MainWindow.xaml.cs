using System.Windows;
using RDPManager.Data;
using RDPManager.Helpers;
using RDPManager.Models;
using RDPManager.Views;

namespace RDPManager;

public partial class MainWindow : Window
{
    private readonly DatabaseHelper _db;

    public MainWindow()
    {
        InitializeComponent();

        var dbPath = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "RDPManager", "connections.db");

        System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(dbPath)!);
        _db = new DatabaseHelper(dbPath);
        LoadData();
    }

    private void LoadData()
    {
        var items = _db.GetAll();
        LvConnections.ItemsSource = items;
        TbEmpty.Visibility = items.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        TbStatus.Text = $"共 {items.Count} 个连接";
    }

    private void BtnAdd_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new ConnectionDialog();
        if (dialog.ShowDialog() == true)
        {
            _db.Insert(dialog.Result);
            LoadData();
        }
    }

    private void BtnEdit_Click(object sender, RoutedEventArgs e)
    {
        EditSelected();
    }

    private void LvConnections_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (LvConnections.SelectedItem is not RdpConnection selected) return;
        RdpLauncher.Launch(selected);
    }

    private void EditSelected()
    {
        if (LvConnections.SelectedItem is not RdpConnection selected) return;

        var dialog = new ConnectionDialog(selected);
        if (dialog.ShowDialog() == true)
        {
            _db.Update(selected);
            LoadData();
        }
    }

    private void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (LvConnections.SelectedItem is not RdpConnection selected) return;

        var result = MessageBox.Show(
            $"确定要删除 {selected.ServerAddress} 的连接吗？",
            "确认删除", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            _db.Delete(selected.Id);
            LoadData();
        }
    }

    private void BtnConnect_Click(object sender, RoutedEventArgs e)
    {
        if (LvConnections.SelectedItem is not RdpConnection selected) return;
        RdpLauncher.Launch(selected);
    }
}
