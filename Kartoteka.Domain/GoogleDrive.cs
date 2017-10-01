using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Threading;
using System.Collections.Generic;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using System.Windows;

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
        public static File CreateDirectory(DriveService _service, string _title, string _description, string _parent)
        {
            File NewDirectory = null;

            // Create metaData for a new Directory
           File body = new File();
            body.Title = _title;
            body.Description = _description;
            body.MimeType = "application/vnd.google-apps.folder";
            body.Parents = new List<ParentReference> () { new ParentReference() { Id = _parent } };
            try
            {
                FilesResource.InsertRequest request = _service.Files.Insert(body);
                NewDirectory = request.Execute();
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred: " + e.Message);
            }

            return NewDirectory;
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
        public static File UploadFile(DriveService _service, string _uploadFile, string _parent)
        {

            if (System.IO.File.Exists(_uploadFile))
            {
                File body = new File();
                body.Title = System.IO.Path.GetFileName(_uploadFile);
                body.Description = "File uploaded by Diamto Drive Sample";
                body.MimeType = GetMimeType(_uploadFile);
                body.Parents = new List<ParentReference>() { new ParentReference() {  Id = _parent } };

                // File's content.
                byte[] byteArray = System.IO.File.ReadAllBytes(_uploadFile);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);
                try
                {
                    FilesResource.InsertMediaUpload request = _service.Files.Insert(body, stream, GetMimeType(_uploadFile));
                    request.Upload();
                    return request.ResponseBody;
                }
                catch (Exception e)
                {
                    MessageBox.Show("An error occurred: " + e.Message);
                    return null;
                }
            }
            else
            {
                MessageBox.Show("File does not exist: " + _uploadFile);
                return null;
            }

        }

    }
}
