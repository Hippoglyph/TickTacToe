using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TickTackToo
{
    abstract class Tree
    {
        protected int value = -2;
        public enum Owner { cross, circle};
        public Owner owner;
        public Tile[,] board;

        abstract public int getValue();
        abstract public Tree[] getBranches();

    }

    class Leaf: Tree
    {
        public Leaf(int value, Owner owner, Tile[,] board)
        {
            this.board = board;
            this.owner = owner;
            this.value = value;
        }

        public override Tree[] getBranches()
        {
            return null;
        }

        public override int getValue()
        {
            return value;
        }
    }

    class Branch: Tree
    {
        private Tree[] branches;

        public override Tree[] getBranches()
        {
            return branches;
        }

        public Branch(Tree[] branches, Owner owner, Tile[,] board)
        {
            this.board = board;
            this.owner = owner;
            this.branches = branches;
        }

        public override int getValue()
        {
            if (value >= -1 && value <= 1)
                return value;

            int minValue = 2;
            int maxValue = -2;
            foreach(Tree branch in branches)
            {
                int value = branch.getValue();
                if (minValue > value)
                    minValue = value;
                if (maxValue < value)
                    maxValue = value;
            }

            if (owner == Owner.cross)
            {
                if (minValue >= -1)
                    value = minValue;
                else
                    Console.Write("Could not calculate value on branch");
            }
            else if (owner == Owner.circle)
            {
                if (maxValue <= 1)
                    value = maxValue;
                else
                    Console.Write("Could not calculate value on branch");
            }
            return value;
        }
    }
}
