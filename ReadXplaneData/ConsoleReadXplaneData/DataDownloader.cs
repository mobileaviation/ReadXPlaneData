using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using ConsoleReadXplaneData;

namespace FSPAirnavDatabaseExporter
{
    class DataDownloader
    {
        public DataDownloader(String Local_path)
        {
            _urls = new List<string>();

            _urls.Add(urls.airports);
            _urls.Add(urls.countries);
            _urls.Add(urls.frequencies);
            _urls.Add(urls.navaids);
            _urls.Add(urls.regions);
            _urls.Add(urls.runways);
            _urls.Add(urls.cities5000);

            _localPath = Local_path;

            log = LogManager.GetCurrentClassLogger();
        }

        private List<String> _urls;
        private String _localPath;
        private Logger log;

        private void deleteOldFiles()
        {
            foreach (String f in _urls)
            {
                String filename = Path.GetFileName(f);
                String deleteFile = _localPath + filename;
                File.Delete(deleteFile);
            }
        }

        private void downloadFiles()
        {
            foreach (String f in _urls)
            {
                String filename = Path.GetFileName(f);
                String downloadFile = _localPath + filename;
                using (WebClient client = new WebClient())
                {
                    //client.DownloadProgressChanged += Client_DownloadProgressChanged;
                    //client.DownloadDataCompleted
                    log.Info("Start download: {0}", f);
                    client.DownloadFile(f, downloadFile);
                    log.Info("Downloaded file {0)", downloadFile);
                }
            }
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            log.Info("Downloading: {0} progress: {1}", sender.ToString(), e.ProgressPercentage);
        }

        public void DownloadFiles()
        {
            deleteOldFiles();
            downloadFiles();
        }

        public event EventHandler DownloadsFinished;

    }
}
