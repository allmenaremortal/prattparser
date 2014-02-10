using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrattParser.Test
{
    /// <summary>
    /// Implements input and expected output values for a unit test. The expected output value
    /// is generally to be compared to a given actual value computed by the unit test.
    /// </summary>
    /// <typeparam name="TIn">The type of the input parameter.</typeparam>
    /// <typeparam name="TExpOut">The type of the expected output parameter.</typeparam>
    public class InputExpectedOutput<TIn, TExpOut>
    {
        private TIn input;
        private TExpOut expectedOutput;

        public InputExpectedOutput(TIn _input, TExpOut _expectedOutput)
        {
            input = _input;
            expectedOutput = _expectedOutput;
        }

        public TIn Input { get { return input; } set { input = Input; } }
        public TExpOut ExpectedOutput { get { return expectedOutput; } set { expectedOutput = ExpectedOutput; } }
    }
}
