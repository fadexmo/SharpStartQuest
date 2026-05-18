using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SharpStartQuest.Data;
using SharpStartQuest.Models;
using SharpStartQuest.Services;

namespace SharpStartQuest.Pages;

public class IndexModel(AppDbContext db, QuestProgressService progressService) : PageModel
{
    public QuestStats Stats { get; private set; } = new(0, 0, 0, 0, 1, 0, 250, 0, 0, []);

    public List<Quest> ActiveQuests { get; private set; } = [];

    public List<Quest> RecentlyCompleted { get; private set; } = [];

    public async Task OnGetAsync()
    {
        var quests = await db.Quests
            .OrderBy(quest => quest.IsCompleted)
            .ThenByDescending(quest => quest.CreatedAt)
            .ToListAsync();

        Stats = progressService.Calculate(quests);
        ActiveQuests = quests
            .Where(quest => !quest.IsCompleted)
            .OrderByDescending(quest => quest.Difficulty)
            .ThenByDescending(quest => quest.XpReward)
            .Take(5)
            .ToList();
        RecentlyCompleted = quests
            .Where(quest => quest.IsCompleted)
            .OrderByDescending(quest => quest.CompletedAt)
            .Take(4)
            .ToList();
    }

    public async Task<IActionResult> OnPostCompleteAsync(int id)
    {
        var quest = await db.Quests.FindAsync(id);

        if (quest is not null)
        {
            quest.Complete();
            await db.SaveChangesAsync();
        }

        return RedirectToPage();
    }
}
