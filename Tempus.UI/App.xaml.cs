namespace Tempus.UI;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        // On phones - use phone tabs.
        if (DeviceInfo.Idiom == DeviceIdiom.Phone)
        {
            Shell.Current.CurrentItem = PhoneTabs;
        }
    }
}
