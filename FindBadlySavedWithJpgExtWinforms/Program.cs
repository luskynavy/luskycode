namespace ImageMismatchFinder;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
        if (args.Length > 0 && args[0] == "--test")
        {
            int exitCode = AppTests.RunAll();
            Environment.Exit(exitCode);
            return;
        }

        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }    
}
