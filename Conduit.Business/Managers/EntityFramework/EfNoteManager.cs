using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Conduit.Business.Helpers;
using Conduit.Business.Messages;
using Conduit.Business.Services;
using Conduit.Business.ViewModels;
using Conduit.Common.DataAccess;
using Conduit.Common.Dto;
using Conduit.Domain;

namespace Conduit.Business.Managers.EntityFramework
{
    public class EfNoteManager : ManagerBase<Note>, INoteServices
    {
        private readonly IRepository<Note> _repository;
        private readonly IMapper _mapper;
        private readonly IArticleServices _articleServices;
        private readonly IUserServices _userServices;
        public EfNoteManager(IRepository<Note> repository, IMapper mapper, IArticleServices articleServices, IUserServices userServices) : base(repository)
        {
            _repository = repository;
            _mapper = mapper;
            _articleServices = articleServices;
            _userServices = userServices;
        }


        public async Task<List<Note>> GetNoteList()
        {
            var commentList = await base.GetList();
            return commentList;
        }

        public async Task<ResultMessage<Note>> GetNoteAsync(int id)
        {
            ResultMessage<Note> resultMessage = new ResultMessage<Note>();
            var user = await Get(p => p.Id == id);
            if (user != null)
            {
                resultMessage.Result = _mapper.Map<Note>(user);
            }
            else
            {
                resultMessage.Errors = new ErrorMessageObj(ErrorMessageCode.NoteNotFound, "Note khong tim thay");
            }
            return resultMessage;
        }

        public async Task<ResultMessage<Note>> InsertAsync(Note note)
        {

            ResultMessage<Note> resultMessage = new ResultMessage<Note>();

            var sonuc = await base.Insert(note);
            if (sonuc > 0)
            {
                resultMessage.Result = note;
            }
            else
            {
                resultMessage.Errors = new ErrorMessageObj(ErrorMessageCode.NoteNotInserted, "Khong the insert note");
            }


            return resultMessage;
        }

        public async Task<ResultMessage<Note>> UpdateAsync(Note note)
        {
            ResultMessage<Note> resultMessage = new ResultMessage<Note>();

            var sonuc = await base.Update(note);
            if (sonuc > 0)
            {
                resultMessage.Result = _mapper.Map<Note>(note);
            }
            else
            {
                resultMessage.Errors = new ErrorMessageObj(ErrorMessageCode.NoteCouldNotUpdated, "Khong the update note.");

            }
            return resultMessage;
            //}
            //catch (Exception ex)
            //{
            //    resultMessage.Errors = new ErrorMessageObj(ErrorMessageCode.CriticalError, ex.Message);
            //    return resultMessage;
            //}

        }
    }
}
