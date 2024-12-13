using PersonalFinanceApp.ViewModel;

namespace PersonalFinanceApp.Src.View;

public class CalendarDayViewModel : BaseViewModel
{
    public DateTime Date { get; set; }

    private string _info1 = string.Empty;

    public string Info1
    {
        get => _info1;
        set
        {
            _info1 = value;
            OnPropertyChanged();
        }
    }

    public bool HaveInfo1 => Info1 != "";

    private string _info2 = string.Empty;

    public string Info2
    {
        get => _info2;
        set
        {
            _info2 = value;
            OnPropertyChanged();
        }
    }

    public bool HaveInfo2 => Info2 != "";

    private string _additionalInfo = string.Empty;

    public string AdditionalInfo
    {
        get => _additionalInfo;
        set
        {
            _additionalInfo = value;
            OnPropertyChanged();
        }
    }
    public bool HaveAdditionalInfo => AdditionalInfo != "";
}