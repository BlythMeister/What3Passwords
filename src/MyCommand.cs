using PwnedClient;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using what3words.dotnet.wrapper;
using what3words.dotnet.wrapper.models;
using Zxcvbn;

namespace What3Passwords
{
    public class MyCommand : AsyncCommand<MySettings>
    {
        private readonly Random random = new();

        public override async Task<int> ExecuteAsync(CommandContext context, MySettings settings)
        {
            var table = new Table().Border(TableBorder.Ascii);
            table.AddColumn("Password").AddColumn("Is Pwnd").AddColumn("Crack Time").AddColumn("Estimated Guesses (Log10)").AddColumn("Warnings");
            table.Caption("Crack Time is a rate of 10B / second - offline attack, fast hash, many cores");
            table.Title("What3Password Generator");

            for (var i = 0; i < settings.NumberOfPasswords; i++)
            {
                var result = await GetRandomLocation(settings);
                var words = result.Words.Split('.');

                if (settings.DoubleUp)
                {
                    var result2 = await GetRandomLocation(settings);
                    words = words.Concat(result2.Words.Split('.')).ToArray();
                }

                var password = GetPassword(settings, words);
                var passwordPwned = "N/A";
                var passwordWarning = "N/A";
                var crackTime = "N/A";
                var estimatedGuesses = "N/A";

                if (settings.CheckPwnd)
                {
                    passwordPwned = (await GetPasswordPwned(password)).ToString();
                }

                if (settings.CheckQuality)
                {
                    var passwordEvaluation = GetPasswordEvaluation(password);

                    crackTime = passwordEvaluation.CrackTimeDisplay.OfflineFastHashing1e10PerSecond;
                    estimatedGuesses = passwordEvaluation.GuessesLog10.ToString(CultureInfo.CurrentCulture);

                    if (!string.IsNullOrWhiteSpace(passwordEvaluation.Feedback.Warning))
                    {
                        passwordWarning = passwordEvaluation.Feedback.Warning;
                    }
                }

                table.AddRow(new Markup(password), new Markup(passwordPwned), new Markup(crackTime), new Markup(estimatedGuesses), new Markup(passwordWarning));
                table.AddEmptyRow();
            }

            AnsiConsole.Write(table);

            return 0;
        }

        private async Task<Address> GetRandomLocation(MySettings settings)
        {
            var lat = GetCoordinate(-90, 90);
            var lng = GetCoordinate(-180, 180);
            var location = new Coordinates(lat, lng);

            var result = await new What3WordsV3(settings.ApiKey).ConvertTo3WA(location).RequestAsync();

            return result.Data;
        }

        private double GetCoordinate(int min, int max)
        {
            var coOrd = (double)random.Next(min, max);
            if (coOrd > min && coOrd < max)
            {
                coOrd += random.NextDouble();
            }
            return coOrd;
        }

        private string GetPassword(MySettings settings, string[] words)
        {
            if (settings.UppercaseFirstLetter)
            {
                words = words.Select(x => $"{x.Substring(0, 1).ToUpper()}{x.Substring(1)}").ToArray();
            }

            var wordText = string.Join(settings.Separator, words);

            var preText = string.Empty;
            if (!string.IsNullOrWhiteSpace(settings.PreText))
            {
                preText = $"{settings.PreText}{settings.Separator}";
            }

            var postText = string.Empty;
            if (!string.IsNullOrWhiteSpace(settings.PostText))
            {
                postText = $"{settings.Separator}{settings.PostText}";
            }

            var preNumber = string.Empty;
            var postNumber = string.Empty;
            if (settings.IncludeNumbers || settings.IncludeNumbersStart)
            {
                preNumber = $"{random.Next(0, 999):D3}{settings.Separator}";
            }

            if (settings.IncludeNumbers || settings.IncludeNumbersEnd)
            {
                postNumber = $"{settings.Separator}{random.Next(0, 999):D3}";
            }

            return $"{preText}{preNumber}{wordText}{postNumber}{postText}";
        }

        private static Result GetPasswordEvaluation(string password)
        {
            return Core.EvaluatePassword(password);
        }

        private static async Task<bool> GetPasswordPwned(string password)
        {
            return await new PasswordChecker().IsCompromisedAsync(password);
        }
    }
}
