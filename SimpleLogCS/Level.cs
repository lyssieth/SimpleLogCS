namespace SimpleLogCS {
    public class Level {
        
        public static readonly Level All = new Level("All", 0);
        public static readonly Level Trace = new Level("Trace", 1);
        public static readonly Level Debug = new Level("Debug", 2);
        public static readonly Level Info = new Level("Info", 3);
        public static readonly Level Warning = new Level("Warning", 4, true);
        public static readonly Level Fatal = new Level("Fatal", 5, true);
        public static readonly Level Off = new Level("NO-LOGGING", 6, true);

        private readonly string _tag;
        private readonly int _priority;
        private readonly bool _isError;

        private Level(string tag, int priority, bool isError = false) {
            _tag = tag;
            _priority = priority;
            _isError = isError;
        }

        public string GetTag => _tag;

        public int GetPriority => _priority;

        public bool IsError => _isError;
    }
}