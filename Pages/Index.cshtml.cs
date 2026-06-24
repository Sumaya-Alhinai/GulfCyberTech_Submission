using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VehicleSubmissionApp.Data;
using VehicleSubmissionApp.Model;


namespace VehicleSubmissionApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        public IndexModel(AppDbContext db) { _db = db; }

        public List<Submission> Submissions { get; set; } = new();
        public SelectList Brands { get; set; } = default!;
        public SelectList Models { get; set; } = default!;

        [BindProperty(SupportsGet = true)] public int? BrandFilter { get; set; }
        [BindProperty(SupportsGet = true)] public int? ModelFilter { get; set; }
        [BindProperty(SupportsGet = true)] public string? Search { get; set; }
        [BindProperty(SupportsGet = true)] public int PageNumber { get; set; } = 1;
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 6;

        public async Task OnGetAsync()
        {
            Brands = new SelectList(_db.Brands.ToList(), "Id", "BrandName");

            var modelsQuery = BrandFilter.HasValue
                ? _db.Models.Where(m => m.BrandId == BrandFilter.Value).ToList()
                : new List<VehicleModel>();
            Models = new SelectList(modelsQuery, "Id", "ModelName");

            var query = _db.Submissions
                .Include(s => s.Brand)
                .Include(s => s.Model)
                .AsQueryable();

            if (BrandFilter.HasValue)
                query = query.Where(s => s.BrandId == BrandFilter.Value);

            if (ModelFilter.HasValue)
                query = query.Where(s => s.ModelId == ModelFilter.Value);

            if (!string.IsNullOrEmpty(Search))
                query = query.Where(s =>
                    s.Name.Contains(Search) ||
                    s.Email.Contains(Search) ||
                    s.CivilId.Contains(Search) ||
                    (s.Phone != null && s.Phone.Contains(Search)));

            int total = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(total / (double)PageSize);
            PageNumber = Math.Max(1, Math.Min(PageNumber, Math.Max(1, TotalPages)));

            Submissions = await query
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }

        public JsonResult OnGetModels(int brandId)
        {
            var models = _db.Models.Where(m => m.BrandId == brandId)
                .Select(m => new { m.Id, m.ModelName }).ToList();
            return new JsonResult(models);
        }
    }
}