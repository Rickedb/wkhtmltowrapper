using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WkHtmlToPdf.Wrapper.Tests.Fixtures;
using WkHtmlTo.Wrapper.Logging;
using System.IO;

namespace WkHtmlToPdf.Wrapper.Tests
{
    public class ConversionOutputEventTests
    {
        private readonly WkHtmlToPdfTestFixture _fixture;

        public ConversionOutputEventTests()
        {
            _fixture = new WkHtmlToPdfTestFixture();
        }

        [Fact(DisplayName = "Ok output parse")]
        public void OkOutputParse()
        {
            var logs = File.ReadAllLines("./Logs/OkLogs.txt");
            var events = new List<ConversionOutputEvent>();
            foreach (var l in logs) 
            {
                var ev = ConversionOutputEvent.Parse(l);
                events.Add(ev);
            }

            Assert.DoesNotContain(events, x => x.EventType == ConversionOutputEventType.Error);
        }


        [Fact(DisplayName = "Error output parse")]
        public void ErrorOutputParse()
        {
            var logs = File.ReadAllLines("./Logs/ErrorLogs.txt");
            var events = new List<ConversionOutputEvent>();
            foreach (var l in logs)
            {
                var ev = ConversionOutputEvent.Parse(l);
                events.Add(ev);
            }

            Assert.Contains(events, x => x.EventType == ConversionOutputEventType.Error);
        }
    }
}
