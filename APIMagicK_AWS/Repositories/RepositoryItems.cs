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
    public class RepositoryItems
    {
        private AzureContext context;
        public RepositoryItems(AzureContext context)
        {
            this.context = context;
        }

        public List<ViewProducto> GetAllDistinctItems()
        {
            List<ViewProducto> productos = this.context.VeiwsProducto.ToList();
            return productos;
        }

        public List<ViewProducto> GetItemsFiltro(string filtro)
        {
            List<ViewProducto> items = this.context.VeiwsProducto.Where(z => z.Nombre.Contains(filtro)).ToList();
            return items;
        }

        public List<VW_ItemsUsuario_Listados> GetItemsId(string productId)
        {
            List<VW_ItemsUsuario_Listados> items = this.context.VeiwsItemsUsuario.Where(z => z.IdProducto == productId).ToList();
            return items;
        }
        public List<VW_ItemsUsuario_Listados> GetItemsIdUsuario(int iduser, string idproducto)
        {
            string sql = "SP_StockItem_Usuario @IdProducto, @IdUsuario";

            SqlParameter paramIdProducto = new SqlParameter("@IdProducto", idproducto);
            SqlParameter paramIdUser = new SqlParameter("@IdUsuario", iduser);

            var consulta = this.context.VeiwsItemsUsuario.FromSqlRaw(sql,paramIdProducto,paramIdUser);
            return consulta.ToList();
        }

        public Item GetItemId(int itemId)
        {
            Item item = this.context.Items.Where(z => z.IdItem == itemId).FirstOrDefault();
            return item;
        }

        internal List<ViewProductoUsuario> GetItemsUser(int idUser)
        {
            List<ViewProductoUsuario> items = this.context.ProductoUsuarios.Where(z => z.IdUser == idUser).ToList();
            return items;
        }

        public bool RegistraCompra(int idComprador, int idVendedorUser, int idItem, int precio)
        {
            int id = this.GetIdCompraMax();
            Compra compra = new Compra()
            {
                IdCompra = id,
                IdComprador = idComprador,
                IdVendedorUser = idVendedorUser,
                IdItem = idItem,
                Precio = precio
            };
            this.context.Add(compra);
            this.context.SaveChanges();
            return true;
        }

        public Boolean UpdateItem(int idItem, string nombre, int idUser, string idProducto, int precio, int estado, string imagen, string descripcion)
        {
            int id = this.GetIdCompraMax();
            Item item = new Item()
            {
                IdItem = idItem,
                Nombre = nombre,
                IdUser = idUser,
                IdProducto = idProducto,
                Precio = precio,
                Estado = estado,
                Imagen = imagen,
                Descripcion = descripcion
            };
            this.context.Update(item);
            this.context.SaveChanges();
            return true;
        }

        public int GetIdCompraMax()
        {
            if (this.context.Compras.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Compras.Max(z => z.IdCompra) + 1;
            }
        }

        public int GetIdItemMax()
        {
            if (this.context.Items.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Items.Max(z => z.IdItem) + 1;
            }
        }

        public List<Compra> GetComprasUsuario(int idUser)
        {
            return this.context.Compras.Where(z => z.IdComprador == idUser).ToList();
        }

        public Usuario GetUsuarioId(int idUser)
        {
            return this.context.Usuarios.Where(z => z.IdUser == idUser).FirstOrDefault();
        }

        public List<ResumenCompra> GetResumenComprasUser(int idUser)
        {
            List<Compra> compras = this.GetComprasUsuario(idUser);
            List<ResumenCompra> resumenesCompras = new List<ResumenCompra>();
            foreach (Compra compra in compras)
            {
                Usuario vendedor = this.GetUsuarioId(compra.IdVendedorUser);
                Usuario comprador = this.GetUsuarioId(compra.IdComprador);
                Item item = this.GetItemId(compra.IdItem);
                ResumenCompra rescomp = new ResumenCompra();
                rescomp.IdCompra = compra.IdCompra;
                rescomp.IdItem = compra.IdItem;
                rescomp.Precio = compra.Precio;
                rescomp.IdComprador = compra.IdComprador;
                rescomp.IdVendedorUser = compra.IdVendedorUser;
                rescomp.IdProducto = item.IdProducto;
                rescomp.Imagen = item.Imagen;
                rescomp.NombreComprador = comprador.Nombre;
                rescomp.NombreVendedor = vendedor.Nombre;
                resumenesCompras.Add(rescomp);
            }
            return resumenesCompras;
        }

        public Boolean DeleteItem(int idItem)
        {
            Item item = this.GetItemId(idItem);
            this.context.Items.Remove(item);
            this.context.SaveChanges();
            return true;
        }

        public void InsertarItem(string nombre, int idUser, string idProducto, int precio, int estado, string imagen, string descripcion)
        {
            int id = this.GetIdItemMax();
            Item item = new Item()
            {
                IdItem = id,
                Nombre = nombre,
                IdUser = idUser,
                IdProducto = idProducto,
                Precio = precio,
                Estado = estado,
                Imagen = imagen,
                Descripcion = descripcion
            };
            this.context.Add(item);
            this.context.SaveChanges();
        }
    }
}
