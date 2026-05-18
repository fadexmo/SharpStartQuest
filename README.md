# SharpStartQuest

Application web ASP.NET Core Razor Pages avec .NET 10.

SharpStartQuest transforme des objectifs en quêtes avec XP, niveaux, badges et suivi de progression.

## Fonctionnalites actuelles

- Tableau de bord SSQ avec niveau, XP, progression et badges.
- Gestion complète des quêtes.
- Actions rapides pour terminer ou rouvrir une quête.
- Base SQLite locale via Entity Framework Core.
- Données de départ créées automatiquement au premier lancement.

## Lancer l'application

```bash
dotnet run
```

Ou avec un port fixe :

```bash
dotnet run --no-launch-profile --urls http://localhost:5050
```

## Compiler

```bash
dotnet build SharpStartQuest.slnx
```

## Données locales

La base SQLite est créée dans `sharpstartquest.db`. Elle est ignorée par Git.
