using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Media;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        private List<Point> snake = new List<Point>();
        private Point food;
        private Point? yellowFood = null;
        private int direction;
        private int score = 0;
        private Random random = new Random();
        private const int boxSize = 10;
        private const double moveSpeed = 20;
        private double currentSpeed = 10;
        private double frameCounter = 0;
        private string playerName;
        private int bestScore;
        private SoundPlayer loseSound;
        private SoundPlayer eatSound;
        private SoundPlayer eatYellowSound;


        public Form1(string username, int bestScore)
        {
            InitializeComponent();
            playerName = username;
            this.bestScore = bestScore;

            loseSound = new SoundPlayer(@"Resources/death.wav");
            eatSound = new SoundPlayer(@"Resources/eat.wav");
            eatYellowSound = new SoundPlayer(@"Resources/eat_yellow.wav");

            lblPlayer.Text = "Player: " + playerName; 
            lblBest.Text = "Best Score: " + bestScore;
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.KeyPreview = true;
            gameTimer.Interval = 100;
            gameTimer.Tick += new EventHandler(GameTimer_Tick);
            this.BackColor = Color.Black;
            lblStart.Visible = true;
            lblSpeed.Text = "Speed: " + Math.Round(currentSpeed, 1).ToString("0.0");
        }

        private void StartGame()
        {
            lblStart.Visible = false;
            snake = new List<Point>();

            int startX = (gamePanel.Width / 2) / boxSize * boxSize;
            int startY = (gamePanel.Height / 2) / boxSize * boxSize;

            snake.Add(new Point(startX, startY));
            direction = 0;
            score = 0;
            currentSpeed = 10;
            lblScore.Text = "Score: 0";
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
                    g.FillRectangle(Brushes.Red, snake[i].X, snake[i].Y, boxSize, boxSize);
                }
                else
                {
                    g.FillRectangle(Brushes.White, snake[i].X, snake[i].Y, boxSize, boxSize);
                }
            }

            g.FillRectangle(Brushes.Blue, food.X, food.Y, boxSize, boxSize);
            if (yellowFood != null)
            {
                g.FillRectangle(Brushes.Yellow, yellowFood.Value.X, yellowFood.Value.Y, boxSize, boxSize);
            }
        }

        private void MoveSnake()
        {
            frameCounter += 10;
            if (frameCounter % moveSpeed != 0) return;

            Point newHead = snake[0];

            switch (direction)
            {
                case 0: newHead.Y -= boxSize; break;
                case 1: newHead.X += boxSize; break;
                case 2: newHead.Y += boxSize; break;
                case 3: newHead.X -= boxSize; break;
            }

            if (newHead == food)
            {
                snake.Insert(0, newHead);
                GenerateFood();
                score += 1;
                lblScore.Text = "Score: " + score.ToString();
                eatSound.Play();
                if (score % 15 == 0)
                {
                    currentSpeed += 0.1;
                    gameTimer.Interval = Math.Max(2, gameTimer.Interval - 20);
                    lblSpeed.Text = "Speed: " + Math.Round(currentSpeed, 1).ToString("0.0");

                }
            }
            else if (yellowFood != null && newHead == yellowFood.Value)
            {
                snake.Insert(0, newHead);
                yellowFood = null;
                score += 2;
                lblScore.Text = "Score" + score.ToString();
                eatYellowSound.Play();
            }
            else
            {
                snake.Insert(0, newHead);
                snake.RemoveAt(snake.Count - 1);
            }

            if (newHead.X < 0 || newHead.Y < 0 || newHead.X >= gamePanel.Width || newHead.Y >= gamePanel.Height)
            {
                gameTimer.Stop();
                loseSound.Play();
                if (score > bestScore)
                {
                    UpdateBestScore(score);
                    lblBest.Text = "Best Score: " + score;
                }
                currentSpeed = 10;
                MessageBox.Show("Game Over! Your score: " + score);
                btnPlayAgain.Visible = true;
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
            currentSpeed = 10;
            StartGame();
            btnPlayAgain.Visible = false;
            lblSpeed.Text = "Speed: " + Math.Round(currentSpeed, 1).ToString("0.0");
        }

        private void UpdateBestScore(int score)
        {
            string connectionString = "Data Source=TRAN-XUAN-HOANG\\SQLEXPRESS;Initial Catalog=SnakeGame;Integrated Security=True";
            string query = "UPDATE Game SET best_score = @BestScore WHERE user_name = @UserName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@UserName", playerName);
                command.Parameters.AddWithValue("@BestScore", score);
                command.ExecuteNonQuery();
            }
        }


        private void lblStart_Click(object sender, EventArgs e)
        {

        }

        private void lblSpeed_Click(object sender, EventArgs e)
        {

        }

        private void lblBest_Click(object sender, EventArgs e)
        {

        }

        private void lblPlayer_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
