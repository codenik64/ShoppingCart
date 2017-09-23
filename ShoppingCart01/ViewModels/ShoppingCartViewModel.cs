using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingCart01.Models;

namespace ShoppingCart01.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
       
    }
}