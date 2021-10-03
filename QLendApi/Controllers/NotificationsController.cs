using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QLendApi.Dtos;
using QLendApi.Models;
using QLendApi.Repositories;
using QLendApi.Responses;
using QLendApi.Services;
using QLendApi.Settings;

namespace QLendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IForeignWorkerRepository foreignWorkerRepository;

        public NotificationsController(
            INotificationService notificationService,
            IForeignWorkerRepository foreignWorkerRepository
        )
        {
            _notificationService = notificationService;

            this.foreignWorkerRepository = foreignWorkerRepository;
        }

        [HttpPut]
        [Route("installations")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> UpdateInstallation(
            [Required] DeviceInstallation deviceInstallation)
        {
            try
            {
                // Console.WriteLine("deviceInstallation {0}", deviceInstallation);
                string json = JsonConvert.SerializeObject(deviceInstallation);
                Console.WriteLine("deviceInstallation {0}", json);

                var foreignWorker = await foreignWorkerRepository.GetByIdAsync(deviceInstallation.UserId);

                if (foreignWorker == null)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10003,
                        Message = "user not found"
                    });
                }

                if (foreignWorker.DeviceTag == null)
                {
                    Console.WriteLine("DeviceTag == null");
                    // add national info into tag ?
                    foreignWorker.DeviceTag = "user_" + foreignWorker.Id;

                    await foreignWorkerRepository.UpdateAsync(foreignWorker);
                }

                var d = new DeviceInstallation
                {
                    InstallationId = deviceInstallation.InstallationId,
                    Platform = deviceInstallation.Platform,
                    PushChannel = deviceInstallation.PushChannel,
                    UserId = deviceInstallation.UserId,
                    Tags = new List<string>()
                };

                d.Tags.Add(foreignWorker.DeviceTag);
                string dString = JsonConvert.SerializeObject(d);
                Console.WriteLine("deviceTag {0}", dString);

                var success = await _notificationService
                    .CreateOrUpdateInstallationAsync(d, HttpContext.RequestAborted);

                Console.WriteLine("success {0}", success);

                if (!success)
                    return new UnprocessableEntityResult();

                return new OkResult();
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90900,
                    Message = $"notifications installations api error:{ex}"
                });
            }
        }

        // [HttpDelete()]
        // [Route("installations/{installationId}")]
        // [ProducesResponseType((int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        // [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        // public async Task<ActionResult> DeleteInstallation(
        //    [Required][FromRoute] string installationId)
        // {
        //     var success = await _notificationService
        //         .DeleteInstallationByIdAsync(installationId, CancellationToken.None);

        //     if (!success)
        //         return new UnprocessableEntityResult();

        //     return new OkResult();
        // }

        [HttpPost]
        [Route("requests")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> RequestPush(
            [Required] NotificationRequest notificationRequest)
        {
            try
            {
                if ((notificationRequest.Silent &&
                string.IsNullOrWhiteSpace(notificationRequest?.Action)) ||
                (!notificationRequest.Silent &&
                string.IsNullOrWhiteSpace(notificationRequest?.Text)))
                return new BadRequestResult();

                var success = await _notificationService
                    .RequestNotificationAsync(notificationRequest, HttpContext.RequestAborted);

                if (!success)
                    return new UnprocessableEntityResult();

                return new OkResult();
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 91000,
                    Message = $"notifications requests api error:{ex}"
                });
            }
        }
    }
}