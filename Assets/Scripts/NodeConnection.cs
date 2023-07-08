namespace DefaultNamespace
{
    public class NodeConnection
    {
        private Node nodeA;
        private Node nodeB;
        private float weight = 1f; // temporary all connections have 1 weight

        public NodeConnection(Node a, Node b)
        {
            nodeA = a;
            nodeB = b;
        }

        public (Node, Node) Nodes()
        {
            return (nodeA, nodeB);
        }
    }
}