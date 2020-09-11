using PromotionEngine.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PromotionEngine.Data
{
    public interface ICheckOutData
    {
        void AddItemsToCart(char id, int qty);
        IEnumerable<Item> GetAllItemsFromCart();
        void RemoveCartItem(char id);
        void EditCartItem(char id, int qty);
        decimal CalculateCheckOutPrice();
    }
}
