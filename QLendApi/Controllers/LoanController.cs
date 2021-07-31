using Microsoft.AspNetCore.Mvc;

namespace QLendApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LoanController : ControllerBase
    {
        // POST /api/loan/apply
        [Route("apply")]
        [HttpPost]
        public ActionResult Apply()
        {
            return StatusCode(201);
        }

        // POST /api/loan/personalInfo1
        [Route("personalInfo1")]
        [HttpPost]
        public ActionResult PersonalInfo1()
        {
            return StatusCode(201);
        }

        // POST /api/loan/personalInfo2
        [Route("personalInfo2")]
        [HttpPost]
        public ActionResult PersonalInfo2()
        {
            return StatusCode(201);
        }

        // POST /api/loan/arc
        [Route("arc")]
        [HttpPost]
        public ActionResult Arc()
        {
            return StatusCode(201);
        }

        // POST /api/loan/signature
        [Route("signature")]
        [HttpPost]
        public ActionResult Signature()
        {
            return StatusCode(201);
        }

        // POST /api/loan/confirm
        [Route("confirm")]
        [HttpPost]
        public ActionResult Confirm()
        {
            return StatusCode(201);
        }

        // POST /api/loan/bankAccount
        [Route("bankAccount")]
        [HttpPost]
        public ActionResult bankAccount()
        {
            return StatusCode(201);
        }

        // POST /api/loan/success
        [Route("success")]
        [HttpPost]
        public ActionResult success()
        {
            return StatusCode(201);
        }
    }
}