using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebForms.Domain.DomainModel.Entities;
using WebForms.Domain.Interfaces.Repositories;
using WebForms.Domain.Interfaces.Services;
using WebForms.Services;
using WebFormsProject.Data.Repositories;

namespace WebFormsProject.Test.Services
{
    [TestClass]
    public class UnitTestUserService
    {
        private IUnityContainer _unityContainer;
        private IUserService _userService;
        private IUserRepository _userRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _unityContainer = new UnityContainer();
            _unityContainer.RegisterType<IUserService, UserService>();
            _unityContainer.RegisterType<IUserRepository, UserRepository>();

            _userService = _unityContainer.Resolve<UserService>();
            _userRepository = _unityContainer.Resolve<UserRepository>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _userService = null;
            _userRepository = null;
            _unityContainer = null;
        }


        [TestMethod]
        public void TestMethodAddUser()
        {
            var newUser = _userService.Insert(new Users { UserName = "test_user" });
            var insertedUser = _userService.Find(newUser.Id);

            // newUser eklenirken null değerler için, default değerler 
            // girilebileceğinden, insertedUser ile birebir uyuşmayacaktır
            // bunun için sadece tanımladığımız özelliği karşılaştırıyoruz
            Assert.AreEqual(insertedUser.UserName, newUser.UserName);
            Assert.AreEqual(1, _userService.Delete(newUser));
        }

        [TestMethod]
        public void TestMethodUpdateUser()
        {
            var newUser = _userService.Insert(new Users { UserName = "test_user", DisplayName = "display name", Email = "test_email@mail.com", Password = "1234" });
            newUser.UserName = "updated_test_user";
            _userService.Update(newUser);

            Assert.AreEqual(newUser.UserName, "updated_test_user");
            Assert.AreEqual(1, _userService.Delete(newUser));
        }

        [TestMethod]
        public void TestMethodDeleteUser()
        {
            var newUser = _userService.Insert(new Users { UserName = "test_user", DisplayName = "display name", Email = "test_email@mail.com", Password = "1234" });
            var affectedRow = _userService.Delete(newUser);
            var deletedUser = _userService.Find(newUser.Id);

            Assert.AreEqual(deletedUser, null);
            Assert.AreEqual(1, affectedRow);
        }
    }
}
