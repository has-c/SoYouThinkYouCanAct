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
            emotionSelection();
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
        }

        async Task EmotionPrediction(MediaFile file)
        {
            var emotionClient = new EmotionServiceClient("fb7d6aa16e9f477fa1b1e2e61f5f5678");

            using (var photoStream = file.GetStream())
            {
                Emotion[] emotionResult = await emotionClient.RecognizeAsync(photoStream);
                if (emotionResult.Any())
                {
                    topEmotionLabel.Text = emotionResult.FirstOrDefault().Scores.ToRankedList().FirstOrDefault().Key;
                }
            }

        }

        public string RandomEmotionGenerator()
        {
            List<String> allEmotions = new List<string>(new string[] { "Anger", "Contempt", "Disgust", "Fear", "Happiness", "Sadness", "Surprise", "Neutral" });

            Random random = new Random();
            int randomNumber = random.Next(0, 8);
            string testEmotion = allEmotions.ElementAt(randomNumber);
            return testEmotion;
        }

        public void emotionSelection()
        {
            string testEmotion = RandomEmotionGenerator();

            if (testEmotion == "Anger")
            {
                gameEmotion.Text = "Produce an ANGRY EXPRESSION";
            }
            else if (testEmotion == "Contempt")
            {
                gameEmotion.Text = "Produce a CONTEMPTIBLE EXPRESSION";
            }
            else if (testEmotion == "Disgust")
            {
                gameEmotion.Text = "Produce a DIGUSTED EXPRESSION";
            }
            else if (testEmotion == "Fear")
            {
                gameEmotion.Text = "Produce a FEARFUL EXPRESSION";
            }
            else if (testEmotion == "Happiness")
            {
                gameEmotion.Text = "Produce a HAPPY EXPRESSION";
            }
            else if (testEmotion == "Sadness")
            {
                gameEmotion.Text = "Produce a SAD EXPRESSION";
            }
            else if (testEmotion == "Surprise")
            {
                gameEmotion.Text = "Produce a SURPRISED EXPRESSION";
            }
            else
            {
                gameEmotion.Text = "Produce a NEUTRAL EXPRESSION";
            }
        }
    }
}
 