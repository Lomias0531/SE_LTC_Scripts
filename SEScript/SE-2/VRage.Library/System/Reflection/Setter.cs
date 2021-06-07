namespace System.Reflection
{
	public delegate void Setter<T, TMember>(ref T obj, in TMember value);
}
