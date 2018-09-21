namespace ConsoleReadXplaneData.EF
{
    using ConsoleReadXplaneData.EF.Models;
    using ConsoleReadXplaneData.Models;
    using FSPService.EF.Models;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;

    public class AirNavDB : DbContext
    {
        // Your context has been configured to use a 'Airport' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'ConsoleReadXplaneData.EF.Airport' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Airport' 
        // connection string in the application configuration file.
        public AirNavDB()
            : base("name=AirNavDB")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<EFAirport> airports { get; set; }
        public virtual DbSet<EFRunway> runways { get; set; }
        public virtual DbSet<EFFrequency> frequencies { get; set; }
        public virtual DbSet<EFNavaid> navaids { get; set; }
        public virtual DbSet<EFFix> fixes { get; set; }
        public virtual DbSet<EFCountry> countries { get; set; }
        public virtual DbSet<EFRegion> regions { get; set; }
        public virtual DbSet<EFTile> tiles { get; set; }
        public virtual DbSet<EFFir> firs { get; set; }
        public virtual DbSet<EFAirspace> airspaces { get; set; }
        public virtual DbSet<EFActivePeriod> activePeriods { get; set; }
        public virtual DbSet<EFActiveDay> activeDays { get; set; }
        public virtual DbSet<EFATCStation> atcStations { get; set; }
        public virtual DbSet<EFLink> links { get; set; }

    }
}