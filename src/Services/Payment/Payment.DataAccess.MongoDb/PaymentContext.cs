using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Payment.Domain;

namespace Payment.DataAccess.MongoDb
{
    public class PaymentContext
    {
        private readonly IMongoDatabase _database = null;

        public PaymentContext()
        {
            // todo: settings move to config
            // docker run -p:27017:27017 --name MongoDb -d mongo
            // docker exec -it MongoDb mongo
            var client = new MongoClient("mongodb://10.0.75.1:27017");

            var pack = new ConventionPack();
            pack.Add(new IgnoreExtraElementsConvention(true));
            pack.Add(new EnumRepresentationConvention(BsonType.String));
            pack.Add(new CamelCaseElementNameConvention());

            BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));
            BsonSerializer.RegisterSerializer(typeof(decimal?), new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));
            ConventionRegistry.Register("Payment API Conventions", pack, t => true);

            _database = client?.GetDatabase("paymentsDb");
        }

        public IMongoCollection<Account> Accounts => _database.GetCollection<Account>("Account");

        public IMongoCollection<Remittance> Remittances => _database.GetCollection<Remittance>("Remittance");
    }
}
