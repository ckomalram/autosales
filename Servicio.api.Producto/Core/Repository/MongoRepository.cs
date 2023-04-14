using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Servicio.api.Producto.Core.Entity;
using Servicio.api.Producto.Core.Entity.Generic;

namespace Servicio.api.Producto.Core.Repository;
public class MongoRepository<TDocument> : IMongoRepository<TDocument> where TDocument : IDocument
{

    private readonly IMongoCollection<TDocument> collectionName;
    private readonly IMongoDatabase database;
    public MongoRepository(IOptions<MongoSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        database = client.GetDatabase(options.Value.Database);
        collectionName = database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
    }

    // Metodo privado que retorna el nombre de la coleccion pasandole el document type.    
    private protected string GetCollectionName(Type documentType)
    {
        return ((BsonCollectionAttributes)documentType.GetCustomAttributes(typeof(BsonCollectionAttributes), true).FirstOrDefault()).CollectionName;
    }

    public async Task<IEnumerable<TDocument>> GetAll()
    {
        var filter = Builders<TDocument>.Filter.Empty;
        var cursor = await collectionName.FindAsync(filter);
        var response = await cursor.ToListAsync();
        return response;
    }

    public async Task<TDocument> GetById(string Id)
    {
        var filter = Builders<TDocument>.Filter.Eq(document => document.Id, Id);
        var response = await collectionName.Find(filter).SingleOrDefaultAsync();
        return response;
    }

    public async Task InsertDocument(TDocument document)
    {
        // document.CreatedDated = DateTime.Now;
        // document.UpdatedDated = DateTime.Now;
        await collectionName.InsertOneAsync(document);
    }

    public async Task UpdateDocument(TDocument document)
    {
        // document.UpdatedDated = DateTime.Now;
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
        await collectionName.FindOneAndReplaceAsync(filter, document);
    }

    public async Task DeleteDocument(string Id)
    {
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, Id);
        await collectionName.FindOneAndDeleteAsync(filter);
    }


    public async Task<PaginationEntity<TDocument>> PaginationByFilter(PaginationEntity<TDocument> pagination)
    {
        var sort = Builders<TDocument>.Sort.Ascending(pagination.Sort);
        var emptyFilter = Builders<TDocument>.Filter.Empty;
        var skip = ((pagination.Page - 1) * pagination.PageSize);

        // Obtener count de toda la colección
        var totalDocument = 0;


        if (pagination.SortDirection == "desc")
        {
            sort = Builders<TDocument>.Sort.Descending(pagination.Sort);
        }

        if (pagination.FilterValue == null)
        {

            var rta = await collectionName
                            .Find(emptyFilter)
                            .Sort(sort)
                            .Skip(skip)
                            .Limit(pagination.PageSize)
                            .ToListAsync();
            pagination.Data = rta;

            totalDocument = (await collectionName
                            .Find(emptyFilter)
                            .ToListAsync()).Count();
        }
        else
        {
            // Expresion regular con valores que coincidadn
            var customFilterValue = ".*" + pagination.FilterValue.Valor + ".*";
            var customFilter = Builders<TDocument>.Filter.Regex(pagination.FilterValue.Propiedad, new MongoDB.Bson.BsonRegularExpression(customFilterValue, "i"));

            var rta = await collectionName
                .Find(customFilter)
                .Sort(sort)
                .Skip(skip)
                .Limit(pagination.PageSize)
                .ToListAsync();
            pagination.Data = rta;

            totalDocument = (await collectionName
                .Find(customFilter)
                .ToListAsync()).Count();
        }

        // long totalDocument = await collectionname.CountDocumentsAsync(emptyFilter);
        var rounded = Math.Ceiling(totalDocument / Convert.ToDecimal(pagination.PageSize));
        var totalPages = Convert.ToInt32(rounded);

        pagination.PagesQuantity = totalPages;
        pagination.TotalRows = totalDocument;

        return pagination;
    }
}

public interface IMongoRepository<TDocument> where TDocument : IDocument
{
    Task<IEnumerable<TDocument>> GetAll();
    Task<TDocument> GetById(string Id);
    Task InsertDocument(TDocument document);
    Task UpdateDocument(TDocument document);
    Task DeleteDocument(string Id);

    Task<PaginationEntity<TDocument>> PaginationByFilter(
PaginationEntity<TDocument> pagination
);
}