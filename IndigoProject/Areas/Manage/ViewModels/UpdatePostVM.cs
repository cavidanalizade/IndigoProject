namespace IndigoProject.Areas.Manage.ViewModels
{
    public class UpdatePostVM
    {
        public int Id { get; set; } 
        public string? Title { get; set; }
        public string? ImageUrl { get; set; }

        public string? Description { get; set; }
        public IFormFile? Image { get; set; }

    }
}
