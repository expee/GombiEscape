using UnityEngine;

namespace GvG
{
    public interface ICaptureable
    {
        Vector2 position { get; }
        Rigidbody2D captureeRigidbody2d { get; }
    }
}
