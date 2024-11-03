namespace BloonsProject
{
    class ConcreteTargetWeak : TargetCreator
    {
        public override ITarget CreateTarget()
        {
            return new TargetWeak();
        }
    }
}