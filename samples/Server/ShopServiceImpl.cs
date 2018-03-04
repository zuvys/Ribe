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
    }
}
