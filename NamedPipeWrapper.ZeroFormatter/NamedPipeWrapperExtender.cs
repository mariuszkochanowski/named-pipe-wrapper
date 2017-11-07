namespace NamedPipeWrapper.ZeroFormatter
{
    public static class NamedPipeWrapperZeroFormatter
	{
		public static void Initialize()
		{
			IO.Serialization.ExtensibilityPoint.CreateSerializer = () => new ZeroFormatterSerializer();
		}
	}
}
