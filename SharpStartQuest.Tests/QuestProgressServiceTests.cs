using SharpStartQuest.Models;
using SharpStartQuest.Services;

namespace SharpStartQuest.Tests;

public class QuestProgressServiceTests
{
    private readonly QuestProgressService _svc = new();

    [Fact]
    public void Calculate_EmptyList_ReturnsDefaults()
    {
        var stats = _svc.Calculate([]);

        Assert.Equal(0, stats.TotalQuests);
        Assert.Equal(0, stats.TotalXp);
        Assert.Equal(1, stats.Level);
        Assert.Equal(0, stats.CompletionRate);
        Assert.Empty(stats.Badges);
    }

    [Fact]
    public void Calculate_NoCompletedQuests_ZeroXp()
    {
        var quests = new List<Quest>
        {
            new() { XpReward = 100, Difficulty = 2 },
            new() { XpReward = 200, Difficulty = 3 }
        };

        var stats = _svc.Calculate(quests);

        Assert.Equal(0, stats.TotalXp);
        Assert.Equal(1, stats.Level);
        Assert.Equal(2, stats.ActiveQuests);
        Assert.Equal(0, stats.CompletionRate);
    }

    [Fact]
    public void Calculate_XpAndLevel_ComputedCorrectly()
    {
        // 250 XP = niveau 2 exact (XpPerLevel = 250)
        var quests = new List<Quest>
        {
            new() { XpReward = 150, IsCompleted = true },
            new() { XpReward = 100, IsCompleted = true }
        };

        var stats = _svc.Calculate(quests);

        Assert.Equal(250, stats.TotalXp);
        Assert.Equal(2, stats.Level);
        Assert.Equal(0, stats.XpIntoLevel);
        Assert.Equal(0, stats.ProgressPercent);
    }

    [Fact]
    public void Calculate_PartialLevel_ProgressPercentCorrect()
    {
        // 125 XP = niveau 1, 50% vers niveau 2
        var quests = new List<Quest>
        {
            new() { XpReward = 125, IsCompleted = true }
        };

        var stats = _svc.Calculate(quests);

        Assert.Equal(1, stats.Level);
        Assert.Equal(125, stats.XpIntoLevel);
        Assert.Equal(50, stats.ProgressPercent);
    }

    [Fact]
    public void Calculate_CompletionRate_RoundsCorrectly()
    {
        var quests = new List<Quest>
        {
            new() { XpReward = 50, IsCompleted = true },
            new() { XpReward = 50, IsCompleted = false },
            new() { XpReward = 50, IsCompleted = false }
        };

        var stats = _svc.Calculate(quests);

        Assert.Equal(33, stats.CompletionRate);
    }

    [Fact]
    public void Calculate_AllCompleted_CompletionRate100()
    {
        var quests = new List<Quest>
        {
            new() { XpReward = 50, IsCompleted = true },
            new() { XpReward = 50, IsCompleted = true }
        };

        var stats = _svc.Calculate(quests);

        Assert.Equal(100, stats.CompletionRate);
    }

    [Fact]
    public void Badges_CreateurDeQuetes_WhenAtLeastOneQuest()
    {
        var stats = _svc.Calculate([new Quest { XpReward = 50 }]);

        Assert.Contains("Créateur de quêtes", stats.Badges);
    }

    [Fact]
    public void Badges_PremiereVictoire_WhenOneCompleted()
    {
        var stats = _svc.Calculate([new Quest { XpReward = 50, IsCompleted = true }]);

        Assert.Contains("Première victoire", stats.Badges);
    }

    [Fact]
    public void Badges_SerieX5_WhenFiveCompleted()
    {
        var quests = Enumerable.Range(0, 5)
            .Select(_ => new Quest { XpReward = 50, IsCompleted = true })
            .ToList();

        var stats = _svc.Calculate(quests);

        Assert.Contains("Série x5", stats.Badges);
    }

    [Fact]
    public void Badges_SerieX5_NotEarnedWithFourCompleted()
    {
        var quests = Enumerable.Range(0, 4)
            .Select(_ => new Quest { XpReward = 50, IsCompleted = true })
            .ToList();

        var stats = _svc.Calculate(quests);

        Assert.DoesNotContain("Série x5", stats.Badges);
    }

    [Fact]
    public void Badges_ChasseurXP_WhenTotalXpAtLeast500()
    {
        var quests = new List<Quest>
        {
            new() { XpReward = 300, IsCompleted = true },
            new() { XpReward = 200, IsCompleted = true }
        };

        var stats = _svc.Calculate(quests);

        Assert.Contains("Chasseur d'XP", stats.Badges);
    }

    [Fact]
    public void Badges_VainqueurDePalier_WhenDifficulty5Completed()
    {
        var stats = _svc.Calculate([new Quest { XpReward = 200, Difficulty = 5, IsCompleted = true }]);

        Assert.Contains("Vainqueur de palier", stats.Badges);
    }

    [Fact]
    public void Badges_VainqueurDePalier_NotEarnedIfDifficulty5NotCompleted()
    {
        var stats = _svc.Calculate([new Quest { XpReward = 200, Difficulty = 5, IsCompleted = false }]);

        Assert.DoesNotContain("Vainqueur de palier", stats.Badges);
    }
}
