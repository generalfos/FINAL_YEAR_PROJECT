using UnityEngine;

namespace BeamMeUpATCA
{
    // Value type for storing layer references. LayerMask.GetMask is expensive and as such bitshift is done here once.
    // boxing from Mask to int value. Implicit operator allows direct Mask bitwise operations. i.e. MaskOne =| MaskTwo
    public struct Mask
    {
        public static readonly Mask Default = new Mask("Default");
        public static readonly Mask Unit = new Mask("Unit");
        public static readonly Mask Building = new Mask("Building");

        private int _mask;
        private readonly string _layerName;

        private Mask(int layerMask, string layerName) { _mask = layerMask; _layerName = layerName; }
        private Mask(string layerName) : this(0, layerName) {}

        public static implicit operator Mask(int i) { return new Mask(i, ""); }
        public static implicit operator int(Mask m) => m._mask != 0 ? m._mask : m._mask = 1 << LayerMask.NameToLayer(m._layerName);
        public override string ToString() => _mask.ToString();
    }
}