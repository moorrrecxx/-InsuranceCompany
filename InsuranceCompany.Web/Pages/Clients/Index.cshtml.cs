using InsuranceCompany.Dal.models;
using InsuranceCompany.Dal.PostgreDBcontext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InsuranceCompany.Web.Pages.Clients
{
    public class IndexModel : PageModel
    {
        private readonly postgreDBcontext _context;

        public IndexModel(postgreDBcontext context)
        {
            _context = context;
        }

        public IList<Client> Clients { get; set; } = new List<Client>();
        public async Task OnGetAsync()
        {
            Clients = await _context.Clients.ToListAsync();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.client_id == id);

            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
