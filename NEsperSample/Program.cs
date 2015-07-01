using System;

using com.espertech.esper.client;
using com.espertech.esper.compat;

namespace NEsperSample
{
    class Program
    {
        static EPRuntime _runtime;

        static void Main(string[] args)
        {
            var address = "example.txt";

            InitializeEsper();

            // baseline time to read file in lines
            var timeToReadFile = PerformanceObserver.TimeMicro(
                    () => LineAvailable.ReadLines(address, line => { }));
            Console.WriteLine("{0,9} microseconds - read file by line", timeToReadFile);

            // baseline time to read file in splits
            var timeToReadParts = PerformanceObserver.TimeMicro(
                    () => LineAvailable.ReadParts(address, parts => { }));
            Console.WriteLine("{0,9} microseconds - read file by parts", timeToReadParts);

            // lets baseline how long it takes to read the file
            var timeToReadEvents = PerformanceObserver.TimeMicro(
                () => LineAvailable.ReadEvents(address, null));
            Console.WriteLine("{0,9} microseconds - read file by event", timeToReadEvents);

            var timeToReadAndProcess = PerformanceObserver.TimeMicro(
                () => LineAvailable.ReadEvents(address, ev => _runtime.SendEvent(ev)));
            Console.WriteLine("{0,9} microseconds - read file and process", timeToReadAndProcess);

            Console.ReadLine();
        }

        static void InitializeEsper()
        {
            // It is critical that you tell Esper what events you want to
            // listen to.  However, registering this way creates events
            // that use the class name (short form) as the event name.  If
            // you choose not to use it, you must use the full class name
            // including namespace to identify the class.
            var config = new Configuration();
            // I prefer add events using the type as a generic argument.
            config.AddEventType<TotalEvent>();

            var EPSP = EPServiceProviderManager.GetDefaultProvider(config);
            _runtime = EPSP.EPRuntime;

            var query = "select * from TotalEvent";
            var statement = EPSP.EPAdministrator.CreateEPL(query);
            Console.WriteLine(statement);
            // now the statement is listening to events, but if you want your handler to receive
            // events, you're going to have to use one of the techniques to listen to events.
            statement.Events += (sender, e) =>
            {
#if NOT_IN_USE
                Console.WriteLine("I just received an event");
                Console.WriteLine("Sender: {0}", sender); // this will be the statement
                Console.WriteLine("NewEvents: {0}", e.NewEvents); // this is probably what you want
                foreach (var nevent in e.NewEvents)
                {
                    // when using '*' the underlying property is the object
                    var @event = nevent.Underlying as TotalEvent;
                    Console.WriteLine(@event);
                    // Assuming you just want to get properties, you can just use the indexer
                    // but you must be sure that the property case is correct.
                    String month = nevent["Month"] as String;
                    Console.WriteLine(month);
                }
#endif
            };
        }
    }
}