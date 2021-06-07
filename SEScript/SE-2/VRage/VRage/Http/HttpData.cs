namespace VRage.Http
{
	public struct HttpData
	{
		public string Name;

		public object Value;

		public HttpDataType Type;

		public HttpData(string name, object value, HttpDataType type)
		{
			Name = name;
			Value = value;
			Type = type;
		}
	}
}
