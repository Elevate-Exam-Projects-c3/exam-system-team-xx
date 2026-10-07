using Microsoft.AspNetCore.Identity;

namespace exam_system.Domain.Entities.Identity;

public class ApplicationRole : IdentityRole<Guid>
{
    public ApplicationRole() => Id = Guid.NewGuid();
    public ApplicationRole(string name = "student") : this()
        => Name = name;


}