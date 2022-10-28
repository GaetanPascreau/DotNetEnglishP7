using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Domain
{
    public class CurvePointDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="must not be null")]
        public int CurveId { get; set; }

        [Required(ErrorMessage = "Term is mandatory")]
        public decimal Term { get; set; }

        [Required(ErrorMessage = "Value is mandatory")]
        public decimal Value { get; set; }

        public DateTime AsOfDate { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
