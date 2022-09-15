namespace BeamMeUpATCA
{
    public abstract class Command
    {
        public bool SkipQueue { private set; get; }
        public bool ResetQueue { private set; get; }
        public string Name { protected set; get; }

        protected Command(bool skipQueue = false, bool resetQueue = false) 
        {
            SkipQueue = skipQueue;
            ResetQueue = resetQueue;
            Name = "Command";
        }

        public abstract void Execute(Unit unit);
    }
}
