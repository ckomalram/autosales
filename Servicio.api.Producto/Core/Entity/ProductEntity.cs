namespace Servicio.api.Producto.Core.Entity;

using MongoDB.Bson.Serialization.Attributes;
using Servicio.api.Producto.Core.Entity.Generic;

[BsonCollectionAttributes("TASProduct")]
public class ProductEntity : Document
{

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("description")]
    public string Description { get; set; }

    [BsonElement("price")]
    public decimal Price { get; set; }

    [BsonElement("stock")]
    public int Stock { get; set; }

    [BsonElement("categoryId")]
    public string CategoryId { get; set; }

    [BsonElement("status")]
    public bool Status { get; set; }




}
