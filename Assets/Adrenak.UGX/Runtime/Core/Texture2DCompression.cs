using UnityEngine;

namespace Adrenak.UGX {
    /// <summary>
    /// Used to represent compression quality levels. This enum can be used
    /// with the <see cref="Texture2D.Compress(bool)"/> method.
    /// Do note that runtime texture compression doesn't work on many mobile
    /// devices.
    /// </summary>
    public enum Texture2DCompression {
        /// <summary>
        /// No compression. The texture will take up highest amounts of memory:
        /// </summary>
        None,

        /// <summary>
        /// Compresses the texture while keeping higher quality. The resulting
        /// texture takes up more memory.
        /// </summary>
        HighQuality,

        /// <summary>
        /// Compresses the texture while compromising the quality to a greater
        /// extent. Ths result is more memory efficient.
        /// </summary>
        LowQuality
    }
}