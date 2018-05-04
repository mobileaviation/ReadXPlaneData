﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleReadXplaneData.Models
{
    public class Country
    {
        public Int32 index;
        public Int32 id;
        public String code;
        public String name;
        public String continent;
        public String wikipedia_link;
        public String keywords;
    }

    public static class CountryFactory
    {
        public static Country GetCountryFromDatatable(DataRow countryData, int Index)
        {
            try
            {
                Country a = new Country();

                a.index = Index;
                a.id = Xs.ToInt(countryData, "id", 0);
                a.code = countryData["code"].ToString();
                a.name = countryData["name"].ToString();
                a.continent = countryData["continent"].ToString();
                a.wikipedia_link = countryData["wikipedia_link"].ToString();
                a.keywords = countryData["keywords"].ToString();

                return a;
            }
            catch (Exception ee)
            {
                return null;
            }
        }
        public static String GetJsonCountryFromDatatable(DataRow data, Int32 index)
        {
            Country country = GetCountryFromDatatable(data, index);
            return JsonConvert.SerializeObject(country, Formatting.Indented);
        }

        public static String GetJsonCountryFromDatatable(Country country)
        {
            return JsonConvert.SerializeObject(country, Formatting.Indented);
        }
    }
}