namespace BeamMeUpATCA
{
    public interface Stowable
    {
        // Takes a Unit and handles it stowing the implementing class.
        void ToggleStow();
        
        bool IsStowed { get; }
    }
}