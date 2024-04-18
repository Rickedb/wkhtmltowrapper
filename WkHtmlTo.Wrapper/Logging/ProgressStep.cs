using System;
using System.Linq;

namespace WkHtmlTo.Wrapper.Logging
{
    public struct ProgressStep
    {
        public string Name { get; }
        public int Current { get; }
        public int Total { get; }

        private ProgressStep(string name, int current, int total)
        {
            Name = name;
            Current = current;
            Total = total;
        }

        internal static ProgressStep ParseOverallProgressStep(string message)
        {
            var startAt = message.IndexOf("(");
            var name = message.Substring(0, startAt).Trim();
            var str = message.Substring(startAt + 1, message.Length - startAt - 2);
            var steps = str.Split('/');
            if (steps.Length == 2)
            {
                int.TryParse(steps[0], out var currentStep);
                int.TryParse(steps[1], out var totalSteps);
                return new ProgressStep(name, currentStep, totalSteps);
            }

            return new ProgressStep(name, -1, -1);
        }

        internal static ProgressStep ParseInnerProgressStep(string message)
        {
            var str = message.Split(']').Last();
            var splitted = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var name = splitted.First();
            int.TryParse(splitted.ElementAt(1), out var currentStep);
            int.TryParse(splitted.Last(), out var totalSteps);
            return new ProgressStep(name, currentStep, totalSteps);
        }
    }
}
