using Spectre.Console.Cli;

namespace What3Passwords
{
    public class MySettings : CommandSettings
    {
        [CommandOption("--api-key <key>")]
        public string ApiKey { get; set; }

        [CommandOption("--number <number>")]
        public int NumberOfPasswords { get; set; }

        [CommandOption("--pre <text>")]
        public string PreText { get; set; }

        [CommandOption("--post <text>")]
        public string PostText { get; set; }

        [CommandOption("--separator <text>")]
        public string Separator { get; set; }

        [CommandOption("--upper-case")]
        public bool UppercaseFirstLetter { get; set; }

        [CommandOption("--double-up")]
        public bool DoubleUp { get; set; }

        [CommandOption("--check-quality")]
        public bool CheckQuality { get; set; }

        [CommandOption("--check-pwnd")]
        public bool CheckPwnd { get; set; }
    }
}
