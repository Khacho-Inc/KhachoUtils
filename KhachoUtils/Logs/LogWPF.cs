using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace KhachoUtils
{
	/// <summary>
	/// Класс, логирующий действия программы.
	/// </summary>
	public class LogWPF : FrameworkElement, ILog
	{
		#region {EVENTS}

		/// <summary>
		/// Возмникает при изменении свойств.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion


		#region {DEPENDENCY_PROPERTIES}

		/// <summary>
		/// Локальное хранилище лога действий.
		/// </summary>
		public static DependencyProperty ActionLogProperty { get; set; }

		#endregion


		#region {PROPERTIES}

		/// <summary>
		/// Локальное хранилище лога действий.
		/// </summary>
		public ObservableCollection<string> ActionLog
		{
			get { return (ObservableCollection<string>)base.GetValue(ActionLogProperty); }
			set { base.SetValue(ActionLogProperty, value); }
		}

		/// <summary>
		/// Возвращает признак необходимость записи лога в файл.
		/// </summary>
		public bool ReportInFile { get; private set; }

		#endregion


		#region {MEMBERS}

		/// <summary>
		/// Файл, хранящий лог.
		/// </summary>
		string file;

		/// <summary>
		/// Арбитр записи в файл логов.
		/// </summary>
		ReaderWriterLockSlim rwls_logFile;

		/// <summary>
		/// Список действий, производимых при добавлении новой записи в лог.
		/// </summary>
		List<Action<string>> logRecordActions;

		#endregion


		#region {CONSTRUCTOR}

		/// <summary>
		/// Статический конструктор.
		/// </summary>
		static LogWPF()
		{
			// регистрируем локальное хранилище лога действий
			ActionLogProperty = DependencyProperty.Register("ActionLog", typeof(ObservableCollection<string>), typeof(LogWPF),
				new UIPropertyMetadata(null, new PropertyChangedCallback(actionLogChanged)));
		}

		/// <summary>
		/// Инициализирует экземпляр класса Log на основе заданных параметров.
		/// </summary>
		/// <param name="file">Файл, хранящий лог.</param>
		public LogWPF(string file)
		{
			// инициализируем список действий, производимых при добавлении записи в лог
			logRecordActions = new List<Action<string>>();

			// инициализируем локальное хранилище лога
			ActionLog = new ObservableCollection<string>();
			// добавляем действие добавления (простите за тавтологию) новой записи в хранилище
			logRecordActions.Add(addLogRecordToContainer);

			if (string.IsNullOrEmpty(file) == false)
			{
				// проверяем статус файла, адресс которого передан в параметрах
				var fInfo = new FileInfo(file);
				// инициализируем арбитр записи в файл логов
				rwls_logFile = new ReaderWriterLockSlim();
				// сохраняем переданные параметры
				this.file = file;
				if (fInfo.Directory.Exists == false)
				// создаем хранилище логов, если оно еще не существует
				{
					Directory.CreateDirectory(fInfo.Directory.FullName);
				}
				//else
				//// если хранилище существует, то отчищаем информацию в нем
				//{
				//	foreach (var oldLogFile in fInfo.Directory.GetFiles())
				//	{
				//		File.Delete(oldLogFile.FullName);
				//	}
				//}
				// создаем файл лога
				using (File.Create(file)) { }

				// добавляем действие внесения новой записи в файл логов в список требуемых действий
				logRecordActions.Add(reportLogInFile);
				// возводим признак необходимости внесения лога в файл
				ReportInFile = true;
			}
		}

		#endregion


		#region {DEPENDENCY_PROPERTIES_METHODS}

		private static void actionLogChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs ea)
		{
			// извлекаем источник события
			var log = depObj as LogWPF;
			// генерируем событие изменения состава лога
			log.onPropertyChanged("ActionLog");
		}

		#endregion


		#region {PUBLIC_METHODS}

		/// <summary>
		/// Вносит запись в лог.
		/// </summary>
		/// <param name="record">Описание события.</param>
		public void LogRecord(string record)
		{
			// формируем новую запись лога, добавляя информацию о времени
			var newRecord = string.Format("{0} >> {1}", DateTime.Now.ToLongTimeString(), record);

			// осущесвтялем необходимые действия
			foreach (var action in logRecordActions) action(newRecord);
		}

		/// <summary>
		/// Вносит запись в лог.
		/// </summary>
		/// <param name="record">Описание события.</param>
		public void LogRecord(List<string> record)
		{
			// формируем новую запись лога, добавляя информацию о времени
			var temp = string.Format("{0} >> {1}", DateTime.Now.ToLongTimeString(), record[0]);
			record.RemoveAt(0);
			record.Insert(0, temp);

			// осущесвтялем необходимые действия
			foreach (var action in logRecordActions)
				foreach (var item in record)
					action(item);
		}

		/// <summary>
		/// Очищает лог ошибок.
		/// </summary>
		public void ClearLog()
		{
			// если хранилище лога существует, то очищаем его
			if (ActionLog != null)
			{
				ActionLog.Clear();
			}
		}

		#endregion


		#region {PRIVATE_METHODS}

		/// <summary>
		/// Провоцирует генерацию события PropertyChanged.
		/// </summary>
		/// <param name="name">Имя измененного параметра.</param>
		private void onPropertyChanged(string name)
		{
			// проверяем наличие подписчиков
			if (PropertyChanged != null)
			{
				// генерируем событие
				PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		/// <summary>
		/// Добавляет новуб запись в хранилище логов.
		/// </summary>
		/// <param name="newRecord">Новая запись.</param>
		private void addLogRecordToContainer(string newRecord)
		{
			base.Dispatcher.Invoke(delegate
			{
				// вносим запись в локальное хранилище
				ActionLog.Add(newRecord);
				// удаляем 10 записей из лога, если в логе более 1000 записей
				if (ActionLog.Count > 1000) for (int i = 0; i > 100; i++) ActionLog.RemoveAt(0);
			});
		}

		/// <summary>
		/// Заносит новую запись лога в файл логов.
		/// </summary>
		/// <param name="newRecord">Новая запись в логе.</param>
		private void reportLogInFile(string newRecord)
		{
			// блокируем доступ к файлу логов из другого потока
			rwls_logFile.EnterWriteLock();
			try
			{
				// если задан файл логов, то вносим в него запись
				if (string.IsNullOrEmpty(file) == false && File.Exists(file) == true)
				{
					File.AppendAllLines(file, new List<string>(1) { newRecord });
				}
			}
			finally
			{
				// разблокируем доступ к файлу логов
				rwls_logFile.ExitWriteLock();
			}
		}

		#endregion
	}
}
