using System;
using System.Windows.Forms;
using ScopedHubMonitor.Scoped;

namespace ScopedHubMonitor
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            FooDemo.Run();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ScopedConnectionForm());
        }
    }

    public abstract class AbstractHub
    {
    }

    public class AnyHub : AbstractHub
    {
        public string Desc { get; set; }
    }

    public interface IHubContext<THub> where THub : AbstractHub
    {
    }

    public class Foo<THub> : IHubContext<THub> where THub : AbstractHub
    {

    }

    public class FooEvent
    {
        public FooEvent(IHubContext<AbstractHub> context)
        {
            Context = context;
        }

        public IHubContext<AbstractHub> Context { get; set; }
    }



    public class FooDemo
    {
        public static void Run()
        {
            var foo = new Foo<AnyHub>();
            //var test = (Foo<AbstractHub>)(object)foo;
            Console.WriteLine("-------------");
        }
    }

}
