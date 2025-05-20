using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercadoLivreAPI.Models
{
    public class Product
    {
        public string title { get; set; }
        public string category_id { get; set; }
        public decimal price { get; set; }
        public string currency_id { get; set; }
        public int available_quantity { get; set; }
        public string buying_mode { get; set; }
        public string condition { get; set; }
        public string listing_type_id { get; set; }
        public List<SaleTerm> sale_terms { get; set; }
        public List<Picture> pictures { get; set; }
        public List<ProductAttribute> attributes { get; set; }
    }
}
