﻿namespace Safety_Toolbox
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnTBTBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Toolbox_Talk());
        }
        private async void OnCertBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Certifications());
        }
        private async void OnLibBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Library());
        }
    }
}