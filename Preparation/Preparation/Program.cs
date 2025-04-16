namespace Preparation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //    int count = 1;
            //label_loop:
            //    Console.WriteLine(count);
            //    count++;
            //    if(count < 10)
            //        goto label_loop;
            //checked and uncheckde roli in c#
            
            const int a = 2147483647;
            const int b = 2147483647;

            int c = unchecked( a + b);

            Console.WriteLine(c);

            Console.ReadKey();
        }
    }


    public class  Peson
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value;}
        }
    }

    // Polymorphism

    public class Animal
    {
        public virtual void Speak()
        {
            Console.WriteLine("Animal speak");
        }

        // Method OverLoading and Compile-time check
        public int Calc(int Num1)
        {
            return Num1 + 1;
        }

        public int Calc(int Num1, int Num2)
        {
            return Num1 + Num2 + 1;
        }
    }

    public class Cat : Animal
    {
        // Run-time method and check
        public override void Speak()
        {
            Console.WriteLine("Cat moews");
        }
    }

}
