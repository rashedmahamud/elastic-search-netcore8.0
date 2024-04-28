using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using elasticsearch.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using static elasticsearch.Controllers.WeatherForecastController;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace elasticsearch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            





            var settings = new ElasticsearchClientSettings(new Uri("http://103.153.130.78:9200/")) .CertificateFingerprint("").Authentication(new BasicAuthentication("elastic", ""));

            var client = new ElasticsearchClient(settings);
            //Creating an index
            var responsesss = await client.Indices.CreateAsync("my_index");
            //Indexing documents
            var doc = new MyDoc
            {
                Id = 1,
                User = "flobernd",
                Message = "Trying out the client, so far so omar?"
            };
            await client.IndexAsync(doc, "my_index");


            List<MyDoc> myDocsList = new List<MyDoc> {
            
            
            new MyDoc { Id = 2, User ="Mahamud", Message="Hello Rashed"},
            new MyDoc { Id = 3,User="Omar Mahamud", Message="Hello Omar"},
            new MyDoc { Id = 4,User="Obayda Mahamud", Message="Hello Obayda"},
            new MyDoc { Id = 5,User="Omayer Mahamud", Message="Hello Omayer"},
            new MyDoc { Id = 6,User="Omayera Mahamud", Message="Hello Omayera"},
            new MyDoc { Id = 7,User="Monir Mahamud", Message="Hello Monir"},
            new MyDoc { Id = 8,User="Monira Mahamud", Message="Hello Omar"}
            };
           
            
            foreach(var item in myDocsList)
            {

                 await client.IndexAsync(item, "my_index");
            }
            // Getting documents
            var responses = await client.GetAsync<MyDoc>("1", idx => idx.Index("my_index"));

            if (responses.IsValidResponse)
            {
                var docc = responses.Source;
            }

            //Searching documents

            var responsess = await client.SearchAsync<MyDoc>(s => s
                                                        .Index("my_index")
                                                        .From(0)
                                                        .Size(10)
                                                        .Query(q => q
                                                            .Term(t => t.User, "Omayera")
                                                        )
                                                    );

            if (responsess.IsValidResponse)
            {
                var doooc = responsess.Documents.FirstOrDefault();
            }

            //Updating documents
            doc.Message = "This is a new message from Omar";

            var responsessss = await client.UpdateAsync<MyDoc, MyDoc>("my_index", 1, u => u
                .Doc(doc));

            //Deleting documents
            var responssse = await client.DeleteAsync("my_index", 1);

            //Deleting an index
            //var resppponse = await client.Indices.DeleteAsync("my_index");



            // ::::::::::::::::::: Get app Prodcut prodcut ::::::::::::::::::::

            string apiUrl = "http://192.168.145.167:8060/api/v1/product/product-stock/internal/get-products-data?currentPage=1&rowsPerPage=1000&statusId=";
            //string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var ProductList = new List<ProductModel.Root>();

            using (HttpClient clientt = new HttpClient())
            {

                clientt.DefaultRequestHeaders.Accept.Clear();
                clientt.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await clientt.GetAsync(apiUrl);

                if ((int)response.StatusCode == StatusCodes.Status200OK)
                {
                    response.EnsureSuccessStatusCode();

                    string responseData = await response.Content.ReadAsStringAsync();

                    var apiResponse1 = JsonConvert.DeserializeObject<Root>(responseData);
                    //var apiResponse = JsonConvert.DeserializeObject<List<ProductModel>>(apiResponse1.data.items);


                   

                    foreach (var item in apiResponse1.data.items)
                    {
                        ProductModel.Root productModel = new ProductModel.Root
                        {


                            Id = item.id,
                            BrandId = item.brandId,
                            BrandNameEn = item.brandNameEn,
                            CategoryId = item.categoryId,
                            CategoryNameEn = item.categoryNameEn,
                            Images = item.images,
                            NameBn = item.nameBn,
                            NameEn = item.nameEn,
                            SellerId = item.sellerId,
                            SellerName = item.sellerName,
                            ShopName = item.shopName,
                            Slug = item.slug,
                            UnitNameEn = item.unitNameEn,
                            Thumbnail = item.thumbnail,
                            StatusId = item.statusId,
                            UnitId = item.unitId,
                            TotalStockQuantity = item.totalStockQuantity,
                            VariantAttributeId1 = item.variantAttributeId1,
                            VariantAttributeId2 = item.variantAttributeId2,
                        };

                       var ProductVariantsDatumList = new List<ProductModel.ProductVariantsDatum>();

                         if(item.ProductAttributesDatum != null) {
                            foreach (var item1 in item.ProductVariantsDatum)
                            {
                                var productVariantsDatum = new ProductModel.ProductVariantsDatum
                                {


                                    CurrentStockQty = item1.currentStockQty,
                                    Id = item1.id,
                                    IsDefault = item1.IsDefault,
                                    Name = item1.name,
                                    Price = item1.price,
                                    ProductId = item1.productId,
                                    PromoEndAt = item1.promoEndAt,
                                    PromoPrice = item1.promoPrice,
                                    PromoStartAt = item1.promoStartAt,
                                    SellerSKU = item1.sellerSKU,
                                    ShopSKU = item1.shopSKU,
                                    StatusId = item1.statusId


                                };
                                ProductVariantsDatumList.Add(productVariantsDatum);
                            }
                            productModel.ProductVariantsDatum = ProductVariantsDatumList;
                        }



                        if (item.variantAttributeValue1 != null)
                        {
                            var variantAttributeValue1List = new List<ProductModel.VariantAttributeValue1>();

                            foreach (var item2 in item.variantAttributeValue1)
                            {
                                var variantAttributeValue1 = new ProductModel.VariantAttributeValue1
                                {


                                    AttributeValueId = item2.AttributeValueId,
                                    AttributeValueName = item2.AttributeValueName,
                                    Images = item2.Images


                                };
                                variantAttributeValue1List.Add(variantAttributeValue1);
                            }

                            productModel.VariantAttributeValue1 = variantAttributeValue1List;
                        }



                        if (item.variantAttributeValue2 != null)
                        {
                            var variantAttributeValue2List = new List<ProductModel.VariantAttributeValue2>();

                            foreach (var item3 in item.variantAttributeValue2)
                            {
                                var variantAttributeValue2 = new ProductModel.VariantAttributeValue2
                                {


                                    AttributeValueId = item3.AttributeValueId,
                                    AttributeValueName = item3.AttributeValueName,
                                    Images = item3.Images

                                };
                                variantAttributeValue2List.Add(variantAttributeValue2);
                            }

                            productModel.VariantAttributeValue2 = variantAttributeValue2List;

                        }

                           

                        ProductList.Add(productModel);

                    }

                }

            }


            //Creating an index
            var res = await client.Indices.CreateAsync("product_index");

            foreach (var item in ProductList)
            {

                await client.IndexAsync(item, "product_index");
            }


            //::::::::::::::::::::::::::End :::::::::::::::::::::






            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        public class MyDoc {

            public int Id { get; set; }
            public string User { get; set; }
            public string Message { get; set; }

        }
        public class Data
        {
            public PageInfo pageInfo { get; set; }
            public List<Item> items { get; set; }
        }

        public class Item
        {
            public int id { get; set; }
            public List<string> images { get; set; }
            public string videoUrl { get; set; }
            public string thumbnail { get; set; }
            public int categoryId { get; set; }
            public int brandId { get; set; }
            public int sellerId { get; set; }
            public List<object> tags { get; set; }
            public object? lowStockQty { get; set; }
            public object? refundable { get; set; }
            public object? maxOrderQty { get; set; }
            public object? minOrderQty { get; set; }
            public int statusId { get; set; }
            public string nameEn { get; set; }
            public string nameBn { get; set; }
            public object? numberOfOrder { get; set; }
            public object? showStockQuantity { get; set; }
            public object? showTextOnly { get; set; }
            public int unitId { get; set; }
            public string slug { get; set; }
            public bool? isInInventory { get; set; }
            public string HighlightEn { get; set; }
            public string HighlightBn { get; set; }
            public string boxItems { get; set; }
            public string DescriptionEn { get; set; }
            public string DescriptionBn { get; set; }
            public int? warrantyType { get; set; }
            public int? warrantyPeriod { get; set; }
            public string? WarrantyPolicyEn { get; set; }
            public string? WarrantyPolicyBn { get; set; }
            public decimal? packageWeightInKg { get; set; }
            public decimal? packageHeightInCm { get; set; }
            public decimal? packageWidthInCm { get; set; }
            public decimal? packageLengthInCm { get; set; }
            public int? variantAttributeId1 { get; set; }
            public List<VariantAttributeValue1> variantAttributeValue1 { get; set; }
            public int? variantAttributeId2 { get; set; }
            public List<VariantAttributeValue2> variantAttributeValue2 { get; set; }
            public int? createdBy { get; set; }
            public DateTime? createdAt { get; set; }
            public object? updatedAt { get; set; }
            public int? updatedBy { get; set; }
            public int? inhouseQuantity { get; set; }
            public object? inventoryQuantity { get; set; }
            public int? totalStockQuantity { get; set; }
            public string categoryNameEn { get; set; }
            public string categoryNameBn { get; set; }
            public string brandNameEn { get; set; }
            public string brandNameBn { get; set; }
            public string unitNameEn { get; set; }
            public string unitNameBn { get; set; }
            public string sellerName { get; set; }
            public bool? isDropToHub { get; set; }
            public bool? isPackageBySeller { get; set; }
            public string shopName { get; set; }
            public bool? isInhouseSeller { get; set; }
            public List<ProductVariantsDatum> ProductVariantsDatum { get; set; }
            public List<ProductAttributesDatum> ProductAttributesDatum { get; set; }
        }

        public class PageInfo
        {
            public int? totalCount { get; set; }
            public int? rowsPerPage { get; set; }
            public int? currentPage { get; set; }
            public int? totalPageCount { get; set; }
            public bool? hasNextPage { get; set; }
        }

        public class ProductAttributesDatum
        {
            public int id { get; set; }
            public int? productId { get; set; }
            public int? attributeId { get; set; }
            public string attributeValue { get; set; }
            public int? statusId { get; set; }
            public int? createdBy { get; set; }
            public DateTime? createdAt { get; set; }
            public object? updatedAt { get; set; }
            public object? updatedBy { get; set; }
        }

        public class ProductVariantsDatum
        {
            public int id { get; set; }
            public int? productId { get; set; }
            public string sellerSKU { get; set; }
            public int? currentStockQty { get; set; }
            public int? price { get; set; }
            public int? statusId { get; set; }
            public int? promoPrice { get; set; }
            public DateTime? promoStartAt { get; set; }
            public DateTime? promoEndAt { get; set; }
            public object? freeItems { get; set; }
            public bool? isVariantAvailable { get; set; }
            public int? attributeId1 { get; set; }
            public int? attributeValueId1 { get; set; }
            public int? attributeId2 { get; set; }
            public int? attributeValueId2 { get; set; }
            public int? createdBy { get; set; }
            public DateTime? createdAt { get; set; }
            public object? updatedAt { get; set; }
            public object? updatedBy { get; set; }
            public string name { get; set; }
            public object? purchasePrice { get; set; }
            public bool? IsDefault { get; set; }
            public int? discountPercentage { get; set; }
            public int? discountAmt { get; set; }
            public string shopSKU { get; set; }
        }

        public class Root
        {
            public string code { get; set; }
            public bool? success { get; set; }
            public string message { get; set; }
            public Data data { get; set; }
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
            public object? AttributeValueName { get; set; }
        }

    }
}
