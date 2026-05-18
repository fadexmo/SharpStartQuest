using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpStartQuest.Data;
using SharpStartQuest.Models;

namespace SharpStartQuest.Pages.Quests;

public class EditModel(AppDbContext db) : PageModel
{
    [BindProperty]
    public Quest Quest { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var quest = await db.Quests.FindAsync(id);

        if (quest is null)
        {
            return NotFound();
        }

        Quest = quest;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var quest = await db.Quests.FindAsync(Quest.Id);

        if (quest is null)
        {
            return NotFound();
        }

        quest.Title = Quest.Title;
        quest.Description = Quest.Description;
        quest.XpReward = Quest.XpReward;
        quest.Difficulty = Quest.Difficulty;

        if (Quest.IsCompleted && !quest.IsCompleted)
        {
            quest.Complete();
        }
        else if (!Quest.IsCompleted && quest.IsCompleted)
        {
            quest.Reopen();
        }

        await db.SaveChangesAsync();

        return RedirectToPage("./Details", new { id = quest.Id });
    }
}
