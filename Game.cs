using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace TickTackToo
{
    class Game
    {
        private GCanvas GCanvas;
        public GameState gamestate;
        public Hooman hooman;
        private Size screenSize;
        private int tileSize;
        private TreeHandler treeHandler;
        private Tree root;
        private Tree currentBranch;

        public enum GameState { choose, crossTurn, circleTurn, end};
        public enum Hooman { cross, circle, none};

        public void startGraphics(Graphics g, Size screenSize)
        {
            GCanvas = new GCanvas(g, Properties.Resources.Choose, Properties.Resources.Board);
            this.screenSize = screenSize;
            tileSize = (int)(screenSize.Width * 0.7 / root.board.GetLength(0));
            draw();
        }

        public Game()
        {
            treeHandler = new TreeHandler();
            root = treeHandler.getRoot();
            reset();
        }

        private void reset()
        {
            gamestate = GameState.choose;
            hooman = Hooman.none;
            currentBranch = treeHandler.getRoot();
        }

        private void draw()
        {
            if (gamestate == GameState.choose)
            {
                GCanvas.drawChooseScreen(screenSize.Width, screenSize.Height);
            }
            else
                GCanvas.drawBoard(currentBranch.board, screenSize.Width, screenSize.Height, tileSize, tileSize);

        }

        public void click(MouseEventArgs e)
        {
            if (gamestate == GameState.choose)
            {
                if (e.X < screenSize.Width / 2)
                    hooman = Hooman.circle;
                else
                    hooman = Hooman.cross;
                gamestate = GameState.crossTurn;
                draw();
            }
            else if (gamestate == GameState.end)
            {
                reset();
                draw();
            }
            else if ((hooman == Hooman.cross && gamestate == GameState.crossTurn) || (hooman == Hooman.circle && gamestate == GameState.circleTurn))
            {
                int x = root.board.GetLength(0) * e.X / screenSize.Width;
                int y = root.board.GetLength(1) * e.Y / screenSize.Height;
                tileClicked(x, y);
                draw();
            }
            AIMove();
        }

        private void tileClicked(int x, int y)
        {
            if(currentBranch.board[x,y].getState() == Tile.TileState.empty)
            {
                Tile[,] newBoard = TreeHandler.copyBoard(currentBranch.board);
                if (gamestate == GameState.crossTurn)
                {
                    newBoard[x, y].setState(Tile.TileState.cross);
                    gamestate = GameState.circleTurn;
                }
                else if (gamestate == GameState.circleTurn)
                {
                    newBoard[x, y].setState(Tile.TileState.circle);
                    gamestate = GameState.crossTurn;
                }
                currentBranch = treeHandler.moveTile(currentBranch, newBoard);
                if (currentBranch.getBranches() == null)
                    gamestate = GameState.end;
            }
        }

        private void AIMove()
        {
            if ((hooman == Hooman.cross && gamestate == GameState.circleTurn) || (hooman == Hooman.circle && gamestate == GameState.crossTurn))
            {
                if (hooman == Hooman.cross)
                    currentBranch = treeHandler.bestMoveCircle(currentBranch);
                else
                    currentBranch = treeHandler.bestMoveCross(currentBranch);

                if (gamestate == GameState.crossTurn)
                    gamestate = GameState.circleTurn;
                else if (gamestate == GameState.circleTurn)
                    gamestate = GameState.crossTurn;
                if (currentBranch.getBranches() == null)
                    gamestate = GameState.end;
                draw();
            }
        }
    }
}
