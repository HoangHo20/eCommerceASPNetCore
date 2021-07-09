using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerceASPNetCore.Domain
{
    public class ProductImage
    {
        public int ID { get; set; }

        public string Url { get; set; }

        public virtual Product Product { get; set; }
    }
}
