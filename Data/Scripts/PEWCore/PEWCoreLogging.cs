using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.ModAPI;

namespace PEWCore
{
    public class PEWCoreLogging
    {
        private static PEWCoreLogging PEWCoreLoggingReference;

        private TextWriter PEWCoreWriter = null;
        private int indent = 0;
        private StringBuilder cache = new StringBuilder();

        static public PEWCoreLogging Instance
        {
            get
            {
                if (MyAPIGateway.Utilities == null)
                    return null;

                if (PEWCoreLoggingReference == null)
                    PEWCoreLoggingReference = new PEWCoreLogging("PEWCore.log");

                return PEWCoreLoggingReference;
            }
        }

        public PEWCoreLogging(string logFile)
        {
            try
            {
                if (MyAPIGateway.Utilities != null)
                    PEWCoreWriter = MyAPIGateway.Utilities.WriteFileInLocalStorage(logFile, typeof(PEWCoreLogging));

                PEWCoreLoggingReference = this;
            }
            catch { }
        }

        public void IncreaseIndent()
        {
            indent++;
        }

        public void DecreaseIndent()
        {
            if (indent > 0)
                indent--;
        }

        public void WriteLine(string text)
        {
            if (PEWCoreWriter == null)
                return;

            if (cache.Length > 0)
                PEWCoreWriter.WriteLine(cache);

            cache.Clear();
            cache.Append(DateTime.Now.ToString("[HH:mm:ss] "));
            for (int i = 0; i < indent; i++)
                cache.Append("\t");

            PEWCoreWriter.WriteLine(cache.Append(text));
            PEWCoreWriter.Flush();
            cache.Clear();
        }

        public void Write(string text)
        {
            if (PEWCoreWriter == null)
                return;

            cache.Append(text);
        }

        internal void Close()
        {
            if (cache.Length > 0)
                PEWCoreWriter.WriteLine(cache);

            PEWCoreWriter.Flush();
            PEWCoreWriter.Close();
            PEWCoreWriter = null;
        }
    }
}
