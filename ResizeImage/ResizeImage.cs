using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Web;
using System.Net;

namespace ResizeImage
{
    public class ResizeImage
    {
        public static string ResizeOrignalImageToThumbImage(string ImageName,string ImageExtension,string OrignalImagePath, Image OrignalImage,string SaveOrignalImagePath,string SaveThumbImagePath,int ResizeWidth,int ResizeHeight)
        {
            string result = "";
            string imageName = Path.GetFileName(ImageName);            
            try
            {
                string originalImagePath = Path.Combine(HttpContext.Current.Server.MapPath(SaveOrignalImagePath));
                bool originalImagefolderExists = Directory.Exists(originalImagePath);
                if (!originalImagefolderExists)
                    Directory.CreateDirectory(originalImagePath);
                string originalImagefile = Path.Combine(originalImagePath, ImageName + "." + ImageExtension);
                if(!string.IsNullOrEmpty(OrignalImagePath))
                {
                    Image image = DownloadImageFromUrl(OrignalImagePath);
                    if (image != null)
                    {
                        Image thumbImage = image.GetThumbnailImage(ResizeWidth, ResizeHeight, () => false, IntPtr.Zero);
                        string thumbImagePath = HttpContext.Current.Server.MapPath(SaveThumbImagePath);
                        bool thumbImagefolderExists = Directory.Exists(thumbImagePath);
                        if (!thumbImagefolderExists)
                            Directory.CreateDirectory(thumbImagePath);
                        string thumbImagefile = Path.Combine(thumbImagePath, ImageName + "_thumb" + "." + ImageExtension);

                        thumbImage.Save(thumbImagefile);
                        image.Save(originalImagefile);
                        result = "Image Resize Successfully";
                    }
                }
                else
                {
                    Image image = OrignalImage;
                    if (image != null)
                    {
                        Image thumbImage = image.GetThumbnailImage(ResizeWidth, ResizeHeight, () => false, IntPtr.Zero);
                        string thumbImagePath = HttpContext.Current.Server.MapPath(SaveThumbImagePath);
                        bool thumbImagefolderExists = Directory.Exists(thumbImagePath);
                        if (!thumbImagefolderExists)
                            Directory.CreateDirectory(thumbImagePath);
                        string thumbImagefile = Path.Combine(thumbImagePath, ImageName + "_thumb" + "." + ImageExtension);

                        thumbImage.Save(thumbImagefile);
                        image.Save(originalImagefile);
                        result = "Image Resize Successfully";
                    }
                }                
                return result;
            }
            catch(Exception ex)
            {
                result = ex.Message;
                return result;
            }            
        }

        public static Image DownloadImageFromUrl(string imageUrl)
        {
            Image image = null;
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(imageUrl);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 30000;
                WebResponse webResponse = webRequest.GetResponse();
                Stream stream = webResponse.GetResponseStream();
                image = Image.FromStream(stream);
                webResponse.Close();
            }
            catch (Exception ex)
            {
                return null;
            }
            return image;
        }
    }
}
