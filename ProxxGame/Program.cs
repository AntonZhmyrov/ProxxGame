using ProxxGame;
using ProxxGame.GameDetails;
using ProxxGame.GameDetails.Validation;
using ProxxGame.Models;

Console.WriteLine("Welcome to the Proxx Game!");
Console.WriteLine("Please, pick the game mode to play: ");
Console.WriteLine("\t0 - Easy (Board 8x8 with 10 black holes)\n" +
                  "\t1 - Medium (Board 16x16 with 40 black holes)\n" +
                  "\t2 - Hard (Board 24x24 with 99 black holes)\n" +
                  "\t3 - Custom (You configure the board size and number of black holes)");
Console.WriteLine();
Console.Write("Your choice: ");

// Here we parse the chosen mode for the current game, there is a basic check below
// It stops program from running if the user input something wrong
bool parsed = Enum.TryParse(Console.ReadLine(), out GameMode pickedGameMode);

if (!parsed)
{
    Console.WriteLine("Wrong game mode :(");
    return;
}

GameSettings gameSettings = DefineGameSettings();

// The instantiation of this validator should ideally be made in a DI container
// But this is an overkill for this type of application
IPositionValidator positionValidator = new PositionValidator(gameSettings); 

// The new game is created with the chosen and defined game settings
var game = new Game(gameSettings);

Console.Clear();

// The game is handled in an infinite loop which allows user to open cell by cell until he/she wins/loses
while (true)
{
    Position? cellPosition = InputCellPosition();

    // If input returns null for validation or parsing issues - the program stops executing
    if (cellPosition == null)
    {
        return;
    }
    
    OpenCellResult result = game.OpenCell(cellPosition);
    var openedPosition = result.Cell.Position;

    // When the opened cell is a black hole - the output shows which cell was opened and the overall state of the game
    // with information about all cells, their positions and whether it is a hole or if not - how many adjacent holes
    // does the cell have
    if (result.IsBlackHole)
    {
        Console.WriteLine($"The cell [{openedPosition.X},{openedPosition.Y}] is a hole! Sorry! " +
                          "You lose! Please, be careful next time :)");
        
        Console.Write(result.OpenedCellsStatesString);
        break;
    }
    
    // When all the not hole cells are opened - the user wins the game
    if (result.IsVictory)
    {
        Console.WriteLine($"You opened the cell with position: [{openedPosition.X},{openedPosition.Y}]. " +
                          $"Adjacent Holes: {result.Cell.AdjacentHoles}");
        
        Console.WriteLine("CONGRATULATIONS! YOU WIN!");
        Console.Write(result.OpenedCellsStatesString);
        break;
    }

    Console.WriteLine($"You opened the cell with position: [{openedPosition.X},{openedPosition.Y}]. " +
                      $"Adjacent Holes: {result.Cell.AdjacentHoles}");
    
    Console.Write(result.OpenedCellsStatesString);
    Console.WriteLine("Press enter to continue...");
    Console.ReadLine();
    Console.Clear();
}

// This method accepts input of game configuration from user and creates a GameSettings object needed 
// for initial game play setup
GameSettings DefineGameSettings()
{
    // If the user picked Custom game mode, we allow him/her to input the sizes of the board + number of black holes
    if (pickedGameMode == GameMode.Custom)
    {
        Console.WriteLine();
        Console.Write("Please, input the number of rows: ");
        var numberOfRows = Console.ReadLine();
    
        Console.Write("Please, input the number of columns: ");
        var numberOfColumns = Console.ReadLine();
    
        Console.Write("Please, input the number of black holes: ");
        var numberOfBlackHoles = Console.ReadLine();
    
        return GameSettings.CreateCustomGameSettings(
            Convert.ToInt32(numberOfRows), 
            Convert.ToInt32(numberOfColumns),
            Convert.ToInt32(numberOfBlackHoles));
    }

    // If the chosen mode is predefined - we let the GameSettings object take care of correct GameSettings object
    // to deal with
    return GameSettings.GetPredefinedGameSettingsFromMode(pickedGameMode);
}

// Here the user inputs the X and Y position of the cell to be opened
Position? InputCellPosition()
{
    Console.WriteLine($"Please, input the cell to open (input from 0 to {gameSettings.XNumberCells - 1} " +
                      $"for X and from 0 to {gameSettings.YNumberCells - 1} for Y): ");
    
    Console.Write("X: ");
    var x = Console.ReadLine();
    
    Console.Write("Y: ");
    var y = Console.ReadLine();

    // The input data is to be parsed into a Position object
    // If parse is unsuccessful - the program stops running
    if (!Position.TryParse(x, y, out var inputPosition))
    {
        Console.WriteLine("Wrong type of input data!" +
                          "Please, next time, input the integer values for X and Y cell position");

        return inputPosition;
    }

    // The ranges of input position get validated
    // If they are wrong - the program stops running
    if (!positionValidator.ValidateRanges(inputPosition))
    {
        Console.WriteLine("Wrong ranges of input data!" +
                          "Please, next time, input the correct ranges for X and Y cell position values" +
                          $"The correct ranges: from 0 to {gameSettings.XNumberCells - 1} for X and " +
                          $"from 0 to {gameSettings.YNumberCells - 1} for Y");

        return inputPosition;
    }

    return inputPosition;
}
