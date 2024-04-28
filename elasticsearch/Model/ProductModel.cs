namespace elasticsearch.Model
{
    public class ProductModel
    {
        public class Root
        {
            public int Id { get; set; }
            public List<string> Images { get; set; }
            public string Thumbnail { get; set; }
            public int CategoryId { get; set; }
            public int? BrandId { get; set; }
            public int? SellerId { get; set; }
            public int StatusId { get; set; }
            public string NameEn { get; set; }
            public string NameBn { get; set; }
            public int? UnitId { get; set; }
            public string Slug { get; set; }
            public int? VariantAttributeId1 { get; set; }
            public List<VariantAttributeValue1> VariantAttributeValue1 { get; set; }
            public int? VariantAttributeId2 { get; set; }
            public List<VariantAttributeValue2> VariantAttributeValue2 { get; set; }
            public int? TotalStockQuantity { get; set; }
            public string CategoryNameEn { get; set; }
            public string BrandNameEn { get; set; }
            public string UnitNameEn { get; set; }
            public string SellerName { get; set; }
            public string ShopName { get; set; }
            public List<ProductVariantsDatum> ProductVariantsDatum { get; set; }
        }
        public class ProductVariantsDatum
        {
            public int Id { get; set; }
            public int? ProductId { get; set; }
            public string SellerSKU { get; set; }
            public int? CurrentStockQty { get; set; }
            public int? Price { get; set; }
            public int? StatusId { get; set; }
            public int? PromoPrice { get; set; }
            public DateTime? PromoStartAt { get; set; }
            public DateTime? PromoEndAt { get; set; }
            public string Name { get; set; }
            public bool? IsDefault { get; set; }
            public string ShopSKU { get; set; }
        }
        public class VariantAttributeValue1
        {
            public List<object> Images { get; set; }
            public int? AttributeValueId { get; set; }
            public string AttributeValueName { get; set; }
        }
        public class VariantAttributeValue2
        {
            public List<object> Images { get; set; }
            public int? AttributeValueId { get; set; }
            public object AttributeValueName { get; set; }
        }
    }
}
