The official Unity 3D 4.3 AngryBots demo game has been modified 
to use Kii mobile backend-as-a-service.

For more info take a look at these files:

- Assets/Scripts/Backend/*
- Assets/Standard assets/GameScore.cs
- Assets/Scripts/Misc/GameOverGUI.cs

Kii Features used:

- User management
    - User Sign-in via dedicated GUI
    - User Registration via dedicated GUI
- Data management
    - Object creation at application level bucket
    (saving damage and death data for player and enemies)
- Analytics
    - Event based Analytics
    (saving end of level and end of game (game over) statistics)
    - Data based Analytics
    (over death data calculating average time for 1st player death, and avg time of level completion)

[![Bitdeli Badge](https://d2weczhvl823v0.cloudfront.net/KiiPlatform/unityangrybotskii/trend.png)](https://bitdeli.com/free "Bitdeli Badge")