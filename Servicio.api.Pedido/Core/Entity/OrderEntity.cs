using MongoDB.Bson.Serialization.Attributes;
using Servicio.api.Pedido.Core.Entity.Generic;

namespace Servicio.api.Pedido.Core.Entity;

[BsonCollectionAttributes("TASOrder")]
public class OrderEntity : Document
{
    [BsonElement("customerId")]
    public string CustomerId { get; set; }

    [BsonElement("products")]
    public IEnumerable<Product> products { get; set; }

    [BsonElement("status")]
    public bool Status { get; set; }

}

// TODO: Se debe relacionar al microservicio de productos.
public class Product
{
    public string Id { get; set; }
    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public string CategoryId { get; set; }

    public bool Status { get; set; }

}