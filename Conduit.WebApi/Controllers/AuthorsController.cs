
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Conduit.Business.Helpers;
using Conduit.Business.Messages;
using Conduit.Business.Services;
using Conduit.Common.Dto;
using Conduit.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Conduit.WebApi.Extensions;
namespace Conduit.WebApi.Controllers
{
    //1.Full Path Link cho author
    //http://localhost:6058/api/authors?pagenumber=1&pagesize=2&orderby=genre&fields=name,id&searchquery=g
    //Da sua Route: http://localhost:58161/api/Authors/GetAuthors?pagenumber=2&pagesize=2&orderby=genre&fields=name,id&searchquery=g
    //2.Ghi log  _logger.LogError("OK");
    //3.Phan trang EF
    //https://gunnarpeipman.com/net/ef-core-paging/
    //https://www.carlrippon.com/scalable-and-performant-asp-net-core-web-apis-paging/
    //Nen dung Dapper: https://github.com/StackExchange/Dapper
    //[Route("api/authors")]
    [Route("api/[controller]")]
    public class AuthorsController : Controller
    {
        private ILibraryRepository _libraryRepository;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;
        ILogger<AuthorsController> _logger;
        private IMapper _mapper;
        public AuthorsController(ILibraryRepository libraryRepository,IMapper mapper,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService, ILogger<AuthorsController> logger)
        {
            _libraryRepository = libraryRepository;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
            _logger = logger;
            _mapper = mapper;
        }
        //Chú ý nên đặt [Route("GetAuthors")] thay cho Name trong Http như [HttpGet(Name = "GetAuthors")]
        [HttpGet]//Dùng Get để lấy danh sách
        [HttpHead]//Dùng Head để lấy danh sách
        [Route("GetAuthors")]//Đặt tên cho Route cần truy cập
        public async Task<IActionResult> GetAuthors(AuthorsResourceParameters authorsResourceParameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {

            //Chạy web dạng console sẽ thấy log ra
            _logger.LogError("OK");
            //Kiểm tra tham số tên cần sắp xếp(order by) có phải thuộc tính của Author
            if (!_propertyMappingService.ValidMappingExistsFor<AuthorDto, Author>
               (authorsResourceParameters.OrderBy))
            {

                return BadRequest(this.BadRequestOrderByExtention<Author>(authorsResourceParameters.OrderBy));
            }
            //Kiểm tra tham số tên cần lấy có phải thuộc tính của Author
            if (!_typeHelperService.TypeHasProperties<AuthorDto>
                (authorsResourceParameters.Fields))
            {
                return BadRequest(this.BadRequestNotFindFieldExtention<Author>());
            }
            //Lấy dữ liệu từ tham số truyền vào
            var authorsFromRepo = await _libraryRepository.GetAuthors(authorsResourceParameters);

            //Áp dụng khi dùng Mapper mà cấu hình trực tiếp trong StartUp, bên dưới là cấu hình profile
            //var authors = Mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo);
            //Khi dùng Profile Mapper thì là _mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo);
            var authors = _mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo);
            
            if (mediaType == "application/vnd.marvin.hateoas+json")
            {
                var paginationMetadata = new
                {
                    totalCount = authorsFromRepo.TotalCount,
                    pageSize = authorsFromRepo.PageSize,
                    currentPage = authorsFromRepo.CurrentPage,
                    totalPages = authorsFromRepo.TotalPages,
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                var links = CreateLinksForAuthors(authorsResourceParameters,
                    authorsFromRepo.HasNext, authorsFromRepo.HasPrevious);

                var shapedAuthors = authors.ShapeData(authorsResourceParameters.Fields);

                var shapedAuthorsWithLinks = shapedAuthors.Select(author =>
                {
                    var authorAsDictionary = author as IDictionary<string, object>;
                    var authorLinks = CreateLinksForAuthor(
                        (Guid)authorAsDictionary["Id"], authorsResourceParameters.Fields);

                    authorAsDictionary.Add("links", authorLinks);

                    return authorAsDictionary;
                });

                var linkedCollectionResource = new
                {
                    value = shapedAuthorsWithLinks,
                    links = links
                };

                return Ok(linkedCollectionResource);
            }
            else
            {
                var previousPageLink = authorsFromRepo.HasPrevious ?
                    CreateAuthorsResourceUri(authorsResourceParameters,
                    ResourceUriType.PreviousPage) : null;

                var nextPageLink = authorsFromRepo.HasNext ?
                    CreateAuthorsResourceUri(authorsResourceParameters,
                    ResourceUriType.NextPage) : null;

                var paginationMetadata = new
                {
                    previousPageLink = previousPageLink,
                    nextPageLink = nextPageLink,
                    totalCount = authorsFromRepo.TotalCount,
                    pageSize = authorsFromRepo.PageSize,
                    currentPage = authorsFromRepo.CurrentPage,
                    totalPages = authorsFromRepo.TotalPages
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                return Ok(this.OkDefaultExtention<IEnumerable<ExpandoObject>>(authors.ShapeData(authorsResourceParameters.Fields)));
            }
        }

        [HttpGet]
        [Route("GetAuthor/{id}")]
        public async Task<IActionResult> GetAuthor(Guid id, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<AuthorDto>
              (fields))
            {
                return BadRequest(this.BadRequestNotFindFieldExtention<Author>());
            }

            var authorFromRepo = await _libraryRepository.GetAuthor(id);

            if (authorFromRepo == null)
            {
                //return NotFound();
                return BadRequest(this.BadRequestNotDataExtention<Author>(id));
            }

            var author = _mapper.Map<AuthorDto>(authorFromRepo);

            var links = CreateLinksForAuthor(id, fields);

            var linkedResourceToReturn = author.ShapeData(fields)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return Ok(this.OkDefaultExtention<ExpandoObject>(linkedResourceToReturn));
        }

        [HttpPost]
        [Route("CreateAuthor")]
        //[RequestHeaderMatchesMediaType("Content-Type",
        //    new[] { "application/vnd.marvin.author.full+json" })]
        public IActionResult CreateAuthor([FromBody] AuthorForCreationDto author)
        {
            if (author == null)
            {
                return BadRequest(this.BadRequestInvalidFormExtention<Author>(new UnprocessableEntityObjectResult(ModelState)));
            }
            //Kiểm tra model AuthorForCreationDto có họp lệ không
            if (!ModelState.IsValid)
            {
                return BadRequest(this.BadRequestInvalidFormExtention<Author>(new UnprocessableEntityObjectResult(ModelState)));
            }
            var authorEntity = _mapper.Map<Author>(author);

            _libraryRepository.AddAuthor(authorEntity);

            if (!_libraryRepository.Save())
            {
                //throw new Exception("Creating an author failed on save.");
                // return StatusCode(500, "A problem happened with handling your request.");
                return BadRequest(this.BadRequestDefaultExtention<Author>());
            }

            var authorToReturn = _mapper.Map<AuthorDto>(authorEntity);

            var links = CreateLinksForAuthor(authorToReturn.Id, null);

            var linkedResourceToReturn = authorToReturn.ShapeData(null)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

//            return CreatedAtRoute("GetAuthor",
//                new { id = linkedResourceToReturn["Id"] },
//                linkedResourceToReturn);
            return Ok(this.OkDefaultExtention<ExpandoObject>(linkedResourceToReturn));
        }


        [HttpPost]
        [Route("CreateAuthorWithDateOfDeath")]
        //[RequestHeaderMatchesMediaType("Content-Type",
        //    new[] { "application/vnd.marvin.authorwithdateofdeath.full+json",
        //            "application/vnd.marvin.authorwithdateofdeath.full+xml" })]
        // [RequestHeaderMatchesMediaType("Accept", new[] { "..." })]
        public IActionResult CreateAuthorWithDateOfDeath(
            [FromBody] AuthorForCreationWithDateOfDeathDto author)
        {
            if (author == null)
            {
                return BadRequest(this.BadRequestInvalidFormExtention<Author>(new UnprocessableEntityObjectResult(ModelState)));
            }
            //Kiểm tra model AuthorForCreationWithDateOfDeathDto có họp lệ không
            if (!ModelState.IsValid)
            {
                return BadRequest(this.BadRequestInvalidFormExtention<Author>(new UnprocessableEntityObjectResult(ModelState)));
            }
            var authorEntity = _mapper.Map<Author>(author);

            _libraryRepository.AddAuthor(authorEntity);

            if (!_libraryRepository.Save())
            {
                return BadRequest(this.BadRequestDefaultExtention<Author>());
                // return StatusCode(500, "A problem happened with handling your request.");
            }

            var authorToReturn = _mapper.Map<AuthorDto>(authorEntity);

            var links = CreateLinksForAuthor(authorToReturn.Id, null);

            var linkedResourceToReturn = authorToReturn.ShapeData(null)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

//            return CreatedAtRoute("GetAuthor",
//                new { id = linkedResourceToReturn["Id"] },
//                linkedResourceToReturn);
            return Ok(this.OkDefaultExtention<ExpandoObject>(linkedResourceToReturn));
        }


        [HttpPut]
        [Route("UpdateAuthor/{authorId}")]
        public async Task<IActionResult> UpdateAuthor(Guid authorId,
            [FromBody] AuthorForCreationWithDateOfDeathDto author)
        {
            if (author == null)
            {
                return BadRequest(this.BadRequestInvalidFormExtention<Author>(new UnprocessableEntityObjectResult(ModelState)));
            }

//            if (book.Description == book.Title)
//            {
//                ModelState.AddModelError(nameof(BookForUpdateDto),
//                    "The provided description should be different from the title.");
//            }

            if (!ModelState.IsValid)
            {
                return BadRequest(this.BadRequestInvalidFormExtention<Author>(new UnprocessableEntityObjectResult(ModelState)));
            }


            if (!_libraryRepository.AuthorExists(authorId))
            {
                //return NotFound();
                return BadRequest(this.BadRequestNotDataExtention<Author>(authorId));
            }

            var authorReturn =await _libraryRepository.GetAuthor(authorId);
            if (authorReturn == null)
            {
                var authorAdd = _mapper.Map<Author>(author);
                authorAdd.Id = authorId;

                _libraryRepository.AddAuthor(authorId, authorAdd);

                if (!_libraryRepository.Save())
                {
                    //throw new Exception($"Upserting book {authorId} for author {authorId} failed on save.");
                    return BadRequest(this.BadRequestDefaultExtention<Author>());
                }

                var bookToReturn = _mapper.Map<BookDto>(authorAdd);
//
//                return CreatedAtRoute("GetBookForAuthor",
//                    new { authorId = authorId, id = bookToReturn.Id },
//                    bookToReturn);
                return Ok(this.OkDefaultExtention<ExpandoObject>(bookToReturn));
            }

            _mapper.Map(author, authorReturn);

            _libraryRepository.UpdateAuthor(authorReturn);

            if (!_libraryRepository.Save())
            {
                //throw new Exception($"Updating book {authorId} for author {authorId} failed on save.");
                return BadRequest(this.BadRequestDefaultExtention<Author>());
            }

            //return NoContent();
            return Ok(this.OkDefaultExtention<ExpandoObject>(authorReturn));
        }

        [HttpPost]
        [Route("BlockAuthorCreation/{id}")]
        public IActionResult BlockAuthorCreation(Guid id)
        {
            if (_libraryRepository.AuthorExists(id))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            return NotFound();
        }

        [HttpDelete]
        [Route("DeleteAuthor/{id}")]
        public async Task<IActionResult> DeleteAuthor(Guid id)
        {
            var authorFromRepo = await _libraryRepository.GetAuthor(id);
            if (authorFromRepo == null)
            {
                //return NotFound();
                return BadRequest(this.BadRequestNotDataExtention<Author>(id));
            }

            _libraryRepository.DeleteAuthor(authorFromRepo);

            if (!_libraryRepository.Save())
            {
                //throw new Exception($"Deleting author {id} failed on save.");
                return BadRequest(this.BadRequestDefaultExtention<Author>());
            }

            //return NoContent();
            return Ok(this.OkDefaultExtention<Author>(authorFromRepo));
        }
        //Tạo tất cả link tương tác với model
        private IEnumerable<LinkDto> CreateLinksForAuthor(Guid id, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                  new LinkDto(_urlHelper.Link("GetAuthor", new { id = id }),
                  "self",
                  "GET"));
            }
            else
            {
                links.Add(
                  new LinkDto(_urlHelper.Link("GetAuthor", new { id = id, fields = fields }),
                  "self",
                  "GET"));
            }

