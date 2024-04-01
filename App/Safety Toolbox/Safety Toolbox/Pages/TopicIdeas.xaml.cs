namespace Safety_Toolbox;
using Microsoft.Data.SqlClient;
using System.Linq;

public partial class TopicIdeas : ContentPage
{
    private List<string> AllTopics;
    public TopicIdeas()
	{
		InitializeComponent();

        if (MainPage.isReadOnly)
        {
            NewTopicArea.IsVisible = false;
        }

        getTopics();
	}

    private void getTopics()
    {
        List<string> topicIdeas = new List<string>();

        string query = "SELECT * FROM Topics";

        try
        {
            using (SqlConnection connection = new SqlConnection(Preferences.Default.Get("DBConn", "Not Found")))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string topic = reader.GetString(0);

                            topicIdeas.Add(topic);
                        }
                    }
                }
            }
        }
        catch
        {
            ConnectionFail.IsVisible = true;
        }

        topicIdeas.Sort(); //sort alphabetically

        TopicsView.ItemsSource = topicIdeas;
        AllTopics = topicIdeas;
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        TopicsView.WidthRequest = 0.8 * width;
        ScrollTopics.HeightRequest = 0.7 * height;
        SearchBar.WidthRequest = 0.5 * width;
    }

    async private void OnAddTopicButtonClicked(object sender, EventArgs e)
    {
        if(NewTopic.Text != "" && NewTopic.Text != null)
        {
            //add new topic
            string query = "Insert into Topics Values (@TopicIdea);";
            
            try {
                using (SqlConnection connection = new SqlConnection(Preferences.Default.Get("DBConn", "Not Found")))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        var topicIdeaParam = new SqlParameter("TopicIdea", NewTopic.Text);
                        command.Parameters.Add(topicIdeaParam);

                        var results = command.ExecuteReader();
                
                    }
                }
            }
            catch
            {
                await DisplayAlert("Database Connection", "There was a problem connecting to the database.", "OK");
            }

            //refresh list
            getTopics();
        }
        
    }

    private void NewTopic_TextChanged(object sender, TextChangedEventArgs e)
  {
        if(NewTopic.Text.ToCharArray().Count() > 200)
        {
            CharLimitWarning.IsVisible = true;
            AddTopicButton.IsEnabled = false;
        }
        else
        {
            CharLimitWarning.IsVisible = false;
            AddTopicButton.IsEnabled = true;
        }
        
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        SearchBar searchBar = (SearchBar)sender;
        if (searchBar != null)
        {
            List<string> topicIdeas = AllTopics;

            var matchingContent = topicIdeas.Where(topicIdea => topicIdea.ToLower().Contains(searchBar.Text.ToLower()));

            TopicsView.ItemsSource = matchingContent;
        }
    }
}