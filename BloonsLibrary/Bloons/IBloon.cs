using BloonsProject;

namespace BloonLibrary.Bloons
{
    public interface IBloon
    {
        void MoveBloonInDirection(Direction direction);
        void TakeDamage(int damage);
    }
}