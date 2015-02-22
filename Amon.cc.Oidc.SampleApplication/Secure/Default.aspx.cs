using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Amon.cc.Oidc.Authentication.Entities;

namespace Amon.cc.Oidc.SampleApplication.Secure
{
	public partial class Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (IsPostBack) { return; }

			user.Text = this.User.Identity.Name;
			UserInformation userInformation = Session["UserInformation"] as UserInformation;
			if (userInformation != null)
			{
				userInformationPanel.Visible = true;
				profilePicture.ImageUrl = userInformation.Picture;
				fullName.Text = userInformation.FullName;
				gender.Text = userInformation.Gender;
			}
		}

		protected void signoff_Click(object sender, EventArgs e)
		{			
			FormsAuthentication.SignOut();
			Session.Abandon();
			Response.Redirect("~/");
		}
	}
}