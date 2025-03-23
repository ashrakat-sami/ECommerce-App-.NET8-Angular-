using ECommerce.Core.Entities.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.DTOs
{
    public record ProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public string CategoryName { get; set; }

        public virtual List<PhotoDto> Photos { get; set; }
    }

    public record PhotoDto
    {
        public string ImageName { get; set; }

        public int ProductId { get; set; }
    }

}
