using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flat.Decoders;
using Flat.Encoders;

namespace Flat
{
    class Decode
    {
        public static TreeWithDependencies<T> HierarchicalGraph<T>(string text, 
            HierarchicalGraphDecoder.CreateNodeFromName<T> createNodeWithName,
            HierarchicalGraphDecoder.AddChildrenToNode<T> addChildrenToNode) 
                =>  HierarchicalGraphDecoder.Decode(text, createNodeWithName, addChildrenToNode);
    }
}
