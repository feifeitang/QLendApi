using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QLendApi.Dtos;
using QLendApi.lib;
using QLendApi.Models;

namespace QLendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly QLendDBContext _context;

        private readonly double _expireMins;

        public UserController(QLendDBContext context)
        {
            _context = context;
            _expireMins = 1;
        }

        // POST /api/user/signup
        [Route("signup")]
        [HttpPost]
        public async Task<ActionResult> SignupUser(SignupUserDto signupUser)
        {

            // check UINo if exist
            if (CertificatesUINoExists(signupUser.UINo))
            {
                return BadRequest("Exist UINo");
            }

            // check PhoneNumber if exist
            if (ForeignWorkersPhoneNumberExists(signupUser.PhoneNumber))
            {
                return BadRequest("Exist Phone Number");
            }

            Certificate certificate = new()
            {
                Uino = signupUser.UINo
            };

            var hashPwd = Crypt.Hash(signupUser.Password);

            ForeignWorker foreignWorker = new()
            {
                PhoneNumber = signupUser.PhoneNumber,
                Password = hashPwd,
                Uino = signupUser.UINo,
                Status = 1,
                RegisterTime = DateTime.UtcNow
            };

            _context.ForeignWorkers.Add(foreignWorker);
            _context.Certificates.Add(certificate);
            await _context.SaveChangesAsync();

            return StatusCode(201);
        }


        // POST /api/user/checkOTP
        [Route("checkOTP")]
        [HttpPost]
        public async Task<ActionResult> CheckOTP(CheckOtpDto checkOtpDto)
        {
            try
            {
                // check user exist, and get user data
                var foreignWorker = await _context.ForeignWorkers.FindAsync(checkOtpDto.Id);

                if (foreignWorker == null)
                {
                    return BadRequest("user not found");
                }

                // check send time and compare OTP number
                if (!CheckOTPSendTimeIsVaild(foreignWorker.OTPSendTIme.Value))
                {
                    return BadRequest("expire, try resend otp");
                }

                if (foreignWorker.OTP != checkOtpDto.OTP)
                {
                    return BadRequest("OTP not equal");
                }

                foreignWorker.Status = 2;

                _context.ForeignWorkers.Update(foreignWorker);

                await _context.SaveChangesAsync();

                return StatusCode(201);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex);
            }
        }

        private bool ForeignWorkersPhoneNumberExists(string phoneNumber)
        {
            return _context.ForeignWorkers.Any(e => e.PhoneNumber == phoneNumber);
        }

        private bool CertificatesUINoExists(string Uino)
        {
            return _context.Certificates.Any(e => e.Uino == Uino);
        }

        private bool CheckOTPSendTimeIsVaild(DateTime sendTime)
        {
            // sendTime need add _expireMins
            var expireTime = sendTime.AddMinutes(this._expireMins);
            DateTime currentTime = DateTime.UtcNow;
            int result = DateTime.Compare(expireTime, currentTime);

            bool res = true;

            if (result < 0)
                res = false;

            return res;
        }
    }
}