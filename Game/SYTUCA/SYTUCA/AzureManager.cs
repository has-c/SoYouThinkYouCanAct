using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using SYTUCA.DataModels;

namespace SYTUCA
{
    class AzureManager
    {
        private static AzureManager instance;
        private MobileServiceClient client;
        private IMobileServiceTable<scoreInformation> scoreModel;

        private AzureManager()
        {
            this.client = new MobileServiceClient("http://emotion-game.azurewebsites.net");
            this.scoreModel = this.client.GetTable<scoreInformation>();
        }

        public MobileServiceClient AzureClient
        {
            get { return client; }
        }

        public static AzureManager AzureManagerInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AzureManager();
                }

                return instance;
            }
        }

        public async Task<List<scoreInformation>> GetScoreInformation()
        {
            return await this.scoreModel.ToListAsync();
        }

        public async Task PostScoreInformation(scoreInformation scoreModel)
        {
            await this.scoreModel.InsertAsync(scoreModel);
        }



    }
}
