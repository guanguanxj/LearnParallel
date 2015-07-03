namespace ParallelTasks
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Net;
    using System.Diagnostics;

    class ParallelInvoke
    {
        static void Main()
        {
            //1.LearnTask
            //var taskTop = new Task(() =>
            //{
            //    Thread.Sleep(500);
            //    Console.WriteLine("任务taskTop运行在线程“{0}”上",
            //        Thread.CurrentThread.ManagedThreadId);
            //});
            //var taskMiddle = new Task(() =>
            //{
            //    Thread.Sleep(500);
            //    Console.WriteLine("任务taskMiddle运行在线程“{0}”上",
            //        Thread.CurrentThread.ManagedThreadId);
            //});
            //var taskBottom = new Task(() =>
            //{
            //    Thread.Sleep(500);
            //    Console.WriteLine("任务taskBottom运行在线程“{0}”上",
            //        Thread.CurrentThread.ManagedThreadId);
            //});
            //taskTop.Start();
            //taskMiddle.Start();
            //taskBottom.Start();
            //Task.WaitAll(new Task[] { taskTop, taskMiddle, taskBottom });

            //2. Compare Parllel to Task
            //ParallelDemo p = new ParallelDemo();

            //p.ParallelDemoRun();
            //3. Compare Parllel with For
            ParallelMethod p = new ParallelMethod();
            p.Method();

            Console.ReadKey();
        }

        public class ParallelDemo
        {
            private Stopwatch stop = new Stopwatch();
            public void Run1()
            {
                Thread.Sleep(2000);
                Console.WriteLine("Task1 run 2s");
            }
            public void Run2()
            {
                Thread.Sleep(3000);
                Console.WriteLine("Task2 run 3s");
            }

            public void ParallelDemoRun()
            {
                stop.Start();
                Parallel.Invoke(Run1, Run2);
                stop.Stop();
                Console.WriteLine("Parallel uses " + stop.ElapsedMilliseconds + "ms"); // about 3013ms

                stop.Restart();
                Run1();
                Run2();
                stop.Stop();
                Console.WriteLine("Normal run " + stop.ElapsedMilliseconds + "ms"); //=4999ms

                var task1 = new Task(() => Run1());
                var task2 = new Task(() => Run2());
                stop.Restart();
                task1.Start();
                task2.Start();
                Task.WaitAll(new Task[] { task1, task2 });
                stop.Stop();
                Console.WriteLine("WaitAll run " + stop.ElapsedMilliseconds + "ms");//=2999s
            }
        }

        public class ParallelMethod
        {
            private Stopwatch stop = new Stopwatch();

            public void Method()
            {
                //** global :Global Varibale **
                 int global =0; 
                stop.Restart();
                Parallel.For(0, 10000, item =>
                {
                    //** sum : Local Variable **
                    //int sum = 0; 
                    for (int j = 0; j < 60000; j++)
                    {
                        global++;
                       // sum++;
                    }
                });
                stop.Stop();
                //** Local Variable takes 328ms **
                //** Global Variable takes 3002ms **
                Console.WriteLine("Parallel run " + stop.ElapsedMilliseconds + "ms"); 
                stop.Restart();
                for (int i = 0; i < 10000; i++)
                {
                    //int sum = 0;
                    for (int j = 0; j < 60000; j++)
                    {
                        global++;
                      //  sum++;
                    }
                }
                stop.Stop();
                //** Local Variable takes 1168ms**
                //** Global Variable takes 1505ms **
                Console.WriteLine("Normal run " + stop.ElapsedMilliseconds + "ms");
            }

        }
    }
}