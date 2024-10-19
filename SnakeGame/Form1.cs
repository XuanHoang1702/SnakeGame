using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        private List<Point> snake = new List<Point>();
        private Point food;
        private Point? yellowFood = null;
        private int direction;
        private int score = 0; // Biến điểm số
        private Random random = new Random();
        private const int boxSize = 10;
        private const double moveSpeed = 10;
        private double currentSpeed = 10;
        private double frameCounter = 0;


        public Form1()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.KeyPreview = true;
            gameTimer.Interval = 100;
            gameTimer.Tick += new EventHandler(GameTimer_Tick);
            //StartGame();
            this.BackColor = Color.Black;
            lblStart.Visible = true;
            lblSpeed.Text = "Speed: " + currentSpeed.ToString();
        }

        private void StartGame()
        {
            lblStart.Visible = false;
            snake = new List<Point>();

            // Tính toán tọa độ giữa màn hình
            int startX = (gamePanel.Width / 2) / boxSize * boxSize;
            int startY = (gamePanel.Height / 2) / boxSize * boxSize;

            snake.Add(new Point(startX, startY)); // Đặt rắn ở giữa
            direction = 0;
            score = 0; // Đặt lại điểm khi bắt đầu game mới
            currentSpeed = 10;
            lblScore.Text = "Score: 0"; // Cập nhật điểm
            lblSpeed.Text = "Speed: " + currentSpeed.ToString();
            GenerateFood();
            yellowFood = null;
            gameTimer.Start();
        }

        private void GenerateFood()
        {
            food = new Point(random.Next(0, gamePanel.Width / boxSize), random.Next(0, gamePanel.Height / boxSize));
            food.X *= boxSize;
            food.Y *= boxSize;
            if (score > 0 && score % 5 == 0 && yellowFood == null)
            {
                yellowFood = new Point(random.Next(0, gamePanel.Width / boxSize), random.Next(0, gamePanel.Height / boxSize));
                yellowFood = new Point(yellowFood.Value.X * boxSize, yellowFood.Value.Y * boxSize);
            }
        }

        private void gamePanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen borderPen = new Pen(Color.Green, 2);
            g.DrawRectangle(borderPen, 0, 0, gamePanel.Width - 5, gamePanel.Height - 5);

            for (int i = 0; i < snake.Count; i++)
            {
                if (i == 0)
                {
                    g.FillRectangle(Brushes.Red, snake[i].X, snake[i].Y, boxSize, boxSize); // Đầu rắn
                }
                else
                {
                    g.FillRectangle(Brushes.White, snake[i].X, snake[i].Y, boxSize, boxSize); // Thân rắn
                }
            }

            g.FillRectangle(Brushes.Blue, food.X, food.Y, boxSize, boxSize); // Thức ăn
            if (yellowFood != null)
            {
                g.FillRectangle(Brushes.Yellow, yellowFood.Value.X, yellowFood.Value.Y, boxSize, boxSize);
            }
        }

        private void MoveSnake()
        {
            frameCounter += 5;
            if (frameCounter % moveSpeed != 0) return;

            Point newHead = snake[0];

            switch (direction)
            {
                case 0: newHead.Y -= boxSize; break; // Lên
                case 1: newHead.X += boxSize; break; // Phải
                case 2: newHead.Y += boxSize; break; // Xuống
                case 3: newHead.X -= boxSize; break; // Trái
            }

            if (newHead == food)
            {
                snake.Insert(0, newHead);
                GenerateFood();
                score += 1;
                lblScore.Text = "Score: " + score.ToString();
                if (score % 15 == 0)
                {
                    currentSpeed += 0.1;
                    gameTimer.Interval = Math.Max(2, gameTimer.Interval - 20); // Giảm thời gian giữa các lần tick
                    lblSpeed.Text = "Speed: " + currentSpeed.ToString();

                }
            }
            else if (yellowFood != null && newHead == yellowFood.Value)
            {
                snake.Insert(0, newHead);
                yellowFood = null;
                score += 2;
                lblScore.Text = "Score" + score.ToString();
            }
            else
            {
                snake.Insert(0, newHead);
                snake.RemoveAt(snake.Count - 1); // Loại bỏ phần đuôi rắn
            }

            if (newHead.X < 0 || newHead.Y < 0 || newHead.X >= gamePanel.Width || newHead.Y >= gamePanel.Height)
            {
                gameTimer.Stop();
                MessageBox.Show("Game Over! Your score: " + score);
                btnPlayAgain.Visible = true; // Hiện nút chơi lại
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            MoveSnake();
            gamePanel.Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!gameTimer.Enabled) { StartGame(); return; }
            if (e.KeyCode == Keys.Up && direction != 2) direction = 0;
            if (e.KeyCode == Keys.Down && direction != 0) direction = 2;
            if (e.KeyCode == Keys.Left && direction != 1) direction = 3;
            if (e.KeyCode == Keys.Right && direction != 3) direction = 1;
        }

        private void btnPlayAgain_Click(object sender, EventArgs e)
        {
            StartGame();
            btnPlayAgain.Visible = false; // Ẩn nút "Chơi lại"
            currentSpeed = 10;
        }

        private void lblStart_Click(object sender, EventArgs e)
        {

        }

        private void lblSpeed_Click(object sender, EventArgs e)
        {

        }
    }
}
