using System;

namespace KhachoUtils.AccessManager
{
	/// <summary>
	/// Интерфейс формы, запрашивающей логин и пароль.
	/// </summary>
	public interface ILoginDialog
	{
		#region {PROPERTIES}

		/// <summary>
		/// Возвращает или устанавливает результат диалога, который вернется методом ShowDialog.
		/// </summary>
		bool? DialogResult { get; set; }

		#endregion


		#region {EVENTS}

		/// <summary>
		/// Происходит после принятия от пользователя аутентификационных данных.
		/// </summary>
		event LoginDialogEventHandler LoginDataAccepted;

		#endregion


		#region {METHODS}

		/// <summary>
		/// Показывает диалог с пользователем.
		/// </summary>
		bool? ShowDialog();

		/// <summary>
		/// Закрывает диалог с пользователем.
		/// </summary>
		void Close();

		/// <summary>
		/// Оповещает пользователя об ошибке аутентификации.
		/// </summary>
		void ShowError();

		#endregion
	}

	/// <summary>
	/// Представляет метод обработки события LoginDataAccepted интерфейса ILoginDialog.
	/// </summary>
	/// <param name="sender">Источник события.</param>
	/// <param name="ea">Данные по событию.</param>
	public delegate void LoginDialogEventHandler(object sender, LoginDialogEventArgs ea);

	/// <summary>
	/// Данные по событиям типа LoginDialogEventHandler.
	/// </summary>
	public class LoginDialogEventArgs : EventArgs
	{
		#region {PROPERTIES}

		/// <summary>
		/// Имя пользователя, введенное пользователем.
		/// </summary>
		public string UserName { get; private set; }

		/// <summary>
		/// Пароль, введенный пользователем.
		/// </summary>
		public string Password { get; private set; }

		#endregion


		#region {CONSTRUCTORS}

		/// <summary>
		/// Инициализирует новый экземпляр класса LoginDialogEventArgs на основе введенных пользователем данных.
		/// </summary>
		/// <param name="userName">Имя пользователя, введенное пользователем.</param>
		/// <param name="password">Пароль, введенный пользователем.</param>
		public LoginDialogEventArgs(string userName, string password)
		{
			// сохраняем параметры
			this.UserName = userName;
			this.Password = password;
		}

		#endregion
	}
}