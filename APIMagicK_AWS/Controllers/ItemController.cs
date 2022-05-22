using APIMagicK_AWS.Helpers;
using APIMagicK_AWS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NugMagicK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIMagicK_Azure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private RepositoryItems repo;
        private HelperOAuthToken helper;

        public ItemController(RepositoryItems repo
            , HelperOAuthToken helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpGet]
        public ActionResult<List<ViewProducto>> ItemsHome()
        {
            List<ViewProducto> items = this.repo.GetAllDistinctItems();
            return items;
        }

        [HttpGet]
        [Route("[action]/{filtro}")]
        public ActionResult<List<ViewProducto>> ItemsHomeFiltro(string filtro)
        {
            List<ViewProducto> items = this.repo.GetItemsFiltro(filtro);
            return items;
        }

        [HttpGet]
        [Authorize]
        [Route("[action]/{id}")]
        public ActionResult<List<ViewProductoUsuario>> ItemsUsuario(int id)
        {
            List<ViewProductoUsuario> items = this.repo.GetItemsUser(id);
            return items;
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public ActionResult<List<VW_ItemsUsuario_Listados>> StockItem(string id)
        {
            List<VW_ItemsUsuario_Listados> items = this.repo.GetItemsId(id);
            return items;
        }

        [HttpGet]
        [Route("[action]/{iduser}/{idproducto}")]
        public ActionResult<List<VW_ItemsUsuario_Listados>> StockItemUsuario(int iduser, string idproducto)
        {
            List<VW_ItemsUsuario_Listados> items = this.repo.GetItemsIdUsuario(iduser, idproducto);
            return items;
        }

        [HttpGet]
        [Route("[action]/{itemId}")]
        public ActionResult<Item> GetItemId(int itemId)
        {
            Item item = this.repo.GetItemId(itemId);
            return item;
        }

        [HttpPut]
        [Authorize]
        public Boolean UpdateItem(Item item)
        {
            if (this.repo.UpdateItem(item.IdItem, item.Nombre, item.IdUser, item.IdProducto, item.Precio, item.Estado, item.Imagen, item.Descripcion))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        public Boolean Registracompra(Compra compra)
        {
            if (this.repo.RegistraCompra(compra.IdComprador, compra.IdVendedorUser, compra.IdItem,compra.Precio))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        [HttpGet]
        [Route("[action]/{idUser}")]
        [Authorize]
        public ActionResult<List<ResumenCompra>> VerificarCompras(int idUser)
        {
            List<ResumenCompra> resumenCompras = this.repo.GetResumenComprasUser(idUser);
            return resumenCompras;
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteItem(int id)
        {
            this.repo.DeleteItem(id);
            return Ok();
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        public ActionResult InsertItem(Item item)
        {
            this.repo.InsertarItem(item.Nombre, item.IdUser, item.IdProducto, item.Precio, item.Estado, item.Imagen, item.Descripcion);
            return Ok();
        }
    }
}
