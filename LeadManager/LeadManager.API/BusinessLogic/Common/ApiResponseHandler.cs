using AutoMapper;
using LeadManager.API.Configuration;
using LeadManager.Core.Entities;
using LeadManager.Core.Interfaces;
using LeadManager.Core.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace LeadManager.API.BusinessLogic.Common
{
    public class ApiResponseHandler : ControllerBase, IApiResponseHandler
    {
        private readonly IMapper _mapper;

        public ApiResponseHandler(IMapper mapper)
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
                return Ok(_mapper.Map<TOut>(searchResult));

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
               return CreatedAtRoute(route, new { id = Id }, _mapper.Map<TOut>(newSource));

            return BadRequest();
        }

        public void ReturnNotFoundIfEntityDoesNotExist<T>(object lookupResult)
        {
            if(lookupResult == null)      
                throw new HttpResponseException(statusCode:400,
                    new ErrorResponse { Error = $"An entity of type {typeof(T).Name} was not Found" }
                );
        }

        public void ReturnBadRequestIfNull<T>(object obj, string errorMessage)
        {
            if (obj == null)
                throw new HttpResponseException(statusCode: 400,
                    new ErrorResponse { Error = $"{typeof(T).Name} was null. {errorMessage}" }
                );
        }
    }

    
}
