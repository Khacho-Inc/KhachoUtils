using System;

namespace KhachoUtils.AccessManager
{
	/// <summary>
	/// Интерфейс формы, изменяющей логин и пароль.
	/// </summary>
	public interface IChangePasswordDialog
	{
		#region {EVENTS}

		/// <summary>
		/// Происходит после принятия от пользователя аутентификационных данных.
		/// </summary>
		event ChangePasswordDialogEventHandler NewLoginDataAccepted;

		#endregion


		#region {METHODS}

		/// <summary>
		/// Показывает диалог с пользователем, отображая в диалоге заданное имя пользователя.
		/// </summary>
		/// <param name="userName">Отображаемое имя пользователя.</param>
		/// <returns>Результат диалога с пользователем.</returns>
		bool? ShowDialog(string userName);

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
	/// Представляет метод обработки события NewLoginDataAccepted интерфейса IChangeLoginDialog.
	/// </summary>
	/// <param name="sender">Источник события.</param>
	/// <param name="ea">Данные по событию.</param>
	public delegate void ChangePasswordDialogEventHandler(object sender, ChangePasswordDialogEventArgs ea);

	/// <summary>
	/// Данные по событиям типа ChangeLoginDialogEventHandler.
	/// </summary>
	public class ChangePasswordDialogEventArgs : EventArgs
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

		/// <summary>
		/// Новый пароль, введенный пользователем.
		/// </summary>
		public string NewPassword { get; private set; }

		#endregion


		#region {CONSTRUCTORS}

		/// <summary>
		/// Инициализирует новый экземпляр класса ChangeLoginDialogEventArgs на основе введенных пользователем данных.
		/// </summary>
		/// <param name="userName">Имя пользователя, введенное пользователем.</param>
		/// <param name="password">Пароль, введенный пользователем.</param>
		/// <param name="newPassword">Новый пароль, введенный пользователем.</param>
		public ChangePasswordDialogEventArgs(string userName, string password, string newPassword)
		{
			// сохраняем параметры
			this.UserName = userName;
			this.Password = password;
			this.NewPassword = newPassword;
		}

		#endregion
	}
}