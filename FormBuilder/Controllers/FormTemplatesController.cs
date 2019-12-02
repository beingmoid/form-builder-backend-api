using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FormBuilder.Model;

namespace FormBuilder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormTemplatesController : ControllerBase
    {
        private readonly FormBuilderContext _context;

        public FormTemplatesController(FormBuilderContext context)
        {
            _context = context;
        }

        // GET: api/FormTemplates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FormTemplate>>> GetFormTemplates()
        {
            return await _context.FormTemplates.ToListAsync();
        }

        // GET: api/FormTemplates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FormTemplate>> GetFormTemplate(int id)
        {
            var formTemplate = await _context.FormTemplates.FindAsync(id);

            if (formTemplate == null)
            {
                return NotFound();
            }

            return formTemplate;
        }

        // PUT: api/FormTemplates/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFormTemplate(int id, FormTemplate formTemplate)
        {
            if (id != formTemplate.Id)
            {
                return BadRequest();
            }

            _context.Entry(formTemplate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormTemplateExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FormTemplates
        [HttpPost]
        public async Task<ActionResult<FormTemplate>> PostFormTemplate(FormTemplate formTemplate)
        {
            _context.FormTemplates.Add(formTemplate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFormTemplate", new { id = formTemplate.Id }, formTemplate);
        }

        // DELETE: api/FormTemplates/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FormTemplate>> DeleteFormTemplate(int id)
        {
            var formTemplate = await _context.FormTemplates.FindAsync(id);
            if (formTemplate == null)
            {
                return NotFound();
            }

            _context.FormTemplates.Remove(formTemplate);
            await _context.SaveChangesAsync();

            return formTemplate;
        }

        private bool FormTemplateExists(int id)
        {
            return _context.FormTemplates.Any(e => e.Id == id);
        }
    }
}
