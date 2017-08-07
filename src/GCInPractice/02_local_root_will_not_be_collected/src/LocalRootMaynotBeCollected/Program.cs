using System;
using System.Threading;

namespace LocalRootMaynotBeCollected
{
    /*
     * We have turned on code optimization on both configurations, so that the
     * GC will perform eager reference look ahead optimizations.
     */

    static class Program
    {
        const string WelcomeMessage =
            "Please press enter key you have lost your patient. Then I will tell" +
            "you if the timer is still a local root.\r\n\r\n" +
            "Now, press any key to start the experiment. And press another enter " +
            "key to stop it.\r\n";

        const string timerMessage = "[TIMER] I am still alive.";

        const string stillReferencedMessage =
            "By the time you pressed a key. The timer has not been collected.";

        const string alreadyBeenCollectedMessage =
            "Oops, the timer has been collected even if the main method has't " +
            "completed yet.";

        const string EndOfExperiment = 
            "================= End of experiment =================";

        const string StartOfExperiment = 
            "================== Experiment Start =================";
        
        static void Main()
        {
            Console.WriteLine(WelcomeMessage);
            Console.ReadLine();
            Console.WriteLine(StartOfExperiment);

            /*
             * First we created a timer instance to periodically output a message,
             * so that we can easily make sure if the timer has been collected.
             */

            var timer = new Timer(
                _ =>
                {
                    Console.WriteLine(timerMessage);

                    /*
                     * You can comment this code to see the timer keep alive for a
                     * relatively long time because of the low memory usage.
                     * 
                     * But if a GC collection occures, since there is no strong
                     * reference that references Timer instance, the timer instance
                     * will be marked and collected.
                     */
                    GC.Collect();
                },
                null,
                0,
                1000);

            /*
             * We use a weak reference to track timer so that the GC will not be
             * interfered by this variable.
             */
            var weakTimer = new WeakReference(timer);

            Console.ReadLine();

            var strongRef = (Timer)weakTimer.Target;
            bool stillReferenced = strongRef != null;

            if (stillReferenced)
            {
                WaitHandle waitHandle = new ManualResetEvent(false);
                strongRef.Dispose(waitHandle);
                waitHandle.WaitOne();
            }

            Console.WriteLine(EndOfExperiment);
            Console.WriteLine(stillReferenced ? stillReferencedMessage : alreadyBeenCollectedMessage);
            
            /*
             * You can uncomment the code to keep the timer alive since the
             * strong reference is kept until the end of Main() method.
             */
            // GC.KeepAlive(timer);
        }
    }
}
