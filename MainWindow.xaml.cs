using System.Windows;
using RDPManager.Data;
using RDPManager.Helpers;
using RDPManager.Models;
using RDPManager.Services;
using RDPManager.Views;

namespace RDPManager;

public partial class MainWindow : Window
{
    private readonly DatabaseHelper _db;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = TranslationService.Instance;

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
        TbStatus.Text = TranslationService.Instance.StatusCount(items.Count);
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
            TranslationService.Instance.DeleteConfirmMessage(selected.ServerAddress),
            TranslationService.Instance.DeleteConfirmTitle,
            MessageBoxButton.YesNo, MessageBoxImage.Question);

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

    private void CbLanguage_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (CbLanguage.SelectedItem is not System.Windows.Controls.ComboBoxItem item) return;
        TranslationService.Instance.CurrentLanguage = item.Tag.ToString()!;
        LoadData();
    }
}
