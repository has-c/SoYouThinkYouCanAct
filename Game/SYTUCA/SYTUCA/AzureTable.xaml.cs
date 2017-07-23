using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SYTUCA.DataModels;

namespace SYTUCA
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AzureTable : ContentPage
    {
        public AzureTable()
        {
            InitializeComponent();
        }

        async void PastScores(object sender, System.EventArgs e)
        {
            List<scoreInformation> scoreModel = await AzureManager.AzureManagerInstance.GetScoreInformation();

            ScoreList.ItemsSource = scoreModel;
        }
    }
}