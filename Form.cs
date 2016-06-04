using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TickTackToo
{
    public partial class TickTackToe : Form
    {
        private Game game = new Game();
        public TickTackToe()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            this.ClientSize = new Size(300, 300);
        }

        private void TickTackToe_MouseClick(object sender, MouseEventArgs e)
        {
            game.click(e);
        }

        private void TickTackToe_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = this.CreateGraphics();
            game.startGraphics(g, this.ClientSize);
        }
    }
}
