using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Middleware
{
    public interface ICustomHttpContextAccessor : IHttpContextAccessor
    {
        string GetHeaders();

        string GetHeaderValue(string key);

        int? GetUserId();

        int[] GetUserRoleId();

        string GetRemoteIpAddress();
    }

    public class CustomHttpContextAccessor : HttpContextAccessor, ICustomHttpContextAccessor
    {
        public string GetHeaderValue(string key)
        {
            if (HttpContext != null)
            {
                return HttpContext.Request.Headers.TryGetValue(key, out var names) ? names.FirstOrDefault() : null;
            }

            return null;
        }

        public int? GetUserId() => int.TryParse(HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "userId")?.Value, out int id) ? id : (int?)null;

        public int[] GetUserRoleId()
        {
            var roles = HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "userRoleId")?.Value;
            if (roles != null)
            {
                List<int> allRoleIds = new List<int>();
                foreach (string role in roles.Split(','))
                {
                    allRoleIds.Add(int.Parse(role));
                }

                return allRoleIds.ToArray();
            }
            else
            {
                return new int[] { };
            }
        }

        public string GetHeaders()
        {
            string headers = null;
            foreach (var key in HttpContext.Request.Headers.Keys)
                headers += $"{key} = {HttpContext.Request.Headers[key]}{Environment.NewLine}";
            return headers;
        }

        public string GetRemoteIpAddress() => HttpContext?.Connection?.RemoteIpAddress?.ToString();
    }
}
