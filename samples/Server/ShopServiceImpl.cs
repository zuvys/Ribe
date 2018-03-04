﻿using System.Threading.Tasks;
using Ribe.Core;
using ServiceInterfaces;

namespace ServiceInterface
{
    [Service(Version = "0.0.1")]
    public class ShopServiceImpl : IShopService
    {
        public GoodsDto GetGoods(int id)
        {
            return new GoodsDto()
            {
                Id = id,
                Name = "Goods_" + id,
                SalePrice = id + 5
            };
        }

        public Task<GoodsDto> GetGoodsAsync(int id)
        {
            return Task.FromResult(GetGoods(id));
        }
    }

    [Service(Version = "0.0.2")]
    public class ShopServiceImpl2 : IShopService
    {
        public GoodsDto GetGoods(int id)
        {
            return new GoodsDto()
            {
                Id = id,
                Name = "Goods_" + id + "_Goods",
                SalePrice = id + 10
            };
        }

        public Task<GoodsDto> GetGoodsAsync(int id)
        {
            return Task.FromResult(GetGoods(id));
        }
    }
}
