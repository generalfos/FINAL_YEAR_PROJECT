using UnityEngine;

namespace BeamMeUpATCA.Extensions 
{
    public static class ComponentExtensions 
    {
        #line hidden // #line preprocessor directive prevents Debug.Log from linking to this method.
        public static T SafeComponent<T>(this MonoBehaviour mono, ref T comp) where T : Component
        {
            // If component is considered safe (not null/undefined) return it.
            if (comp) return comp;
            Debug.LogError(mono.name + " requires a valid '" + typeof(T) + "' inspector field."
                           + "Failing dependencies safely. This may lead to instability.", mono.gameObject);
            // Return an attached PlayerInput, otherwise attach one and return it.
            // comp is a references and such also modifies the value.
            comp = mono.TryGetComponent<T>(out T foundComponent) ? foundComponent : mono.gameObject.AddComponent<T>();
            return comp;
        }
        #line default   
    }
}