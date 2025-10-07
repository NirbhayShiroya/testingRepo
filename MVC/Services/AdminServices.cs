using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Repositories;

namespace MVC.Services
{
    public class AdminServices
    {
        private readonly NpgsqlConnection _con;

        public AdminServices(NpgsqlConnection connection)
        {
            _con = connection;
        }

        public List<EmployeeModel> FetchAllEmployee()
        {
            var emp = new List<EmployeeModel>();
            var qry = "SELECT * FROM t_mvc_employees";
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(qry, _con))
                {
                    _con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        emp.Add(new EmployeeModel
                        {
                            Eid = Convert.ToInt32(reader["c_eid"]),
                            Name = reader["c_name"].ToString(),
                            Email = reader["c_email"].ToString(),
                            Password = reader["c_password"].ToString(),
                            ProfileImage = reader["c_profileimage"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in employee fetch: " + ex.Message);
            }
            finally
            {
                _con.Close();
            }
            return emp;
        }

        public int AddEmployee(EmployeeModel employee)
        {
            try
            {
                var cmd = new NpgsqlCommand(@"SELECT * FROM t_mvc_employees WHERE c_email = @email", _con);
                cmd.Parameters.AddWithValue("@email", employee.Email);

                _con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        _con.Close();
                        return 0;
                    }
                    else
                    {
                        _con.Close();
                        _con.Open();
                        NpgsqlCommand com = new NpgsqlCommand(@"INSERT INTO t_mvc_employees (c_name, c_email, c_password, c_profileimage) VALUES (@c_name, @c_email, @c_password, @c_profileimage)", _con);
                        com.Parameters.AddWithValue("@c_name", employee.Name);
                        com.Parameters.AddWithValue("@c_email", employee.Email);
                        com.Parameters.AddWithValue("@c_password", employee.Password);
                        com.Parameters.AddWithValue("@c_profileimage", (object?)employee.ProfileImage ?? DBNull.Value);
                        com.ExecuteNonQuery();
                        _con.Close();
                        return 1;
                    }
                }

            }
            catch (Exception ex)
            {
                _con.Close();
                Console.WriteLine("Register Failed, Error :-" + ex.Message);
                return -1;
            }
        }

        public void UpdateEmployeeDetails(EmployeeModel employee)
        {
            if (employee.ProfileImageFile != null && employee.ProfileImageFile.Length > 0)
            {
                string directoryPath = Path.Combine("..", "MVC", "wwwroot", "profile_images");
                var filename = employee.Email + Path.GetExtension(employee.ProfileImageFile.FileName);
                var filepath = Path.Combine(directoryPath, filename);
                Directory.CreateDirectory(directoryPath);

                employee.ProfileImage = filename;
            }

            _con.Open();
            var sql = @"UPDATE t_mvc_employees SET 
                    c_name = @c_name,
                    c_email = @c_email,
                    c_password = @c_password,
                    c_profileimage = @c_profileimage
                WHERE c_eid = @id";
            using var cmd = new NpgsqlCommand(sql, _con);
            cmd.Parameters.AddWithValue("@c_name", employee.Name);
            cmd.Parameters.AddWithValue("@c_email", employee.Email);
            cmd.Parameters.AddWithValue("@c_password", employee.Password);
            cmd.Parameters.AddWithValue("@c_profileimage", (object?)employee.ProfileImage ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@id", employee.Eid);
            cmd.ExecuteNonQuery();
            _con.Close();
        }

        

    }
}