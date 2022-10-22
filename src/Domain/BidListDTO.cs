using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Domain
{
    public class CurvePointtDTO
    {
        public int BidListId { get; set; }

        [Required(ErrorMessage ="Account is mandatory")]
        public string Account { get; set; }

        [Required(ErrorMessage = "Type is mandatory")]
        public string Type { get; set; }

        public decimal BidQuantity { get; set; }
        public DateTime BidListDate { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime RevisionDate { get; set; }
    }
}
