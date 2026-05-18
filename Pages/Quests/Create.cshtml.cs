using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpStartQuest.Data;
using SharpStartQuest.Models;

namespace SharpStartQuest.Pages.Quests;

public class CreateModel(AppDbContext db) : PageModel
{
    [BindProperty]
    public Quest Quest { get; set; } = new()
    {
        XpReward = 50,
        Difficulty = 2
    };

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        Quest.CreatedAt = DateTime.UtcNow;
        db.Quests.Add(Quest);
        await db.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
