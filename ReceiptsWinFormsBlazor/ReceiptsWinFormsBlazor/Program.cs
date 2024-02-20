namespace ReceiptsBlazorWinForms
{
    public static class Program
    {
        public static Form1 MyForm { get; set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            MyForm = new Form1();
            Application.Run(MyForm);
        }
    }
}