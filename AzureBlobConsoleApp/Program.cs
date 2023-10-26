using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Policy;

namespace AzureBlobConsoleApp
{
    public class Program
    {
        static string connectionString = "DefaultEndpointsProtocol=https;AccountName=axosbloblearning;AccountKey=rG6FBbbYMC74AhMjKxd7zcelZrfdpIlr6J3npUMGoDO6ALH8T+ZqVE7wxrtYDCPhZsOJtDDekKQ8+AStW5oXeg==;EndpointSuffix=core.windows.net";
        //containers: axosblob-pdf, privatecontainer
        static string containerName = "privatecontainer";
        static string directory = @"C:\axosbloblearning\";
        static string path = directory + containerName;
        static BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
        static BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName); 

        static void Main(string[] args)
        {
            // Pls enable each function to test
            //DownloadBlobs();
            //UploadBlobs();
            DeleteBlobs();

            #region disable
            //    Console.WriteLine("u = upload, d = download, x = delete, z = exit");
            //    int x = Console.Read();
            //    char userInput = Convert.ToChar(x);

            //    #region While loop
            //    while (userInput != 'z')
            //    {
            //        switch (userInput)
            //        {
            //            case 'u'
            //            :
            //                UploadBlobs();
            //                break;

            //            case 'd':
            //                DownloadBlobs(); 
            //                break;

            //            case 'x':
            //                DeleteBlobs();
            //                break;

            //            default: break;
            //        }

            //        Console.Clear();
            //        Console.WriteLine("u = upload, d = download, x = delete, z = exit");
            //        x = Console.Read();
            //        userInput = Convert.ToChar(x); ;
            //    }
            #endregion
        }

        private static void DeleteBlobs()
        {
            string blobName = "free-images.jpg";
            BlobClient blobclient = new BlobClient(connectionString, containerName, blobName);
            blobclient.Delete();
            Console.WriteLine("Blob file " + blobName + " has been deleted.");
            Console.Read();
        }

        private static void UploadBlobs()
        {
            var files = Directory.GetFiles(path);
            foreach(var file in files)
            {
                using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(file)))
                {
                    blobContainerClient.UploadBlob(Path.GetFileName(file), stream);
                }
                Console.WriteLine(file + " uploaded!");
            }
            Console.WriteLine("All files has been downloaded successfully!");
            Console.Read();
        }

        private static void DownloadBlobs()
        {
            int ctr = 0;
            var blobs = blobContainerClient.GetBlobs();

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            foreach (var blob in blobs)
            {
                Console.WriteLine(blob.Name);
                BlobClient blobClient = blobContainerClient.GetBlobClient(blob.Name);

                blobClient.DownloadTo(path + "\\" + blob.Name);

                if (!string.IsNullOrEmpty(blob.Name))
                {
                    ctr++;
                }
            }

            string message = ctr > 0 ? "All files has been downloaded successfully." : "No files to download";
            Console.WriteLine(message);

            Console.Read();
        }
    }
}
