using System;
using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using NaturalUruguayGateway.NaturalUruguayEngineInterface.Services;
using NaturalUruguayGateway.WebApi.Filters;

namespace NaturalUruguayGateway.WebApi.Controllers
{
    [Route("regions")]
    public class RegionsController : BaseApiController
    {
        private readonly IRegionsService service;

        public RegionsController(IRegionsService service)
        {
            this.service = service;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetRegionsAsync([FromQuery] PagingModel pagingModel)
        {
            try
            {
                var paginatedRegions = await service.GetRegionsAsync(pagingModel);
                return Ok(paginatedRegions);
            }
            catch (Exception ex)
            {
                var exceptionResponse = GetExceptionResponse(ex, Request);
                return exceptionResponse;
            }
        }
        
        [AuthorizationFilter(Roles = new [] { "Super admin" })]
        [Route("{id}/spots")]
        [HttpPost]
        public async Task<IActionResult> AddSpotToRegionAsync(int id, [FromBody] Spot spotModel)
        {
            try
            {
                spotModel.RegionId = id;
                var spot = await service.AddSpotToRegionAsync(spotModel);
                return Ok(spot);
            }
            catch (Exception ex)
            {
                var exceptionResponse = GetExceptionResponse(ex, Request);
                return exceptionResponse;
            }
        }
        
        [Route("{id}/spots")]
        [HttpGet]
        public async Task<IActionResult> GetSpotsByRegionAsync(int id, [FromQuery] PagingModel pagingModel)
        {
            try
            {
                PaginatedModel<Spot> paginatedLSpots  = await service.GetSpotsByRegionAsync(id, pagingModel);
                return Ok(paginatedLSpots);
            }
            catch (Exception ex)
            {
                var exceptionResponse = GetExceptionResponse(ex, Request);
                return exceptionResponse;
            }
        }
    }
}