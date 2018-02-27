using System;
using System.Collections.Generic;

namespace SimpleLogCS.Utilities {
    
    public class StringSubstitutor {

        private readonly Dictionary<string, string> _dict;

        public StringSubstitutor(Dictionary<string, string> dictionary) {
            _dict = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
        }

        public StringSubstitutor(Dictionary<string, string> dictionary, string prefix, string suffix) {
            _dict = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
            _pfx = prefix ?? throw new ArgumentNullException(nameof(prefix));
            _sfx = suffix ?? throw new ArgumentNullException(nameof(suffix));
        }

        private string _pfx = "%";
        private string _sfx = "%";

        public string Prefix {
            get => _pfx;
            set => _pfx = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Suffix {
            get => _sfx;
            set => _sfx = value ?? throw new ArgumentNullException(nameof(value));
        }
        
        public string Replace(string input) {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (input == "") throw new Exception("Input must not be empty!");
            
            var temp = input;
            foreach (var key in _dict.Keys) {
                temp = temp.Replace(_pfx + key + _sfx, _dict[key]);
            }

            return temp;
        }

        public string Replace(string input, int offset, int length) {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (input == "") throw new Exception("Input must not be empty!");
            if (!(offset >= 0 && offset < input.Length && offset + length < input.Length))
                throw new ArgumentOutOfRangeException(nameof(offset));

            var temp = input.Substring(offset, offset + length);
            foreach (var key in _dict.Keys) {
                temp = temp.Replace(_pfx + key + _sfx, _dict[key]);
            }

            return temp;
        }
    }
}