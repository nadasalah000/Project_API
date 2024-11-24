using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabt.Core.Entities;
using Talabt.Core.Order_Aggregate;

namespace Talabt.Repository.Data
{
    public static class MyDbContextSeed
    {
        public static async Task SeedAsync(MyDbContext dbContext)
        {
            #region Product Brand
            if(!dbContext.ProductBrands.Any())
            {
                var BrandsData = File.ReadAllText("../Talabt.Repository/Data/DataSeed/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);
                if (Brands?.Count > 0)
                {
                    foreach (var Brand in Brands)
                    {
                        await dbContext.Set<ProductBrand>().AddAsync(Brand);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            #endregion
            #region Product Type
            if (!dbContext.ProductTypes.Any())
            {
                var TypesData = File.ReadAllText("../Talabt.Repository/Data/DataSeed/types.json");
                var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);
                if (Types?.Count > 0)
                {
                    foreach (var Type in Types)
                    {
                        await dbContext.Set<ProductType>().AddAsync(Type);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
               
            #endregion
            #region Product
            if(!dbContext.Products.Any())
            {
                var ProductsData = File.ReadAllText("../Talabt.Repository/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                if (products?.Count > 0)
                {
                    foreach (var product in products)
                    {
                        await dbContext.Set<Product>().AddAsync(product);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            #endregion
            #region Delivery Method
            if (!dbContext.DeliveryMethods.Any())
            {
                var DeliveryMethodData = File.ReadAllText("../Talabt.Repository/Data/DataSeed/delivery.json");
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodData);
                if (DeliveryMethods?.Count > 0)
                {
                    foreach (var DeliveryMethod in DeliveryMethods)
                    {
                        await dbContext.Set<DeliveryMethod>().AddAsync(DeliveryMethod);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            #endregion
        }
    }
}
