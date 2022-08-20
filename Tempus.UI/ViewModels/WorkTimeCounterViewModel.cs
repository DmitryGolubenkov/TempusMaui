using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static System.Net.Mime.MediaTypeNames;

namespace Tempus.UI.ViewModels;

[INotifyPropertyChanged]
public partial class WorkTimeCounterViewModel
{
    #region MVVM Properties

    #region Input

    [ObservableProperty]
    private TimeSpan arrivalTime;

    [ObservableProperty]
    private TimeSpan dinnerStartTime;

    [ObservableProperty]
    private TimeSpan dinnerEndTime;

    [ObservableProperty]
    private TimeSpan departureTime;

    #endregion

    #region Computed

    /// <summary>
    /// Work time before dinner
    /// </summary>
    [ObservableProperty]
    private TimeSpan? firstHalfOfDayTimeSpan;
    
    /// <summary>
    /// Time spent on dinner
    /// </summary>
    [ObservableProperty]
    private TimeSpan? dinnerTimeSpan;

    /// <summary>
    /// Work time after dinner
    /// </summary>
    [ObservableProperty]
    private TimeSpan? secondHalfOfDayTimeSpan;

    /// <summary>
    /// Total work time
    /// </summary>
    [ObservableProperty]
    private TimeSpan? totalWorkTimeTimeSpan;

    #endregion

    #endregion

    #region MVVM Commands

    /// <summary>
    /// Sets arrival time to current time.
    /// </summary>
    [RelayCommand]
    private void SetArrivalTimeNow() => ArrivalTime = DateTime.Now.TimeOfDay;

    /// <summary>
    /// Sets dinner start time to current time.
    /// </summary>
    [RelayCommand]
    private void SetDinnerStartTimeNow() => DinnerStartTime = DateTime.Now.TimeOfDay;

    /// <summary>
    /// Sets dinner end time to current time.
    /// </summary>
    [RelayCommand]
    private void SetDinnerEndTimeNow() => DinnerEndTime = DateTime.Now.TimeOfDay;

    /// <summary>
    /// Sets departure time to current time.
    /// </summary>
    [RelayCommand]
    private void SetDepartureTimeNow() => DepartureTime = DateTime.Now.TimeOfDay;

    /// <summary>
    /// Recalculates computed timespans.
    /// </summary>
    [RelayCommand] 
    private void RefreshDayStatistics()
    {
        FirstHalfOfDayTimeSpan = DinnerStartTime - ArrivalTime;
        SecondHalfOfDayTimeSpan = DepartureTime - DinnerEndTime;
        TotalWorkTimeTimeSpan = FirstHalfOfDayTimeSpan + SecondHalfOfDayTimeSpan;
        DinnerTimeSpan = CalculateDinnerTimeSpan();
    }

    /// <summary>
    /// Calculates departure time using arrival and dinner times. If dinner time is not set, it uses default dinner timespan value.
    /// </summary>
    [RelayCommand]
    private void CalculateDepartureTime()
    {
        try
        {
            // If dinner is not set
            if (DinnerStartTime == TimeSpan.Zero || DinnerEndTime == TimeSpan.Zero)
            {
                DepartureTime = ArrivalTime + TimeSpan.FromHours(9);
            }
            else
            {
                DepartureTime = ArrivalTime + TimeSpan.FromHours(8) + CalculateDinnerTimeSpan();
            }
        }
        catch (ArgumentException)
        {
            //TODO: error message
        }
    }

    /// <summary>
    /// Resets all values in calculator.
    /// </summary>
    [RelayCommand]
    private void Reset()
    {
        // UI
        ArrivalTime = TimeSpan.Zero;
        DinnerStartTime = TimeSpan.Zero;
        DinnerEndTime = TimeSpan.Zero;
        DepartureTime = TimeSpan.Zero;

        // Computed
        FirstHalfOfDayTimeSpan = TimeSpan.Zero;
        SecondHalfOfDayTimeSpan = TimeSpan.Zero;
        TotalWorkTimeTimeSpan = TimeSpan.Zero;
        DinnerTimeSpan = TimeSpan.Zero;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Calculates time that was spent on dinner.
    /// </summary>
    private TimeSpan CalculateDinnerTimeSpan()
    {
        return DinnerEndTime - DinnerStartTime;
    }


    //Refresh statistics if relevant time changes occur.
    partial void OnDinnerEndTimeChanging(TimeSpan value) => RefreshDayStatistics();
    partial void OnDepartureTimeChanged(TimeSpan value) => RefreshDayStatistics();


    #endregion
}
