using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Sharing
{
    public class ProductParam
    {
        public string Sort { get; set; } = "ByName";

        public int? CategoryId { get; set; }
        public string Search { get; set; }="";
        public int MaxPageSize { get; set; } = 6;


        private int pageSize=3;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value>MaxPageSize ?MaxPageSize:value; }
        }



        public int PageNumber { get; set; } = 1;




    }
}
