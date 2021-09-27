using Nep.Scheduling;
using System;
using System.Diagnostics;

namespace Sandbox
{
    
    [DebuggerDisplay(@"{At.Date.ToString(""dd-MM-yyyy"")}")]
    public class DueDate : ISchedulable
    {
        private DateTime at;
        private int dropbox;
        public DueDate(int Dropbox, DateTime At)
        {
            dropbox = Dropbox;
            at = At;
        }
        public DateTime At => at;
        public int Dropbox => dropbox;

        public ISchedulable Clone(DateTime DateTime)
        {
            return new DueDate(Dropbox, DateTime);
        }
    }
}
