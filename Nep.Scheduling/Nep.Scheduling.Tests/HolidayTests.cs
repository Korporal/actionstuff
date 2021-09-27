using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static Nep.Scheduling.TestHolidayDefinitions;

namespace Nep.Scheduling
{
    [TestClass]
    public class HolidayTests
    {
        [TestMethod]
        public void TestCreate1()
        {
            var h = new Holiday(DT2021_JAN_01, DT2021_JAN_01, Exclusivity.FullyInclusive);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestCreate2()
        {
            var h = new Holiday(DT2021_JAN_01, DT2021_JAN_01, Exclusivity.FullyExclusive);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestCreate3()
        {
            var h = new Holiday(DT2021_JAN_01, 0, Exclusivity.FullyInclusive);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestCreate4()
        {
            var h = new Holiday(DT2021_JAN_01, 0, Exclusivity.FullyExclusive);
        }

        [TestMethod]
        public void TestCreate5()
        {
            var h = new Holiday(DT2021_JAN_01, 1, Exclusivity.FullyInclusive);
        }

        [TestMethod]
        public void TestCreate6()
        {
            var h = new Holiday(DT2021_JAN_01, 1, Exclusivity.FullyExclusive);
        }

        [TestMethod]
        public void TestEndDate1()
        {
            var h = new Holiday(DT2021_JAN_01, 1, Exclusivity.StartIsInclusive);

            Assert.AreEqual(DT2021_JAN_02, h.EndDate);
        }

        [TestMethod]
        public void TestEndDate2()
        {
            var h = new Holiday(DT2021_JAN_01, 1, Exclusivity.EndIsInclusive);

            Assert.AreEqual(DT2021_JAN_02, h.EndDate);
        }


        [TestMethod]
        public void TestDuration1()
        {
            var h = new Holiday(DT2021_JAN_01, DT2021_JAN_01, Exclusivity.FullyInclusive);

            Assert.AreEqual(1, h.Duration);

        }

        [TestMethod]
        public void TestWithinHoliday1()
        {
            var h = new Holiday(DT2021_JAN_01, DT2021_JAN_01, Exclusivity.FullyInclusive);

            Assert.IsTrue(h.FallsWithinHoliday(DT2021_JAN_01));

        }

        [TestMethod]
        public void TestWithinHoliday2()
        {
            var h = new Holiday(DT2021_JAN_01, DT2021_JAN_01, Exclusivity.FullyInclusive);

            Assert.IsFalse(h.FallsWithinHoliday(DT2021_JAN_02));

        }

        [TestMethod]
        public void TestWithinHoliday3()
        {
            var h = new Holiday(DT2021_JAN_01, DT2021_JAN_10, Exclusivity.StartIsInclusive);

            Assert.IsTrue(h.FallsWithinHoliday(DT2021_JAN_01));
            Assert.IsFalse(h.FallsWithinHoliday(DT2021_JAN_10));

        }

        [TestMethod]
        public void TestWithinHoliday4()
        {
            var h = new Holiday(DT2021_JAN_01, DT2021_JAN_10, Exclusivity.EndIsInclusive);

            Assert.IsTrue(h.FallsWithinHoliday(DT2021_JAN_10));
            Assert.IsFalse(h.FallsWithinHoliday(DT2021_JAN_01));

        }


        [TestMethod]
        public void TestDuration2()
        {
            var h = new Holiday(DT2021_JAN_01, DT2021_JAN_10, Exclusivity.FullyInclusive);

            Assert.AreEqual(10, h.Duration);

        }

        [TestMethod]
        public void TestDuration3()
        {
            var h = new Holiday(DT2021_JAN_01, DT2021_JAN_10, Exclusivity.FullyExclusive);

            Assert.AreEqual(8, h.Duration);

        }

        [TestMethod]
        public void TestDuration4()
        {
            var h = new Holiday(DT2021_JAN_01, DT2021_JAN_10, Exclusivity.StartIsInclusive);

            Assert.AreEqual(9, h.Duration);

        }

        [TestMethod]
        public void TestDuration5()
        {
            var h = new Holiday(DT2021_JAN_01, DT2021_JAN_10, Exclusivity.EndIsInclusive);

            Assert.AreEqual(9, h.Duration);

        }

    }
}
