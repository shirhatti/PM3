using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace System.Diagnostics
{
    internal static class ProcessExtensions
    {
        public static string FormatDetailsAsString(this Process process)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Process Name: {process.ProcessName}");
            sb.AppendLine($"Process Id:   {process.Id}");
            sb.AppendLine();
            sb.AppendLine("Modules");
            foreach (ProcessModule module in process.Modules)
            {
                sb.AppendLine($"  {module.ModuleName}");
            }
            return sb.ToString();
        }
    }
}
