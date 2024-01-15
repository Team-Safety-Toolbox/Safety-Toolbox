namespace Safety_Toolbox.Pages;

public partial class ReportSettings : ContentPage
{
    public ReportSettings()
	{
		InitializeComponent();

        //TODO
        //get data for this user, do they currently get emails and which options
        ParentCheckBox.IsChecked = true;// IsChecked="{Binding ParentChecked}"
        FirstCheckBox.IsEnabled = true;
        FifteenthCheckBox.IsEnabled = true;
        FirstCheckBox.IsChecked = true;
        FifteenthCheckBox.IsChecked = false;

    }

    void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        //TODO
        //Update this data in the user's table whether they would like emails
        //based on the values of the check boxes


        FirstCheckBox.IsEnabled = ParentCheckBox.IsChecked;
        FifteenthCheckBox.IsEnabled = ParentCheckBox.IsChecked;
        //if (ParentCheckBox.IsChecked == true)
        //{
        //    FirstCheckBox.IsEnabled = true;
        //    FifteenthCheckBox.IsEnabled = true;
        //}
        //else
        //{
        //    FirstCheckBox.IsEnabled = false;
        //    FifteenthCheckBox.IsEnabled = false;
        //}
    }
}