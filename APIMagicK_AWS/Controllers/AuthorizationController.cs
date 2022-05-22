using APIMagicK_AWS.Helpers;
using APIMagicK_AWS.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NugMagicK.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace APIMagicK_AWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private RepositoryUsuarios repo;
        private HelperOAuthToken helper;

        public AuthorizationController(RepositoryUsuarios repo
            , HelperOAuthToken helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult ValidarUsuario(LoginModel model)
        {

            Usuario usuario = this.repo.AutentificaUsuario(model.Email, model.Password);

            if (usuario == null)
            {
                return Unauthorized();
            }
            else
            {
                SigningCredentials credentials =
                    new SigningCredentials(this.helper.GetKeyToken(), SecurityAlgorithms.HmacSha256);

                string jsonUsuario = JsonConvert.SerializeObject(usuario);
                Claim[] claims = new[]
                {
                    new Claim("UserData", jsonUsuario)
                };
                JwtSecurityToken token =
                    new JwtSecurityToken(
                        claims: claims,
                        issuer: this.helper.Issuer,
                        audience: this.helper.Audience,
                        signingCredentials: credentials,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        notBefore: DateTime.UtcNow
                        );
                return Ok(
                    new
                    {
                        response =
                        new JwtSecurityTokenHandler().WriteToken(token)
                    });
            }
        }

    }
}
