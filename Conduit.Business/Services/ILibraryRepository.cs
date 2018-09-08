using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Conduit.Business.Helpers;
using Conduit.Domain;

namespace Conduit.Business.Services
{
    public interface ILibraryRepository
    {
        Task<PagedList<Author>> GetAuthors(AuthorsResourceParameters authorsResourceParameters);
        Task<Author> GetAuthor(Guid authorId);
        IEnumerable<Author> GetAuthors(IEnumerable<Guid> authorIds);
        void AddAuthor(Author author);
        void DeleteAuthor(Author author);
        void UpdateAuthor(Author author);
        bool AuthorExists(Guid authorId);
        IEnumerable<Book> GetBooksForAuthor(Guid authorId);
        Book GetBookForAuthor(Guid authorId, Guid bookId);
        void AddBookForAuthor(Guid authorId, Book book);
        void UpdateBookForAuthor(Book book);
        void DeleteBook(Book book);
        bool Save();
        void AddAuthor(Guid authorId, Author authorEntity);
    }
}
