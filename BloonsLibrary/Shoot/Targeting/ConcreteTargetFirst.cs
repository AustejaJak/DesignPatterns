namespace BloonsProject
{
    class ConcreteTargetFirst : TargetCreator
    {
        public override ITarget CreateTarget()
        {
            return new TargetFirst();
        }
    }
}