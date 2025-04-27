using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.DTOs
{
    public class EmailDTO
    {
        // This class is used to send the email
        public EmailDTO(string to, string from, string subject, string content)
        {
            To = to;
            From = from;
            Subject = subject;
            Content = content;
        } 
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string Body { get; set; }
    }
}
