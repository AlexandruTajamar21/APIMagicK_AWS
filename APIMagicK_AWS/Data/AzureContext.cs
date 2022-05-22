using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NugMagicK.Models;

namespace APIMagicK_AWS.Data
{
    public class AzureContext : DbContext
    {
        public AzureContext(DbContextOptions<AzureContext> options) : base(options) { }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ViewProducto> VeiwsProducto { get; set; }
        public DbSet<VW_ItemsUsuario_Listados> VeiwsItemsUsuario { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<ViewProductoUsuario> ProductoUsuarios { get; set; }
    }
}
