namespace BloonLibrary
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}