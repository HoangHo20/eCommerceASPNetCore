using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerMVCSite.Services.Interface;
using eCommerceASPNetCore.Domain;

namespace CustomerMVCSite.Services
{
    public class ProductService : IProductService
    {
        public List<Product> getAllProductAndItsProperties()
        {
            throw new NotImplementedException();
        }

        public List<Product> getAllProductAndProductImageOnly()
        {
            return createDumbData(false, true, false);
        }

        public List<Product> getAllProductCustomProperties(bool description, bool images, bool subcategory)
        {
            return createDumbData(description, images, subcategory);
        }

        // Dumb data
        private List<Product> createDumbData(bool hasDescription = false, bool hasImages = false, bool hasSubcategory = false)
        {
            List<Product> products = new List<Product>();

            string[] pName = { "Ryzen 5 2600", "Ryzen 5 3500X", "Core i3 9100f", "Core i5-9400F" };
            string[] pDesc = { "Hello world", "Aloha the wold Im new", "Ohaio the earth people", "Konichiwa Core I5 powerful"};
            decimal[] pPrice = { 500, 600, 700, 893.23m };
            string[] iUrl =
            {
                "https://lh3.googleusercontent.com/x2jhKl5LlrHdtKrhuhKXiXiXpWUichYSEUGSZ206ijL1b0q5bhNnV0_bu9VdPlMRdPeDvf76uMqNh8qsH78=w1000-rw",
                "https://ae01.alicdn.com/kf/H439a46a4770a4b4a8a5f2ae81ffa308e0/AMD-Ryzen-5-2600-R5-2600-3-4-GHz-6-L-i-M-i-Hai-Nh.jpg",
                "http://thnhatrang.vn/media/product/15646_1.jpg",
                "https://ae01.alicdn.com/kf/H3893e7fdba134413ac45dd6b288f2be7U/AMD-Ryzen-5-3500X-R5-3500X-3-6-GHz-6-L-i-6-Ch-B.jpg",
                "https://thnhatrang.vn/media/product/120_13775_1.jpg",
                "https://songphuong.vn/Content/uploads/2020/04/1-intel-core-i3-9100f-songphuong.vn_.jpg",
                "https://product.hstatic.net/1000333506/product/9400f.jpg",
                "https://td-tech.vn/wp-content/uploads/2019/10/i5-9400f.png",
            };

            Category category = new Category();

            SubCategory sub1 = new SubCategory() { ID = 1, Name = "Ryzen 5 Old" };
            SubCategory sub2 = new SubCategory() { ID = 2, Name = "Ryzen 5 new" };
            SubCategory sub3 = new SubCategory() { ID = 3, Name = "Core I3" };
            SubCategory sub4 = new SubCategory() { ID = 4, Name = "Core I5" };

            category.SubCategories = new List<SubCategory>();
            category.SubCategories.Add(sub1);
            category.SubCategories.Add(sub2);
            category.SubCategories.Add(sub3);
            category.SubCategories.Add(sub4);

            // Each product has 2 images
            int imageGotCount = 0;
            for (int i = 0; i < pName.Length; i++)
            {
                // Product 1
                Product prod = new Product()
                {
                    ID = i+1,
                    Name = pName[i],
                    Price = pPrice[i],
                };

                if (hasDescription)
                {
                    prod.Description = pDesc[i];
                }

                if (hasImages)
                {
                    prod.Images = new List<ProductImage>();

                    for (int m = 0; m < 2; m++)
                    {
                        prod.Images.Add(new ProductImage()
                        {
                            ID = imageGotCount + 1,
                            Url = iUrl[imageGotCount]
                        });

                        imageGotCount++;
                    }
                }

                if (hasSubcategory)
                {
                    prod.SubCategory = category.SubCategories[i];
                }

                products.Add(prod);
            }

            return products;
        }
    }
}
