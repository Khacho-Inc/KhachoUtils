using Microsoft.VisualStudio.TestTools.UnitTesting;
using KhachoUtils;
using System.Collections.Generic;
using System.IO;
using System;
using System.Windows.Controls;

namespace KhachoUtilsTests
{
	/// <summary>
	/// Тесты к логу WPF.
	/// </summary>
	[TestClass]
	public class LogWPFTests
	{
		/// <summary>
		/// Проверяем работу конструкторов.
		/// </summary>
		[TestMethod]
		public void KhachoUtils_LogWPF_Create()
		{
			var fileName = "c:\\test_KhachoUtils_LogWPF_Create.txt";
			if (File.Exists(fileName)) File.Delete(fileName);

			try
			{
				// этап 1
				var log = new LogWPF(fileName);
				Assert.IsTrue(log.ReportInFile, "этап 1: запись в файл не будет производиться");
				Assert.IsTrue(File.Exists(fileName), "этап 1: не был создан файл логов");
				if (File.Exists(fileName)) File.Delete(fileName);

				// этап 2
				log = new LogWPF(null);
				Assert.IsFalse(log.ReportInFile, "этап 2: запись в файл будет производиться");
				Assert.IsFalse(File.Exists(fileName), "этап 2: был создан файл логов");
				if (File.Exists(fileName)) File.Delete(fileName);
			}
			catch (Exception excp)
			{
				Assert.Fail(excp.Message);
			}
			if (File.Exists(fileName)) File.Delete(fileName);
		}

		/// <summary>
		/// Проверяем работу функций добавления записей.
		/// </summary>
		[TestMethod]
		public void KhachoUtils_LogWPF_Execute()
		{
			var fileName = "c:\\test_KhachoUtils_LogWPF_Execute.txt";
			if (File.Exists(fileName)) File.Delete(fileName);

			try
			{
				var log = new LogWPF(fileName);

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

		/// <summary>
		/// Проверяем правильность работы с файлом логов.
		/// </summary>
		[TestMethod]
		public void KhachoUtils_LogWPF_File()
		{
			var fileName = "c:\\test_KhachoUtils_LogWPF_Simple.txt";
			if (File.Exists(fileName) == true) File.Delete(fileName);
			var log = new LogWPF(fileName);

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

		/// <summary>
		/// Проверяем работу привязки отображателя логов.
		/// </summary>
		[TestMethod]
		public void KhachoUtils_LogWPF_Binding()
		{
			var lv = new ListView();
			var log = new LogWPF(null);
			lv.DataContext = log;
			lv.ItemsSource = log.ActionLog;

			log.LogRecord("777");

			Assert.AreEqual(log.ActionLog.Count, lv.Items.Count, 0, "привязка не удалась");
		}
	}
}
