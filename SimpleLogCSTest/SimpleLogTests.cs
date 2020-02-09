using System;
using NUnit.Framework;
using SimpleLogCS;

namespace SimpleLogCSTest {

	[TestFixture]
	public class SimpleLogTests {

		[Test]
		public void TestLogCreation() {
			var log = SimpleLog.GetLog("Test");

			log.Info("This was a triumph");
			log.Warn("I'm making a note here");
			log.Fatal("Huge Success");
		}
	}
}