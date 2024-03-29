using Microsoft.Data.SqlClient;
namespace Safety_Toolbox;

public partial class TopicIdeas : ContentPage
{
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

        using (SqlConnection connection = new SqlConnection(Constants.connectionString))
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

        topicIdeas.Sort(); //sort alphabetically

        TopicsView.ItemsSource = topicIdeas;
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        TopicsView.WidthRequest = 0.8 * width;
        ScrollTopics.HeightRequest = 0.7 * height;
    }

    private void OnAddTopicButtonClicked(object sender, EventArgs e)
    {
        if(NewTopic.Text != "" && NewTopic.Text != null)
        {
            //add new topic
            string query = "Insert into Topics Values (@TopicIdea);";

            using (SqlConnection connection = new SqlConnection(Constants.connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    var topicIdeaParam = new SqlParameter("TopicIdea", NewTopic.Text);
                    command.Parameters.Add(topicIdeaParam);

                    var results = command.ExecuteReader();
                
                }
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
}