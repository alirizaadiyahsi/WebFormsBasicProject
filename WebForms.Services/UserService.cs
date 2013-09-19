using System.Data;
using WebForms.Domain.DomainModel.Entities;
using WebForms.Domain.Interfaces.Repositories;
using WebForms.Domain.Interfaces.Services;

namespace WebForms.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Tüm kullanıcılar
        /// </summary>
        /// <returns></returns>
        public DataTable GetAll()
        {
            return _userRepository.GetAll();
        }

        /// <summary>
        /// Role göre kullanıcılar
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public DataTable GetAllByRoleName(string roleName)
        {
            return _userRepository.GetAllByRoleName(roleName);
        }

        /// <summary>
        /// Kullanıcı bul
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Users Find(int userId)
        {
            return _userRepository.Find(userId);
        }

        /// <summary>
        /// Kullanıcı ekle
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Eklenen kullanıcı</returns>
        public Users Insert(Users user)
        {
            return _userRepository.Insert(user);
        }

        /// <summary>
        /// Kullanıcı güncelle
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Etkilenen satır sayısı</returns>
        public int Update(Users user)
        {
            return _userRepository.Update(user);
        }

        /// <summary>
        /// Kullanıcı sil
        /// </summary>
        /// <param name="user">Kullanıcı</param>
        /// <returns>Etkilenen satır sayısı</returns>
        public int Delete(Users user)
        {
            return _userRepository.Delete(user);
        }

        /// <summary>
        /// Kullanıcı sil
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <returns>Etkilenen satır sayısı</returns>
        public int Delete(int userId)
        {
            return _userRepository.Delete(userId);
        }
    }
}
