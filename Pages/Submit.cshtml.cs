using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using VehicleSubmissionApp.Data;
using VehicleSubmissionApp.Model;

namespace VehicleSubmissionApp.Pages
{
    public class SubmitModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public SubmitModel(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        [BindProperty] public Submission Submission { get; set; } = new();
        [BindProperty] public IFormFile? CivilIdImage { get; set; }
        public SelectList Brands { get; set; } = default!;
        public SelectList Models { get; set; } = default!;

        public void OnGet()
        {
            Brands = new SelectList(_db.Brands.ToList(), "Id", "BrandName");
            Models = new SelectList(new List<VehicleModel>(), "Id", "ModelName");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Submission.Brand");
            ModelState.Remove("Submission.Model");
            ModelState.Remove("Submission.CivilIdImagePath"); 
            ModelState.Remove("CivilIdImagePath");

            if (CivilIdImage == null || CivilIdImage.Length == 0)
                ModelState.AddModelError("CivilIdImage", "Civil ID image is required.");
            else
            {
                if (CivilIdImage.Length > 2 * 1024 * 1024)
                    ModelState.AddModelError("CivilIdImage", "File size must not exceed 2MB.");
                var allowed = new[] { ".jpg", ".jpeg", ".png" };
                var ext = Path.GetExtension(CivilIdImage.FileName).ToLower();
                if (!allowed.Contains(ext))
                    ModelState.AddModelError("CivilIdImage", "Only JPG and PNG files are allowed.");
            }

            if (!ModelState.IsValid)
            {
                Brands = new SelectList(_db.Brands.ToList(), "Id", "BrandName");
                Models = new SelectList(_db.Models.Where(m => m.BrandId == Submission.BrandId).ToList(), "Id", "ModelName");
                return Page();
            }

            // Create uploads folder if not exists
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var imgExt = Path.GetExtension(CivilIdImage!.FileName).ToLower();
            var fileName = Guid.NewGuid() + imgExt;
            var path = Path.Combine(uploadsFolder, fileName);
            using (var stream = new FileStream(path, FileMode.Create))
                await CivilIdImage.CopyToAsync(stream);

            Submission.CivilIdImagePath = "/uploads/" + fileName;
            _db.Submissions.Add(Submission);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Record submitted successfully!";
            return RedirectToPage("/Index");
        }

        public JsonResult OnGetModels(int brandId)
        {
            var models = _db.Models.Where(m => m.BrandId == brandId)
                .Select(m => new { m.Id, m.ModelName }).ToList();
            return new JsonResult(models);
        }
    }
}