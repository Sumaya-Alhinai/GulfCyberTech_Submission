using System.ComponentModel.DataAnnotations;

namespace VehicleSubmissionApp.Model
{
    public class Submission
    {
        public int Id { get; set; }

        [Required] public string Name { get; set; } = string.Empty;
        [Required] public int BrandId { get; set; }
        [Required] public int ModelId { get; set; }
        [Required] public string CivilId { get; set; } = string.Empty;
        public string? Phone { get; set; }
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Required] public string CivilIdImagePath { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; } = DateTime.Now;

        public Brand Brand { get; set; } = null!;
        public VehicleModel Model { get; set; } = null!;
    }
}

