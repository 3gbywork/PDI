using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EWSoftware.PDI.Parser;
using EWSoftware.PDI.Properties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PDI.NetCoreDemos.Converters;
using PDI.NetCoreDemos.Data;
using PDI.NetCoreDemos.Models;

namespace PDI.NetCoreDemos.Controllers
{
    public class VCardDtoesController : Controller
    {
        private readonly VCardContext _context;

        public VCardDtoesController(VCardContext context)
        {
            _context = context;
        }

        // GET: VCardDtoes
        public async Task<IActionResult> Index()
        {
            // the result is ordered by ID, we need orderby SortableName property manually
            var vCards = await _context.VCards.ToListAsync();
            return View(vCards.OrderBy(v => v.SortableName, StringComparer.CurrentCultureIgnoreCase));
        }

        // GET: VCardDtoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vCardDto = await _context.VCards
                .SingleOrDefaultAsync(m => m.ID == id);
            if (vCardDto == null)
            {
                return NotFound();
            }

            return View(vCardDto);
        }

        // GET: VCardDtoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VCardDtoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Version,LastName,FirstName,MiddleName,Title,Suffix,SortString,FormattedName,Nickname,Organization,JobTitle,Role,Units,Categories")] VCardDto vCardDto)
        {
            if (ModelState.IsValid)
            {
                vCardDto.LastRevision = DateTime.Now;

                vCardDto = ParseDto(vCardDto);
                _context.Add(vCardDto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vCardDto);
        }

        // GET: VCardDtoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vCardDto = await _context.VCards.SingleOrDefaultAsync(m => m.ID == id);
            if (vCardDto == null)
            {
                return NotFound();
            }
            return View(vCardDto);
        }

        // POST: VCardDtoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Version,LastName,FirstName,MiddleName,Title,Suffix,SortString,FormattedName,Nickname,Organization,JobTitle,Role,Units,Categories")] VCardDto vCardDto)
        {
            if (id != vCardDto.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    vCardDto.LastRevision = DateTime.Now;

                    vCardDto = ParseDto(vCardDto);
                    _context.Update(vCardDto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VCardDtoExists(vCardDto.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vCardDto);
        }

        // GET: VCardDtoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vCardDto = await _context.VCards
                .SingleOrDefaultAsync(m => m.ID == id);
            if (vCardDto == null)
            {
                return NotFound();
            }

            return View(vCardDto);
        }

        // POST: VCardDtoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vCardDto = await _context.VCards.SingleOrDefaultAsync(m => m.ID == id);
            _context.VCards.Remove(vCardDto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VCardDtoExists(int id)
        {
            return _context.VCards.Any(e => e.ID == id);
        }

        private VCardDto ParseDto(VCardDto vCardDto)
        {
            return VCardConverter.VCardToDto(VCardConverter.DtoToVCard(vCardDto), vCardDto);
        }

        [HttpPost, ActionName("UploadFiles")]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files, string fileEncoding, string propEncoding)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            try
            {
                using (var reader = new StreamReader(filePath, Encoding.GetEncoding(fileEncoding)))
                {
                    BaseProperty.DefaultEncoding = Encoding.GetEncoding(propEncoding);

                    var vCards = VCardParser.ParseFromStream(reader);
                    var vCardDtoes = VCardConverter.CollectionToDtoes(vCards);

                    return View("Index", vCardDtoes);
                }
            }
            catch (Exception ex)
            {
                return Ok(string.Format("Unable to parse file: {0}", ex.Message));
            }
        }
    }
}
