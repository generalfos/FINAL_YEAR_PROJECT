namespace BeamMeUpATCA
{
    public interface Workable
    {
        // Takes a Unit and handles preforming work on the implementing class.
        void Work(Unit unit);
        
        Unit WorkingUnit { get; set; }

        void Rest(Unit unit);
    }
}