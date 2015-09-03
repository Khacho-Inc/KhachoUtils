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
		/// Возвращает авторизованного пользователя или null, если его нет.
		/// </summary>
		/// <returns>Авторизованный пользователь или null, если авторизованного пользователя нет.</returns>
		public User GetCurrentUser()
		{
			return currentUser;
		}

		/// <summary>
		/// Сбрасывает текущего пользователя.
		/// </summary>
		public void ClearCurrentUser()
		{
			// сбрасываем текущего пользователя
			currentUser = null;
		}

		#endregion
	}
}
