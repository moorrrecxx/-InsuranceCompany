using InsuranceCompany.Dal.models;
using InsuranceCompany.Dal.PostgreDBcontext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InsuranceCompany.Web.Pages.Clients
{
    public class EditModel : PageModel
    {
        private readonly postgreDBcontext _context;
        public EditModel(postgreDBcontext context)
        {
            _context = context;
        }

        [BindProperty]
        public Client EditClient { get; set; } = new Client();
        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            EditClient = await _context.Clients.FirstOrDefaultAsync(c => c.client_id == id);

            if (EditClient == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(EditClient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Clients.AnyAsync(e => e.client_id == EditClient.client_id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
