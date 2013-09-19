using Microsoft.Practices.Unity;
using System.Web;
using Unity.WebForms;
using WebForms.Domain.Interfaces.Repositories;
using WebForms.Domain.Interfaces.Services;
using WebForms.Services;
using WebFormsProject.Data.Repositories;

[assembly: WebActivator.PostApplicationStartMethod(typeof(WebFormsProject.Website.App_Start.UnityWebFormsStart), "PostStart")]
namespace WebFormsProject.Website.App_Start
{
    /// <summary>
    ///		Startup class for the Unity.WebForms NuGet package.
    /// </summary>
    internal static class UnityWebFormsStart
    {
        /// <summary>
        ///     Initializes the unity container when the application starts up.
        /// </summary>
        /// <remarks>
        ///		Do not edit this method. Perform any modifications in the
        ///		<see cref="RegisterDependencies" /> method.
        /// </remarks>
        internal static void PostStart()
        {
            IUnityContainer container = new UnityContainer();
            HttpContext.Current.Application.SetContainer(container);

            RegisterDependencies(container);
        }

        /// <summary>
        ///		Registers dependencies in the supplied container.
        /// </summary>
        /// <param name="container">Instance of the container to populate.</param>
        private static void RegisterDependencies(IUnityContainer container)
        {
            container.BindInRequestScope<IUserService, UserService>();

            container.BindInRequestScope<IUserRepository, UserRepository>();
        }
    }

    /// <summary>
    /// Bind the given interface in request scope
    /// </summary>
    public static class IOCExtensions
    {
        public static void BindInRequestScope<T1, T2>(this IUnityContainer container) where T2 : T1
        {
            container.RegisterType<T1, T2>(new HierarchicalLifetimeManager());
        }

        public static void BindInSingletonScope<T1, T2>(this IUnityContainer container) where T2 : T1
        {
            container.RegisterType<T1, T2>(new ContainerControlledLifetimeManager());
        }
    }
}