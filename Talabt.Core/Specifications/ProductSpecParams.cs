using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabt.Core.Specifications
{
    public class ProductSpecParams
    {
        public string? Sort {  get; set; }
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        private int pageSize=5;
        public int PageSize {
            get { return pageSize; }
            set { PageSize = value > 10 ? 10 : value; }
        }
        public int PageIndex { get; set; } = 1;
    }
}
