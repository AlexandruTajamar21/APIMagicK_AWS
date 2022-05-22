using APIMagicK_AWS.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NugMagicK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIMagicK_AWS.Repositories
{
    public class RepositoryUsuarios
    {
        private AzureContext context;

        public RepositoryUsuarios(AzureContext context)
        {
            this.context = context;
        }

        public Usuario AutentificaUsuario(string email, string password)
        {
            var consulta = from datos in this.context.Usuarios
                           where datos.Correo == email
                           && datos.Contraseña == password
                           select datos;
            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                return consulta.First();
            }
        }

        public void InsertUsuario(string nombre, string password, string direccion, string correo, string tipo)
        {
            int id = this.GetMaxId();
            if(direccion == null)
            {
                direccion = "";
            }
            Usuario usuario = new Usuario()
            {
                IdUser = id,
                Nombre = nombre,
                Contraseña = password,
                Direccion = direccion,
                Correo = correo,
                TipoUsuario = tipo
            };
            this.context.Add(usuario);
            this.context.SaveChanges();
        }

        public Usuario GetUserId(int id)
        {
            Usuario user = this.context.Usuarios.Where(z => z.IdUser == id).FirstOrDefault();
            return user;
        }

        public int GetMaxId()
        {
            int id = 1;
            if (this.context.Usuarios.Count() > 0)
            {
                int max = this.context.Usuarios.Max(x => x.IdUser);
                id = max + 1;
                return id;
            }
            else
            {
                return 1;
            }
        }

        public void DeleteItem(int id)
        {
            string sql = "SP_DELETE_CASCADE_USER @IdUser";

            SqlParameter paramIdProducto = new SqlParameter("@IdUser", id);

            var consulta = this.context.Database.ExecuteSqlRaw(sql, paramIdProducto);
        }

        internal List<Usuario> GetUsuarios()
        {
            List<Usuario> usuarios = this.context.Usuarios.ToList();
            return usuarios;
        }

        public Usuario ExisteUsuario(string email)
        {
            Usuario user = this.context.Usuarios.Where(z => z.Correo == email).FirstOrDefault();
            return user;
        }
    }
}
