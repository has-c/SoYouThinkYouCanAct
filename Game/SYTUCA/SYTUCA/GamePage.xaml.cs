using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using System.Collections.Generic;
using SYTUCA.DataModels;


namespace SYTUCA
{
  
    public partial class GamePage : ContentPage
    {
        public GamePage()
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
            await DecisionRequestAsync();
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

        public class Globals
        {
            public static string refEmotion = null;
            public static string realEmotion = null;
            public static int totalScore = 0;
            public static int count = 0;
        }

        public async Task DecisionRequestAsync()
        {
            if (Globals.refEmotion == Globals.realEmotion)
            {
                Globals.totalScore += 1;
                scoreTag.Text = "Score = " + Convert.ToString(Globals.totalScore);
                RandomEmotionGenerator();

            }
            else
            {
                scoreInformation model = new scoreInformation()
                {
                    Score = Convert.ToString(Globals.totalScore)
                };

                await AzureManager.AzureManagerInstance.PostScoreInformation(model);


                await Navigation.PushAsync(new GameOverPage());


            }
        }
    }



}

