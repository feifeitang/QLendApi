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

        public UserController(QLendDBContext context)
        {
            _context = context;
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

            ForeignWorker foreignWorkers = new()
            {
                PhoneNumber = signupUser.PhoneNumber,
                Password = hashPwd,
                Uino = signupUser.UINo,
                Status = 1,
                RegisterTime = DateTime.UtcNow
            };

            _context.ForeignWorkers.Add(foreignWorkers);
            _context.Certificates.Add(certificate);
            await _context.SaveChangesAsync();

            return StatusCode(201);
        }

        private bool ForeignWorkersPhoneNumberExists(string phoneNumber)
        {
            return _context.ForeignWorkers.Any(e => e.PhoneNumber == phoneNumber);
        }

        private bool CertificatesUINoExists(string Uino)
        {
            return _context.Certificates.Any(e => e.Uino == Uino);
        }

        // POST /api/user/sendOTP
        [Route("sendOTP")]
        [HttpPost]
        public async Task<ActionResult> SendOTP(SendOtpDto sendOtpDto)
        {
            // check user exist, and get user data
            var foreignWorker = await _context.ForeignWorkers.FindAsync(sendOtpDto.Id);

            if (foreignWorker == null)
            {
                return BadRequest("user not found");
            }

            Random rnd = new Random();
            int OTP = rnd.Next(100000,999999);

            foreignWorker.OTP = OTP;
            foreignWorker.OTPSendTIme = DateTime.UtcNow;

            _context.ForeignWorkers.Update(foreignWorker);

            await _context.SaveChangesAsync();

            return StatusCode(201);
        }
    }
}