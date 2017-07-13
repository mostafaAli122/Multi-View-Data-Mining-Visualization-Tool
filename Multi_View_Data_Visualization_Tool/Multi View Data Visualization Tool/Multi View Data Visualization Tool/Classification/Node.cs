using System;
using System.Collections.Generic;
using System.Linq;

namespace Multi_View_Data_Visualization_Tool
{
	public class Node
	{
		Attribute attribute { get; set; }

		Dictionary<string, Node> children { get; set; }

		public Node ()
		{

		}

		public Node (Attribute best)
		{
			this.attribute = best;
			this.children = new Dictionary<string, Node> ();
		}

		public void AddChild (Value value, Node node)
		{
			if (children.ContainsKey (value.AsString ())) {
				this.children [value.AsString ()] = node;
			} else {
				this.children.Add (value.AsString (), node);
			}
		}

		public virtual Value Choose (Data example)
		{
			return this.PickChild (example).Choose (example);
		}

		public Node PickChild (Data example)
		{
			return this.children [example.Values [this.attribute.Name].AsString ()];
		}

		public virtual int Size ()
		{
			return children
				.Select (x => x.Value.Size ())
				.Aggregate (1, (x,y) => x + y);
		}

        public virtual string Display(int depth = 1)
        {
            var s = String.Empty;
            foreach (var kvp in this.children)
            {
                s = String.Format("\t {0}\n \t \n {1}{2} = {3}:{4} \n", s, new String(' ', depth * 3), attribute.Name, kvp.Key, kvp.Value.Display(depth + 1), depth);
            }
            return s;
        }

		public bool IsEndNode ()
		{
			return children.Values.All (x => x.IsLeaf ());
		}

		public virtual bool IsLeaf ()
		{
			return false;
		}
	}
}

