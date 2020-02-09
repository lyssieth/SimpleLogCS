namespace SimpleLogCS {

	/// <summary>
	/// All of the logging levels used in <see cref="SimpleLog"/>.
	/// </summary>
	public class Level {

		/// <summary>
		/// All Logging messages, no matter what.
		/// </summary>
		public static readonly Level All = new Level("All", 0);

		/// <summary>
		/// Trace level, and below
		/// </summary>
		public static readonly Level Trace = new Level("Trace", 1);

		/// <summary>
		/// Debug level, and below
		/// </summary>
		public static readonly Level Debug = new Level("Debug", 2);

		/// <summary>
		/// Info level, and below
		/// </summary>
		public static readonly Level Info = new Level("Info", 3);

		/// <summary>
		/// Warning level, and below
		/// </summary>
		public static readonly Level Warning = new Level("Warning", 4, true);

		/// <summary>
		/// Fatal level, and below
		/// </summary>
		public static readonly Level Fatal = new Level("Fatal", 5, true);

		/// <summary>
		/// Logging is off.
		/// </summary>
		public static readonly Level Off = new Level("NO-LOGGING", 6, true);

		private Level(string tag, int priority, bool isError = false) {
			GetTag = tag;
			GetPriority = priority;
			IsError = isError;
		}

		/// <summary>
		/// Gets the Tag of the Level.
		/// </summary>
		public string GetTag { get; }

		/// <summary>
		/// Gets the Priority of the level.
		/// </summary>
		public int GetPriority { get; }

		/// <summary>
		/// Gets whether the level is an error.
		/// </summary>
		public bool IsError { get; }
	}
}