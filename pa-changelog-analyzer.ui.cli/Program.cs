using PaChangelogAnalyzer.Ui.Cli.Options;

namespace PaChangelogAnalyzer.Ui.Cli;

public partial class Program
{
    private static IConfigurationRoot? configuration;

    private static readonly LoggingLevelSwitch levelSwitch = new LoggingLevelSwitch();

    public static async Task<int> Main(string[] args)
    {
        var parserResult = new Parser(config =>
        {
            config.CaseInsensitiveEnumValues = true;
            config.HelpWriter = Console.Error;
        }).ParseArguments<InitDbOptions, VerifyOptions>(args);

        if (parserResult.Tag == ParserResultType.NotParsed)
        {
            await Console.Out.WriteLineAsync($"Command parsing error");
            Log.Fatal("Returning {@ReturnCode}", ProgramReturnCodes.CommandParseError);
            return ProgramReturnCodes.CommandParseError;
        }

        SetupLogging(parserResult);

        var host = BuildHost(args);

        using (var serviceScope = host.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;

            try
            {
                var app = services.GetRequiredService<App>();
                await app.Run(parserResult);

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "An error occured. Returning {@ReturnCode}", ProgramReturnCodes.UnhandledError);
                return ProgramReturnCodes.UnhandledError;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}