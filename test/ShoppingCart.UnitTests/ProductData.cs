namespace ShoppingCart.UnitTests
{
    public class ProductData
    {
        public string Name;
        public int Price;
        public string Sku;

        public override bool Equals(object obj)
        {
            ProductData pd = (ProductData)obj;
            return Name.Equals(pd.Name) && Sku.Equals(pd.Sku) && Price == pd.Price;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Sku.GetHashCode() ^ Price.GetHashCode();
        }
    }
}