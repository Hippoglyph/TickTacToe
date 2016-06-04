using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TickTackToo
{
    class Tile
    {
        Bitmap texture;
        TileState state;

        public enum TileState { empty, cross, circle}

        public Tile()
        {
            state = TileState.empty;
        }

        public void setState(TileState state)
        {
            this.state = state;
        }

        public TileState getState()
        {
            return state;
        }

        public Bitmap getTexture()
        {
            return texture;
        }
    }
}
