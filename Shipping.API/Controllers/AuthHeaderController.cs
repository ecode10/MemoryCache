using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Shipping.API.Token;

namespace Shipping.API.Controllers
{
    /// <summary>
    /// Classe que verifica a autenticidade do Token
    /// </summary>
    public class AuthHeaderController : Controller
    {
        /// <summary>
        /// Method that authenticate token sent by user
        /// </summary>
        /// <param name="req">HttpRequestHeaders</param>
        /// <returns>HttpStatusCode</returns>
        public HttpStatusCode CheckToken(HttpRequestHeaders req)
        {
            var headers = req; //this.Request.Headers;
            string token = string.Empty;
            string pwd = string.Empty;

            //check contain or not the header follow the parameters
            if (headers.Contains("token") && headers.Contains("pwd"))
            {
                token = headers.GetValues("token").First();
                pwd = headers.GetValues("pwd").First();
            }

            try
            {
                //check the token is the same
                //validate
                //Config.Development.json file 
                if (token.Equals(TokenWebAPI.publicToken) &&
                    pwd.Equals(TokenWebAPI.pwdToken))
                    return HttpStatusCode.Accepted;

                //not validate, unauthorize
                return HttpStatusCode.Unauthorized;
            }
            catch
            {
                return HttpStatusCode.Unauthorized;
            }
        }
    }
}
