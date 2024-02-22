using System;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace AutoDeclaratif
{
    public class DateHoursDb
    {
        private string _dbConnectionString;

        public DateHoursDb(string connectionString)
        {
            _dbConnectionString = connectionString;

            var connection = new SqliteConnection(_dbConnectionString);

            var table = connection.Query<string>("SELECT name FROM sqlite_master WHERE type='table' AND name = 'DateHours';");
            var tableName = table.FirstOrDefault();
            if (!string.IsNullOrEmpty(tableName) && tableName == "DateHours")
                return;

            connection.Execute("Create Table DateHours (" +
                "Date VARCHAR(100) NOT NULL," +
                "Arrival VARCHAR(100) NULL," +
                "Break VARCHAR(100) NULL," +
                "Departure VARCHAR(1000) NULL);");

            connection.Close();
        }

        public int DeleteTable()
        {
            var connection = new SqliteConnection(_dbConnectionString);

            return connection.Execute("DELETE FROM DateHours;");
        }

        /// <summary>
        /// Insert a new DateHours
        /// </summary>
        /// <param name="dateHours"></param>
        /// <returns></returns>
        public int Create(DateHours dateHours)
        {
            var connection = new SqliteConnection(_dbConnectionString);

            return connection.Execute("INSERT INTO DateHours (Date, Arrival, Break, Departure)" +
                "VALUES (@Date, @Arrival, @Break, @Departure);", dateHours);
        }

        /// <summary>
        /// Update or insert a DateHours
        /// </summary>
        /// <param name="dateHours"></param>
        /// <returns></returns>
        public int UpdateOrInsert(DateHours dateHours)
        {
            if (Get(dateHours.Date).Any())
            {
                return Update(dateHours);
            }
            else
            {
                return Create(dateHours);
            }
        }

        /// <summary>
        /// Update a DateHours
        /// </summary>
        /// <param name="dateHours"></param>
        /// <returns></returns>
        public int Update(DateHours dateHours)
        {
            var connection = new SqliteConnection(_dbConnectionString);

            return connection.Execute("UPDATE DateHours" +
                " SET Arrival = @Arrival, Break = @Break, Departure = @Departure" +
                " WHERE Date = @Date;", dateHours);
        }

        /// <summary>
        /// Get DateHours for a date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public IEnumerable<DateHours> Get(DateTime date)
        {
            var connection = new SqliteConnection(_dbConnectionString);

            return connection.Query<DateHours>("SELECT rowid AS Id, Date, Arrival, Break, Departure" +
                " FROM DateHours" +
                " WHERE Date = @Date;", date);
        }

        /// <summary>
        /// Get All DateHours
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DateHours> GetAll()
        {
            var connection = new SqliteConnection(_dbConnectionString);

            return connection.Query<DateHours>("SELECT rowid AS Id, Date, Arrival, Break, Departure FROM DateHours;");
        }
    }

    public class DateHours
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Arrival { get; set; }
        public string Break { get; set; }
        public string Departure { get; set; }
    }
}