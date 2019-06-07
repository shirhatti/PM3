using Microsoft.Diagnostics.Tools.RuntimeClient;
using Microsoft.Diagnostics.Tracing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Text;
using Terminal.Gui;

namespace ProcessManager
{
    internal class CountersTextView : TextView
    {
        public CountersTextView(Rect frame) : base(frame)
        { }
        public CountersTextView() : base()
        { }
        private Process _process;
        private ulong _sessionId;

        public void Start(Process process)
        {
            _process = process;
            var providerList = new List<Provider>()
            {
                new Provider(name: "System.Runtime",
                            keywords: 0x1,
                            eventLevel: EventLevel.Verbose)
            };
            var configuration = new SessionConfiguration(
                circularBufferSizeMB: 100,
                outputPath: "",
                providers: providerList);

            var binaryReader = EventPipeClient.CollectTracing(_process.Id, configuration, out _sessionId);
            var source = new EventPipeEventSource(binaryReader);

            source.Dynamic.All += Dynamic_All;
        }

        public void Stop()
        {
            EventPipeClient.StopTracing(_process.Id, _sessionId);
            _process = null;
        }

        private void Dynamic_All(TraceEvent obj)
        {
            Console.WriteLine(obj.EventName);
            if (obj.EventName.Equals("EventCounters"))
            {
                Console.WriteLine(obj.EventName);
            }
        }

        public static CountersTextView Create()
        {
            var statusBar = new CountersTextView()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            return statusBar;
        }
    }
}
