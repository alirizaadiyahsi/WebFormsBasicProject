using System.Data;
using WebForms.Domain.DomainModel.Entities;

namespace WebForms.Domain.Interfaces.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Tüm kullanıcılar
        /// </summary>
        /// <returns></returns>
        DataTable GetAll();

        /// <summary>
        /// Role göre kullanıcılar
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        DataTable GetAllByRoleName(string roleName);

        /// <summary>
        /// Kullanıcı bul
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Users Find(int userId);

        /// <summary>
        /// Kullanıcı ekle
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Eklenen kullanıcı</returns>
        Users Insert(Users user);

        /// <summary>
        /// Kullanıcı güncelle
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Etkilenen satır sayısı</returns>
        int Update(Users user);

        /// <summary>
        /// Kullanıcı sil
        /// </summary>
        /// <param name="user">Kullanıcı</param>
        /// <returns>Etkilenen satır sayısı</returns>
        int Delete(Users user);

        /// <summary>
        /// Kullanıcı sil
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Etkilenen satır sayısı</returns>
        int Delete(int userId);
    }
}
