using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Conduit.Business.Messages;
using Conduit.Common.Dto;
using Conduit.Domain;

namespace Conduit.Business.Services
{
    public interface INoteServices
    {
        Task<List<Note>> GetNoteList();
        Task<ResultMessage<Note>> GetNoteAsync(int id);
        Task<ResultMessage<Note>> InsertAsync(Note user);
        Task<ResultMessage<Note>> UpdateAsync(Note user);
        //Task<ResultMessage<User>> DeleteAsync(int id);
        //Task<ResultMessage<UserDto>> GetUserAsync(int id);
        //Task<ResultMessage<UserDto>> GetUserModel(string username);


        //Task<User> Get(Expression<Func<User, bool>> expression);

        //IQueryable<User> GetIncludes(params Expression<Func<User, object>>[] includes);
    } 
    
}
