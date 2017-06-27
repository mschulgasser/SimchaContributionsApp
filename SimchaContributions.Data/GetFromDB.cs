using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimchaContributions.Data
{
    public class GetFromDB
    {
        private string _conStr;
        public GetFromDB(string conStr)
        {
            _conStr = conStr;
        }
        public List<Contributor> GetContributors()
        {
            List<Contributor> result = new List<Contributor>();
            using (SqlConnection connection = new SqlConnection(_conStr))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Contributors";
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Contributor c = new Contributor();
                    c.Id = (int)reader["Id"];
                    c.FirstName = (string)reader["FirstName"];
                    c.LastName = (string)reader["LastName"];
                    c.CellNumber = (string)reader["CellNumber"];
                    c.DateCreated = (DateTime)reader["DateCreated"];
                    c.AlwaysInclude = (bool)reader["AlwaysInclude"];
                    result.Add(c);
                }
            }
            return result;
        }
        //public List<Contributor> GetContributors(string searchTerm)
        //{
        //    List<Contributor> result = new List<Contributor>();
        //    using (SqlConnection connection = new SqlConnection(_conStr))
        //    using (SqlCommand command = connection.CreateCommand())
        //    {
        //        command.CommandText = "SELECT * FROM Contributors WHERE FirstName LIKE @searchTerm OR LastName LIKE @searchTerm";
        //        command.Parameters.AddWithValue("@searchTerm", $"'%{searchTerm}%'");
        //        connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            Contributor c = new Contributor();
        //            c.Id = (int)reader["Id"];
        //            c.FirstName = (string)reader["FirstName"];
        //            c.LastName = (string)reader["LastName"];
        //            c.CellNumber = (string)reader["CellNumber"];
        //            c.DateCreated = (DateTime)reader["DateCreated"];
        //            c.AlwaysInclude = (bool)reader["AlwaysInclude"];
        //            result.Add(c);
        //        }
        //    }
        //    return result;
        //}
        //private List<Deposit> GetDeposits(int contributorId)
        //{
        //    List<Deposit> result = new List<Deposit>();
        //    using (SqlConnection connection = new SqlConnection(_conStr))
        //    using (SqlCommand command = connection.CreateCommand())
        //    {
        //        command.CommandText = "SELECT * FROM Deposits WHERE ContributorId = @id";
        //        command.Parameters.AddWithValue("@id", contributorId);
        //        connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            Deposit d = new Deposit();
        //            d.Amount = (decimal)reader["Amount"];
        //            d.Id = (int)reader["Id"];
        //            d.ContributorId = (int)reader["ContributorId"];
        //            result.Add(d);
        //        }
        //        return result;
        //    }
        //}
        public  List<Contribution> GetContributions()
        {
            List<Contribution> result = new List<Contribution>();
            using (SqlConnection connection = new SqlConnection(_conStr))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Contributions";
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Contribution c = new Contribution();
                    c.Amount = (decimal)reader["Amount"];
                    c.SimchaId = (int)reader["SimchaId"];
                    c.ContributorId = (int)reader["ContributorId"];
                    result.Add(c);
                }
                return result;
            }
        }
        public decimal GetBalance(int contributorId)
        {
            decimal balance = 0;
            List<Transaction> transactions = GetTransactions(contributorId);
            foreach(Transaction t in transactions)
            {
                balance += t.Amount;
            }
            return balance;
        }
        public Contributor GetContributor(int id)
        {
            Contributor c = new Contributor();
            using (SqlConnection connection = new SqlConnection(_conStr))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Contributors WHERE Id = @id";
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                c.Id = (int)reader["Id"];
                c.FirstName = (string)reader["FirstName"];
                c.LastName = (string)reader["LastName"];
                c.CellNumber = (string)reader["CellNumber"];
                c.DateCreated = (DateTime)reader["DateCreated"];
                c.AlwaysInclude = (bool)reader["AlwaysInclude"];
            }
            return c;
        }
        public List<Transaction> GetTransactions(int id)
        {
            List<Transaction> result = new List<Transaction>();
            using (SqlConnection connection = new SqlConnection(_conStr))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Contributions c JOIN Simchas s " +
                    "ON c.SimchaId = s.Id WHERE c.ContributorId = @id";
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new Transaction()
                    {
                        Description = $"Contribution to the {(string)reader["Name"]} Simcha",
                        Amount = -((decimal)reader["Amount"]),
                        Date = (DateTime)reader["Date"]
                    });
                }
            }
            using (SqlConnection connection = new SqlConnection(_conStr))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Deposits WHERE ContributorId = @id";
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new Transaction()
                    {
                        Description = $"Deposit",
                        Amount = (decimal)reader["Amount"],
                        Date = (DateTime)reader["Date"]
                    });
                }
            }
            result = result.OrderBy(t => t.Date).ToList();
            return result;
        }
        public List<Simcha> GetSimchas()
        {
            List<Simcha> result = new List<Simcha>();
            using (SqlConnection connection = new SqlConnection(_conStr))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Simchas";
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new Simcha()
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"],
                        Date = (DateTime)reader["Date"]
                    });
                }
            }
            return result;
        }
        public decimal GetTotal(int simchaId)
        {
            decimal total = 0;
            using (SqlConnection connection = new SqlConnection(_conStr))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT ISNULL(SUM(c.Amount), 0) FROM Contributions c WHERE c.SimchaId = @simchaId";
                command.Parameters.AddWithValue("@simchaId", simchaId);
                connection.Open();
                total = (decimal)command.ExecuteScalar();
            }
            return total;
        }
        public int GetContributorCount(int simchaId)
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(_conStr))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(c.ContributorId) FROM Contributions c WHERE c.SimchaId = @simchaId";
                command.Parameters.AddWithValue("@simchaId", simchaId);
                connection.Open();
                count = (int)command.ExecuteScalar();
            }
            return count;
        }
        public double GetContributorContribution(int contributorId, int simchaId)
        {
            double contribution = 0;
            using (SqlConnection connection = new SqlConnection(_conStr))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Contributions" +
                    " WHERE SimchaId = @simchaId AND ContributorId = @contributorId";
                command.Parameters.AddWithValue("@simchaId", simchaId);
                command.Parameters.AddWithValue("@contributorId", contributorId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                bool hasData = reader.Read();
                if (hasData)
                {
                    contribution = (double)(decimal)reader["Amount"];
                }
            }
            return contribution;
        }
        public Simcha GetSimcha(int id)
        {
            Simcha simcha = new Simcha();
            using (SqlConnection connection = new SqlConnection(_conStr))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Simchas WHERE Id = @id";
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                simcha.Id = (int)reader["Id"];
                simcha.Date = (DateTime)reader["Date"];
                simcha.Name = (string)reader["Name"];
            }
            return simcha;
        }
        public int GetTotalContributors()
        {
            int total = 0;
            using (SqlConnection connection = new SqlConnection(_conStr))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM Contributors";
                connection.Open();
                total = (int)command.ExecuteScalar();
            }
            return total;
        }
        public decimal GetTotalMoney()
        {
            decimal total = 0;
            using (SqlConnection connection = new SqlConnection(_conStr))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT SUM(Amount) FROM Deposits";
                connection.Open();
                total = (int)(decimal)command.ExecuteScalar();
            }
            using (SqlConnection connection = new SqlConnection(_conStr))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT SUM(Amount) FROM Contributions";
                connection.Open();
                total -= (int)(decimal)command.ExecuteScalar();
            }
            return total;
        }
    }
}
 