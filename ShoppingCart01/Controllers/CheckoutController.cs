﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCart01.Models;

namespace ShoppingCart01.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        ApplicationDbContext storeDB = new ApplicationDbContext();
        const string PromoCode = "FREE";
        // GET: Checkout
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddressAndPayment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new Order();
            TryUpdateModel(order);
            try
            {
                //if (string.Equals(values["PromoCode"], PromoCode,
                //StringComparison.OrdinalIgnoreCase) == false)
                //{
                //    return View(order);
                //}

                //else
                //{
                    order.Username = User.Identity.Name;
                    order.OrderDate = DateTime.Now;
                    //Save Order
                    storeDB.order.Add(order);
                    storeDB.SaveChanges();
                    //Process the order
                    var cart = ShoppingCart.GetCart(this.HttpContext);
                    cart.CreateOrder(order);
                    return RedirectToAction("Complete",
                    new { id = order.OrderId });
               
            }
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }
        }
        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = storeDB.order.Any(
            o => o.OrderId == id &&
            o.Username == User.Identity.Name);
            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }


    }
}