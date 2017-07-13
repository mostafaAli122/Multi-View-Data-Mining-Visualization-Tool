using System;
using System.IO;

namespace Multi_View_Data_Visualization_Tool
{

    class Classifier
    {
        Arff arff { get; set; }
        DecisionBuilder builder { get; set; }
        bool prune { get; set; }

        public Classifier(Arff arff, bool prune = false)
        {
            this.arff = arff;
            this.builder = new DecisionBuilder(this.arff);
            this.prune = prune;
        }

        public String DrawTree()
        {
            var tree = builder.BuildTree(arff.Data, arff.Attributes, this.prune);
            String s = tree.Display();
            return s;
        }

    }
}
