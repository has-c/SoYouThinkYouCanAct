using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SYTUCA
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            startAnimation();
            
        }

        public async void startAnimation()
        {
            picture.Opacity = 0;
            await picture.FadeTo(1, 5000);
            await NavigationRequestAsync();
        }

        public async Task NavigationRequestAsync()
        {
            if (picture.Opacity == 1)
            {
                await Navigation.PushAsync(new RulesPage());
            }
        }
    }
}
