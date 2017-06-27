using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimchaContributions.Data
{
    public class AddToDB
    {
        private string _conStr;
        public AddToDB(string conStr)
        {
            _conStr = conStr;
        }
        public void AddSimcha(Simcha s)
        {
            using (SqlConnection connection = new SqlConnection(_conStr))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Simchas (Name, Date) VALUES (@name, @date)";
                command.Parameters.AddWithValue("@name", s.Name);
                command.Parameters.AddWithValue("@date", s.Date);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public int AddContributor(Contributor c)
        {
            int id = 0;
            using (SqlConnection connection = new SqlConnection(_conStr))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Contributors (FirstName, LastName, CellNumber, DateCreated, AlwaysInclude) "
                    + " VALUES (@firstName, @lastName, @cellNumber, @date, @alwaysInclude)";
                command.Parameters.AddWithValue("@firstName", c.FirstName);
                command.Parameters.AddWithValue("@lastName", c.LastName);
                command.Parameters.AddWithValue("@cellNumber", c.CellNumber);
                command.Parameters.AddWithValue("@date", c.DateCreated);
                command.Parameters.AddWithValue("@alwaysInclude", c.AlwaysInclude);
                connection.Open();
                command.CommandText += " SELECT @@IDENTITY";
                id = (int)(decimal)command.ExecuteScalar();
            }
            return id;
        }
        public void AddDeposit(Deposit d)
        {
            using (SqlConnection connection = new SqlConnection(_conStr))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Deposits (Amount, ContributorId, Date) "
                    + " VALUES (@amount, @contributorId, @date)";
                command.Parameters.AddWithValue("@amount", d.Amount);
                command.Parameters.AddWithValue("@contributorId", d.ContributorId);
                command.Parameters.AddWithValue("@date", d.Date);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public void AddContribution(Contribution c)
        {
            GetFromDB db = new GetFromDB(_conStr);
            Contribution exists = db.GetContributions().Find(contribution => contribution.SimchaId == c.SimchaId
                && contribution.ContributorId == c.ContributorId);
            using (SqlConnection connection = new SqlConnection(_conStr))
            using (SqlCommand command = connection.CreateCommand())
            {
                if (exists != null)
                {
                    command.CommandText = "UPDATE Contributions SET Amount = @amount WHERE SimchaId = @simchaId AND ContributorId = @contributorId";
                }
                else
                {
                    command.CommandText = "INSERT INTO Contributions (Amount, ContributorId, SimchaId) "
                    + " VALUES (@amount, @contributorId, @simchaId)";
                }
                command.Parameters.AddWithValue("@amount", c.Amount);
                command.Parameters.AddWithValue("@contributorId", c.ContributorId);
                command.Parameters.AddWithValue("@simchaId", c.SimchaId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public void DeleteContribution(int contributorId, int simchaId)
        {
            using (SqlConnection connection = new SqlConnection(_conStr))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "DELETE Contributions WHERE ContributorId = @contributorId AND SimchaId = @simchaId";
                command.Parameters.AddWithValue("@contributorId", contributorId);
                command.Parameters.AddWithValue("@simchaId", simchaId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public void EditContributor(Contributor c)
        {
            using (SqlConnection connection = new SqlConnection(_conStr))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE Contributors SET FirstName = @firstName , LastName = @lastName," +
                    " CellNumber = @cellNumber, DateCreated = @date, AlwaysInclude = @alwaysInclude WHERE Id = @id";
                command.Parameters.AddWithValue("@id", c.Id);
                command.Parameters.AddWithValue("@firstName", c.FirstName);
                command.Parameters.AddWithValue("@lastName", c.LastName);
                command.Parameters.AddWithValue("@cellNumber", c.CellNumber);
                command.Parameters.AddWithValue("@date", c.DateCreated);
                command.Parameters.AddWithValue("@alwaysInclude", c.AlwaysInclude);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        
    }
}