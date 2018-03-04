
namespace ServiceInterfaces
{
    public interface IShopService
    {
        GoodsDto GetGoods(int id);
    }

    public class GoodsDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal SalePrice { get; set; }
    }
}
