using System;
using System.Collections.Generic;

namespace SimpleLogCS.Utilities {
    
    /// <summary>
    /// Substitutes things in Strings... kinda hard to explain.
    /// 
    /// <remarks>To be honest, I should've just made this into a separate thing... but screw it.</remarks>
    /// </summary>
    public class StringSubstitutor {

        private readonly Dictionary<string, string> _dict;

        /// <summary>
        /// Creates a StringSubstitutor instance with the given Dictionary.
        /// </summary>
        /// <param name="dictionary">Dictionary used to substitute things within a String.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="dictionary"/> is null.</exception>
        public StringSubstitutor(Dictionary<string, string> dictionary) {
            _dict = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
        }

        /// <summary>
        /// Creates a StringSubstitutor instance with the given Dictionary, prefix and suffix.
        /// </summary>
        /// <param name="dictionary">Dictionary used to substitute things within a String.</param>
        /// <param name="prefix">What needs to be before the key.</param>
        /// <param name="suffix">What needs to be after the key.</param>
        /// <exception cref="System.ArgumentNullException">When any of the given arguments are null.</exception>
        public StringSubstitutor(Dictionary<string, string> dictionary, string prefix, string suffix) {
            _dict = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
            _pfx = prefix ?? throw new ArgumentNullException(nameof(prefix));
            _sfx = suffix ?? throw new ArgumentNullException(nameof(suffix));
        }

        private string _pfx = "%";
        private string _sfx = "%";

        /// <summary>
        /// The prefix (what needs to be before the key)
        /// </summary>
        /// <exception cref="System.ArgumentNullException">When trying to set it to a null value.</exception>
        public string Prefix {
            get => _pfx;
            set => _pfx = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// The suffix (what needs to be after the key)
        /// </summary>
        /// <exception cref="System.ArgumentNullException">When trying to set it to a null value.</exception>
        public string Suffix {
            get => _sfx;
            set => _sfx = value ?? throw new ArgumentNullException(nameof(value));
        }
        
        /// <summary>
        /// Substitutes the keys for given values.
        /// </summary>
        /// <param name="input">String within which to substitute.</param>
        /// <returns>A substituted String.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="input"/> is null.</exception>
        /// <exception cref="Exception">When <paramref name="input"/> is empty.</exception>
        public string Replace(string input) {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (input == "") throw new Exception("Input must not be empty!");
            
            var temp = input;
            foreach (var key in _dict.Keys) {
                temp = temp.Replace(_pfx + key + _sfx, _dict[key]);
            }

            return temp;
        }

        /// <summary>
        /// Substitutes the keys for given values.
        /// </summary>
        /// <param name="input">String within which to substitute.</param>
        /// <param name="offset">Index where to start substitution.</param>
        /// <param name="length">Length of the substitution area.</param>
        /// <returns>A substituted string. Anything outside the area is not returned.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="input"/> is null.</exception>
        /// <exception cref="Exception">When <paramref name="input"/> is empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Either when <paramref name="offset"/> is outside the bounds of <paramref name="input"/>, or if <paramref name="offset"/> + <paramref name="length"/> goes outside the length of <paramref name="input"/></exception>
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