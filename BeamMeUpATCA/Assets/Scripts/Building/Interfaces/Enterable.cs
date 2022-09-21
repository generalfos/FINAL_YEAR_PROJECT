using System.Collections.Generic;

namespace BeamMeUpATCA
{
    public interface Enterable
    {
        // Takes a Unit and handles it entering the implementing class.
        void Enter(Unit unit);
        bool IsInside(Unit unit);
        void Leave(Unit unit);
    }
    
    
}