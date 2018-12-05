using System.IO;

using RLBotDotNet;

namespace DadBot
{
    class Program
    {
        static void Main(string[] args)
        {
            // Read the port from port.cfg.
            const string file = "port.cfg";
            string text = File.ReadAllLines(file)[0];
            int port = int.Parse(text);

            // BotManager is a generic which takes in your bot as its T type.
            BotManager<DadBot> botManager = new BotManager<DadBot>();
            // Start the server on the port given in the port.cfg file.
            botManager.Start(port);
        }
    }
}
