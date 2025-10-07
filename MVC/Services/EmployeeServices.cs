using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Repositories;

namespace MVC.Services
{

    public class EmployeeServices
    {
        private readonly NpgsqlConnection _con;
        public EmployeeServices(NpgsqlConnection connection)
        {
            _con = connection;
        }

        public void TestDatabaseConnection()
        {
            try
            {
                _con.Open();

                if (_con.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Database connection successful!");
                else
                    Console.WriteLine("Database connection failed!");

                _con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in connection: " + ex.Message);
            }
        }

        

        public EmployeeModel Login(vm_login user)
        {
            EmployeeModel employees = new EmployeeModel();
            var qry = "SELECT * FROM t_mvc_employees WHERE c_email = @c_email And c_password = @c_password";
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(qry, _con))
                {
                    cmd.Parameters.AddWithValue("c_email", user.Email);
                    cmd.Parameters.AddWithValue("c_password", user.Password);
                    _con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        employees.Eid = Convert.ToInt32(reader["c_eid"]);
                        employees.Name = reader["c_name"].ToString();
                        employees.Email = reader["c_email"].ToString();
                        employees.ProfileImage = reader["c_profileimage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Login Error : " + ex.Message);
            }
            finally
            {
                _con.Close();
            }
            return employees;
        }
    }
}