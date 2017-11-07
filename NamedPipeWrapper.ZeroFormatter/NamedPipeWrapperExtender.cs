namespace NamedPipeWrapper.ZeroFormatter
{
    public class NamedPipeWrapperZeroFormatter
	{
		public static void Initialize()
		{
			IO.Serialization.ExtensibilityPoint.CreateSerializer = () => new ZeroFormatterSerializer();
		}
	}
}
