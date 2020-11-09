using ShoppingCart.UnitTests;

public class ProductProxy : Product
{
    public ProductProxy(string sku)
    {
        Sku = sku;
    }

    public int Price
    {
        get
        {
            ProductData pd = Db.GetProductData(Sku);
            return pd.Price;
        }
    }

    public string Name
    {
        get
        {
            ProductData pd = Db.GetProductData(Sku);
            return pd.Name;
        }
    }

    public string Sku { get; }
}