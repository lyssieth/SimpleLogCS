using System;
using System.Collections.Generic;
using SimpleLogCS.Utilities;

namespace SimpleLogCS {
    
    public class SimpleLog {

        // Static Level
        public static Level LEVEL = Level.Info;

        // Event Definition
        public delegate void LogHandler(object myObject, LogArgs args);
        public delegate void ErrorHandler(object myObject, ErrorArgs args);

        public event LogHandler OnLog;
        public event ErrorHandler OnError;

        // Actual Logging Stuff
        private static readonly string FORMAT = "[%time%] [%level%] [%name%]: %text%";
        private static readonly Dictionary<string, SimpleLog> LOGS = new Dictionary<string, SimpleLog>();

        private readonly string _name;
        private Level _level;
        private SimpleLog(string name) {
            _name = name;
        }

        public string Name => _name;

        public Level Level {
            get => _level;
            set => _level = value;
        }

        public static SimpleLog GetLog(string name) {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (name == "") throw new Exception("Name cannot be empty!");
            
            if (!LOGS.ContainsKey(name.ToLower()))
                LOGS.Add(name.ToLower(), new SimpleLog(name));

            return LOGS[name.ToLower()];
        }

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
            var o = new StringSubstitutor(dict).Replace(FORMAT);
            Print(o, level);
        }

        public void Log(Exception ex) {
            OnError?.Invoke(this, new ErrorArgs(this, ex));
            
            Log(Level.Fatal, "Encountered an exception:");
            Log(Level.Fatal, ex.StackTrace);
        }

        public void Trace(object msg) {
            Log(Level.Trace, msg);
        }

        public void Debug(object msg) {
            Log(Level.Debug, msg);
        }

        public void Info(object msg) {
            Log(Level.Info, msg);
        }

        public void Warn(object msg) {
            Log(Level.Warning, msg);
        }

        public void Fatal(object msg) {
            Log(Level.Fatal, msg);
        }

        private void Print(string msg, Level level) {
            if (level.GetPriority < (_level?.GetPriority ?? LEVEL.GetPriority))
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

    public class LogArgs : EventArgs {

        private readonly SimpleLog _log;
        private readonly Level _level;
        private readonly object _message;

        public LogArgs(SimpleLog log, Level level, object message) {
            _log = log;
            _level = level;
            _message = message;
        }

        public SimpleLog Log => _log;
        public Level Level => _level;
        public object Message => _message;
    }

    public class ErrorArgs : EventArgs {

        private readonly SimpleLog _log;
        private readonly Exception _exception;

        public ErrorArgs(SimpleLog log, Exception exception) {
            _log = log;
            _exception = exception;
        }

        public SimpleLog Log => _log;
        public Exception Exception => _exception;
    }
}