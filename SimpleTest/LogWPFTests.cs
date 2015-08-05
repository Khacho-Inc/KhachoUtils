using Microsoft.VisualStudio.TestTools.UnitTesting;
using KhachoUtils;
using System.Collections.Generic;
using System.IO;
using System;
using System.Windows.Controls;

namespace KhachoUtilsTests
{
    [TestClass]
    public class LogWPFTests
	{
		[TestMethod]
		public void KhachoUtils_LogWPF_Execute()
		{
			var lb = new ListBox();
			var fileName = "c:\\test_KhachoUtils_LogWPF_Execute.txt";
			if (File.Exists(fileName)) File.Delete(fileName);

			try
			{
				var log = new LogWPF(lb, fileName);

				var message = "simple message";
				var messages = new List<string>(3);
				messages.Add("complex message: row 1");
				messages.Add("complex message: row 2");
				messages.Add("complex message: row 3");

				log.LogRecord(message);
				log.LogRecord(messages);
			}
			catch (Exception excp)
			{
				Assert.Fail(excp.Message);
			}
			if (File.Exists(fileName)) File.Delete(fileName);
		}


        [TestMethod]
        public void KhachoUtils_LogWPF_SimpleTest()
        {
			var fileName = "c:\\test_KhachoUtils_LogWPF_Simple.txt";
			if (File.Exists(fileName) == true) File.Delete(fileName);
            var log = new LogWPF(null, fileName);

            log.LogRecord("c:\\test.txt");

            log.LogRecord(new List<string>(2)
            {
                "",
                "c:\\test.txt"
            });

			Assert.IsTrue(File.Exists(fileName), "тестовый файл не создан");
			var contain = File.ReadAllLines(fileName);
			Assert.AreEqual(contain.Length, 3, 0, "содержимое файла не совпадает с ожиданиями");

			File.Delete(fileName);
        }

		[TestMethod]
		public void KhachoUtils_LogWPF_Create()
		{
			var lb = new ListBox();
			var fileName = "c:\\test_KhachoUtils_LogWPF_Create.txt";
			if (File.Exists(fileName)) File.Delete(fileName);

			try
			{
				// этап 1
				var log = new LogWPF(lb, fileName);
				Assert.IsTrue(log.ReportInFile, "этап 1: запись в файл не будет производиться");
				Assert.IsTrue(log.ViewInForm, "этап 1: отоброажение в контроле не будет производиться");
				Assert.IsTrue(File.Exists(fileName), "этап 1: не был создан файл логов");
				if (File.Exists(fileName)) File.Delete(fileName);

				// этап 2
				log = new LogWPF(null, fileName);
				Assert.IsTrue(log.ReportInFile, "этап 2: запись в файл не будет производиться");
				Assert.IsFalse(log.ViewInForm, "этап 2: будет производиться отоброажение в контроле");
				Assert.IsTrue(File.Exists(fileName), "этап 2: не был создан файл логов");
				if (File.Exists(fileName)) File.Delete(fileName);

				// этап 3
				log = new LogWPF(lb, null);
				Assert.IsFalse(log.ReportInFile, "этап 3: будет производиться запись в файл");
				Assert.IsTrue(log.ViewInForm, "этап 3: отоброажение в контроле не будет производиться");
				Assert.IsFalse(File.Exists(fileName), "этап 3: был создан файл логов");
				if (File.Exists(fileName)) File.Delete(fileName);

				// этап 4
				log = new LogWPF(null, null);
				Assert.IsFalse(log.ReportInFile, "этап 4: будет производиться запись в файл");
				Assert.IsFalse(log.ViewInForm, "этап 4: будет производиться отоброажение в контроле");
				Assert.IsFalse(File.Exists(fileName), "этап 4: был создан файл логов");
				if (File.Exists(fileName)) File.Delete(fileName);
			}
			catch (Exception excp)
			{
				Assert.Fail(excp.Message);
			}
			if (File.Exists(fileName)) File.Delete(fileName);
		}
    }
}
