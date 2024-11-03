namespace BloonsProject
{
    class ConcreteTargetStrong : TargetCreator
    {
        public override ITarget CreateTarget()
        {
            return new TargetStrong();
        }
    }
}