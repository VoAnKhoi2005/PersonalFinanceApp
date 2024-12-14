using PersonalFinanceApp.ViewModel;

namespace PersonalFinanceApp.Src.View;

public class CalendarButtonViewModel : BaseViewModel
{
    public DateTime Date { get; set; }

    public string Info1
    {
        get => _info1;
        set
        {
            _info1 = value;
            OnPropertyChanged();
        }
    }
    private string _info1 = string.Empty;
    public bool HaveInfo1 => Info1 != "";

    public string Info2
    {
        get => _info2;
        set
        {
            _info2 = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(HaveInfo2));
        }
    }
    private string _info2 = string.Empty;
    public bool HaveInfo2 => !string.IsNullOrEmpty(Info2);

    public string AdditionalInfo
    {
        get => _additionalInfo;
        set
        {
            _additionalInfo = value;
            OnPropertyChanged();
        }
    }
    private string _additionalInfo = string.Empty;
    public bool HaveAdditionalInfo => AdditionalInfo != "";
}