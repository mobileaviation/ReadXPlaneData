using ConsoleReadXplaneData;
using ConsoleReadXplaneData.EF;
using FSPService.EF.Models;
using FSPService.Models;
using FSPService.MySql;
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

        private List<Link> links;
        private String countryCode_Filter;
        public List<Link> Links { get { return links; } }
        private Logger log;

        private List<Link> getLinksfromMSDB()
        {
            List<Link> resLinks = new List<Link>();
            List<EFLink> eflinks;
            using (AirNavDB db = new AirNavDB())
            {
                if (countryCode_Filter.Length > 0)
                {
                    eflinks = (from l in db.links
                                where l.enabled == true
                                && l.countrycode==countryCode_Filter
                                select l).ToList();

                }
                else
                {
                    eflinks = (from l in db.links
                                where l.enabled == true
                                select l).ToList();

                }

                var res = from ll in eflinks select new Link()
                {
                    country = ll.country,
                    countrycode = ll.countrycode,
                    openaiplink = ll.openaiplink,
                    xsourlink = ll.xsourlink,
                    enabled = ll.enabled,
                    weblink = ll.weblink,
                    openaip_enabled = ll.openaip_enabled,
                    openaip_id = ll.openaip_id,
                    local_filename = ll.local_filename
                };
                resLinks = res.ToList();
            }
            return resLinks;
        }

        private List<Link> getLinksFromMyDB(String basePath)
        {
            List<Link> resLinks;
            MyDatabase myDB = new MyDatabase(basePath);

            resLinks = myDB.GetLinks();

            return resLinks;
        }

        private List<Link> getLinksfromDB(String basePath, ExportType exportType)
        {
            links = new List<Link>();
            if (exportType==ExportType.MsSql)
            {
                links = getLinksfromMSDB();
            }

            if (exportType==ExportType.MySql)
            {
                links = getLinksFromMyDB(basePath);
            }


            return links;
        }

        public Boolean DownloadXSourLinks(String basePath, ExportType exportType)
        {
            List<Link> links = getLinksfromDB(basePath,exportType);
            foreach (Link l in links)
            {
                downloadFile(l, basePath);
            }


            log.Info("*************************Download results **************************");
            foreach(Link link in (from l in links where l.local_filename==null select l))
            {
                log.Debug("Remote file not found for: {0}", link.xsourlink);
            }

            return (from l in links where l.local_filename == null select l).Count() == 0;
        }

        private void downloadFile(Link url, string basePath)
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
