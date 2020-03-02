using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace datorium_snake_game_cs
{
    class Food : PictureBox
    {
        public Food()
        {
            InitializeFood();
        }

        private void InitializeFood()
        {
            this.BackColor = Color.Red;
            this.Height = 20;
            this.Width = 20;
        }
    }
}
