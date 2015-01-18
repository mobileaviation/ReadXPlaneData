﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConsoleReadXplaneData {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Tables {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Tables() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ConsoleReadXplaneData.Tables", typeof(Tables).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to create index airports_maplocationid_index
        ///            on tbl_Airports(MapLocation_ID);.
        /// </summary>
        internal static string CreateAirportMapLocationIDIndex {
            get {
                return ResourceManager.GetString("CreateAirportMapLocationIDIndex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to create table tbl_Airports (_id integer primary key autoincrement,
        ///            id integer, 
        ///            ident text,
        ///            type text, 
        ///            name text,
        ///            latitude_deg real,
        ///            longitude_deg real,
        ///            elevation_ft real,
        ///            continent text,
        ///            iso_country text,
        ///            iso_region text,
        ///            municipality text,
        ///            scheduled_service text,
        ///            gps_code text,
        ///            iata_code text,
        ///            local_code text,
        ///   [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string CreateAirportTable {
            get {
                return ResourceManager.GetString("CreateAirportTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE &quot;android_metadata&quot; (&quot;locale&quot; TEXT DEFAULT &apos;en_US&apos;).
        /// </summary>
        internal static string CreateAndroidMetadata {
            get {
                return ResourceManager.GetString("CreateAndroidMetadata", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to create table tbl_Continent (_id integer primary key autoincrement,
        ///            id integer,
        ///            name text,
        ///            code text);.
        /// </summary>
        internal static string CreateContinentTable {
            get {
                return ResourceManager.GetString("CreateContinentTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to create table tbl_Country (_id integer primary key autoincrement,
        ///            id integer,
        ///            code text,
        ///            name text,
        ///            continent text,
        ///            wikipedia_link text,
        ///            keywords text);.
        /// </summary>
        internal static string CreateCountryTable {
            get {
                return ResourceManager.GetString("CreateCountryTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to create index fixes_location_index
        ///            on tbl_Fixes(
        ///            latitude_deg,
        ///            longitude_deg);.
        /// </summary>
        internal static string CreateFixesLocationIndex {
            get {
                return ResourceManager.GetString("CreateFixesLocationIndex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to create index fixes_maplocationid_index
        ///            on tbl_Fixes(MapLocation_ID);.
        /// </summary>
        internal static string CreateFixesMapLocationIDIndex {
            get {
                return ResourceManager.GetString("CreateFixesMapLocationIDIndex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to create index navaids_name_index
        ///            on tbl_Fixes(
        ///            name);.
        /// </summary>
        internal static string CreateFixesNameIndex {
            get {
                return ResourceManager.GetString("CreateFixesNameIndex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to create table tbl_Fixes(_id integer primary key autoincrement, name text,
        ///            ident text, latitude_deg real,
        ///            longitude_deg real,
        ///            MapLocation_ID integer,
        ///            pid integer);.
        /// </summary>
        internal static string CreateFixesTable {
            get {
                return ResourceManager.GetString("CreateFixesTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to create index airport_frequencies_airport_ref on
        ///tbl_AirportFrequencies (airport_ref, airport_ident);.
        /// </summary>
        internal static string CreateFrequenciesAirportIdentIndex {
            get {
                return ResourceManager.GetString("CreateFrequenciesAirportIdentIndex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to create table tbl_AirportFrequencies(_id integer primary key autoincrement,
        ///      id integer,
        ///      airport_ref integer,
        ///      airport_ident text,
        ///      type text,
        ///      description text,
        ///      frequency_mhz real);.
        /// </summary>
        internal static string CreateFrequenciesTable {
            get {
                return ResourceManager.GetString("CreateFrequenciesTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to create index location_index
        ///            on tbl_Airports (
        ///            latitude_deg,
        ///            longitude_deg);.
        /// </summary>
        internal static string CreateLocationAirportTableIndex {
            get {
                return ResourceManager.GetString("CreateLocationAirportTableIndex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to create index name_ident_index 
        ///            on tbl_Airports (
        ///            id, 
        ///            name, 
        ///            ident);.
        /// </summary>
        internal static string CreateNameIdentAirportTableIndex {
            get {
                return ResourceManager.GetString("CreateNameIdentAirportTableIndex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to create index navaids_ident_name_index on tbl_Navaids (ident, name);.
        /// </summary>
        internal static string CreateNavaidsIdentNameIndex {
            get {
                return ResourceManager.GetString("CreateNavaidsIdentNameIndex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to create index navaids_location_index on tbl_Navaids(latitude_deg, longitude_deg);.
        /// </summary>
        internal static string CreateNavaidsLocationIndex {
            get {
                return ResourceManager.GetString("CreateNavaidsLocationIndex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to create index navaids_maplocationid_index
        ///            on tbl_Navaids(MapLocation_ID);.
        /// </summary>
        internal static string CreateNavAidsMapLocationIdIndex {
            get {
                return ResourceManager.GetString("CreateNavAidsMapLocationIdIndex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to create table tbl_Navaids (_id integer primary key autoincrement,
        ///id integer,
        ///filename text,
        ///ident text,
        ///name text,
        ///type text,
        ///frequency_khz real,
        ///latitude_deg real,
        ///longitude_deg real,
        ///elevation_ft integer,
        ///iso_country text,
        ///dme_frequency_khz real,
        ///dme_channel real,
        ///dme_latitude_deg real,
        ///dme_longitude_deg real,
        ///dme_elevation_ft integer,
        ///slaved_variation_deg real,
        ///magnetic_variation_deg real,
        ///usageType text,
        ///power text,
        ///associated_airport text,
        ///associated_airport_id integer,
        ///MapLocatio [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string CreateNavaidsTable {
            get {
                return ResourceManager.GetString("CreateNavaidsTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to create table tbl_Region (_id integer primary key autoincrement,
        ///            id integer,
        ///            code text, 
        ///            local_code text, name text,
        ///            continent text, iso_country text,
        ///            wikipedia_link text,
        ///            keywords text);.
        /// </summary>
        internal static string CreateRegionsTable {
            get {
                return ResourceManager.GetString("CreateRegionsTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to create index runway_helocation_index 
        ///            on tbl_Runways (
        ///            he_latitude_deg,
        ///            he_longitude_deg
        ///            );.
        /// </summary>
        internal static string CreateRunwaysAirportHELocationIndex {
            get {
                return ResourceManager.GetString("CreateRunwaysAirportHELocationIndex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to create index runway_airportident_index
        ///            on tbl_Runways  (
        ///            airport_ident, 
        ///            airport_ref
        ///            );.
        /// </summary>
        internal static string CreateRunwaysAirportIdentIndex {
            get {
                return ResourceManager.GetString("CreateRunwaysAirportIdentIndex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to create index runway_lelocation_index 
        ///            on tbl_Runways (
        ///            le_latitude_deg,
        ///            le_longitude_deg
        ///            );.
        /// </summary>
        internal static string CreateRunwaysAirportLELocationIndex {
            get {
                return ResourceManager.GetString("CreateRunwaysAirportLELocationIndex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to create table tbl_Runways (_id integer primary key autoincrement, id integer,
        ///            airport_ref integer,
        ///            airport_ident text,
        ///            length_ft integer,
        ///            width_ft  integer, lighted integer, closed integer,
        ///            surface  text,
        ///            le_ident  text,
        ///            le_latitude_deg  real,
        ///            le_longitude_deg  real,
        ///            le_elevation_ft  integer,
        ///            le_heading_degT real,
        ///            le_displaced_threshold_ft integer,
        ///            he_ide [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string CreateRunwaysTable {
            get {
                return ResourceManager.GetString("CreateRunwaysTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to create table tbl_Properties (_id integer primary key autoincrement,
        ///name text,
        ///value1 text,
        ///value2 text);.
        /// </summary>
        internal static string CreateTableProperties {
            get {
                return ResourceManager.GetString("CreateTableProperties", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to drop index navaids_location_index;.
        /// </summary>
        internal static string DropFixesLocationIndex {
            get {
                return ResourceManager.GetString("DropFixesLocationIndex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to drop index navaids_name_index;.
        /// </summary>
        internal static string DropFixesNameIndex {
            get {
                return ResourceManager.GetString("DropFixesNameIndex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to drop index airport_frequencies_airport_ref;.
        /// </summary>
        internal static string DropFrequenciesAirportIdentIndex {
            get {
                return ResourceManager.GetString("DropFrequenciesAirportIdentIndex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to drop index location_index;.
        /// </summary>
        internal static string DropLocationAirportTableIndex {
            get {
                return ResourceManager.GetString("DropLocationAirportTableIndex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to drop index name_ident_index;.
        /// </summary>
        internal static string DropNameIdentAirportTableIndex {
            get {
                return ResourceManager.GetString("DropNameIdentAirportTableIndex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to drop index navaids_ident_name_index;.
        /// </summary>
        internal static string DropNavaidsIdentNameIndex {
            get {
                return ResourceManager.GetString("DropNavaidsIdentNameIndex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to drop index navaids_location_index;.
        /// </summary>
        internal static string DropNavaidsLocationIndex {
            get {
                return ResourceManager.GetString("DropNavaidsLocationIndex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO &quot;android_metadata&quot; VALUES (&apos;en_US&apos;);.
        /// </summary>
        internal static string InsertAndroidMetadata {
            get {
                return ResourceManager.GetString("InsertAndroidMetadata", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to insert into tbl_Properties(name, value1, value2) values(&quot;DB_VERSION&quot;, &quot;7&quot;, &quot;2014-09-20&quot;);
        ///.
        /// </summary>
        internal static string InsertProperties {
            get {
                return ResourceManager.GetString("InsertProperties", resourceCulture);
            }
        }
    }
}
