using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace datorium_snake_game_cs
{
    public partial class Game : Form
    {
        #region Decleared variables

        private int verticalVelocity = 0;
        private int horizontalVelocity = 0;
        private int tempHorizontalVelocity;
        private int tempVerticalVelocity;
        private int snakeSpeed = 20;

        private Random rand = new Random();
        private GameZone Zone;
        private List<SnakePixel> Snake = new List<SnakePixel>();
        private Timer GameTimer;
        private Food FreshFood;

        #endregion

        public Game()
        {
            InitializeComponent();
            InitializeGameZone();
            InitializeGame();
            InitializeSnake();
            InitializeGameTimer();
            InitializeFood();
        }

        #region Initializations

        private void InitializeFood()
        {
            FreshFood = new Food();
            FoodRegenerate();
            this.Controls.Add(FreshFood);
            FreshFood.BringToFront();
        }

        private void InitializeGameTimer()
        {
            GameTimer = new Timer();
            GameTimer.Interval = 400;
            GameTimer.Tick += new EventHandler(GameTimer_Tick);
            GameTimer.Start();
        }
        
        private void InitializeSnake()
        {
            horizontalVelocity = snakeSpeed;

            #region head

            Snake.Add(new SnakePixel());
            Snake[0].Left = 200;
            Snake[0].Top = 200;
            Snake[0].Image = Properties.Resources.snake_head_0;
            Snake[0].BackColor = Color.Transparent;
            this.Controls.Add(Snake[0]);
            Snake[0].BringToFront();

            #endregion

            #region body

            Snake.Add(new SnakePixel());
            Snake[1].Left = 180;
            Snake[1].Top = 200;
            Snake[1].Image = Properties.Resources.snake_straight_90;
            this.Controls.Add(Snake[1]);
            Snake[1].BringToFront();

            #endregion

            #region tail

            Snake.Add(new SnakePixel());
            Snake[Snake.Count -1].Left = 160;
            Snake[Snake.Count - 1].Top = 200;
            Snake[Snake.Count - 1].Image = Properties.Resources.snake_tail_0;
            Snake[Snake.Count - 1].BackColor = Color.Transparent;
            this.Controls.Add(Snake[2]);
            Snake[Snake.Count - 1].BringToFront();

            #endregion
        }

        private void InitializeGame()
        {
            this.Width = 600;
            this.Height = 600;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.BackColor = Color.AliceBlue;
            this.KeyDown += new KeyEventHandler(Game_KeyDown);
        }

        private void InitializeGameZone()
        {
            Zone = new GameZone();
            Zone.Left = 0;
            Zone.Top = 0;
            Zone.BackColor = Color.White;
            this.Controls.Add(Zone);
        }

        #endregion

        private int RandBetween(int a, int b)
        {
            return rand.Next(a, b + 1);
        }

        private void GameOver()
        {
            GameTimer.Stop();
            Snake[0].BringToFront();
            this.BackColor = Color.Red;
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            SnakeMove();
            SnakeFoodCollision();
            SnakeRectangleCollision();
            SnakeItselfCollision();
        }

        #region snakeMove

        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    tempVerticalVelocity = 0;
                    tempHorizontalVelocity = snakeSpeed;
                    break;
                case Keys.Down:
                    tempVerticalVelocity = snakeSpeed;
                    tempHorizontalVelocity = 0;
                    break;
                case Keys.Left:
                    tempVerticalVelocity = 0;
                    tempHorizontalVelocity = -snakeSpeed;
                    break;
                case Keys.Up:
                    tempVerticalVelocity = -snakeSpeed;
                    tempHorizontalVelocity = 0;
                    break;
            }
        }

        private void SnakeMove()
        {
            SetDirection();

            for(int i = Snake.Count - 1; i > 0; i--)
            {
                Snake[i].Left = Snake[i - 1].Left;
                Snake[i].Top = Snake[i - 1].Top;
            }

            Snake[0].Left += horizontalVelocity;
            Snake[0].Top += verticalVelocity;

            SetBodySprite();
            SetTailSprite();
            SetHeadSprite();
        }

        private void SetDirection()
        {
            if(Snake.Count > 1)
            {
                if(tempHorizontalVelocity == -horizontalVelocity || tempVerticalVelocity == -verticalVelocity)
                    return;
            }
            horizontalVelocity = tempHorizontalVelocity;
            verticalVelocity = tempVerticalVelocity;
        }

        #endregion

        #region set sprite

        private void SetBodySprite()
        {
            for(int i = Snake.Count -2; i > 0; i--)
            {
                if ((Snake[i - 1].Left < Snake[i].Left) && (Snake[i + 1].Left > Snake[i].Left))
                    Snake[i].Image = Properties.Resources.snake_straight_0;
                else if ((Snake[i + 1].Left < Snake[i].Left) && (Snake[i - 1].Left > Snake[i].Left))
                    Snake[i].Image = Properties.Resources.snake_straight_0;
                else if ((Snake[i - 1].Top < Snake[i].Top) && (Snake[i + 1].Top > Snake[i].Top))
                    Snake[i].Image = Properties.Resources.snake_straight_90;
                else if ((Snake[i + 1].Top < Snake[i].Top) && (Snake[i - 1].Top > Snake[i].Top))
                    Snake[i].Image = Properties.Resources.snake_straight_90;
                else if ((Snake[i - 1].Top < Snake[i].Top) && (Snake[i + 1].Left < Snake[i].Left))
                    Snake[i].Image = Properties.Resources.snake_corner_90;
                else if ((Snake[i + 1].Top < Snake[i].Top) && (Snake[i - 1].Left < Snake[i].Left))
                    Snake[i].Image = Properties.Resources.snake_corner_90;
                else if ((Snake[i - 1].Left < Snake[i].Left) && (Snake[i + 1].Top > Snake[i].Top))
                    Snake[i].Image = Properties.Resources.snake_corner_0;
                else if ((Snake[i + 1].Left < Snake[i].Left) && (Snake[i - 1].Top > Snake[i].Top))
                    Snake[i].Image = Properties.Resources.snake_corner_0;
                else if ((Snake[i - 1].Top < Snake[i].Top) && (Snake[i + 1].Left > Snake[i].Left))
                    Snake[i].Image = Properties.Resources.snake_corner_180;
                else if ((Snake[i + 1].Top < Snake[i].Top) && (Snake[i - 1].Left > Snake[i].Left))
                    Snake[i].Image = Properties.Resources.snake_corner_180;
                else if ((Snake[i - 1].Left > Snake[i].Left) && (Snake[i + 1].Top > Snake[i].Top))
                    Snake[i].Image = Properties.Resources.snake_corner_270;
                else if ((Snake[i + 1].Left > Snake[i].Left) && (Snake[i - 1].Top > Snake[i].Top))
                    Snake[i].Image = Properties.Resources.snake_corner_270;
            }
        }

        private void SetHeadSprite()
        {
            if (Snake[1].Left < Snake[0].Left)
                Snake[0].Image = Properties.Resources.snake_head_0;
            else if (Snake[1].Left > Snake[0].Left)
                Snake[0].Image = Properties.Resources.snake_head_180;
            else if (Snake[1].Top < Snake[0].Top)
                Snake[0].Image = Properties.Resources.snake_head_90;
            else if (Snake[1].Top > Snake[0].Top)
                Snake[0].Image = Properties.Resources.snake_head_270;
        }

        private void SetTailSprite()
        {
            if (Snake[Snake.Count - 2].Left < Snake[Snake.Count - 1].Left)
                Snake[Snake.Count - 1].Image = Properties.Resources.snake_tail_180;
            else if (Snake[Snake.Count - 2].Left > Snake[Snake.Count - 1].Left)
                Snake[Snake.Count - 1].Image = Properties.Resources.snake_tail_0;
            else if (Snake[Snake.Count - 2].Top < Snake[Snake.Count - 1].Top)
                Snake[Snake.Count - 1].Image = Properties.Resources.snake_tail_270;
            else if (Snake[Snake.Count - 2].Top > Snake[Snake.Count - 1].Top)
                Snake[Snake.Count - 1].Image = Properties.Resources.snake_tail_90;
            Snake[Snake.Count - 1].BringToFront();
        }

        #endregion

        #region food eaten

        private void AddSnakePixel()
        {
            Snake.Add(new SnakePixel());
            Snake[Snake.Count - 1].Top = Snake[Snake.Count - 2].Top;
            Snake[Snake.Count - 1].Left = Snake[Snake.Count - 2].Left;
            Snake[Snake.Count - 1].BackColor = Color.Transparent;
            this.Controls.Add(Snake[Snake.Count - 1]);
        }

        private void FoodRegenerate()
        {
            FreshFood.Left = 20 * RandBetween(0, 19);
            FreshFood.Top = 20 * RandBetween(0, 19);

        }

        #endregion

        #region Collisions

        private void SnakeFoodCollision()
        {
            if (Snake[0].Bounds.IntersectsWith(FreshFood.Bounds))
            {
                FoodRegenerate();
                AddSnakePixel();
                if(GameTimer.Interval > 5)
                    GameTimer.Interval -= 5;
            }
        }

        private void SnakeRectangleCollision()
        {
            if (!Zone.Bounds.Contains(Snake[0].Bounds))
            {
                GameOver();
            }
        }

        private void SnakeItselfCollision()
        {
            for(int i = Snake.Count - 1; i > 2; i--)
            {
                if (Snake[0].Bounds.IntersectsWith(Snake[i].Bounds))
                    GameOver();
            }
        }

        #endregion
    }
}
