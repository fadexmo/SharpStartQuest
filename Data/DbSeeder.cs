using SharpStartQuest.Models;

namespace SharpStartQuest.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        db.Database.EnsureCreated();

        if (db.Quests.Any()) return;

        db.Quests.AddRange(
            new Quest
            {
                Title = "Livrer le premier tableau de quêtes",
                Description = "Transformer le modèle de départ en suivi de quêtes clair et utile.",
                XpReward = 80,
                Difficulty = 2,
                Category = QuestCategory.Frontend,
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new Quest
            {
                Title = "Créer un modèle de données propre",
                Description = "Garder le modèle central simple, validé et facile à faire évoluer.",
                XpReward = 120,
                Difficulty = 3,
                Category = QuestCategory.Backend,
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new Quest
            {
                Title = "Valider le premier gros palier",
                Description = "Terminer une étape importante du projet et récupérer l'XP.",
                XpReward = 200,
                Difficulty = 5,
                Category = QuestCategory.Autre,
                CreatedAt = DateTime.UtcNow
            });

        db.SaveChanges();
    }
}
