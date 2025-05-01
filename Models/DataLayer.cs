using Microsoft.Data.SqlClient; // Updated namespace
using KendoCrudMvc.Models;

namespace KendoCrudMvc.Models
{
    public class Datalayer
    {
        private readonly string _connectionString;

        public Datalayer(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("mycon");
        }

        public void ExecuteQuery(string sql, params SqlParameter[] parameters)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddRange(parameters);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<UserModel> GetUsers()
        {
            var userList = new List<UserModel>();
            string sql = "SELECT * FROM tbluser";

            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new UserModel
                        {
                            UserId = reader.GetInt32(reader.GetOrdinal("userid")),
                            Email = reader.GetString(reader.GetOrdinal("email")),
                            Mobile = reader.GetString(reader.GetOrdinal("mobile")),
                            Password = reader.GetString(reader.GetOrdinal("password")),
                            DateOfBirth = reader.GetDateTime(reader.GetOrdinal("dob"))
                        };
                        // Log the user data to the console
                        Console.WriteLine($"UserId: {user.UserId}, Email: {user.Email}, Mobile: {user.Mobile}, DateOfBirth: {user.DateOfBirth:dd-MM-yyyy}");
                        userList.Add(user);
                    }
                }
            }

            return userList;
        }
    }
}
