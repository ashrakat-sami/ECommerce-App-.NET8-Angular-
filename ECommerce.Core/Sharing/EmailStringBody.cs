using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Sharing
{
    public class EmailStringBody
    {
        // This method is used to create the email body for the user
        public static string send(string email, string token, string component, string message)
        {
            // Token is used to verify the user and reset password
            // Component is used to identify the type of email (register, reset password, etc.)
            // Message is used to display the message in the email
            string encodeToken = Uri.EscapeDataString(token);
            return $@"
                    <html lang='en'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <style>
                            .button {{
                                border: none;
                                border-radius: 10px;
                                background: linear-gradient(45deg, #ff7e5f, #feb47b);
                                box-shadow: 0 4px 15px rgba(0, 0, 0, 0.2);
                                transition: all 0.3s ease;
                                cursor: pointer;
                                color: #fff;
                                padding: 15px 30px;
                                text-align: center;
                                text-decoration: none;
                                display: inline-block;
                                font-size: 16px;
                                font-family: 'Arial', sans-serif;
                                font-weight: bold;
                                animation: glow 1.5s infinite alternate;
                            }}
                        </style>
                    </head>
                        <body>
                            <h1>{message}</h1>
                                <hr>
                                <br>
                            <a class=""button"" href=""https://localhost:4200/Account/component?email={email}&code={encodeToken}"">{message}</a>
                        </body>
                    </html>
                    ";
        }
    }
}
