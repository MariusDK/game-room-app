using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameRoomApp.Util.CART
{
    public class Node
    {
        public string value { set; get; }
        public Node right { set; get; }
        public Node left { set; get; }

        public Node()
        { }

        public Node(string value, Node right, Node left)
        {
            this.value = value;
            this.right = right;
            this.left = left;
        }
    }
}