            links.Add(
              new LinkDto(_urlHelper.Link("DeleteAuthor", new { id = id }),
              "delete_author",
              "DELETE"));

            links.Add(
              new LinkDto(_urlHelper.Link("CreateBookForAuthor", new { authorId = id }),
              "create_book_for_author",
              "POST"));

            links.Add(
               new LinkDto(_urlHelper.Link("GetBooksForAuthor", new { authorId = id }),
               "books",
               "GET"));

            return links;
        }
        //Tạo link phân trang
        private IEnumerable<LinkDto> CreateLinksForAuthors(
            AuthorsResourceParameters authorsResourceParameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
               new LinkDto(CreateAuthorsResourceUri(authorsResourceParameters,
               ResourceUriType.Current)
               , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new LinkDto(CreateAuthorsResourceUri(authorsResourceParameters,
                  ResourceUriType.NextPage),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateAuthorsResourceUri(authorsResourceParameters,
                    ResourceUriType.PreviousPage),
                    "previousPage", "GET"));
            }

            return links;
        }
        //Tạo tùy chọn Header
        [HttpOptions]
        public IActionResult GetAuthorsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

        //Tạo resource phân trang
        private string CreateAuthorsResourceUri(
            AuthorsResourceParameters authorsResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetAuthors",
                      new
                      {
                          fields = authorsResourceParameters.Fields,
                          orderBy = authorsResourceParameters.OrderBy,
                          searchQuery = authorsResourceParameters.SearchQuery,
                          genre = authorsResourceParameters.Genre,
                          pageNumber = authorsResourceParameters.PageNumber - 1,
                          pageSize = authorsResourceParameters.PageSize
                      });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetAuthors",
                      new
                      {
                          fields = authorsResourceParameters.Fields,
                          orderBy = authorsResourceParameters.OrderBy,
                          searchQuery = authorsResourceParameters.SearchQuery,
                          genre = authorsResourceParameters.Genre,
                          pageNumber = authorsResourceParameters.PageNumber + 1,
                          pageSize = authorsResourceParameters.PageSize
                      });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetAuthors",
                    new
                    {
                        fields = authorsResourceParameters.Fields,
                        orderBy = authorsResourceParameters.OrderBy,
                        searchQuery = authorsResourceParameters.SearchQuery,
                        genre = authorsResourceParameters.Genre,
                        pageNumber = authorsResourceParameters.PageNumber,
                        pageSize = authorsResourceParameters.PageSize
                    });
            }
        }
    }
}
