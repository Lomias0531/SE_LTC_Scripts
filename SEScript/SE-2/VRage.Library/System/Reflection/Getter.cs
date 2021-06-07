namespace System.Reflection
{
	public delegate void Getter<T, TMember>(ref T obj, out TMember value);
}
