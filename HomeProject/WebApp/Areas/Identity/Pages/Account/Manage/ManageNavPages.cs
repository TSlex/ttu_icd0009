using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

#pragma warning disable 1591
namespace WebApp.Areas.Identity.Pages.Account.Manage
{
    public static class ManageNavPages
    {
        public static string Avatar => "Avatar";
        
        public static string Index => "Index";

        public static string Email => "Email";

        public static string ChangePassword => "ChangePassword";

        public static string ExternalLogins => "ExternalLogins";

        public static string PersonalData => "PersonalData";

        public static string TwoFactorAuthentication => "TwoFactorAuthentication";
        
        public static string PrivacyNSecurity => "PrivacyNSecurity";

        public static string AvatarNavClass(ViewContext viewContext) => PageNavClass(viewContext, Avatar);
        
        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string EmailNavClass(ViewContext viewContext) => PageNavClass(viewContext, Email);

        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        public static string ExternalLoginsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ExternalLogins);

        public static string PersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, PersonalData);

        public static string TwoFactorAuthenticationNavClass(ViewContext viewContext) => PageNavClass(viewContext, TwoFactorAuthentication);
        
        public static string PrivacyNSecurityNavClass(ViewContext viewContext) => PageNavClass(viewContext, PrivacyNSecurity);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : "";
        }
    }
}
