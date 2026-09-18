namespace exam_system;

public class AllDiplomasDto
{
    public Guid Id { get; set; } = default!;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int QuizzesCount { get; set; } = 0;
    public int EnrollmentsCount { get; set; } = 0;
    public DateTime CreatedAt { get; set; }
}
