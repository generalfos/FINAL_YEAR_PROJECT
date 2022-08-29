namespace BeamMeUpATCA
{
    public abstract class Command
    {
        public bool IsFinished { protected set; get; }

        public abstract void Execute();
    }
}
