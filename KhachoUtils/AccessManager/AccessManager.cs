using System.Collections.Generic;

namespace KhachoUtils.AccessManager
{
	/// <summary>
	/// Класс, описывающий диспетчер доступа.
	/// </summary>
	public class AccessManager
	{
		#region {PROPERTIES}

		/// <summary>
		/// Возвращает признак наличия авторизованного пользователя.
		/// </summary>
		public bool HasAuthenticatedUser
		{
			get { return currentUser != null; }
		}

		#endregion


		#region {MEMBERS}

		/// <summary>
		/// Список пользователей, доступный в приложении.
		/// </summary>
		List<User> users;

		/// <summary>
		/// Текущий аутентифицированный пользователь.
		/// </summary>
		User currentUser;

		#endregion


		#region {CONSTRUCTORS}

		/// <summary>
		/// Инициализирует новый экземпляр класса AccessManager с заданным списком пользователей.
		/// </summary>
		/// <param name="users"></param>
		public AccessManager(List<User> users)
		{
			// сохраняем параметры
			this.users = users;
		}

		#endregion


		#region {PRIVATE_METHODS}

		/// <summary>
		/// Проводит попытку аутентификации с заданными параметрами и возвращает результат попытки.
		/// </summary>
		/// <param name="userName">Имя пользователя.</param>
		/// <param name="password">Пароль.</param>
		/// <returns>true - аутентификация прошла успешно; false - имя пользователя и/или пароль неверны.</returns>
		private bool login(string userName, string password)
		{
			// ищем полностью совпавшую пару <имя пользователя, пароль>
			currentUser = users.Find(user => user.CheckAccess(userName, password));
			// возвращаем результат
			return (currentUser != null);
		}

		#endregion


		#region {PUBLIC_METHODS}

		/// <summary>
		/// Проводит попытку аутентификации с заданными параметрами и возвращает результат попытки.
		/// </summary>
		/// <param name="userName">Имя пользователя.</param>
		/// <param name="password">Пароль.</param>
		/// <returns>true - аутентификация прошла успешно; false - имя пользователя и/или пароль неверны.</returns>
		public bool Login(string userName, string password)
		{
			// проводим аутентификацию
			return login(userName, password);
		}

		/// <summary>
		/// Ведет диалог с пользователем, запрашивая у него данные аутентификации, посредством переданного окна.
		/// </summary>
		/// <param name="window">Окно, ведущее диалог с пользователем.</param>
		public void ShowDialog(ILoginDialog window)
		{
			// подписываемся на событие принятия данных
			window.LoginDataAccepted += Window_LoginDataAccepted;
			// начинаем диалог
			window.ShowDialog();
		}

		/// <summary>
		/// Производит настройку программы в соответствии с правами текущего пользователя.
		/// </summary>
		public void ConfigureAccess()
		{
			// проверяем наличие аутентифицированного пользователя
			if (HasAuthenticatedUser)
				// проводим настройку программы
				currentUser.ConfigureAccess();
		}

		/// <summary>
		/// Записывает в переданные параметры имя пользователя и пароль, если имеется авторизованный пользователь. В противном случаем возвращает пустые строки.
		/// </summary>
		/// <param name="userName">Параметр, в который будет записано имя пользователя.</param>
		/// <param name="password">Параметр, в который будет записан пароль.</param>
		public void SaveCurrentData(out string userName, out string password)
		{
			// проверяем наличие авторизованного пользователя
			if (HasAuthenticatedUser)
			{
				// возвращаем имя и пароль авторизоавнного пользователя
				userName = currentUser.GetName();
				password = currentUser.GetPassword();
			}
			else
			{
				// возвращаем пустые данные
				userName = string.Empty;
				password = string.Empty;
			}
		}

		/// <summary>
		/// Возвращает описание авторизованного пользователя или пустую строку, если авторизованного пользователя нет.
		/// </summary>
		/// <returns>Описание авторизованного пользователя или пустая строка.</returns>
		public string GetCurrentUserDescription()
		{
			// проверяем наличие авторизованного пользователя
			if (HasAuthenticatedUser)
				// возвращаем описание
				return currentUser.Description;
			else
				// возвращаем пустую строку
				return string.Empty;
		}

		/// <summary>
		/// Сбрасывает текущего пользователя.
		/// </summary>
		public void ClearCurrentUser()
		{
			// сбрасываем текущего пользователя
			currentUser = null;
		}

		/// <summary>
		/// Ведет диалог с пользователем по смене пароля на текущего пользователя.
		/// </summary>
		/// <param name="window">Окно, ведущее диалог с пользователем.</param>
		public void ChangePassword(IChangePasswordDialog window)
		{
			// проверяем наличие авторизованного пользователя
			if (HasAuthenticatedUser == false) return;

			// подписываемся на событие принятия данных
			window.NewLoginDataAccepted += Window_NewLoginDataAccepted;
			// начинаем диалог, передавая в него имя текущего пользователя
			window.ShowDialog(currentUser.GetName());
		}

		#endregion


		#region {EVENT_METHODS}

		private void Window_LoginDataAccepted(object sender, LoginDialogEventArgs ea)
		{
			// извлекаем окно
			var window = sender as ILoginDialog;
			// проверяем правильность введенных данных
			if (login(ea.UserName, ea.Password) == true)
			{
				// отписываемся от события
				window.LoginDataAccepted -= Window_LoginDataAccepted;
				// выставляем результат диалога
				window.DialogResult = true;
				// закрываем окно
				window.Close();
			}
			else
			{
				// оповещаем пользователя об ошибке аутентификации
				window.ShowError();
			}
		}

		private void Window_NewLoginDataAccepted(object sender, ChangePasswordDialogEventArgs ea)
		{
			// извлекаем окно
			var window = sender as IChangePasswordDialog;
			// проверяем правильность введенных данных
			if (login(ea.UserName, ea.Password) == true)
			{
				// отписываемся от события
				window.NewLoginDataAccepted -= Window_NewLoginDataAccepted;
				// закрываем окно
				window.Close();
				// меняем пароль пользователю
				currentUser.SetNewPassword(ea.NewPassword);
			}
			else
			{
				// оповещаем пользователя об ошибке аутентификации
				window.ShowError();
			}
		}

		#endregion
	}
}
