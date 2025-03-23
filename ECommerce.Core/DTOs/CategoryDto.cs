using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.DTOs
{
   public record CategoryDto
   (
       string Name,
       string Description
   );

    public record CategoryUpdateDto
   (
       string Name,
       string Description,
       int Id   
   );
}
