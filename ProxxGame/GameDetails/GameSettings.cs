using ProxxGame.Models;

namespace ProxxGame.GameDetails;

/// <summary>
/// The class which represents game settings for the created game
/// </summary>
public class GameSettings
{
    private GameSettings() {}
    
    /// <summary>
    /// The number of cells on X axis
    /// </summary>
    public int XNumberCells { get; private init; }
    
    /// <summary>
    /// The number of cells on Y axis
    /// </summary>
    public int YNumberCells { get; private init; }
    
    /// <summary>
    /// The number of black holes in the game
    /// </summary>
    public int NumberOfBlackHoles { get; private init; }

    /// <summary>
    /// Returns preset game settings for Easy game mode
    /// </summary>
    private static GameSettings GetSettingsForEasyMode() =>
        new()
        {
            XNumberCells = 8,
            YNumberCells = 8,
            NumberOfBlackHoles = 10
        };

    /// <summary>
    /// Returns preset game settings for Medium game mode
    /// </summary>
    private static GameSettings GetSettingsForMediumMode() =>
        new()
        {
            XNumberCells = 16,
            YNumberCells = 16,
            NumberOfBlackHoles = 40
        };
    
    /// <summary>
    /// Returns preset game settings for Hard game mode
    /// </summary>
    private static GameSettings GetSettingsForHardMode() =>
        new()
        {
            XNumberCells = 24,
            YNumberCells = 24,
            NumberOfBlackHoles = 99
        };
    
    /// <summary>
    /// A factory method which constructs a game settings object using the chosen game mode by the user
    /// </summary>
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

    /// <summary>
    /// Creates a custom game settings object with specified by user parameters
    /// </summary>
    public static GameSettings CreateCustomGameSettings(int xNumberCells, int yNumberCells, int numberOfBlackHoles)
        =>
            new()
            {
                XNumberCells = xNumberCells,
                YNumberCells = yNumberCells,
                NumberOfBlackHoles = numberOfBlackHoles
            };
}