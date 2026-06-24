namespace VehicleSubmissionApp.Model
{
    public class VehicleModel
    {
        public int Id { get; set; }
        public string ModelName { get; set; } = string.Empty;
        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;
    }
}
