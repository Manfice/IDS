using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IndDev.Domain.Abstract;

namespace IndDev.Controllers
{
    [Authorize(Roles = "A,O,M")]
    public class CrmController : ApiController
    {
        private readonly ICrm _repo;

        public CrmController(ICrm repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IHttpActionResult GetCompanys()
        {
            return Ok(_repo.Company);
        }

    }
}
