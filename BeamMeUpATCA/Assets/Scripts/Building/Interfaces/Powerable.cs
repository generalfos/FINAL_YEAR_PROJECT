namespace BeamMeUpATCA
{
    public interface Powerable
    {
        // Toggles buildings power source on
        void TogglePower();
        
        bool IsGeneratorOn { get; }
    }
}