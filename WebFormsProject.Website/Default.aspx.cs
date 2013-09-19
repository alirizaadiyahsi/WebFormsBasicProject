using Microsoft.Practices.Unity;
using System;
using WebForms.Domain.Interfaces.Services;

namespace WebFormsProject.Website
{
    public partial class Default : System.Web.UI.Page
    {
        [Dependency]
        public IUserService _userService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}