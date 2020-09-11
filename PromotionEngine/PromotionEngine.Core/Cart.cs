using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PromotionEngine.Core
{
    public class Cart
    {
        private List<Item> CartItems;

        public bool IsPromotionApplied { get; set; }

        public Cart()
        {
            CartItems = new List<Item>();
            IsPromotionApplied = false;
        }

        public IEnumerable<Item> GetAllCartItems()
        {
            return CartItems.OrderBy(item => item.Sku).ToList();
        }

        public Item GetCartItemById(char id)
        {
            return CartItems.FirstOrDefault(i => i.Sku == id);
        }
        public void AddCartItems(Item item)
        {
            CartItems.Add(item);
        }

        public Item EditCart(char id, int quantity)
        {
            var item = CartItems.FirstOrDefault(i => i.Sku == id);

            if (item != null)
            {
                CartItems.Remove(item);
                item.Quantity += quantity;
                CartItems.Add(item);
            }

            return item;
        }

        public Item RemoveCartItem(char id)
        {
            var item = CartItems.FirstOrDefault(i => i.Sku == id);

            if (item != null)
            {
                CartItems.Remove(item);
            }

            return item;
        }
    }
}
