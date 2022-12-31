using Amazon.Runtime.Internal.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Podcasts;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using Stripe;
using System;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/podcasts")]
    [ApiController]
    public class PodcastApiController : BaseApiController
    {
        private IPodcastService _podcastService = null;
        private IAuthenticationService<int> _authService = null;

        public PodcastApiController(IPodcastService podcastService, IAuthenticationService<int> authService, ILogger<PodcastApiController> logger) : base(logger)
        {
            _podcastService = podcastService;
            _authService = authService;

        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<ItemsResponse<Podcast>> GetAll()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<Podcast> podcasts = _podcastService.GetAllPodcast();

                if(podcasts == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemsResponse<Podcast> { Items = podcasts };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(code, response);
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(PodcastAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                int id = _podcastService.AddPodcast(model, userId);

                ItemResponse<int> response = new ItemResponse<int>() { Item = id };
                result = Created201(response);
            }
            catch(Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(PodcastUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _podcastService.UpdatePodcast(model);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(code, response);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            {
                int code = 200;
                BaseResponse response = null;

                try
                {
                    _podcastService.DeletePodcast(id);

                    response = new SuccessResponse();
                }
                catch (Exception ex)
                {
                    code = 500;
                    response = new ErrorResponse(ex.Message);
                    base.Logger.LogError(ex.ToString());
                }
                return StatusCode(code, response);
            }
        }

    }
}
