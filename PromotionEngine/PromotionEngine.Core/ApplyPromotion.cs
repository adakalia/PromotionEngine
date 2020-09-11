using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PromotionEngine.Core
{
    public class ApplyPromotion
    {

        public static decimal ApplyPromotions(Cart cart, IEnumerable<Promotions> promotions)
        {
            decimal totalPrice = 0;

            var items = cart.GetAllCartItems();
            List<char> cartItemsSkuList = new List<char>();
            List<char> promotionApplied = new List<char>();

            foreach (var item in items)
            {
                cartItemsSkuList.Add(item.Sku);
            }


            decimal discountedSinglePromotionPrice = 0;
            foreach (var item in items)
            {
                var itemPromotion = promotions.FirstOrDefault(p => p.PrimarySku == item.Sku && p.Type == "Single");
                if (itemPromotion != null && item.Quantity >= itemPromotion.Quantity)
                {
                    int itemsRemaining = 0;
                    int promotionQty = itemPromotion.Quantity;
                    decimal promotionPrice = itemPromotion.Price;
                    int itemInPromotion = Math.DivRem(item.Quantity, promotionQty, out itemsRemaining);
                    discountedSinglePromotionPrice += (itemInPromotion * promotionPrice + itemsRemaining * item.Price);
                    promotionApplied.Add(item.Sku);
                }
            }

            decimal discountedMultiplePromotionPrice = 0;
            foreach (var item in items)
            {
                var itemPromotion = promotions.FirstOrDefault(p => p.PrimarySku == item.Sku && p.Type == "Multiple" && p.SecondarySku != '\0');
                if (itemPromotion != null)
                {
                    var secondarySku = promotions.FirstOrDefault(p => p.PrimarySku == item.Sku && p.Type == "Multiple").SecondarySku;
                    int primaryQty = item.Quantity;
                    decimal primaryPrice = item.Price;
                    int secondaryQty = items.FirstOrDefault(i => i.Sku == secondarySku).Quantity;
                    decimal secondaryPrice = items.FirstOrDefault(i => i.Sku == secondarySku).Price;
                    if (secondaryQty != 0)
                    {
                        if (primaryQty > secondaryQty)
                        {
                            int normalQty = primaryQty - secondaryQty;
                            int promoQty = (primaryQty + secondaryQty + -normalQty) / 2;
                            discountedMultiplePromotionPrice += (promoQty * itemPromotion.Price) + (normalQty * primaryPrice);
                        }
                        else if (primaryQty < secondaryQty)
                        {
                            int normalQty = secondaryQty - primaryQty;
                            int promoQty = (primaryQty + secondaryQty + -normalQty) / 2;
                            discountedMultiplePromotionPrice += (promoQty * itemPromotion.Price) + (normalQty * secondaryPrice);
                        }
                        else
                        {
                            int totalQty = (primaryQty + secondaryQty) / 2;
                            discountedMultiplePromotionPrice += totalQty * itemPromotion.Price;
                        }
                        promotionApplied.Add(item.Sku);
                        promotionApplied.Add(secondarySku);
                    }
                }
            }

            decimal priceWithoutPromotion = 0;

            var remainingItems = cartItemsSkuList.Where(i => !promotionApplied.Contains(i)).ToList();
            foreach (var remain in remainingItems)
            {
                var item = items.FirstOrDefault(i => i.Sku == remain);
                priceWithoutPromotion += item.Quantity * item.Price;
            }

            totalPrice = discountedSinglePromotionPrice + discountedMultiplePromotionPrice + priceWithoutPromotion;
            cart.IsPromotionApplied = true;

            return totalPrice;
        }
        public static bool CheckPromotionStatus(Cart cart)
        {
            return cart.IsPromotionApplied;
        }
    }
}
