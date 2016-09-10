using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using IndDev.Domain.Abstract;
using IndDev.Domain.ViewModels;

namespace IndDev.Controllers
{
    public class FeedbackController : ApiController
    {
        private readonly IMailRepository _mail;

        public FeedbackController(IMailRepository mailRepository)
        {
            _mail = mailRepository;
        }
        [HttpPost]
        public async Task<IHttpActionResult> CallMe(Feedback feedback)
        {
            var number = Regex.Match(feedback.Phone, @"^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{4})(?: *x(\d+))?\s*$");
            if (!number.Success) return BadRequest();
            await _mail.FeedbackAsync(feedback);
            return Ok(feedback);
        }

    }
}

