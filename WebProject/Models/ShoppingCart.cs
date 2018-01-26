using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProject.Models
{
    public class ShoppingCart
    {
        public List<ShoppingCartItem> ListItem { get; set; }

        public ShoppingCart()
        {
            ListItem = new List<ShoppingCartItem>();
        }
        public void AddToCart(ShoppingCartItem item)
        {
            if ((ListItem.Where(s => s.ProductId == item.ProductId)).Any())
            {
                var myItem = ListItem.Single(s => s.ProductId == item.ProductId);
                myItem.Quantity += item.Quantity;
                myItem.Total += item.Quantity * item.Price;
            }
            else
            {
                ListItem.Add(item);
            }
        }
        public bool RemoveFromCart(int lngProductSellID)
        {
            ShoppingCartItem existsItem = ListItem.Where(x => x.ProductId == lngProductSellID).SingleOrDefault();
            if (existsItem != null)
            {
                ListItem.Remove(existsItem);
            }
            return true;
        }
        public bool UpdateQuantity(int lngProductSellID, int intQuantity)
        {
            ShoppingCartItem existsItem = ListItem.Where(x => x.ProductId == lngProductSellID).SingleOrDefault();
            if (existsItem != null)
            {
                existsItem.Quantity = intQuantity;
                existsItem.Total = existsItem.Quantity * existsItem.Price;
            }
            return true;
        }
        public bool EmptyCart()
        {
            ListItem.Clear();
            return true;
        }
    }
}