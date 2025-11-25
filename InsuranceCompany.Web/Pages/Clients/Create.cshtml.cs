using InsuranceCompany.Dal.models;
using InsuranceCompany.Dal.PostgreDBcontext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InsuranceCompany.Web.Pages.Clients
{
    public class CreateModel : PageModel
    {
        private readonly postgreDBcontext _context;
        public CreateModel(postgreDBcontext context)
        {
            _context = context;
        }

        [BindProperty]
        public Client NewClient { get; set; } = new Client();
        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            NewClient.created_at = DateTime.UtcNow;
            _context.Clients.Add(NewClient);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
