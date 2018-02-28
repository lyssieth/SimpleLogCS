using System;
using System.Collections.Generic;
using SimpleLogCS.Utilities;

namespace SimpleLogCS {
    
    /// <summary>
    /// The class which does everything needed.
    /// </summary>
    public class SimpleLog {

        // Static Level
        /// <summary>
        /// Statically changeable level which is displayed in the Console.
        /// This can be overridden for every instance of SimpleLog.
        /// </summary>
        public static Level GlobalLevel = Level.Info;

        // Event Definition
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

        // Actual Logging Stuff
        private const string Format = "[%time%] [%level%] [%name%]: %text%";
        private static readonly Dictionary<string, SimpleLog> Logs = new Dictionary<string, SimpleLog>();

        private readonly string _name;
        private Level _level;
        private SimpleLog(string name) {
            _name = name;
        }

        /// <summary>
        /// Name of the SimpleLog instance. Used in the format.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Level of this SimpleLog instance. Overrides the Global level when it comes to Console logging.
        /// </summary>
        public Level Level {
            get => _level;
            set => _level = value;
        }

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
        /// <param name="level">Level of the message to log.</param>
        /// <param name="msg">The message to log.</param>
        public void Log(Level level, object msg) {
            OnLog?.Invoke(this, new LogArgs(this, level, msg));

            var dict = new Dictionary<string, string> {
                {
                    "time", DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy")
                }, {
                    "level", level.GetTag
                }, {
                    "name", _name
                }, {
                    "text", msg.ToString()
                }
            };
            var o = new StringSubstitutor(dict).Replace(Format);
            Print(o, level);
        }

        /// <summary>
        /// Logs any Exception with the level of Fatal
        /// </summary>
        /// <param name="ex">The Exception to log.</param>
        public void Log(Exception ex) {
            OnError?.Invoke(this, new ErrorArgs(this, ex));
            
            Log(Level.Fatal, "Encountered an exception:");
            Log(Level.Fatal, ex.StackTrace);
        }

        /// <summary>
        /// Logs a message with the level of Trace
        /// </summary>
        /// <param name="msg">The message to log</param>
        public void Trace(object msg) {
            Log(Level.Trace, msg);
        }

        /// <summary>
        /// Logs a message with the level of Debug
        /// </summary>
        /// <param name="msg">The message to log</param>
        public void Debug(object msg) {
            Log(Level.Debug, msg);
        }

        /// <summary>
        /// Logs a message with the level of Info
        /// </summary>
        /// <param name="msg">The message to log</param>
        public void Info(object msg) {
            Log(Level.Info, msg);
        }

        /// <summary>
        /// Logs a message with the level of Warning
        /// </summary>
        /// <param name="msg">The message to log</param>
        public void Warn(object msg) {
            Log(Level.Warning, msg);
        }

        /// <summary>
        /// Logs a message with the level of Fatal
        /// </summary>
        /// <param name="msg">The message to log</param>
        public void Fatal(object msg) {
            Log(Level.Fatal, msg);
        }

        private void Print(string msg, Level level) {
            if (level.GetPriority < (_level?.GetPriority ?? GlobalLevel.GetPriority))
                return;
            
            if (level.IsError) {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(msg);
                Console.ForegroundColor = ConsoleColor.White;
            } else {
                Console.WriteLine(msg);
            }
        }
    }

    /// <summary>
    /// The arguments for a Log event.
    /// </summary>
    public class LogArgs : EventArgs {

        private readonly SimpleLog _log;
        private readonly Level _level;
        private readonly object _message;

        /// <summary>
        /// The constructor for the arguments of a Log event.
        /// </summary>
        /// <param name="log">The SimpleLog instance which sent the event.</param>
        /// <param name="level">The Level of the event.</param>
        /// <param name="message">The message of the event.</param>
        public LogArgs(SimpleLog log, Level level, object message) {
            _log = log;
            _level = level;
            _message = message;
        }

        /// <summary>
        /// The SimpleLog instance which sent the event.
        /// </summary>
        public SimpleLog Log => _log;
        /// <summary>
        /// The Level of the event.
        /// </summary>
        public Level Level => _level;
        /// <summary>
        /// The Message of the event.
        /// </summary>
        public object Message => _message;
    }

    /// <summary>
    /// The arguments for an Error event.
    /// </summary>
    public class ErrorArgs : EventArgs {

        private readonly SimpleLog _log;
        private readonly Exception _exception;

        /// <summary>
        /// The constructor for the arguments of an Error event.
        /// </summary>
        /// <param name="log">The SimpleLog instance which sent the event.</param>
        /// <param name="exception">The Exception of the event.</param>
        public ErrorArgs(SimpleLog log, Exception exception) {
            _log = log;
            _exception = exception;
        }

        /// <summary>
        /// The SimpleLog instance which sent the event.
        /// </summary>
        public SimpleLog Log => _log;
        /// <summary>
        /// The Exception of the event.
        /// </summary>
        public Exception Exception => _exception;
    }
}