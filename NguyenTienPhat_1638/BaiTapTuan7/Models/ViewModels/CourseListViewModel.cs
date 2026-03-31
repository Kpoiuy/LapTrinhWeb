namespace BaiTapTuan7.Models.ViewModels;

public class CourseListViewModel
{
    public IReadOnlyList<CourseCardViewModel> Courses { get; set; } = [];
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
    public int TotalItems { get; set; }
    public string? Keyword { get; set; }
}
