namespace KhachoUtils.AccessManager
{
	/// <summary>
	/// Абстрактный класс пользователя программы.
	/// </summary>
	public abstract class User
	{
		#region {PROPERTIES}

		/// <summary>
		/// Описание пользователя.
		/// </summary>
		public abstract string Description { get; }

		#endregion


		#region {MEMBERS}

		/// <summary>
		/// Имя пользователя.
		/// </summary>
		string userName;

		/// <summary>
		/// Пароль.
		/// </summary>
		string password;

		#endregion


		#region {CONSTRUCTORS}

		/// <summary>
		/// Проводит инициализацию абстрактного класса User на основен переданных параметров.
		/// </summary>
		/// <param name="userName">Проверяемое имя пользователя.</param>
		/// <param name="password">Проверяемый пароль.</param>
		public User(string userName, string password)
		{
			// сохраняем параметры
			this.userName = userName;
			this.password = password;
		}

		#endregion


		#region {PUBLIC_METHODS}

		/// <summary>
		/// Проводит проверку соответствия переданных имени пользователя и пароля текущему экземпляру пользователя.
		/// </summary>
		/// <param name="userName">Проверяемое имя пользователя.</param>
		/// <param name="password">Проверяемый пароль.</param>
		/// <returns>true - имя пользователя и пароль совпали; false - имя пользователя и/или пароль не совпали.</returns>
		public bool CheckAccess(string userName, string password)
		{
			// возвращаем результат сверки
			return (this.userName.Equals(userName) && this.password.Equals(password));
		}

		/// <summary>
		/// Устанавливает новый пароль.
		/// </summary>
		/// <param name="password">Новый пароль.</param>
		public void SetNewPassword(string password)
		{
			// меням пароль на новый
			this.password = password;
		}

		/// <summary>
		/// Возвращает имя пользователя.
		/// </summary>
		/// <returns>Имя пользователя.</returns>
		public string GetName()
		{
			// возвращаем имя пользователя
			return userName;
		}

		/// <summary>
		/// Возвращает пароль пользователя.
		/// </summary>
		/// <returns>Пароль пользователя.</returns>
		public string GetPassword()
		{
			// возвращаем пароль пользователя
			return password;
		}

		#endregion
	}
}
