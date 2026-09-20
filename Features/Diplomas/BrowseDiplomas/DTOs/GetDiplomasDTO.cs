namespace exam_system.Features.Diplomas.BrowseDiplomas.DTOs
{
    public class GetDiplomasDTO
    {
        public Guid Id { get; set; } = default!;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int QuizzesCount { get; set; } = 0;

    }

}
