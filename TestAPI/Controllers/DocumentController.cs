using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.Models;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly DocumentDbContext _context;
        public DocumentController(DocumentDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] Document document)
        {
            await _context.Documents.AddAsync(document);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = document.Id }, document);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Document))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var entity = await _context.Documents.FindAsync(id);
            if(entity == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(entity);
            }
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type=typeof(Document))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] JsonPatchDocument<Document> patchEntity)
        {
            var entity = await _context.Documents.FirstOrDefaultAsync(e => e.Id == id);
            if(entity == null)
            {
                return NotFound();
            }
            else if(entity.Status == StatusEnum.published)
            {
                return BadRequest();
            }
            else {
                entity.Modified_Date = DateTime.Now;
                patchEntity.ApplyTo(entity, ModelState);
                return Ok(entity);
            }
        }

        [HttpPost("{id}/publish")]
        public async Task<IActionResult> Publish(Guid id)
        {
            var entity = await _context.Documents.FindAsync(id);
            if(entity == null)
            {
                return NotFound();
            }
            else if(entity.Status == StatusEnum.published)
            {
                return BadRequest();
            }
            else
            {
                entity.Status = StatusEnum.published;
                entity.Modified_Date = DateTime.Now;
                _context.SaveChanges();
                return Ok(entity);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page, int perPage)
        {
            if(_context.Documents == null)
            {
                return NotFound();
            }
            var pageResults = Convert.ToDouble(perPage);
            var pageCount = Math.Ceiling(_context.Documents.Count() / pageResults);

            var documents = await _context.Documents
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();

            var response = new DocumentResponse
            {
                Documents = documents,
                CurrentPage = page,
                Pages = (int)pageCount
            };
            return Ok(response);
        }


    }
}
