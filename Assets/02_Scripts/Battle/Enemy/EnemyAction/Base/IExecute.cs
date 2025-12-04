public interface IExecute
{
    public bool CanExecute(Target target);
    public void Execute(Target target);
}