using System.Collections.Generic;
using NUnit.Framework;
using SimpleLogCS.Utilities;

namespace SimpleLogCSTest {
    [TestFixture]
    public class SubstitutorTests {

        [Test]
        public void TestSub() {
            var input = "This %type% is %adj%!";
            var expected = "This String is good!";

            var dict = new Dictionary<string, string>();
            dict.Add("type", "String");
            dict.Add("adj", "good");
            var sub = new StringSubstitutor(dict);

            var actual = sub.Replace(input);
            
            Assert.AreEqual(expected, actual);
        }
    }
}