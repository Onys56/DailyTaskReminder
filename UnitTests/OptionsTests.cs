using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DailyTaskReminder;

namespace UnitTests
{

    /// <summary>
    /// Tests the parsing of the command line arguemnts.
    /// </summary>
    [TestClass]
    public class OptionsTests
    {
        [TestMethod]
        [DataRow("", "Tasks.txt", "Reminders.json", "25566", "*", false, false)]
        [DataRow("-t MyTasks.txt -h", "MyTasks.txt", "Reminders.json", "25566", "*", true, false)]
        [DataRow("--port 28465 -a https://mydomain.com -v", "Tasks.txt", "Reminders.json", "28465", "https://mydomain.com", false, true)]
        [DataRow("-h --reminders Rem.json", "Tasks.txt", "Rem.json", "25566", "*", true, false)]
        [DataRow("-t t.txt -r r.json -p 12345 -a https://me.com -h -v", "t.txt", "r.json", "12345", "https://me.com", true, true)]
        public void ParsesOptionsSuccessfuly(string arguments, string expectedTaskPath, string expectedRemindersPath,
            string expectedPort, string expectedAccess, bool expectedHush, bool expectedVerbose)
        {
            //Arrange
            string[] args = arguments.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            //Act
            Options options = Options.ParseArguments(args);

            //Assert
            Assert.AreEqual(expectedTaskPath, options.TasksPath);
            Assert.AreEqual(expectedRemindersPath, options.RemindersPath);
            Assert.AreEqual(expectedPort, options.Port);
            Assert.AreEqual(expectedAccess, options.Access);
            Assert.AreEqual(expectedHush, options.Hush);
            Assert.AreEqual(expectedVerbose, options.Verbose);
        }
    }
}
