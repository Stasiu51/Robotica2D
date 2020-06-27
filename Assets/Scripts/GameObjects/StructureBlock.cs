namespace GameObjects
{
    public class StructureBlock : SingleBlock
    {
        private readonly Routing _routing = Routing.newBasicRouting();
        
        public override int getMass() => SINGLEBLOCKMASS;

        public override void initialSetup()
        {
        }

        public override Routing getRouting()
        {
            return _routing;
        }

        public override Source getSource()
        {
            throw new System.Exception("Structure has no source");
        }

        public override bool hasSource()
        {
            return false;
        }

        
    }
}