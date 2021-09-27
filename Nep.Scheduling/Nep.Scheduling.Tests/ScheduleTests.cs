using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using static Nep.Scheduling.TestHolidayDefinitions;

namespace Nep.Scheduling
{
    [TestClass]
    public class ScheduleTests
    {
        [TestMethod]
        public void TestCreate1()
        {
            var schedule = new Schedule(true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPopulate0()
        {
            var schedule = new Schedule(true);
            schedule.Add(null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestPopulate1()
        {
            var schedule = new Schedule(true);
            schedule.AddRange(Enumerable.Empty<TestDateContainer>());
        }

        [TestMethod]
        public void TestPopulate2()
        {
            var schedule = new Schedule(true);

            schedule.Add(new TestDateContainer(DT2021_JAN_01));
            schedule.Add(new TestDateContainer(DT2021_JAN_17));
            schedule.Add(new TestDateContainer(DT2021_JAN_23));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestPopulate3()
        {
            var schedule = new Schedule(true);

            schedule.Add(new TestDateContainer(DT2021_JAN_01));
            schedule.Add(new TestDateContainer(DT2021_JAN_17));
            schedule.Add(new TestDateContainer(DT2021_JAN_01));
        }

        [TestMethod]
        public void TestApplyHoliday1()
        {
            var schedule = new Schedule(true);

            schedule.Add(new TestDateContainer(DT2021_JAN_01));
            schedule.Add(new TestDateContainer(DT2021_JAN_08));
            schedule.Add(new TestDateContainer(DT2021_JAN_15));

            var holiday = new Holiday(DT2021_JAN_04, DT2021_JAN_08, Exclusivity.FullyInclusive);

            var s = schedule.Adjust(holiday, Adjustment.Apply, out _, 7);

            Assert.IsTrue(s.Items[0].At == DT2021_JAN_01);
            Assert.IsTrue(s.Items[1].At == DT2021_JAN_15);
            Assert.IsFalse(s.Items[2].At == DT2021_JAN_22);
        }

        [TestMethod]
        public void TestRetractHoliday1()
        {
            var schedule = new Schedule(true);

            schedule.Add(new TestDateContainer(DT2021_JAN_01));
            schedule.Add(new TestDateContainer(DT2021_JAN_08));
            schedule.Add(new TestDateContainer(DT2021_JAN_15));

            var holiday1 = new Holiday(DT2021_JAN_04, DT2021_JAN_08, Exclusivity.FullyInclusive);
            var holiday2 = new Holiday(DT2021_JAN_07, DT2021_JAN_10, Exclusivity.FullyInclusive);

            var s1 = schedule.Adjust(holiday1, Adjustment.Apply, out _, 7).Adjust(holiday2, Adjustment.Apply, out _, 7);
            var s2 = s1.Adjust(holiday1, Adjustment.Retract, out _, 7).Adjust(holiday2, Adjustment.Retract, out _, 7);

            Assert.IsTrue(s2.Items[0].At == DT2021_JAN_01);
            Assert.IsTrue(s2.Items[1].At == DT2021_JAN_08);
            Assert.IsTrue(s2.Items[2].At == DT2021_JAN_15);

            Assert.IsTrue(schedule.GetScheduleString() == s2.GetScheduleString());
        }

        [TestMethod]
        public void TestRetractHoliday2()
        {
            var schedule = new Schedule(true);

            schedule.Add(new TestDateContainer(DT2021_JAN_01));
            schedule.Add(new TestDateContainer(DT2021_JAN_08));
            schedule.Add(new TestDateContainer(DT2021_JAN_15));
            schedule.Add(new TestDateContainer(DT2021_JAN_22));
            schedule.Add(new TestDateContainer(DT2021_JAN_29));
            schedule.Add(new TestDateContainer(DT2021_FEB_05));
            schedule.Add(new TestDateContainer(DT2021_FEB_12));
            schedule.Add(new TestDateContainer(DT2021_FEB_19));

            var holiday1 = new Holiday(DT2021_JAN_04, DT2021_JAN_08, Exclusivity.FullyInclusive);
            var holiday2 = new Holiday(DT2021_JAN_17, DT2021_JAN_22, Exclusivity.FullyInclusive);

            var s1 = schedule.Adjust(holiday1, Adjustment.Apply, out _, 7).Adjust(holiday2, Adjustment.Apply, out _, 7);
            var s2 = s1.Adjust(holiday2, Adjustment.Retract, out _, 7).Adjust(holiday1, Adjustment.Retract, out _, 7);

            Assert.IsTrue(s2.Items[0].At == DT2021_JAN_01);
            Assert.IsTrue(s2.Items[1].At == DT2021_JAN_08);
            Assert.IsTrue(s2.Items[2].At == DT2021_JAN_15);
            Assert.IsTrue(s2.Items[3].At == DT2021_JAN_22);
            Assert.IsTrue(s2.Items[4].At == DT2021_JAN_29);
            Assert.IsTrue(s2.Items[5].At == DT2021_FEB_05);
            Assert.IsTrue(s2.Items[6].At == DT2021_FEB_12);
            Assert.IsTrue(s2.Items[7].At == DT2021_FEB_19);

            Assert.IsTrue(schedule.GetScheduleString() == s2.GetScheduleString());
        }

        [TestMethod]
        public void TestRetractHoliday3()
        {
            var schedule = new Schedule(true);

            schedule.Add(new TestDateContainer(DT2021_JAN_01));
            schedule.Add(new TestDateContainer(DT2021_JAN_08));
            schedule.Add(new TestDateContainer(DT2021_JAN_15));
            schedule.Add(new TestDateContainer(DT2021_JAN_22));
            schedule.Add(new TestDateContainer(DT2021_JAN_29));
            schedule.Add(new TestDateContainer(DT2021_FEB_05));
            schedule.Add(new TestDateContainer(DT2021_FEB_12));
            schedule.Add(new TestDateContainer(DT2021_FEB_19));

            var holiday1 = new Holiday(DT2021_JAN_04, DT2021_JAN_08, Exclusivity.FullyInclusive);
            var holiday2 = new Holiday(DT2021_JAN_17, DT2021_JAN_22, Exclusivity.FullyInclusive);

            var s1 = schedule.Adjust(holiday1, Adjustment.Apply, out _, 7).Adjust(holiday2, Adjustment.Apply, out _, 7);
            var s2 = s1.Adjust(holiday2, Adjustment.Retract, out _,  7).Adjust(holiday1, Adjustment.Retract, out _, 7);

            Assert.IsTrue(s2.Items[0].At == DT2021_JAN_01);
            Assert.IsTrue(s2.Items[1].At == DT2021_JAN_08);
            Assert.IsTrue(s2.Items[2].At == DT2021_JAN_15);
            Assert.IsTrue(s2.Items[3].At == DT2021_JAN_22);
            Assert.IsTrue(s2.Items[4].At == DT2021_JAN_29);
            Assert.IsTrue(s2.Items[5].At == DT2021_FEB_05);
            Assert.IsTrue(s2.Items[6].At == DT2021_FEB_12);
            Assert.IsTrue(s2.Items[7].At == DT2021_FEB_19);
        }

        [TestMethod]
        public void TestAdjsutability1()
        {
            var schedule = new Schedule(true);

            schedule.Add(new TestDateContainer(DT2021_JAN_10));
            schedule.Add(new TestDateContainer(DT2021_JAN_14));
            schedule.Add(new TestDateContainer(DT2021_JAN_15));
            schedule.Add(new TestDateContainer(DT2021_JAN_22));
            schedule.Add(new TestDateContainer(DT2021_JAN_29));
            schedule.Add(new TestDateContainer(DT2021_FEB_05));
            schedule.Add(new TestDateContainer(DT2021_FEB_12));
            schedule.Add(new TestDateContainer(DT2021_FEB_19));

            var holiday1 = new Holiday(DT2021_JAN_04, DT2021_JAN_08, Exclusivity.FullyInclusive);
            var holiday2 = new Holiday(DT2021_FEB_20, DT2021_FEB_27, Exclusivity.FullyInclusive);

            Assert.IsTrue(schedule.IsAdjustedBy(holiday1, Adjustment.Apply, 7));
            Assert.IsFalse(schedule.IsAdjustedBy(holiday2, Adjustment.Apply, 7));

        }
    }
}