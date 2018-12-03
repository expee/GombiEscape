using UnityEngine;

namespace GvG
{
    public interface IMoveable
    {
        void MoveTo(Vector2 target);
        void SpeedUp(Vector2 direction);
        void ChangeTargetPosition(Vector2 newTarget);
        Vector2 actualMoveDirection { get; }
        Vector2 currentTargetPosition { get; }
        bool stopped { get; }
        Vector2 currentPosition { get; }
    }
}
