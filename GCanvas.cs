using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TickTackToo
{
    class GCanvas
    {
        private Graphics drawHandler;
        private Bitmap chooseScreen;
        private Bitmap backBoard;
        private Bitmap empty;
        private Bitmap circle;
        private Bitmap cross;

        public GCanvas(Graphics g, Bitmap chooseScreen, Bitmap backBoard)
        {
            drawHandler = g;
            this.chooseScreen = chooseScreen;
            this.backBoard = backBoard;
            empty = Properties.Resources.Empty;
            circle = Properties.Resources.Circle;
            cross = Properties.Resources.Cross;
        }

        public void drawChooseScreen(int width, int height)
        {
            drawHandler.DrawImage(chooseScreen, 0, 0, width, height);
        }

        public void drawBoard(Tile[,] board, int screenWidth, int screenHeight, int tileWidth, int tileHeight)
        {
            drawHandler.DrawImage(backBoard, 0, 0, screenWidth, screenHeight);
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    int xPos = (screenWidth / (board.GetLength(0)*2)) * (x*2 + 1);
                    int yPos = (screenHeight / (board.GetLength(1) * 2)) * (y * 2 + 1);
                    drawHandler.DrawImage(getTextureFor(board[x,y]), xPos - tileWidth / 2, yPos - tileHeight / 2, tileWidth, tileHeight);
                }
            }
        }

        private Bitmap getTextureFor(Tile tile)
        {
            switch (tile.getState())
            {
                case Tile.TileState.cross:
                    return cross;
                case Tile.TileState.circle:
                    return circle;
                default:
                    return empty;
            }
        }
    }
}
