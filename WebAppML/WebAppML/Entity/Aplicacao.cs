using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WebAppML.Entity
{
    public class Aplicacao
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("IdUsuario")]
        public long IdUsuario { get; set; }

        [BsonElement("Nome")]
        public string Nome { get; set; }

        [BsonElement("Senha")]
        public string Senha { get; set; }

        [BsonElement("AppId")]
        public string AppId { get; set; }

        [BsonElement("ChaveSecreta")]
        public string ChaveSecreta { get; set; }

        [BsonElement("RedirectURI")]
        public string RedirectURI { get; set; }

        [BsonElement("Codigo")]
        public string Codigo { get; set; }

        [BsonElement("Token")]
        public Token Token { get; set; }
    }
}
