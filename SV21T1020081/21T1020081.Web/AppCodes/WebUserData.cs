using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace _21T1020081.Web
{
    /// <summary>
    /// lưu giữ thoong tin người dùng
    /// </summary>
    public class WebUserData
    {
        public string? UserId { get; set; } = "";

        public string? UserName { get; set; } = "";
        public string? DisplayName { get; set; } = "";
        public string? Photo { get; set; } = "";
        public List<string> Roles { get; set; }
        private List<Claim> Claims
            {
            get 
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(nameof(UserId), UserId),
                    new Claim(nameof(UserName), UserName),
                    new Claim(nameof(DisplayName), DisplayName),
                    new Claim(nameof(Photo), Photo)
                };
                if (Roles != null ) 
                    foreach (var role in Roles)
                        claims.Add(new Claim(ClaimTypes.Role, role));
                return claims;
            }
        }
        public ClaimsPrincipal CreatePrincipal()
        {
            var indentity = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(indentity);
            return principal;
        }

    }
}
