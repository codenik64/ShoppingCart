using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCart01.Models;
using ShoppingCart01.ViewModels;
using Microsoft.AspNet.Identity;

namespace ShoppingCart01.Controllers
{
    public class ShoppingCartController : Controller
    {
        ApplicationDbContext storeDB = new ApplicationDbContext();
        ShoppingCart sc = new ShoppingCart();
        // GET: ShoppingCart
        [Authorize]
        public ActionResult Index(Cart c)
        {
            //string val = User.Identity.GetUserName();
            //var useru = storeDB.Cart.Where(x => x.CartId.Equals(val) && x.IsComplete == false).ToList();
            //if (useru.Count != 0)
            //{

                var cart = ShoppingCart.GetCart(this.HttpContext);
            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal(),
                
                };
                // Return the view
                return View(viewModel);
            //}
            //else
            //{
            //    var ShoppingCart = Models.ShoppingCart.GetCart(this.HttpContext);
            //    ShoppingCart.EmptyCart();

            //    return View(ShoppingCart);

            //}

        }

        public ActionResult AddToCart(int id)
        {
            // Retrieve the album from the database
            var addedAlbum = storeDB.product
            .Single(album => album.ProductID == id);
            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.AddToCart(addedAlbum);
            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Get the name of the album to display confirmation
            string albumName = storeDB.Cart
            .Single(item => item.RecordId == id).product.Name;
            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);
            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(albumName) +
            " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }
      
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}