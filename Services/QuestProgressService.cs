using SharpStartQuest.Models;

namespace SharpStartQuest.Services;

public sealed record QuestStats(
    int TotalQuests,
    int ActiveQuests,
    int CompletedQuests,
    int TotalXp,
    int Level,
    int XpIntoLevel,
    int XpForNextLevel,
    int ProgressPercent,
    int CompletionRate,
    IReadOnlyList<string> Badges);

public class QuestProgressService
{
    private const int XpPerLevel = 250;

    public QuestStats Calculate(IEnumerable<Quest> quests)
    {
        var questList = quests.ToList();
        var completed = questList.Where(quest => quest.IsCompleted).ToList();
        var totalXp = completed.Sum(quest => quest.XpReward);
        var level = totalXp / XpPerLevel + 1;
        var xpIntoLevel = totalXp % XpPerLevel;
        var progressPercent = (int)Math.Round(xpIntoLevel * 100d / XpPerLevel);
        var completionRate = questList.Count == 0
            ? 0
            : (int)Math.Round(completed.Count * 100d / questList.Count);

        return new QuestStats(
            questList.Count,
            questList.Count(quest => !quest.IsCompleted),
            completed.Count,
            totalXp,
            level,
            xpIntoLevel,
            XpPerLevel,
            progressPercent,
            completionRate,
            BuildBadges(questList, completed, totalXp));
    }

    private static List<string> BuildBadges(List<Quest> quests, List<Quest> completed, int totalXp)
    {
        var badges = new List<string>();

        if (quests.Count > 0)
        {
            badges.Add("Créateur de quêtes");
        }

        if (completed.Count > 0)
        {
            badges.Add("Première victoire");
        }

        if (completed.Count >= 5)
        {
            badges.Add("Série x5");
        }

        if (totalXp >= 500)
        {
            badges.Add("Chasseur d'XP");
        }

        if (completed.Any(quest => quest.Difficulty == 5))
        {
            badges.Add("Vainqueur de palier");
        }

        return badges;
    }
}
