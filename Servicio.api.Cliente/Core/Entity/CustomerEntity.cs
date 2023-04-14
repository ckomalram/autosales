using MongoDB.Bson.Serialization.Attributes;
using Servicio.api.Cliente.Core.Entity.Generic;

namespace Servicio.api.Cliente.Core.Entity;

[BsonCollectionAttributes("TASCustomer")]
public class CustomerEntity : Document
{

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("lastname")]
    public string Lastname { get; set; }

    [BsonElement("email")]
    public string Email { get; set; }

    [BsonElement("cellphone")]
    public string Cellphone { get; set; }

    [BsonElement("address")]
    public string Address { get; set; }

    [BsonElement("status")]
    public bool Status { get; set; }




}
