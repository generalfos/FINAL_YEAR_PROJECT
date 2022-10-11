using System.Collections.Generic;

namespace BeamMeUpATCA
{
    public interface Enterable
    {
        void Enter(Unit unit);
        
        bool IsInside(Unit unit);
        
        void Leave(Unit unit);
    }
}