

using CrystalCrash.Main;
using System;

try
{
    using var game = new BattleBot.Game1();
    game.Run();

}
catch (Exception e)
{
    using CrashHandler crash = new("The game unexpectedly crashed." +
                    "\nMaybe tell an egg head or bean counter about it?" +
                    "\nA detailed description of the problem is below:\n\n" + e.Message + "\n" + e.StackTrace);
    crash.Run();
}

