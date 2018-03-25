using Ribe.Core;
using ServiceInterfaces;
using System.Threading.Tasks;

namespace ServiceInterface
{
    [Service(Version = "0.0.1")]
    public class ShopServiceImpl : IShopService
    {
        public void Get(int i)
        {
  
        }

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

        public void Set(int i)
        {
            
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
            return Task.Run<GoodsDto>(() =>
            {
                System.Threading.Thread.Sleep(2000);
                return GetGoods(id);
            });
        }

        public void Get(int i)
        {
          
        }

        public void Set(int i)
        {

        }
    }
}
