using System;
using System.Collections.Generic;

namespace Contracts
{
    /// <summary>
    /// DatabaseEntry is a class that will be serialized into the database and deserialized out of the database. 
    /// It contains all the fields specified in the requirements. 
    /// </summary>

    [Serializable]
    public class DatabaseEntry
    {
        private int id;
        private string region;
        private string city;
        private string year;
        private double[] consumption = new double[12]; // Consumption for each month

        public DatabaseEntry()
        {
        }

        public DatabaseEntry(int id, string region, string city, string year, double[] consumption)
        {
            this.Id = id;
            this.Region = region;
            this.City = city;
            this.Year = year;
            this.Consumption = consumption;
        }

        public int Id { get => id; set => id = value; }
        public string Region { get => region; set => region = value; }
        public string City { get => city; set => city = value; }
        public string Year { get => year; set => year = value; }
        public double[] Consumption { get => consumption; set => consumption = value; }

        public override bool Equals(object obj)
        {
            var entry = obj as DatabaseEntry;
            return entry != null &&
                   Id == entry.Id &&
                   Region == entry.Region &&
                   City == entry.City &&
                   Year == entry.Year &&
                   EqualityComparer<double[]>.Default.Equals(Consumption, entry.Consumption);
        }
        
        public override int GetHashCode()
        {
            var hashCode = 429245927;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Region);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(City);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Year);
            hashCode = hashCode * -1521134295 + EqualityComparer<double[]>.Default.GetHashCode(Consumption);
            return hashCode;
        }

        public double GetYearlyConsumption()
        {
            double sum = 0;
            for(int i=0; i<12; i++)
            {
                sum += Consumption[i];
            }
            return sum;
        }
    }
}
