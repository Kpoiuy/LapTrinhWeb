namespace BaiTapTuan7.Models.ViewModels;

public class CourseCardViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
    public int Credits { get; set; }
    public string Lecturer { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public bool IsEnrolled { get; set; }
}
