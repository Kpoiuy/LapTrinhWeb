namespace BaiTapTuan7.Models.ViewModels;

public class AdminDashboardViewModel
{
    public int TotalCourses { get; set; }
    public int TotalCategories { get; set; }
    public int TotalEnrollments { get; set; }
    public IReadOnlyList<TopCourseItem> TopCourses { get; set; } = [];
}

public class TopCourseItem
{
    public string CourseName { get; set; } = string.Empty;
    public int Enrollments { get; set; }
}
