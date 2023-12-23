namespace MykroFramework.Debugging
{
    public struct DebuggerDescription
    {
        public string Name;
        public string Description;

        public DebuggerDescription(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
