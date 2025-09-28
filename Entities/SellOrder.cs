using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Formats.Asn1;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class SellOrder
    {
        [Key]
        public Guid? SellOrderID { get; set; }

        [Required(ErrorMessage = "Stock Symbol can't be blank")]
        [StringLength(50)]
        public string? StockSymbol { get; set; }

        [Required(ErrorMessage = "Stock Name can't be blank")]
        [StringLength(200)]
        public string? StockName { get; set; }

        public DateTime? DateAndTimeOfOrder { get; set; }

        [Range(1, 100000, ErrorMessage = "Quantity should be between 1 and 100000")]
        public uint Quantity { get; set; }

        [Range(1, 10000, ErrorMessage = "Price should be between 1 and 10000")]
        public double Price { get; set; }
    }
}
