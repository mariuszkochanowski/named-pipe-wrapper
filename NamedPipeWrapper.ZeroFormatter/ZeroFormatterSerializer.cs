using NamedPipeWrapper.IO.Serialization;

namespace NamedPipeWrapper.ZeroFormatter
{
	internal class ZeroFormatterSerializer : ISerializer
	{
		public T Deserialize<T>(byte[] serializedData) where T : class
		{
			var data = serializedData;
			return global::ZeroFormatter.ZeroFormatterSerializer.Deserialize<T>(data);

			//using (var memoryStream = new MemoryStream(data))
			//{
			//	return ZeroFormatter.ZeroFormatterSerializer.Deserialize<T>(memoryStream);
			//}
		}

		public byte[] Serialize<T>(T value) where T : class
		{
			return global::ZeroFormatter.ZeroFormatterSerializer.Serialize(value);
			//using (var memoryStream = new MemoryStream())
			//{

			//	ZeroFormatter.ZeroFormatterSerializer.Serialize(memoryStream, value);
			//	return memoryStream.ToArray();
			//}
		}
	}
}