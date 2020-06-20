using System;
using System.Collections.Generic;

namespace SimpleLog {

    /// <summary>
	/// The class which does everything needed.
	/// </summary>
    public class SimpleLog {
        // Static Global Level
        /// <summary>
        /// Statically changeable level which is displayed in the Console.
        /// This can be overridden for every instance of SimpleLog.
        /// </summary>
        public static Level GlobalLevel = Level.Info;

        // Event Related Stuff
        /// <summary>
        /// Handles 'Log' level events (all of them).
        /// </summary>
        /// <param name="myObject">The sender of the event</param>
        /// <param name="args">Event parameters</param>
        public delegate void LogHandler(object myObject, LogArgs args);

        /// <summary>
        /// Handles 'Error' level events.
        /// </summary>
        /// <param name="myObject"></param>
        /// <param name="args"></param>
        public delegate void ErrorHandler(object myObject, ErrorArgs args);

        /// <summary>
        /// Whenever a 'Log' event gets triggered.
        /// </summary>
        public event LogHandler OnLog;

        /// <summary>
        /// Whenever an 'Error' event gets triggered.
        /// </summary>
        public event ErrorHandler OnError;

        private static readonly Dictionary<string, SimpleLog> Logs = new Dictionary<string, SimpleLog>();

        private SimpleLog(string name) {
            Name = name;
        }

        /// <summary>
		/// Name of the SimpleLog instance. Used in the format.
		/// </summary>
		public string Name { get; }

        /// <summary>
        /// Level of this SimpleLog instance. Overrides the Global level when it comes to Console logging.
        /// </summary>
        public Level Level { get; set; }

        /// <summary>
		/// Gets a SimpleLog instance with the given name.
		/// </summary>
		/// <param name="name">Name of the SimpleLog instance</param>
		/// <returns>A SimpleLog instance with the given name.</returns>
		/// <exception cref="System.ArgumentNullException">Thrown if <paramref name="name"/> is null.</exception>
		/// <exception cref="System.Exception">Thrown if <paramref name="name"/> is empty.</exception>
		public static SimpleLog GetLog(string name) {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (name == "") throw new Exception("Name cannot be empty!");

            if (!Logs.ContainsKey(name.ToLower()))
                Logs.Add(name.ToLower(), new SimpleLog(name));

            return Logs[name.ToLower()];
        }

        /// <summary>
        /// Logs a message into console, with the given level.
        /// </summary>
        /// <param name="msg">The message to log.</param>
        /// <param name="level">Level of the message to log.</param>
        public void Log(object msg, Level level) {
            OnLog?.Invoke(this, new LogArgs(this, level, msg));
            if (level.GetPriority < (Level?.GetPriority ?? GlobalLevel.GetPriority))
                return;

            level.SetColors();
            Console.WriteLine(FormatMessage(msg, level));
            Console.ResetColor();
        }

        /// <summary>
        /// Logs a message with the level of Trace
        /// </summary>
        /// <param name="msg">The message to log</param>
        public void Trace(object msg) => Log(msg, Level.Trace);

        /// <summary>
        /// Logs a message with the level of Debug
        /// </summary>
        /// <param name="msg">The message to log</param>
        public void Debug(object msg) => Log(msg, Level.Debug);

        /// <summary>
        /// Logs a message with the level of Info
        /// </summary>
        /// <param name="msg">The message to log</param>
        public void Info(object msg) => Log(msg, Level.Info);

        /// <summary>
        /// Logs a message with the level of Warning
        /// </summary>
        /// <param name="msg">The message to log</param>
        public void Warn(object msg) => Log(msg, Level.Warning);

        /// <summary>
        /// Logs a message with the level of Fatal
        /// </summary>
        /// <param name="msg">The message to log</param>
        public void Fatal(object msg) => Log(msg, Level.Fatal);

        private string FormatMessage(object msg, Level level) {
            var time = DateTime.Now.ToString("HH:mm:ss");

            if (msg is Exception e) {
                OnError?.Invoke(this, new ErrorArgs(this, e));
                return $"{time} [{level}]: {e.GetType()} {e.Message}: {e.Source} {e.TargetSite}";
            }
            return $"{time} [{level}]: {msg}";
        }
    }

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
        public static readonly Level Trace = new Level("Trace", 1, false, ConsoleColor.DarkGray);

        /// <summary>
        /// Debug level, and below
        /// </summary>
        public static readonly Level Debug = new Level("Debug", 2, false, ConsoleColor.DarkGray);

        /// <summary>
        /// Info level, and below
        /// </summary>
        public static readonly Level Info = new Level("Info", 3, false, ConsoleColor.Gray);

        /// <summary>
        /// Warning level, and below
        /// </summary>
        public static readonly Level Warning = new Level("Warning", 4, true, ConsoleColor.Yellow);

        /// <summary>
        /// Fatal level, and below
        /// </summary>
        public static readonly Level Fatal = new Level("Fatal", 5, true, ConsoleColor.Black, ConsoleColor.Red);

        /// <summary>
        /// Logging is off.
        /// </summary>
        public static readonly Level Off = new Level("NO-LOGGING", 6, true);

        private Level(string tag, int priority, bool isError = false, ConsoleColor foreground = ConsoleColor.Gray, ConsoleColor background = ConsoleColor.Black) {
            GetTag = tag;
            GetPriority = priority;
            IsError = isError;
            Foreground = foreground;
            Background = background;
        }

        /// <summary>
        /// Sets the Console colors to the ones of the level.
        /// </summary>
        public void SetColors() {
            Console.ForegroundColor = Foreground;
            Console.BackgroundColor = Background;
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

        /// <summary>
        /// Gets the Level's foreground color
        /// </summary>
        public ConsoleColor Foreground { get; }

        /// <summary>
        /// Gets the Level's background color
        /// </summary>
        public ConsoleColor Background { get; }
    }

    /// <summary>
	/// The arguments for a Log event.
	/// </summary>
	public class LogArgs : EventArgs {

        /// <summary>
        /// The constructor for the arguments of a Log event.
        /// </summary>
        /// <param name="log">The SimpleLog instance which sent the event.</param>
        /// <param name="level">The Level of the event.</param>
        /// <param name="message">The message of the event.</param>
        public LogArgs(SimpleLog log, Level level, object message) {
            Log = log;
            Level = level;
            Message = message;
        }

        /// <summary>
        /// The SimpleLog instance which sent the event.
        /// </summary>
        public SimpleLog Log { get; }

        /// <summary>
        /// The Level of the event.
        /// </summary>
        public Level Level { get; }

        /// <summary>
        /// The Message of the event.
        /// </summary>
        public object Message { get; }
    }

    /// <summary>
    /// The arguments for an Error event.
    /// </summary>
    public class ErrorArgs : EventArgs {

        /// <summary>
        /// The constructor for the arguments of an Error event.
        /// </summary>
        /// <param name="log">The SimpleLog instance which sent the event.</param>
        /// <param name="exception">The Exception of the event.</param>
        public ErrorArgs(SimpleLog log, Exception exception) {
            Log = log;
            Exception = exception;
        }

        /// <summary>
        /// The SimpleLog instance which sent the event.
        /// </summary>
        public SimpleLog Log { get; }

        /// <summary>
        /// The Exception of the event.
        /// </summary>
        public Exception Exception { get; }
    }
}
