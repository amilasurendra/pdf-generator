using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomGenerator
{
    public class DocumentTree
    {

        public DocumentTree()
        {
            Root = new Feature();
            Root.Name = "document"; //TODO: provide external interface
        }

        public Feature Root { get; set; }


        /// <summary>
        /// Returns list of leaf features at any given instance
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Feature> GetLeafNodes() {
            List<Feature> leaves = new List<Feature>();
            CalculateLeafNodes(Root, leaves);
            return leaves;
        }



        /// <summary>
        /// Returns depth of nesting level of the given type, along given node to the document root.
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="feature"></param>
        /// <returns></returns>
        public int GetNestedDepth(Feature currentNode, String feature) {


            //If a parent is repeated and most higher feature(closer to root) is parent return 1 
            //if parent equals current return  real depth 

            Feature tmpNode = currentNode;

            while (tmpNode.Parent != null)
            {
                if (tmpNode.Name.Equals(feature)) break;

                int parentRepeats = GetNestedCount(tmpNode, tmpNode.Name);

                String rootFeature = GetRootFeature(tmpNode, feature, tmpNode.Name);

                if (parentRepeats > 1 && rootFeature.Equals(tmpNode.Name))
                {
                    return 0;
                }

                tmpNode = tmpNode.Parent;

            }
            return GetNestedCount(currentNode, feature);
        }


        private String GetRootFeature(Feature currentNode, String feature1, String feature2) {

            int feature1Depth = GetDepthToFirst(currentNode, feature1);
            int feature2Depth = GetDepthToFirst(currentNode, feature2);

            return feature1Depth < feature2Depth ? feature1 : feature2;        
        }


        /// <summary>
        /// Returns number of repititions for the given feature along the current node to the root
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="feature"></param>
        /// <returns></returns>
        private int GetNestedCount(Feature currentNode, String feature) {

            Feature immidiateParent = currentNode;
            int depthCount = 0;

            while (immidiateParent != null)
            {
                if (immidiateParent.Name.Equals(feature)) depthCount++;
                immidiateParent = immidiateParent.Parent;
            }

            return depthCount;
        }


        /// <summary>
        /// returns the depth to the first occurance of a given feature along root to the current node
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="feature"></param>
        /// <returns></returns>
        private int GetDepthToFirst(Feature currentNode, String feature) {

            Feature immidiateParent = currentNode.Parent;

            List<String> path = new List<String>();
            path.Add(currentNode.Name);

            while (immidiateParent != null)
            {
                path.Add(immidiateParent.Name);
                immidiateParent = immidiateParent.Parent;
            }

            return path.Count - path.LastIndexOf(feature);
        }


        public String GetNodePath(Feature node) {

            List<String> featureName = new List<String>();
            Feature parent = node;

            while ( parent != null)
            {
                featureName.Add(parent.Name);
                parent = parent.Parent;
            }

            StringBuilder path = new StringBuilder();

            for (int i = featureName.Count()-1 ; i >= 0; i--)
            {
                path.Append("/");
                path.Append(featureName[i]);
            }

            return path.ToString();
        }


        private void CalculateLeafNodes(Feature node, List<Feature> leaves) {

            if (IsLeaf(node))
            {
                leaves.Add(node);
                return;
            }
            else {
                foreach (var childNode in node.GetChildFeatures())
                {
                    CalculateLeafNodes(childNode, leaves);
                }
            }
        }


        private bool IsLeaf(Feature node) {
            if (node.GetSubFeatureCount() == 0) {
                return true;
            }
            return false;
        }



    }
}
