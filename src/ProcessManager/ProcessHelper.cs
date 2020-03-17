using Microsoft.Diagnostics.NETCore.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ProcessManager
{
    internal static class ProcessHelper
    {
        public static IList<Process> GetProcessList()
        {
            return DiagnosticsClient.GetPublishedProcesses()
                .Select(GetProcessById)
                .Where(process => process != null)
                .OrderBy(process => process.ProcessName)
                .ThenBy(process => process.Id)
                .ToList();
        }

        private static Process GetProcessById(int processId)
        {
            try
            {
                return Process.GetProcessById(processId);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
    }
}
