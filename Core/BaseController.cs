using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication2.Models;

namespace WebApplication2.Core
{
    public abstract class BaseController : ControllerBase
    {
        private string? _userId;
        private string? _name;
        private string? _email;

        protected string UserId
        {
            get
            {
                _userId ??= User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                return _userId;
            }
        }

        protected string Name
        {
            get
            {
                _name ??= User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                return _name;
            }
        }

        protected string Email
        {
            get
            {
                _email ??= User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                return _email;
            }
        }
    }
}
