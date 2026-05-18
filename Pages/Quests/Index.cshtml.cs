using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SharpStartQuest.Data;
using SharpStartQuest.Models;
using SharpStartQuest.Services;

namespace SharpStartQuest.Pages.Quests;

public class IndexModel(AppDbContext db, QuestProgressService progressService) : PageModel
{
    public List<Quest> Quests { get; private set; } = [];

    public QuestStats Stats { get; private set; } = new(0, 0, 0, 0, 1, 0, 250, 0, 0, []);

    [BindProperty(SupportsGet = true)]
    public string Filtre { get; set; } = "toutes";

    public async Task OnGetAsync()
    {
        var allQuests = await db.Quests
            .AsNoTracking()
            .OrderBy(quest => quest.IsCompleted)
            .ThenByDescending(quest => quest.CreatedAt)
            .ToListAsync();

        Stats = progressService.Calculate(allQuests);

        Quests = Filtre switch
        {
            "actives" => allQuests.Where(quest => !quest.IsCompleted).ToList(),
            "terminees" => allQuests.Where(quest => quest.IsCompleted).ToList(),
            _ => allQuests
        };
    }

    public async Task<IActionResult> OnPostCompleteAsync(int id)
    {
        var quest = await db.Quests.FindAsync(id);

        if (quest is not null)
        {
            quest.Complete();
            await db.SaveChangesAsync();
        }

        return RedirectToPage(new { Filtre });
    }

    public async Task<IActionResult> OnPostReopenAsync(int id)
    {
        var quest = await db.Quests.FindAsync(id);

        if (quest is not null)
        {
            quest.Reopen();
            await db.SaveChangesAsync();
        }

        return RedirectToPage(new { Filtre });
    }
}
