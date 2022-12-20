using ProxxGame.Models;

namespace ProxxGame.GameDetails;

public class GameSettings
{
    private GameSettings() {}
    
    public int XNumberCells { get; private init; }
    
    public int YNumberCells { get; private init; }
    
    public int NumberOfBlackHoles { get; private init; }

    private static GameSettings GetSettingsForEasyMode() =>
        new()
        {
            XNumberCells = 8,
            YNumberCells = 8,
            NumberOfBlackHoles = 10
        };

    private static GameSettings GetSettingsForMediumMode() =>
        new()
        {
            XNumberCells = 16,
            YNumberCells = 16,
            NumberOfBlackHoles = 40
        };
    
    private static GameSettings GetSettingsForHardMode() =>
        new()
        {
            XNumberCells = 24,
            YNumberCells = 24,
            NumberOfBlackHoles = 99
        };
    

    public static GameSettings GetPredefinedGameSettingsFromMode(GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameMode.Easy:
                return GetSettingsForEasyMode();
            
            case GameMode.Medium:
                return GetSettingsForMediumMode();
            
            case GameMode.Hard:
                return GetSettingsForHardMode();
            
            default: 
                return GetSettingsForMediumMode();
        }
    }

    public static GameSettings CreateCustomGameSettings(int xNumberCells, int yNumberCells, int numberOfBlackHoles)
        =>
            new()
            {
                XNumberCells = xNumberCells,
                YNumberCells = yNumberCells,
                NumberOfBlackHoles = numberOfBlackHoles
            };
}