using Microsoft.Diagnostics.NETCore.Client;
using Microsoft.Diagnostics.Tools.Counters;
using Microsoft.Diagnostics.Tracing;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
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
        private EventPipeSession _session;
        private Task _task;
        private IDictionary<string, string> _counters;

        public void Start(Process process)
        {
            _process = process;
            _counters = new SortedDictionary<string, string>();
            _task = new Task(() =>
            {
                var diagnosticsClient = new DiagnosticsClient(_process.Id);
                var providerList = new List<EventPipeProvider>()
                {
                    new EventPipeProvider(name: "System.Runtime",
                                keywords: long.MaxValue,
                                eventLevel: EventLevel.Verbose,
                                arguments: new Dictionary<string, string>() { { "EventCounterIntervalSec", "1" } })
                };

                _session = diagnosticsClient.StartEventPipeSession(
                                providers: providerList,
                                requestRundown: false);
                
                var source = new EventPipeEventSource(_session.EventStream);

                source.Dynamic.AddCallbackForProviderEvent("System.Runtime", "EventCounters", CounterEvent);
                source.Process();
            });

            _task.Start();
        }

        public void Update()
        {
            var sb = new StringBuilder();
            foreach(var counter in _counters)
            {
                sb.AppendLine($"{counter.Key, -40} {counter.Value}");
            }
            Text = sb.ToString();
            SetNeedsDisplay();
        }

        public void Stop()
        {
            if (_process != null)
            {
                _session.Stop();
                _counters = null;
                _task = null;
                _process = null;
            }
        }

        private void CounterEvent(TraceEvent obj)
        {
            IDictionary<string, object> payloadVal = (IDictionary<string, object>)(obj.PayloadValue(0));
            IDictionary<string, object> payloadFields = (IDictionary<string, object>)(payloadVal["Payload"]);

            // There really isn't a great way to tell whether an EventCounter payload is an instance of 
            // IncrementingCounterPayload or CounterPayload, so here we check the number of fields 
            // to distinguish the two.
            ICounterPayload payload;
            if (payloadFields.ContainsKey("CounterType"))
            {
                payload = payloadFields["CounterType"].Equals("Sum") ? (ICounterPayload)new IncrementingCounterPayload(payloadFields) : (ICounterPayload)new CounterPayload(payloadFields);
            }
            else
            {
                payload = payloadFields.Count == 6 ? (ICounterPayload)new IncrementingCounterPayload(payloadFields) : (ICounterPayload)new CounterPayload(payloadFields);
            }
            _counters[payload.GetDisplay()] = payload.GetValue();
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
