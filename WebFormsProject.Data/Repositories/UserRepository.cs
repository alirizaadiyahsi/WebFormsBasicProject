using System;
using System.Data;
using System.Data.SqlClient;
using WebForms.Domain.DomainModel.Entities;
using WebForms.Domain.Interfaces.Repositories;

namespace WebFormsProject.Data.Repositories
{
    public class UserRepository : BaseDataAccess, IUserRepository
    {
        /// <summary>
        /// Tüm kullanıcılar
        /// </summary>
        /// <returns></returns>
        public DataTable GetAll()
        {
            SqlCommand command = GetCommand("SELECT * FROM Users");
            DataTable dt = GetTable(command);

            return dt;
        }

        /// <summary>
        /// Role göre kullanıcılar
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public DataTable GetAllByRoleName(string roleName)
        {
            SqlCommand command = GetCommand("SELECT * FROM Users U, UserInRoles UR, Roles R WHERE (UR.RoleId = R.Id AND UR.UserId = U.Id AND R.Name = @roleName)");
            command.Parameters.AddWithValue("@roleName", roleName);
            DataTable dt = GetTable(command);

            return dt;
        }

        /// <summary>
        /// Kullanıcı bul
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Users Find(int userId)
        {
            Users user = null;
            SqlCommand command = GetCommand("SELECT TOP 1 * FROM Users WHERE Id = @UserId");
            command.Parameters.AddWithValue("@UserId", userId);
            DataRow dr = GetFirstRow(command);

            if (dr != null)
            {
                user.Id = dr.Get<int>("Id");
                user.UserName = dr.GetString("UserName");
                user.DisplayName = dr.GetString("DisplayName");
                user.Email = dr.GetString("Email");
                user.LastLoginDate = dr.GetNullable<DateTime?>("LastLoginDate");
                user.LastLoginIP = dr.GetString("LastLoginIP");
                user.Password = dr.GetString("Password");
            }

            return user;
        }

        /// <summary>
        /// Kullanıcı ekle
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Eklenen kullanıcı</returns>
        public Users Insert(Users user)
        {
            SqlCommand command = GetCommand("INSERT INTO Users (UserName, DisplayName, Email, LastLoginDate, LastLoginIP, Password) VALUES (@UserName, @DisplayName, @Email, @LastLoginDate, @LastLoginIP, @Password); SELECT SCOPE_IDENTITY()");
            command.Parameters.AddWithValue("@UserName", user.UserName, null);
            command.Parameters.AddWithValue("@DisplayName", user.DisplayName, null);
            command.Parameters.AddWithValue("@Email", user.Email, null);
            command.Parameters.AddWithValue("@LastLoginDate", user.LastLoginDate, null);
            command.Parameters.AddWithValue("@LastLoginIP", user.LastLoginIP, null);
            command.Parameters.AddWithValue("@Password", user.Password, null);

            user.Id = SafeExecuteScalar(command);

            return user;
        }

        /// <summary>
        /// Kullanıcı güncelle
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Etkilenen satır sayısı</returns>
        public int Update(Users user)
        {
            SqlCommand command = GetCommand("UPDATE Users SET UserName=ISNULL(@UserName,UserName), DisplayName=ISNULL(@DisplayName,DisplayName), Email=ISNULL(@Email,Email), LastLoginDate=ISNULL(@LastLoginDate,LastLoginDate), LastLoginIP=ISNULL(@LastLoginIP=LastLoginIP), Password=ISNULL(@Password,Password) WHERE Id=@UserId");
            command.Parameters.AddWithValue("@UserId", user.Id);
            command.Parameters.AddWithValue("@UserName", user.UserName, null);
            command.Parameters.AddWithValue("@DisplayName", user.DisplayName, null);
            command.Parameters.AddWithValue("@Email", user.Email, null);
            command.Parameters.AddWithValue("@LastLoginDate", user.LastLoginDate, null);
            command.Parameters.AddWithValue("@LastLoginIP", user.LastLoginIP, null);
            command.Parameters.AddWithValue("@Password", user.Password, null);

            return SafeExecuteNonQuery(command);
        }

        /// <summary>
        /// Kullanıcı sil
        /// </summary>
        /// <param name="user">Kullanıcı</param>
        /// <returns>Etkilenen satır sayısı</returns>
        public int Delete(Users user)
        {
            SqlCommand command = GetCommand("DELETE FROM Users WHERE Id = @UserId");
            command.Parameters.AddWithValue("@UserId", user.Id);
            return SafeExecuteNonQuery(command);
        }

        /// <summary>
        /// Kullanıcı sil
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Etkilenen satır sayısı</returns>
        public int Delete(int userId)
        {
            SqlCommand command = GetCommand("DELETE FROM Users WHERE Id = @UserId");
            command.Parameters.AddWithValue("@UserId", userId);
            return SafeExecuteNonQuery(command);
        }
    }
}
