using Catalog.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.UnitTests.MockData
{
    public class ProductMockData
    {
        public static List<Product> GetProducts()
        {
            return new List<Product>{
             new Product{
                 Id = "602d2149e773f2a3990b47f1",
                 Name = "iPhone X",
                 Category = "Smart phone",
                 Summary = "",
                 Description = "",
                 ImageFile = "",
                 Price = 100
             },
             new Product{
                 Id = "602d2149e773f2a3990b47f2",
                 Name = "Sumsung A10",
                 Category = "Smart phone",
                 Summary = "",
                 Description = "",
                 ImageFile = "",
                 Price = 150
             },
             new Product{
                 Id = "602d2149e773f2a3990b47f3",
                 Name = "iPhone X",
                 Category = "Smart phone",
                 Summary = "",
                 Description = "",
                 ImageFile = "",
                 Price = 200
             }
         };
        }

        public static List<Product> GetEmptyProduct()
        {
            return new List<Product>();
        }

        public static Product NewProduct()
        {
            return new Product
            {
                Id = "602d2149e773f2a3990b47f4",
                Name = "iPhone X7",
                Category = "Smart phone",
                Summary = "",
                Description = "iPhone X mobile phone",
                ImageFile = "",
                Price = 120
            };
        }
    }
}
