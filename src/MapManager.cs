namespace MapManager;

using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

public class PluginInfo : BasePlugin
{
    public override string ModuleName => "MapManager";
    public override string ModuleVersion => "0.2";
    public override string ModuleAuthor => "johnoclock";
    public override string ModuleDescription => "Allows the usage of more workshop maps.";

    private string FilePath => Path.Join(ModuleDirectory, "MapList.txt");

    public override void Load(bool hotReload)
    {

        if (!File.Exists(FilePath))
            File.Create(FilePath);

        RegisterEventHandler<EventCsIntermission>(EventGameEnd);
        Console.WriteLine("MapManager is loaded");
    }


    private HookResult EventGameEnd(EventCsIntermission @event, GameEventInfo info)
    {
        Console.WriteLine("Game has ended. Changing map...");

        // Read all lines from MapList.txt into an array
        string[] mapIdArray = File.ReadAllLines(FilePath);

        // Check if there are any IDs in the file
        if (mapIdArray.Length == 0)
        {
            Console.WriteLine("No map IDs found in file.");
            return HookResult.Continue; // Or however you wish to handle this case
        }

        // Use a random number generator to pick a random ID from the array
        string randomMapId = mapIdArray[new Random().Next(mapIdArray.Length)]; // Get a random map ID

        // Use the random map ID in the server command
        Server.ExecuteCommand($"host_workshop_map {randomMapId}");

        return HookResult.Continue;
    }
}
