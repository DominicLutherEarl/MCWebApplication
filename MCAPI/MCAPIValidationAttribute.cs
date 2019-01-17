using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Mc.TD.Upload.Domain.DataMatch
{
    [AttributeUsage(AttributeTargets.Property |  AttributeTargets.Field, AllowMultiple = true)]
    public sealed class MCAPIValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool result = true;
            if (1==1) { }
            if (ErrorMessageResourceName == "orderid")
            {

            }
            // Add validation logic here.
            return result;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name);
        }


        //public async void UploadToBLOB()
        //{
        //    CloudStorageAccount _cloudStorageAccount = null;
        //    CloudBlobContainer _cloudBlobContainer = null;
        //    BlobContinuationToken _blobContinuationToken = null;
        //    CloudBlobClient _cloudBlobClient;
        //    //BlobContainerPermissions _permissions;
        //    //CloudBlockBlob _cloudBlockBlob;

        //    string sourceFile = null;
        //    string destinationFile = null;
        //    string storageConnectionString = Environment.GetEnvironmentVariable("storageconnectionstring");
        //    string localPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //    string localFileName = "QuickStart_" + Guid.NewGuid().ToString() + ".txt";
        //    sourceFile = Path.Combine(localPath, localFileName);

        //    File.WriteAllText(sourceFile, "Hello, World!");
        //    if (CloudStorageAccount.TryParse(storageConnectionString, out _cloudStorageAccount))
        //    {
        //        try
        //        {
        //            _cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
        //            _cloudBlobContainer = _cloudBlobClient.GetContainerReference("quickstartblobs" + Guid.NewGuid().ToString());
        //            await _cloudBlobContainer.CreateAsync();
        //            await _cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
        //            await _cloudBlobContainer.GetBlockBlobReference(localFileName).UploadFromFileAsync(sourceFile);
        //            do
        //            {
        //                var results = await _cloudBlobContainer.ListBlobsSegmentedAsync(null, _blobContinuationToken);
        //                _blobContinuationToken = results.ContinuationToken;
        //                foreach (IListBlobItem item in results.Results)
        //                {
        //                    Console.WriteLine(item.Uri);
        //                }
        //            } while (_blobContinuationToken != null);
        //            destinationFile = sourceFile.Replace(".txt", "_DOWNLOADED.txt");
        //            await _cloudBlobContainer.GetBlockBlobReference(localFileName).DownloadToFileAsync(destinationFile, FileMode.Create);
        //        }
        //        catch (StorageException ex)
        //        {
        //        }
        //        finally
        //        {
        //            if (_cloudBlobContainer != null)
        //            {
        //                await _cloudBlobContainer.DeleteIfExistsAsync();
        //            }
        //            File.Delete(sourceFile);
        //            File.Delete(destinationFile);
        //        }
        //    }
        //}
    }
}