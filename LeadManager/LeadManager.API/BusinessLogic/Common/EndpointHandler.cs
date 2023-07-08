using AutoMapper;
using LeadManager.Core.Interfaces;
using LeadManager.Core.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace LeadManager.API.BusinessLogic.Common
{
    public class EndpointHandler : ControllerBase, IApiEndpointHandler
    {
        private readonly IMapper _mapper;

        public EndpointHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public bool IsValidEntitySearchResult<T>(object lookupResult)
        {
            return lookupResult is T && lookupResult != null;
        }

        public IActionResult ReturnSearchResult<Ts, TOut>(object searchResult)
        {
            if (searchResult is Ts && searchResult != null)
                return Ok(_mapper.Map<Ts>(searchResult));

            return NotFound();
        }

        public IActionResult ReturndUpdateResult(bool updateSuccess)
        {
            if (updateSuccess)
                return NoContent();

            return BadRequest();
        }

        public IActionResult ReturnDeleteResult(bool deleteSuccess)
        {
            if (deleteSuccess)
                return NoContent();

            return BadRequest();
        }

        public IActionResult ReturnCreateResult<TOut>(bool createdResult,
                                                string route,
                                                string Id,
                                                object newSource)
        {
            if (createdResult)
               return CreatedAtRoute("GetSource", new { id = Id }, _mapper.Map<TOut>(newSource));

            return BadRequest();
        }
    }
}
