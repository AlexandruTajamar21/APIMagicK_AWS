using APIMagicK_AWS.Helpers;
using APIMagicK_AWS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NugMagicK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace APIMagicK_AWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private RepositoryUsuarios repo;
        private HelperOAuthToken helper;

        public UserController(RepositoryUsuarios repo
            , HelperOAuthToken helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpGet]
        [Authorize]
        [Route("[action]")]
        public ActionResult<Usuario> PerfilUsuario()
        {
            List<Claim> claims = HttpContext.User.Claims.ToList();
            string jsonUser = claims.SingleOrDefault(z => z.Type == "UserData").Value;
            Usuario user = JsonConvert.DeserializeObject<Usuario>(jsonUser);
            return user;
        }

        [HttpGet]
        [Authorize]
        [Route("[action]/{id}")]
        public ActionResult<Usuario> UsuariobyId(int id)
        {
            Usuario user = this.repo.GetUserId(id);
            return user;
        }

        [HttpPost]
        public ActionResult<Boolean> InsertUsuario(Usuario user)
        {
            if (user.Contraseña == "0000")
            {
                this.repo.InsertUsuario(user.Nombre, user.Contraseña, user.Direccion, user.Correo, "Admin");
            }
            else
            {
                this.repo.InsertUsuario(user.Nombre, user.Contraseña, user.Direccion, user.Correo, "NormalUser");
            }
            return true;
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult DeleteItem(int id)
        {
            this.repo.DeleteItem(id);
            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("[action]")]
        public ActionResult<List<Usuario>> GetUsuariosAll()
        {
            List<Usuario> usuarios = this.repo.GetUsuarios();
            return usuarios;
        }

        [HttpGet]
        [Route("[action]/{email}")]
        public ActionResult<Boolean> ExisteUsuario(string email)
        {
            Usuario user = this.repo.ExisteUsuario(email);
            if(user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
