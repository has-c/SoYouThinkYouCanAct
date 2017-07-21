using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using System.Collections.Generic;

namespace Game
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            RandomEmotionGenerator();
        }

        


        private async void loadCamera(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                Directory = "Sample",
                Name = $"{DateTime.UtcNow}.jpg"
            });

            if (file == null)
                return;

            image.Source = ImageSource.FromStream(() =>
            {
                return file.GetStream();
            });


            await EmotionPrediction(file);
            DecisionRequest();
        }






        public async Task EmotionPrediction(MediaFile file)
        {
            var emotionClient = new EmotionServiceClient("fb7d6aa16e9f477fa1b1e2e61f5f5678");

            using (var photoStream = file.GetStream())
            {
                Emotion[] emotionResult = await emotionClient.RecognizeAsync(photoStream);
                if (emotionResult.Any())
                {
                    string actualEmotion = emotionResult.FirstOrDefault().Scores.ToRankedList().FirstOrDefault().Key;
                    Globals.realEmotion = actualEmotion;
                    topEmotionLabel.Text = "Emotion: " + actualEmotion;
                    
                }
            }

            
        }


        public void RandomEmotionGenerator()
        {
          List<String> allEmotions = new List<string>(new string[] { "Anger", "Contempt", "Disgust", "Fear", "Happiness", "Sadness", "Surprise", "Neutral" });

            Random random = new Random();
            int randomNumber = random.Next(0, 8);
            string chosenEmotion = allEmotions.ElementAt(randomNumber);
            Globals.refEmotion = chosenEmotion;
            gameEmotion.Text = "Produce an expression of " + chosenEmotion.ToUpper();
        }

        public  class Globals
        {
            public static string refEmotion = null;
            public static string realEmotion = null;
        }

        public void DecisionRequest()
        {
            if (Globals.refEmotion == Globals.realEmotion)
            {
                decisionLabel.Text = "yes";
            }
            else
            {
                decisionLabel.Text = "no";

            }
        }
    }

    

}


       