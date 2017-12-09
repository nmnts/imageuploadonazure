using System;
using System.IO;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ResizeImage
{
    public class UploadImageCloud
    {
        public static string UploadImageOnAzureBlob(string StorageConnectionString,string containerName,string ImageName,string ImageExtension,string ImagePathFromImageSave)
        {
            string result = "";
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);
                CloudBlobContainer ThumbImage_container = blobClient.GetContainerReference(containerName);
                if (container.CreateIfNotExists())
                {
                    // configure container for public access
                    var permissions = container.GetPermissions();
                    permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                    container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                }

                CloudBlockBlob cloudBlockBlobThumbImage = container.GetBlockBlobReference(ImageName + "." + ImageExtension); //file name
                cloudBlockBlobThumbImage.Properties.ContentType = "image/" + ImageExtension;
                using (var fileStream = File.OpenRead(ImagePathFromImageSave + "/" + ImageName + "." + ImageExtension))
                {
                    cloudBlockBlobThumbImage.UploadFromStream(fileStream);
                }
                Console.WriteLine("Blob_thumImage Url : {0}", cloudBlockBlobThumbImage.Uri);
                result = cloudBlockBlobThumbImage.Uri.ToString();
                result = "Image Upload Successfully";
                return result;
            }
            catch(Exception ex)
            {
                result = ex.Message;
                return result;
            }            
        }

        public Stream GetStream(Image img, ImageFormat format)
        {
            var ms = new MemoryStream();
            img.Save(ms, format);
            return ms;
        }
    }
}
