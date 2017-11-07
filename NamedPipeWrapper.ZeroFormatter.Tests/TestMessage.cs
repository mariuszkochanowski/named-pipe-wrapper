using System;
using ZeroFormatter;

namespace NamedPipeWrapper.ZeroFormatter.Tests
{
	// properties must be virtual

	[ZeroFormattable]
	public class TestMessage
	{
		[Index(0)]
		public virtual decimal DecimalValue { get; set; }
		[Index(1)]
		public virtual string StringValue { get; set; }
		[Index(2)]
		public virtual int IntValue { get; set; }

		public override bool Equals(object obj)
		{
			var other = (TestMessage) obj;

			return DecimalValue == other.DecimalValue
			       && string.Equals(StringValue, other.StringValue, StringComparison.Ordinal)
			       && IntValue == other.IntValue;
		}
	}
}