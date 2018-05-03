using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleReadXplaneData.Models
{
    public static class Xs
    {
        public static Int32 ToInt(DataRow row, String Field, Int32 Default)
        {
            try
            {
                return Convert.ToInt32((string.IsNullOrEmpty(row[Field].ToString())) ? Default : row[Field]);
            }
            catch(Exception ee)
            {
                return Default;
            }
        }

        public static Double ToDouble(DataRow row, String Field, Double Default)
        {
            try
            {
                return Convert.ToDouble((string.IsNullOrEmpty(row[Field].ToString())) ? Default : row[Field], System.Globalization.CultureInfo.InvariantCulture);
            }
            catch(Exception ee)
            {
                return Default;
            }
        }


    }
}
