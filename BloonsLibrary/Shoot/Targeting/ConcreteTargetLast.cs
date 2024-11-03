namespace BloonsProject
{
    class ConcreteTargetLast : TargetCreator
    {
        public override ITarget CreateTarget()
        {
            return new TargetLast();
        }
    }
}