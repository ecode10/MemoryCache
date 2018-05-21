namespace Shipping.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Configuration;
    using System.Net.Http;
    using System.Net;
    using Shipping.API.Model;

    [Produces("application/json")]
    [Route("api/ShippingService")]
    public class ShippingController : Controller
    {
        #region private members

        /// <summary>
        /// Fake DHL client. Used to simulate calls to UPS service.
        /// </summary>
        private readonly DHL.Fake.Clients.PickupClient dhlPickupClient = new DHL.Fake.Clients.PickupClient();

        /// <summary>
        /// Fake UPS client. Used to simulate calls to UPS service.
        /// </summary>
        private readonly Ups.Fake.Clients.PickupService upsPickupClient = new Ups.Fake.Clients.PickupService();

        /// <summary>
        /// Configuration variavel
        /// .json file
        /// </summary>
        public static IConfiguration Configuration { get; set; }

        private readonly bool useCache;

        private static MemoryCache cache = new MemoryCache(new MemoryCacheOptions());

        #endregion private members

        #region Ctors

        public ShippingController()
        {
            //get the configuration cache by configuration
            //see on the configuration file and startup.cs
            this.useCache = Boolean.Parse(Configuration["Cache:Enabled"]);
        }

        #endregion Ctors

        #region Actions

        [Route("GetPickups")]
        [HttpPost]
        public IEnumerable<Pickup> GetPickup([FromBody]string shipperId)
        {
            //check the token - you can enbale the token to use this web api
            //checkToken();

            if (shipperId == Shipper.DHL)
            {
                return GetDHLPickups();
            }
            else if (shipperId == Shipper.UPS)
            {
                return GetUPSPickups();
            }

            throw new IndexOutOfRangeException($"Shipper unknown: {shipperId}. Please try again.");

        }

        /// <summary>
        /// Method private - Get the pickups of DHL
        /// Using cashe if is enabled.
        /// </summary>
        /// <returns>IEnumerable</returns>
        private IEnumerable<Pickup> GetDHLPickups()
        {
            if (useCache && cache.Count > 0)
            {
                var result = cache.Get("dhl-pickups") as List<Pickup>;

                if (result != null && result.Any())
                {
                    return result;
                }

            }

            Task<IEnumerable<DHL.Fake.Clients.PickupSlot>> t = this.dhlPickupClient.GetPickupSlots();

            var dhlPickups = t.GetAwaiter().GetResult();

            var pickups = dhlPickups.Select(i => new Pickup
            {
                MinDate = i.BookDate,
                MaxDate = i.BookDate,
                ShipperId = Shipper.DHL
            }).ToList();

            if (this.useCache && cache.Count == 0)
                cache.Set("dhl-pickups", pickups);

            return pickups;
        }

        /// <summary>
        /// Method private that get the pickups from UPS
        /// Direct of data
        /// </summary>
        /// <returns>IEnumerable</returns>
        private IEnumerable<Pickup> GetUPSPickups()
        {
            IEnumerable<Ups.Fake.Clients.PickupBook> upsPickups = this.upsPickupClient.GetBookedPickups();

            return upsPickups.Select(i => new Pickup
            {
                MinDate = i.FromDate,
                MaxDate = i.ToDate,
                ShipperId = Shipper.UPS
            });
        }

        /// <summary>
        /// CheckToken sent - its necessary to send paramaters in the header request
        /// parameters: token and pwd
        /// </summary>
        private void checkToken()
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage();
            AuthHeaderController authHeader = new AuthHeaderController();

            //check parameters and compare
            if (authHeader.CheckToken(httpRequest.Headers) == HttpStatusCode.Unauthorized)
            {
                throw new Exception("Unauthorized: - " + HttpStatusCode.Unauthorized);
            }
        }

        #endregion Actions
    }

    
    
}