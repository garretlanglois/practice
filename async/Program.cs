using System;
using System.Threading;
using System.Collections.Concurrent;

/** 

Two different types of threads in Dotnet, foreground and background threads.
When the main exits will it await the thread completion

*/

for (int i = 0; i < 1000; i++)
{
    int capturedValue = i;

    //Action is a delegate that has already been defined
    MyThreadPool.QueueUserWorkItem(() =>
    {
        Console.WriteLine(capturedValue);
        Thread.Sleep(1000);
    });
}

Console.ReadLine();


static class MyThreadPool
{
    private static readonly BlockingCollection<Action> s_workItems = new();

    //Action is parameterless and void return function execution
    public static void QueueUserWorkItem(Action action) => s_workItems.Add(action);

    static MyThreadPool()
    {
        for (int i = 0; i < Environment.ProcessorCount; i++)
        {
            new Thread(() =>
            {
                while (true)
                {
                    Action workItem = s_workItems.Take();
                    workItem();
                }
            })
            //Now the threads won't keep the process from exiting
            { IsBackground = true }.Start();
        }
    }

}
//To add more features