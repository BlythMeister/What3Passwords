using Spectre.Console.Cli;

namespace What3Passwords
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandApp();

            app.Configure(config =>
            {
                config.AddCommand<MyCommand>("run");
            });

            return app.Run(args);
        }
    }
}
