using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SharpStartQuest.Data;
using SharpStartQuest.Models;

namespace SharpStartQuest.Pages.Quests;

public class DeleteModel(AppDbContext db) : PageModel
{
    public Quest Quest { get; private set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var quest = await db.Quests.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id);

        if (quest is null)
        {
            return NotFound();
        }

        Quest = quest;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var quest = await db.Quests.FindAsync(id);

        if (quest is not null)
        {
            db.Quests.Remove(quest);
            await db.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
