namespace TrakQ.Service;
public class MonthBoardService
{
    public ObservableCollection<KeyValuePair<int, string>> Years { get; init; } = [
        new (2022, "2022"),
        new (2023, "2023"),
        new (2024, "2024"),
        new (2025, "2025"),
    ];
    public ObservableCollection<KeyValuePair<int, string>> Months { get; init; } = [
        new (1, "January"),
        new (2, "February"),
        new (3, "March"),
        new (4, "April"),
        new (5, "May"),
        new (6, "June"),
        new (7, "July"),
        new (8, "August"),
        new (9, "September"),
        new (10, "October"),
        new (11, "November"),
        new (12, "December"),
    ];




    private int _selectedMonth;
    public KeyValuePair<int, string> SelectedMonth
    {
        get
        {
            return Months.FirstOrDefault(a => a.Key == _selectedMonth);
        }
    }

    private int _selectedYear;
    public KeyValuePair<int, string> SelectedYear
    {
        get { return Years.FirstOrDefault(a => a.Key == _selectedYear); }
    }

    public MonthBoardService()
    {
        var current = DateTime.Now;
        _selectedMonth = current.Month;
        _selectedYear = current.Year;
    }


    public void SetMonthAndYear(KeyValuePair<int, string> month, KeyValuePair<int, string> year)
    {
        _selectedMonth = month.Key;
        _selectedYear = year.Key >= 2022 ? year.Key : DateTime.Now.Year;
    }
}
