using PromotionEngine.Core;
using System.Collections.Generic;

namespace PromotionEngine.Data
{
    public class InMemoryData : ICheckOutData
    {
        List<Item> items;
        Cart cart;
        List<Promotions> promotions;
        public InMemoryData()
        {
            items = new List<Item>()
            {
                new Item{Sku='A',Quantity=0,Price=50 },
                new Item{Sku='B',Quantity=0,Price=30 },
                new Item{Sku='C',Quantity=0,Price=20 },
                new Item{Sku='D',Quantity=0,Price=15 }
            };
            cart = new Cart();
            foreach (var item in items)
            {
                cart.AddCartItems(item);
            }
            promotions = new List<Promotions>()
            {
                new Promotions{Id=1, Type="Single",Details="3 of A's for 130", Quantity = 3, Price = 130, PrimarySku='A'},
                new Promotions{Id=2, Type="Single",Details="2 of B's for 45", Quantity = 2, Price = 45, PrimarySku='B'},
                new Promotions{Id=3, Type="Multiple",Details="C & D For 30", Quantity = 1, Price = 30, PrimarySku='C', SecondarySku = 'D'},
            };
        }

        public void AddItemsToCart(char id, int qty)
        {
            var item = cart.GetCartItemById(id);

            if (item != null)
            {
                cart.RemoveCartItem(id);
                item.Quantity = qty;
                cart.AddCartItems(item);
            }
        }


        public void EditCartItem(char id, int qty)
        {
            cart.EditCart(id, qty);
        }

        public IEnumerable<Item> GetAllItemsFromCart()
        {
            return cart.GetAllCartItems();
        }

        public void RemoveCartItem(char id)
        {
            cart.RemoveCartItem(id);
        }

        public decimal CalculateCheckOutPrice()
        {
            decimal cartPrice = 0;
            decimal finalPrice = 0;
            var cartItems = cart.GetAllCartItems();
            foreach (var item in cartItems)
            {
                cartPrice += (item.Price * item.Quantity);
            }
            finalPrice = cartPrice;

            if (!ApplyPromotion.CheckPromotionStatus(cart))
            {
                finalPrice = ApplyPromotion.ApplyPromotions(cart, promotions);
            }

            return finalPrice;
        }

    }
}
