using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BrainWare.Web.Controllers {

    public class OrderController : System.Web.Http.ApiController {
        [HttpGet]
        public async Task<IEnumerable<Models.Order>> GetOrders(int CompanyID = 1) {
            return await Data.Orders.GetForCompany(CompanyID);
        }
    }
}
