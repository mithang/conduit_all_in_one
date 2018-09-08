using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conduit.Business.Helpers;
using Conduit.Common.Dto;
using Conduit.Data;
using Conduit.Domain;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Business.Services
{
    public class LibraryRepository : ILibraryRepository
    {
        private ConduitContext _context;
        private IPropertyMappingService _propertyMappingService;

        public LibraryRepository(ConduitContext context, 
            IPropertyMappingService propertyMappingService)
        {
            _context = context;
            _propertyMappingService = propertyMappingService;
        }

        public void AddAuthor(Author author)
        {
            author.Id = Guid.NewGuid();
            _context.Authors.Add(author);

            // the repository fills the id (instead of using identity columns)
            if (author.Books.Any())
            {
                foreach (var book in author.Books)
                {
                    book.Id = Guid.NewGuid();
                }
            }
        }

        public async void AddBookForAuthor(Guid authorId, Book book)
        {
            var author = await GetAuthor(authorId);
            if (author != null)
            {
                // if there isn't an id filled out (ie: we're not upserting),
                // we should generate one
                if (book.Id == null)
                {
                    book.Id = Guid.NewGuid();
                }
                author.Books.Add(book);
            }
        }

        public bool AuthorExists(Guid authorId)
        {
            return _context.Authors.Any(a => a.Id == authorId);
        }

        public void DeleteAuthor(Author author)
        {
            _context.Authors.Remove(author);
        }

        public void DeleteBook(Book book)
        {
            _context.Books.Remove(book);
        }

        public async Task<Author> GetAuthor(Guid authorId)
        {
            return await _context.Authors.FirstOrDefaultAsync(a => a.Id == authorId);
        }

        public async Task<PagedList<Author>> GetAuthors(
            AuthorsResourceParameters authorsResourceParameters)
        {
            //C1:
            //            var query = _context.Set<Author>();
            //            var page = query.OrderBy(e => e.Id)
            //                .Select(e => e)
            //                .Skip(1).Take(2)
            //                .GroupBy(e => new { Total = query.Count() })
            //                .FirstOrDefault();
            //
            //            if (page != null)
            //            {
            //                int total = page.Key.Total;
            //                List<Author> events = page.Select(e => e).ToList();
            //            }
            //C2:
//            ViewBag.PageNumber = pageNumber;
//            const int pageSize = 25;
//            DateTime twoDaysAgo = DateTime.Now.AddDays(-2);
//            var groupSummaries = _recipeContext.Groups.OrderBy(g => g.Name)
//                .Select(g => new GroupSummaryModel
//                {
//                    Id = g.Id,
//                    Name = g.Name,
//                    Description = g.Description,
//                    NumberOfUsers = g.Users.Count(),
//                    NumberOfNewRecipes = g.Recipes.Count(r => r.PostedOn > twoDaysAgo)
//                }).Skip(pageSize * pageNumber)
//                .Take(pageSize);
//
//            return View(groupSummaries);



            var collectionBeforePaging =
                _context.Authors.ApplySort(authorsResourceParameters.OrderBy,
                _propertyMappingService.GetPropertyMapping<AuthorDto, Author>());

            if (!string.IsNullOrEmpty(authorsResourceParameters.Genre))
            {
                // trim & ignore casing
                var genreForWhereClause = authorsResourceParameters.Genre
                    .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Genre.ToLowerInvariant() == genreForWhereClause);
            }

            if (!string.IsNullOrEmpty(authorsResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = authorsResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Genre.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.FirstName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.LastName.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

//            return PagedList<Author>.Create(collectionBeforePaging,
//                authorsResourceParameters.PageNumber,
//                authorsResourceParameters.PageSize);               
              return await PagedList<Author>.CreateAsync(collectionBeforePaging,
                authorsResourceParameters.PageNumber,
                authorsResourceParameters.PageSize);   
        }

        public IEnumerable<Author> GetAuthors(IEnumerable<Guid> authorIds)
        {
            return _context.Authors.Where(a => authorIds.Contains(a.Id))
                .OrderBy(a => a.FirstName)
                .OrderBy(a => a.LastName)
                .ToList();
        }

        public void UpdateAuthor(Author author)
        {
            // no code in this implementation
        }

        public Book GetBookForAuthor(Guid authorId, Guid bookId)
        {
            return _context.Books
              .Where(b => b.AuthorId == authorId && b.Id == bookId).FirstOrDefault();
        }

        public IEnumerable<Book> GetBooksForAuthor(Guid authorId)
        {
            return _context.Books
                        .Where(b => b.AuthorId == authorId).OrderBy(b => b.Title).ToList();
        }

        public void UpdateBookForAuthor(Book book)
        {
            // no code in this implementation
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void AddAuthor(Guid authorId, Author authorEntity)
        {
            _context.Authors.Add(authorEntity);
        }
    }
}
