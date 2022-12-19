using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Infrastructure.Data
{
    public class CatalogContextSeed
    {
        public static async Task SeedAsync(CatalogContext catalogContext,
            ILoggerFactory loggerFactory, int retry = 0)
        {
            var retryForAvailability = retry;
            try
            {
                if (catalogContext.Database.IsSqlServer())
                {
                    catalogContext.Database.Migrate();
                }

                if (!await catalogContext.CatalogBrands.AnyAsync())
                {
                    await catalogContext.CatalogBrands.AddRangeAsync(
                        GetPreconfiguredCatalogBrands());

                    await catalogContext.SaveChangesAsync();
                }

                if (!await catalogContext.CatalogTypes.AnyAsync())
                {
                    await catalogContext.CatalogTypes.AddRangeAsync(
                        GetPreconfiguredCatalogTypes());

                    await catalogContext.SaveChangesAsync();
                }

                if (!await catalogContext.CatalogItems.AnyAsync())
                {
                    await catalogContext.CatalogItems.AddRangeAsync(
                        GetPreconfiguredItems());

                    await catalogContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability >= 10) throw;

                retryForAvailability++;
                var log = loggerFactory.CreateLogger<CatalogContextSeed>();
                log.LogError(ex.Message);
                await SeedAsync(catalogContext, loggerFactory, retryForAvailability);
                throw;
            }
        }

        static IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands()
        {
            return new List<CatalogBrand>
            {
                new("Nike"),
                new("Under Armour"),
                new("Champion"),
                new("Adidas"),
                new("Other")
            };
        }

        static IEnumerable<CatalogType> GetPreconfiguredCatalogTypes()
        {
            return new List<CatalogType>
            {
                new("Sweatshirt"),
                new("T-Shirt"),
                new("Pants"),
                new("Accessory")
            };
        }

        static IEnumerable<CatalogType> GetPreconfiguredCatalogColors()
        {
            return new List<CatalogType>
            {
                new("Blue"),
                new("Black"),
                new("White"),
                new("Grey")
            };
        }

        static IEnumerable<CatalogItem> GetPreconfiguredItems()
        {
            return new List<CatalogItem>
            {
                new(5,1, "LEAGUE ESSENTIAL FLEECE HOOD", "LEAGUE ESSENTIAL FLEECE HOOD", 54.99M,  "https://cdn.shopify.com/s/files/1/0008/6491/1404/products/d98e9824eafcdea23fa8d71f9dc22cc1_600x.jpg?v=1670533441", "Blue"),
                new(5,1, "MV SPORT PRO-WEAVE HOOD", "MV SPORT PRO-WEAVE HOOD", 52.99M, "https://cdn.shopify.com/s/files/1/0008/6491/1404/products/8a4d5886372be7c2e25a4317a70ff304_600x.jpg?v=1660068213","WHite"),
                new(3,1, "CHAMPION WOMEN'S UNIVERSITY PO HOOD", "CHAMPION WOMEN'S UNIVERSITY PO HOOD", 39.99M,  "https://cdn.shopify.com/s/files/1/0008/6491/1404/products/958e76673ca22928b28f38de2eda97ae_800x.jpg?v=1664816631", "White"),
                new(5,2, "MV SPORT EVEREST SUSTAINABLE TEE", "MV SPORT EVEREST SUSTAINABLE TEE", 18.99M, "https://cdn.shopify.com/s/files/1/0008/6491/1404/products/b3debab5239ee857fb7bed84682ce54a_900x.jpg?v=1669755808","Grey"),
                new(5,2, "STORM MASCOT S/S TEE", "STORM MASCOT S/S TEE", 16.99M, "https://cdn.shopify.com/s/files/1/0008/6491/1404/products/75e7432ea535e9933f897ea484352681_800x.jpg?v=1661205824", "Blue"),
                new(3,2, "CHAMPION WOMEN'S BOYFRIEND CROP TEE", "CHAMPION WOMEN'S BOYFRIEND CROP TEE", 19.99M, "https://cdn.shopify.com/s/files/1/0008/6491/1404/products/95c8752630e44570724c9ed13cbeb678_900x.jpg?v=1666905621","White"),
                new(5,2, "USCAPE MIDWEIGHT SHORT SLEEVE", "USCAPE MIDWEIGHT SHORT SLEEVE",  24.99M, "https://cdn.shopify.com/s/files/1/0008/6491/1404/products/bfd432e59db5624090cf791d7a688d86_900x.jpg?v=1662537969","Grey"),
                new(1,3, "NIKE CLUB FLEECE JOGGER", "NIKE CLUB FLEECE JOGGER", 65.99M, "https://cdn.shopify.com/s/files/1/0008/6491/1404/products/af4e21f40652371b4bfa94ecb82352a0_900x.jpg?v=1666083965", "Black"),
                new(2,3, "UNDER ARMOUR F21 MENS AF JOGGER", "UNDER ARMOUR F21 MENS AF JOGGER", 64.99M, "https://cdn.shopify.com/s/files/1/0008/6491/1404/products/0d3e5af01bdc6b4610f8a70a0c83d79d_700x.jpg?v=1660057420","Blue"),
                new(5,3, "MV SPORT VINTAGE FLEECE PANT WITH LEG GRAPHIC", "MV SPORT VINTAGE FLEECE PANT WITH LEG GRAPHIC", 34.99M, "https://cdn.shopify.com/s/files/1/0008/6491/1404/products/8e3146e65faa1b76a7f6f25bbc1a7a80_900x.jpg?v=1669759417","Red"),
                new(5,4, "MOUNTAIN MAN KEY TAG", "MOUNTAIN MAN KEY TAG", 3.99M, "https://cdn.shopify.com/s/files/1/0008/6491/1404/products/6841ee5a34600f5934f25aa2dd1c727d_900x.jpg?v=1657731825","Black"),
                new(5,4, "RIGID ID HOLDER", "RIGID ID HOLDER", 5.99M, "https://cdn.shopify.com/s/files/1/0008/6491/1404/products/084b3b62b99b4254b1e6d15f30722b1a_900x.jpg?v=1582926756", "Black")
            };
        }
    }
}
