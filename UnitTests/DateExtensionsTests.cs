using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DailyTaskReminder.Tasks;

namespace UnitTests
{
    /// <summary>
    /// Tests date extension methods used for calculating task reminder times and deadlines.
    /// </summary>
    [TestClass]
    public class DateExtensionsTests
    {
        [TestMethod]
        [DataRow(2021, 9, 22, DayOfWeek.Wednesday, 0)] // 2021.2.22 = Wednesday
        [DataRow(2021, 9, 22, DayOfWeek.Thursday, 1)]
        [DataRow(2021, 9, 22, DayOfWeek.Friday, 2)]
        [DataRow(2021, 9, 22, DayOfWeek.Saturday, 3)]
        [DataRow(2021, 9, 22, DayOfWeek.Sunday, 4)]
        [DataRow(2021, 9, 22, DayOfWeek.Monday, 5)]
        [DataRow(2021, 9, 22, DayOfWeek.Tuesday, 6)]
        [DataRow(2000, 2, 29, DayOfWeek.Friday, 3)] // 2000.2.29 = Tuesday
        [DataRow(2000, 2, 27, DayOfWeek.Friday, 5)] // 2000.2.27 = Monday
        public void HowManyDaysUntilThisDayOfWeek(int year, int month, int day, DayOfWeek weekday, int expected)
        {
            //Arrange
            DateTimeOffset d = new DateTime(year, month, day);

            //Act
            int result = d.HowManyDaysUntilThisDayOfWeek(weekday);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(2021, 9, 22, 30, 8)]
        [DataRow(2021, 9, 22, 31, 39)]
        [DataRow(2021, 9, 22, 21, 29)]
        [DataRow(2021, 9, 22, 1, 9)]
        [DataRow(2000, 2, 28, 29, 1)]
        [DataRow(2001, 2, 28, 29, 29)]
        public void HowManyDaysUntilThisDayOfMonth(int year, int month, int day, int wantedDay, int expected)
        {
            //Arrange
            DateTimeOffset d = new DateTime(year, month, day);

            //Act
            int result = d.HowManyDaysUntilThisDayOfMonth(wantedDay);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(2021, 2024)]
        [DataRow(1999, 2000)]
        [DataRow(2099, 2104)]
        [DataRow(2028, 2032)]
        [DataRow(2000, 2004)]
        public void FindNextLeapYear(int year, int expected)
        {
            //Arrange
            DateTimeOffset d = new DateTime(year, 1, 1);

            //Act
            int result = d.FindNextLeapYear();

            //Assert
            Assert.AreEqual(expected, result);
        }
    }
}
