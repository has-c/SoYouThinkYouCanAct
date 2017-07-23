using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SYTUCA
{
    
    public partial class GameOverPage : ContentPage
    {
        public GameOverPage()
        {
            InitializeComponent();
            beginAnimation();
        }


        public async void beginAnimation()
        {
            image.Opacity = 1;
            await image.FadeTo(0, 5000);
            await NavigateToDecisionPage();
        }

        public async Task NavigateToDecisionPage()
        {
            if (image.Opacity == 0)
            {
                await Navigation.PushAsync(new AzureTable());
;            }
        }
    }
}