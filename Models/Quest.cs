using System.ComponentModel.DataAnnotations;

namespace SharpStartQuest.Models;

public class Quest
{
    public int Id { get; set; }

    [Required]
    [StringLength(90, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Range(10, 1000)]
    public int XpReward { get; set; } = 50;

    [Range(1, 5)]
    public int Difficulty { get; set; } = 1;

    public bool IsCompleted { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? CompletedAt { get; set; }

    public void Complete()
    {
        IsCompleted = true;
        CompletedAt ??= DateTime.UtcNow;
    }

    public void Reopen()
    {
        IsCompleted = false;
        CompletedAt = null;
    }
}
