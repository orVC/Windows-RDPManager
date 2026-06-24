using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace RDPManager.Services;

public class TranslationService : INotifyPropertyChanged
{
    public static TranslationService Instance { get; } = new();

    private Dictionary<string, string> _strings = new();
    private string _currentLang = "zh-CN";

    public string CurrentLanguage
    {
        get => _currentLang;
        set
        {
            if (_currentLang == value) return;
            _currentLang = value;
            LoadLanguage(value);
            Refresh();
        }
    }

    public string this[string key] => _strings.GetValueOrDefault(key, key);

    public string AppTitle => this["AppTitle"];
    public string AppSubtitle => this["AppSubtitle"];
    public string BtnConnect => this["BtnConnect"];
    public string BtnAdd => this["BtnAdd"];
    public string BtnEdit => this["BtnEdit"];
    public string BtnDelete => this["BtnDelete"];
    public string ColServerAddress => this["ColServerAddress"];
    public string ColUsername => this["ColUsername"];
    public string ColNotes => this["ColNotes"];
    public string LabelDoubleClick => this["LabelDoubleClick"];
    public string EmptyState => this["EmptyState"];
    public string AddTitle => this["AddTitle"];
    public string EditTitle => this["EditTitle"];
    public string DialogSubtitle => this["DialogSubtitle"];
    public string LabelServerAddress => this["LabelServerAddress"];
    public string LabelUsername => this["LabelUsername"];
    public string LabelPassword => this["LabelPassword"];
    public string LabelNotes => this["LabelNotes"];
    public string BtnSave => this["BtnSave"];
    public string BtnCancel => this["BtnCancel"];
    public string ValidationServerRequired => this["ValidationServerRequired"];
    public string ValidationTitle => this["ValidationTitle"];
    public string DeleteConfirmTitle => this["DeleteConfirmTitle"];
    public string LanguageLabel => this["LanguageLabel"];

    public string Version
    {
        get
        {
            var ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            return ver != null ? $"v{ver.Major}.{ver.Minor}.{ver.Build}" : "v1.0.0";
        }
    }

    public string AppTitleWithVersion => $"{AppTitle} {Version}";

    public string StatusCount(int count) => string.Format(this["StatusCount"], count);
    public string DeleteConfirmMessage(string server) => string.Format(this["DeleteConfirmMessage"], server);

    private TranslationService()
    {
        LoadLanguage(_currentLang);
    }

    private void LoadLanguage(string lang)
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", $"{lang}.json");
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            _strings = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new();
        }
        else
        {
            _strings = new Dictionary<string, string>();
        }
    }

    public void Refresh()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
