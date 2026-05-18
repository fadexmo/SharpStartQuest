using Microsoft.EntityFrameworkCore;
using SharpStartQuest.Data;
using SharpStartQuest.Models;
using SharpStartQuest.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<QuestProgressService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (db.Quests.Any())
    {
        var premiereQuete = db.Quests.SingleOrDefault(quest => quest.Id == 1 && quest.XpReward == 80 && quest.Difficulty == 2);
        if (premiereQuete is not null)
        {
            premiereQuete.Title = "Livrer le premier tableau de quêtes";
            premiereQuete.Description = "Transformer le modèle de départ en suivi de quêtes clair et utile.";
        }

        var deuxiemeQuete = db.Quests.SingleOrDefault(quest => quest.Id == 2 && quest.XpReward == 120 && quest.Difficulty == 3);
        if (deuxiemeQuete is not null)
        {
            deuxiemeQuete.Title = "Créer un modèle de données propre";
            deuxiemeQuete.Description = "Garder le modèle central simple, validé et facile à faire évoluer.";
        }

        var troisiemeQuete = db.Quests.SingleOrDefault(quest => quest.Id == 3 && quest.XpReward == 200 && quest.Difficulty == 5);
        if (troisiemeQuete is not null)
        {
            troisiemeQuete.Title = "Valider le premier gros palier";
            troisiemeQuete.Description = "Terminer une étape importante du projet et récupérer l'XP.";
        }

        db.SaveChanges();
    }
    else
    {
        db.Quests.AddRange(
            new Quest
            {
                Title = "Livrer le premier tableau de quêtes",
                Description = "Transformer le modèle de départ en suivi de quêtes clair et utile.",
                XpReward = 80,
                Difficulty = 2,
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new Quest
            {
                Title = "Créer un modèle de données propre",
                Description = "Garder le modèle central simple, validé et facile à faire évoluer.",
                XpReward = 120,
                Difficulty = 3,
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new Quest
            {
                Title = "Valider le premier gros palier",
                Description = "Terminer une étape importante du projet et récupérer l'XP.",
                XpReward = 200,
                Difficulty = 5,
                CreatedAt = DateTime.UtcNow
            });

        db.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Erreur");
    app.UseHsts();
}

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
