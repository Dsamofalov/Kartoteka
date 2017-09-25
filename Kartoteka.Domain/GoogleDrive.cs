using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kartoteka.Domain
{
    public class GoogleDrive
    {
        public static void Autorisation()
        {
            string[] scopes = new string[] { DriveService.Scope.Drive,
                                 DriveService.Scope.DriveFile};
            var clientId = "808494698501-464qd8j0t0gcg9ci5vh1feedbi78me5a.apps.googleusercontent.com";
            var clientSecret = "eJjHJnJKXc6fsdTXpwbGb-C8";
            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            },
                scopes, Environment.UserName, CancellationToken.None, new FileDataStore("Daimto.GoogleDrive.Auth.Store")).Result;

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Drive API Quickstart",
            });

        }
        
    }
}
