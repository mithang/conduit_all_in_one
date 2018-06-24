using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conduit.Business.Services;
using Conduit.Common.Dto;
using Conduit.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Note")]
    public class NoteController : Controller
    {
        private readonly INoteServices _noteSercices;

        public NoteController(INoteServices noteSercices)
        {
            _noteSercices = noteSercices;
        }
        [HttpGet("ListNote")]
        [Authorize]
        public async Task<List<Note>> GetNoteAsync()
        {
            return await _noteSercices.GetNoteList();
        }
        [HttpGet("GetNoteById")]
        public async Task<IActionResult> GetNoteByIdAsync(int id)
        {
            var durum = await _noteSercices.GetNoteAsync(id);
            if (durum.Errors == null)
            {
                return Ok(durum.Result);
            }
            return BadRequest(durum.Errors);
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]Note note)
        {
            if (ModelState.IsValid)
            {
                var durum = await _noteSercices.InsertAsync(note);
                if (durum.Errors == null)
                {
                    return Ok(durum);
                }
                return Ok(durum);
            }
            return BadRequest(ModelState);
        }
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody]Note note)
        {
            //Model is valid mi yapılıcak...
            if (ModelState.IsValid)
            {
                var durum = await _noteSercices.UpdateAsync(note);
                if (durum.Errors == null)
                {
                    return Ok(durum);
                }
                return Ok(durum);
            }
            return BadRequest(ModelState);
        }

    }
}