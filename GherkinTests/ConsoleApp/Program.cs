namespace ConsoleApp
{
    public class Program
    {
        //Simple program that use Calc class
        private static void Main(string[] args)
        {
            var c = new Calc();

            var x = 1;
            var y = 2;

            Console.WriteLine($"Hello, Add {x} + {y} = " + c.Add(x, y));
        }
    }
}