using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Threading;
using System.Collections.Generic;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using System.Windows;
using System.Linq;

namespace Kartoteka.Domain
{
    public class GoogleDrive
    {
        public static DriveService Authorization()
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
        private static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }
        public static Dictionary<string, string> GetFolders(DriveService _service, Dictionary<string, string> folders)
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
        public static File UploadFile(DriveService _service,string fileName, ExportData _uploadFile, string _parent)
        {
                File body = new File();
                body.Title = fileName;
                body.Description = "File uploaded by Diamto Drive Sample";
                body.MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                body.Parents = new List<ParentReference>() { new ParentReference() {  Id = _parent } };

                System.IO.MemoryStream stream = new System.IO.MemoryStream(_uploadFile.Data);
                try
                {
                    FilesResource.InsertMediaUpload request = _service.Files.Insert(body, stream,body.MimeType);
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
