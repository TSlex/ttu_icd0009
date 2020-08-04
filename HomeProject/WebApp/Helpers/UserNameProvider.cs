using ee.itcollege.aleksi.Contracts.DAL.Base;
using Microsoft.AspNetCore.Http;

namespace WebApp.Helpers
{
    /// <inheritdoc />
    public class UserNameProvider: IUserNameProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <inheritdoc />
        public UserNameProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Return current user username
        /// </summary>
        public string CurrentUserName => _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "-";
    }
}