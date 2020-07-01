using System;
using System.Collections.Generic;
using System.Linq;

namespace PromotionEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            decimal totalPrice = 0;
            List<Promotion> activePromotions = new List<Promotion>()
            {
                new Promotion
                {
                    PromotionCode="NProducts",
                    ReducedPrice=130,
                    SkuId="A",
                    isSelected=true,
                    NoOfItems=3
                },
                 new Promotion
                {
                    PromotionCode="ComboOffer",
                    ReducedPrice=40,
                    SkuId="B",
                    SkuId2="C",
                    isSelected=false,
                    
                },
                  new Promotion
                {
                    PromotionCode="x%OfUnitPrice",
                    isSelected=false,
                    Discount=10

                }
            };
            List<Product> orderedProducts = new List<Product>()
            {
                new Product
                {
                    SkuId="A",
                    UnitPrice=50,
                    Quantity=5
                },
                 new Product
                {
                    SkuId="B",
                    UnitPrice=30,
                    Quantity=3
                },
                  new Product
                {
                    SkuId="C",
                    UnitPrice=20,
                    Quantity=2
                }
            };
            totalPrice =ApplyPromotion(activePromotions, orderedProducts);
            Console.WriteLine($"TotalPrice is {totalPrice}");
        }
        public static decimal ApplyPromotion(List<Promotion> activePromotions, List<Product> OrderedProducts)
        {
            decimal totalPrice = 0;
            foreach(Promotion promotion in activePromotions)
            {
                if (promotion.isSelected)
                {
                    switch (promotion.PromotionCode)
                    {
                        case "NProducts":
                            List<Product> eligibleProducts = OrderedProducts.Where(w => w.SkuId == promotion.SkuId).ToList();
                            List<Product> NotEligibleProducts = OrderedProducts.Where(w => w.SkuId != promotion.SkuId).ToList();
                            decimal NotEligibleProductsPrice = CalculatePriceAfterDiscount(NotEligibleProducts, promotion);
                            decimal ProductCount = eligibleProducts.Count / promotion.NoOfItems;
                            decimal PriceAfterApplyPromotionCode = ProductCount * promotion.ReducedPrice;
                            decimal remainingProducts = eligibleProducts.Count % promotion.NoOfItems;
                            decimal remainingProductsPrice = remainingProducts * eligibleProducts.Select(s => s.UnitPrice).ToList().FirstOrDefault();
                            totalPrice = PriceAfterApplyPromotionCode + remainingProductsPrice+NotEligibleProductsPrice;
                            break;
                        case "ComboOffer":
                            break;
                        case "x%OfUnitPrice":
                            totalPrice = CalculatePriceAfterDiscount(OrderedProducts, promotion);
                            break;
                    }
                }
            }

            return totalPrice;
        }
        public static decimal CalculatePriceAfterDiscount(List<Product> products,Promotion promotion)
        {
            decimal price = 0;
            foreach(Product product in products)
            {
                price += (product.Quantity * product.UnitPrice) * (promotion.Discount / 100);
            }
            return price;
        }
    }
    
    public class Promotion
    {
        public string PromotionCode { get; set; }
        public int Discount { get; set; }
        public string SkuId { get; set; }
        public bool isComboOffer { get; set; }
        public int NoOfItems { get; set; }
        public bool isSelected { get; set; }
        public string SkuId2 { get; set; }
        public decimal ReducedPrice { get; set; }

    }
    public class Product
    {
        public string SkuId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
