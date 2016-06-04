using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TickTackToo
{
    class TreeHandler
    {
        Tree root;
        public TreeHandler()
        {
            Tile[,] board = new Tile[3, 3];
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                    board[x, y] = new Tile();
            }
            root = createTree(board, Tree.Owner.cross);
            root.getValue();
        }

        public Tree getRoot()
        {
            return root;
        }

        private Tree createTree(Tile[,] board, Tree.Owner owner)
        {
            int value = ifBoardFinnishValue(board);
            if (value >= -1 && value <= 1)
                return new Leaf(value, owner, board);

            List<Tree> branches = new List<Tree>();
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if(board[x,y].getState() == Tile.TileState.empty)
                    {
                        Tile[,] newBoard = copyBoard(board);
                        if (owner == Tree.Owner.cross)
                        {
                            newBoard[x, y].setState(Tile.TileState.cross);
                            branches.Add(createTree(newBoard, Tree.Owner.circle));
                        }
                        else {
                            newBoard[x, y].setState(Tile.TileState.circle);
                            branches.Add(createTree(newBoard, Tree.Owner.cross));
                        }
                        
                    }
                }
            }
            if (branches.Count == 0)
                return new Leaf(0, owner, board);

            return new Branch(branches.ToArray(), owner, board);
        }

        private int ifBoardFinnishValue(Tile[,] board)
        {
            for (int y = 0; y < 3; y++)
            {
                Tile.TileState state = board[1, y].getState();
                if (state != Tile.TileState.empty && board[0,y].getState() == state && board[2, y].getState() == state)
                {
                    if (state == Tile.TileState.cross)
                        return -1;
                    else
                        return 1;
                }
            }

            for (int x = 0; x < 3; x++)
            {
                Tile.TileState state = board[x, 1].getState();
                if (state != Tile.TileState.empty && board[x, 0].getState() == state && board[x, 2].getState() == state)
                {
                    if (state == Tile.TileState.cross)
                        return -1;
                    else
                        return 1;
                }
            }

            Tile.TileState stateC = board[1, 1].getState();
            if (stateC != Tile.TileState.empty)
            {
                if((board[0, 0].getState() == stateC && board[2, 2].getState() == stateC) || (board[2, 0].getState() == stateC && board[2, 0].getState() == stateC))
                {
                    if (stateC == Tile.TileState.cross)
                        return -1;
                    else
                        return 1;
                }
            }
            return -2;
        }

        public Tree moveTile(Tree currentBranch, Tile[,] target)
        {
            foreach(Tree branch in currentBranch.getBranches())
            {
                if (checkBranch(branch, target))
                    return branch;
            }
            Console.Out.WriteLine("Could not find a branch to path to");
            return null;
        }

        private bool checkBranch(Tree branch, Tile[,] target)
        {
            for (int x = 0; x < target.GetLength(0); x++)
            {
                for (int y = 0; y < target.GetLength(1); y++)
                {
                    if (branch.board[x, y].getState() != target[x, y].getState())
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private Tree bestMove(Tree currentBranch, int prio)
        {
            Random rnd = new Random();
            List<Tree> ones = new List<Tree>();
            List<Tree> zeros = new List<Tree>();
            List<Tree> minusOnes = new List<Tree>();
            foreach(Tree branch in currentBranch.getBranches())
            {
                if (branch.getValue() == 1)
                    ones.Add(branch);
                else if (branch.getValue() == 0)
                    zeros.Add(branch);
                else
                    minusOnes.Add(branch);
            }
            List<Tree> list;
            if (prio == -1)
            {
                if (minusOnes.Count > 0)
                    list = minusOnes;
                else if (zeros.Count > 0)
                    list = zeros;
                else
                    list = ones;
            }
            else
                if (ones.Count > 0)
                list = ones;
            else if (zeros.Count > 0)
                list = zeros;
            else
                list = minusOnes;
            return list[rnd.Next(list.Count)];
        }

        public Tree bestMoveCross(Tree currentBranch)
        {
            return bestMove(currentBranch, -1);
        }

        public Tree bestMoveCircle(Tree currentBranch)
        {
            return bestMove(currentBranch, 1);
        }


        public static Tile[,] copyBoard(Tile[,] board)
        {
            Tile[,] newBoard = new Tile[board.GetLength(0), board.GetLength(1)];
            for (int x = 0; x < newBoard.GetLength(0); x++)
            {
                for (int y = 0; y < newBoard.GetLength(1); y++)
                {
                    newBoard[x, y] = new Tile();
                    newBoard[x, y].setState(board[x, y].getState());
                }
            }
            return newBoard;
        }
    }
}
