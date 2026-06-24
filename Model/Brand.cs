namespace VehicleSubmissionApp.Model
{
    public class Brand
    {
      
            public int Id { get; set; }
            public string BrandName { get; set; } = string.Empty;
            public ICollection<VehicleModel> Models { get; set; } = new List<VehicleModel>();
        }
    
}
