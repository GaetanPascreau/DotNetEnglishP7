using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Domain
{
    public class TradeDTO
    {
        [Required(ErrorMessage = "must not be null")]
        public int TradeId { get; set; }

        [Required(ErrorMessage = "Account is mandatory")]
        public string Account { get; set; }

        [Required(ErrorMessage = "Type is mandatory")]
        public string Type { get; set; }

        public decimal? BuyQuantity { get; set; }

        public decimal? SellQuantity { get; set; }

        public decimal? BuyPrice { get; set; }

        public decimal? SellPrice { get; set; }

        public DateTime TradeDate { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime RevisionDate { get; set; }
    }  
}
