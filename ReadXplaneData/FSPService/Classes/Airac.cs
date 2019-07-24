using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSPService.Classes
{
    public class Airac
    {
        public Airac()
        {
            epoch = DateTime.Parse("10/1/1901");
        }

        private DateTime epoch;

        public String getCurrentAirac()
        {
            DateTime curr = DateTime.Now;
            String c = _getCycleForDate(curr);
            return c;
        }

        public String getAiracForDate(DateTime date)
        {
            String c = _getCycleForDate(date);
            return c;
        }
             

        private String _getCycleForDate(DateTime date)
        {
            TimeSpan diff = date.Subtract(epoch);
            double airacC = diff.TotalDays / 28;
            double nAiracC = Math.Round(airacC + 1) * 28;

            TimeSpan ndiff = TimeSpan.FromDays(nAiracC);
            DateTime nAirac = epoch.Add(ndiff);

            TimeSpan nndiff = nAirac.Subtract(DateTime.Parse("1/1/" + date.Year.ToString()));
            double cycles = Math.Round(nndiff.TotalDays / 28);


            String cycle = date.Year.ToString().Substring(2) + String.Format("{0:00}", cycles);
            return cycle;
        }
    }
}
