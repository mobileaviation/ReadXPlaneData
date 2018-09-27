using ConsoleReadXplaneData.EF;
using FSPService.EF.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FSPAirspacesDatabaseExporter
{
    public class Downloader
    {
        public Downloader(String countryCode)
        {
            this.countryCode_Filter = countryCode;
            log = LogManager.GetCurrentClassLogger();
        }

        private List<EFLink> links;
        private String countryCode_Filter;
        public List<EFLink> Links { get { return links; } }
        private Logger log;

        private void getLinksfromDB()
        {
            using (AirNavDB db = new AirNavDB())
            {
                if (countryCode_Filter.Length > 0)
                {
                    var links = from l in db.links
                                where l.enabled == true
                                && l.countrycode==countryCode_Filter
                                select l;

                    this.links = links.ToList();
                }
                else
                {
                    var links = from l in db.links
                                where l.enabled == true
                                select l;

                    this.links = links.ToList();
                }
            }
        }

        public Boolean DownloadXSourLinks(String basePath)
        {
            getLinksfromDB();
            foreach (EFLink link in links)
            {
                downloadFile(link, basePath);
            }


            log.Info("*************************Download results **************************");
            foreach(EFLink link in (from l in links where l.local_filename==null select l))
            {
                log.Debug("Remote file not found for: {0}", link.xsourlink);
            }

            return (from l in links where l.local_filename == null select l).Count() == 0;
        }

        private void downloadFile(EFLink url, string basePath)
        {
            String filename = Path.GetFileName(url.xsourlink);
            String downloadFile = basePath + filename;
            
            using (WebClient client = new WebClient())
            {
                //client.DownloadProgressChanged += Client_DownloadProgressChanged;
                //client.DownloadDataCompleted
                try
                {
                    log.Info("Start download: {0}", url.xsourlink);
                    client.DownloadFile(url.xsourlink, downloadFile);
                    log.Info("Downloaded file {0}", downloadFile);
                    url.local_filename = downloadFile;
                }
                catch(Exception ee)
                {
                    log.Error(ee, "Download error: {0}, {1}", url.xsourlink, ee.Message);
                }
            }
        }
    }
}
