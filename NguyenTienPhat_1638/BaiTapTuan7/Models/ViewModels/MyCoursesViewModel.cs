namespace BaiTapTuan7.Models.ViewModels;

public class MyCoursesViewModel
{
    public IReadOnlyList<Enrollment> Enrollments { get; set; } = [];
    public int TotalCourses { get; set; }
    public int TotalCredits { get; set; }
    public DateTime? LatestEnrollDate { get; set; }
}
