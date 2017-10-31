using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kartoteka.Domain;
using Google.Apis.Drive.v2;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Windows;

namespace Kartoteka.GoogleDrive
{
    public class DefaultGoogleDriveService:IGoogleDriveService
    {
        public  DriveService Authorization()
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
            return service;

        }
        public  Dictionary<string, string> GetFolders(DriveService _service, Dictionary<string, string> folders)
        {
            FilesResource.ListRequest listRequest = _service.Files.List();
            var files = listRequest.Execute();
            foreach (File file in files.Items)
            {
                if (file.MimeType == "application/vnd.google-apps.folder")
                {
                    folders.Add(file.Id, file.Title);
                }
            }
            return folders;

        }
        public  File UploadFile(DriveService _service, ExportData _uploadFile)
        {
            File body = new File();
            body.Title = _uploadFile.FileName;
            body.Description = "File uploaded by Diamto Drive Sample";
            body.MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            body.Parents = new List<ParentReference>() { new ParentReference() { Id = _uploadFile.Folder } };

            System.IO.MemoryStream stream = new System.IO.MemoryStream(_uploadFile.Data);
            try
            {
                FilesResource.InsertMediaUpload request = _service.Files.Insert(body, stream, body.MimeType);
                request.Upload();
                return request.ResponseBody;
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred: " + e.Message);
                return null;
            }
        }
    }
}
