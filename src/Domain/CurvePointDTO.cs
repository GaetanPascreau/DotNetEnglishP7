using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Domain
{
    public class CurvePointDTO
    {
        public int Id { get; set; }
        public int CurveId { get; set; }
        public double Term { get; set; }
        public double Value { get; set; }
    }
}
