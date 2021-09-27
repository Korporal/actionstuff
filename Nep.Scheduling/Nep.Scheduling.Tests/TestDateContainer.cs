using System;
using System.Diagnostics;

namespace Nep.Scheduling
{
    [DebuggerDisplay(@"{At.Date.ToString(""dd-MM-yyyy"")}")]
    public class TestDateContainer : ISchedulable
    {
        private DateTime at;
        public TestDateContainer(DateTime At)
        {
            at = At;
        }
        public DateTime At => at;


        public ISchedulable Clone(DateTime DateTime)
        {
            return new TestDateContainer(DateTime);
        }
    }
}