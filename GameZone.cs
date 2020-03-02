using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace datorium_snake_game_cs
{
    class GameZone : PictureBox
    {

        public GameZone() 
        {
            InitializeGameZone();
        }

        private void InitializeGameZone()
        {
            this.Width = 400;
            this.Height = 400;
        }
    }
}
