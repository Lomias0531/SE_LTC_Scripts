namespace VRage.Game.VisualScripting
{
	[VisualScriptingEvent(new bool[]
	{
		true,
		true,
		true
	})]
	public delegate void TriggerEventComplex(string triggerName, long entityId, string entityName);
}
