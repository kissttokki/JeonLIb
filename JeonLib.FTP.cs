using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;


namespace JeonLib.FTP
{
    public class JeonFTP
    {
        string ftpPath;
        ICredentials credentials;


        public JeonFTP(string _ftpPath, ICredentials _credentials)
        {
            ftpPath = _ftpPath;
            credentials = _credentials;
        }

        public void MakeDirectory(string path)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(path);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            request.Credentials = credentials;
            request.GetResponse().Close();
        }

        public void UploadFile(string ftpPathAndFileName, string filePath)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpPathAndFileName);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = credentials;
            FileInfo fileInfo = new FileInfo(filePath);
            FileStream fileStream = fileInfo.OpenRead();
            Stream stream = null;
            byte[] buf = new byte[2048];
            int currentOffset = 0;
            try
            {
                stream = request.GetRequestStream();
                currentOffset = fileStream.Read(buf, 0, 2048);
                while (currentOffset != 0)
                {
                    stream.Write(buf, 0, currentOffset);
                    currentOffset = fileStream.Read(buf, 0, 2048);
                }
            }
            catch (Exception c)
            {
                throw c;
            }
            finally
            {
                fileStream.Dispose();
                if (stream != null)
                    stream.Dispose();
            }
        }



        public bool IsExistFile(string fileFath)
        {

            WebRequest request = WebRequest.Create(fileFath);
            request.Credentials = credentials;
            request.Method = WebRequestMethods.Ftp.GetFileSize;
            try
            {
                request.GetResponse();
                return true;
            }
            catch (WebException e)
            {
                FtpWebResponse response = (FtpWebResponse)e.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    return false;
                else
                {
                    Console.WriteLine("Error: " + e.Message);
                    return false;
                }
            }
        }

        public bool IsExistDirectory(string basePath, string directoryname)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(basePath);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = credentials;
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            string data = string.Empty;
            try
            {
                if (response != null)
                {
                    StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                    data = streamReader.ReadToEnd();
                }
            }
            finally
            {
                if (response != null)
                    response.Close();
            }

            return (data.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Any(t => t == directoryname));

        }


        public void RemoveDirectory(string directoryPath)
        {
            FtpWebRequest listRequest = (FtpWebRequest)WebRequest.Create(directoryPath);
            listRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            listRequest.Credentials = credentials;

            List<string> lines = new List<string>();

            using (FtpWebResponse listResponse = (FtpWebResponse)listRequest.GetResponse())
            using (Stream listStream = listResponse.GetResponseStream())
            using (StreamReader listReader = new StreamReader(listStream))
            {
                while (!listReader.EndOfStream)
                {
                    lines.Add(listReader.ReadLine());
                }
            }

            foreach (string line in lines)
            {
                string[] tokens =
                    line.Split(new[] { ' ' }, 9, StringSplitOptions.RemoveEmptyEntries);
                string name = tokens[8];
                string permissions = tokens[0];

                string fileUrl = directoryPath + "/" + name;

                if (permissions[0] == 'd')
                {
                    RemoveDirectory(fileUrl + "/");
                }
                else
                {
                    FtpWebRequest deleteRequest = (FtpWebRequest)WebRequest.Create(fileUrl);
                    deleteRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                    deleteRequest.Credentials = credentials;

                    deleteRequest.GetResponse();
                }
            }

            FtpWebRequest removeRequest = (FtpWebRequest)WebRequest.Create(directoryPath);
            removeRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;
            removeRequest.Credentials = credentials;
            removeRequest.GetResponse();
        }

        public void DownloadFile(string targetPath, string savePath,
            Action<object, System.ComponentModel.AsyncCompletedEventArgs> completed,
            Action<object, DownloadProgressChangedEventArgs, long> progress = null, long byte_total = 0)
        {

            using (WebClient cli = new WebClient())
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(targetPath);
                request.Method = WebRequestMethods.Ftp.GetFileSize;
                request.Credentials = credentials;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                // FTP 사용자 설정
                cli.Credentials = credentials;
                cli.DownloadFileAsync(new Uri(targetPath), savePath);
                cli.DownloadFileCompleted += (tc, c) => completed(tc, c);
                byte_total = Convert.ToInt64(response.ContentLength);
                if (progress != null)
                    cli.DownloadProgressChanged += (tc, c) => progress(tc, c, byte_total);
            }
        }
    }
}
