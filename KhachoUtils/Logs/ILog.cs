using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace KhachoUtils
{
	/// <summary>
	/// Интерфейс логов.
	/// </summary>
	public interface ILog : INotifyPropertyChanged
	{
		#region {PROPERTIES}

		/// <summary>
		/// Локальное хранилище лога действий.
		/// </summary>
		ObservableCollection<string> ActionLog { get; set; }

		/// <summary>
		/// Возвращает признак необходимость записи лога в файл.
		/// </summary>
		bool ReportInFile { get; }

		#endregion

		
		#region {METHODS}

		/// <summary>
		/// Вносит запись в лог.
		/// </summary>
		/// <param name="record">Описание события.</param>
		void LogRecord(string record);

		/// <summary>
		/// Вносит запись в лог.
		/// </summary>
		/// <param name="record">Описание события.</param>
		void LogRecord(List<string> record);

		/// <summary>
		/// Очищает лог ошибок.
		/// </summary>
		void ClearLog();

		#endregion
	}
}
