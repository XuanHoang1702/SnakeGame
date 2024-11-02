using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using WMPLib;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        // Fields and constants
        private List<Point> snake = new List<Point>();
        private Point food;
        private Point? yellowFood = null;
        private int direction;
        private int score = 0;
        private readonly Random random = new Random();
        private const int boxSize = 20;
        private const double moveSpeed = 20;
        private double currentSpeed = 10;
        private double frameCounter = 0;
        private string playerName;
        private int bestScore;

        // Sound player
        private readonly WindowsMediaPlayer playEatSound = new WindowsMediaPlayer();
        private readonly WindowsMediaPlayer playEaYellowtSound = new WindowsMediaPlayer();
        private readonly WindowsMediaPlayer playDeathtSound = new WindowsMediaPlayer();
        private readonly WindowsMediaPlayer playLevelUpSound = new WindowsMediaPlayer();
        private readonly WindowsMediaPlayer playStartSound = new WindowsMediaPlayer();


        // Constructor
        public Form1(string username, int bestScore)
        {
            InitializeComponent();
            playerName = username;
            this.bestScore = bestScore;

            lblPlayer.Text = "Player: " + playerName;
            lblBest.Text = "Best Score: " + bestScore;

            // Initialize sound player
            playEatSound = new WindowsMediaPlayer();
            playEaYellowtSound = new WindowsMediaPlayer();
            // Event handlers
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.KeyPreview = true;
            gameTimer.Interval = 100;
            gameTimer.Tick += new EventHandler(GameTimer_Tick);
            this.BackColor = Color.Black;
            lblStart.Visible = true;
            lblSpeed.Text = "Speed: " + Math.Round(currentSpeed, 1).ToString("0.0");
        }

        // Game control methods
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

            for (int i = 1; i < snake.Count; i++)
            {
                if (newHead == snake[i])
                {
                    PlayDeathSound();
                    gameTimer.Stop();
                    if (score > bestScore)
                    {
                        UpdateBestScore(score);
                        lblBest.Text = "Best Score: " + score;
                    }
                    currentSpeed = 10;
                    MessageBox.Show("Game Over! Your score: " + score);
                    btnPlayAgain.Visible = true;
                    return;
                }
            }

            if (newHead == food)
            {
                snake.Insert(0, newHead);
                GenerateFood();
                score += 1;
                lblScore.Text = "Score: " + score.ToString();
                PlayEatSound();

                if (score % 15 == 0)
                {
                    PlayLevelUpSound();
                    currentSpeed += 0.1;
                    gameTimer.Interval = Math.Max(2, gameTimer.Interval - 20);
                    lblSpeed.Text = "Speed: " + Math.Round(currentSpeed, 1).ToString("0.0");
                }
            }
            else if (yellowFood != null && newHead == yellowFood.Value)
            {
                
                snake.Insert(0, newHead);
                PlayEatSound();
                yellowFood = null;
                score += 2;
                lblScore.Text = "Score: " + score.ToString();
            }
            else
            {
                snake.Insert(0, newHead);
                snake.RemoveAt(snake.Count - 1);
            }

            if (newHead.X < 0 || newHead.Y < 0 || newHead.X >= gamePanel.Width || newHead.Y >= gamePanel.Height)
            {
                PlayDeathSound();
                gameTimer.Stop();
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


        private void GenerateFood()
        {
            food = new Point(random.Next(0, gamePanel.Width / boxSize), random.Next(0, gamePanel.Height / boxSize));
            food.X *= boxSize;
            food.Y *= boxSize;

            if (score > 0 && score % 5 == 0 && yellowFood == null)
            {
                PlayEatYellowSound();
                yellowFood = new Point(random.Next(0, gamePanel.Width / boxSize), random.Next(0, gamePanel.Height / boxSize));
                yellowFood = new Point(yellowFood.Value.X * boxSize, yellowFood.Value.Y * boxSize);
            }
        }

        // UI and rendering methods
        private void gamePanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen borderPen = new Pen(Color.Green, 2);
            g.DrawRectangle(borderPen, 0, 0, gamePanel.Width - 5, gamePanel.Height - 5);

            for (int i = 0; i < snake.Count; i++)
            {
                int red = 255 ;
                int green = 255 - (i * 5 > 255 ? 255 : i * 5);
                int blue = 255 - (i * 2 > 255 ? 255 : i * 2);
                Brush brush = new SolidBrush(Color.FromArgb(red, green, blue));

                g.FillRectangle(i == 0 ? Brushes.Red : brush, snake[i].X, snake[i].Y, boxSize, boxSize);
            }

            g.FillRectangle(Brushes.Blue, food.X, food.Y, boxSize, boxSize);
            if (yellowFood != null)
                g.FillRectangle(Brushes.Yellow, yellowFood.Value.X, yellowFood.Value.Y, boxSize, boxSize);
        }


        // Event handling methods
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            MoveSnake();
            gamePanel.Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            playStartSound.controls.stop();
            if (!gameTimer.Enabled) 
            { StartGame();  return; }
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

        private void PlayEatSound()
        {
            try
            {
                playEatSound.URL = "D:\\LTC#\\SnakeGame\\SnakeGame\\Sound\\eat.wav";
                playEatSound.settings.volume = 100;
                playEatSound.controls.play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error playing eat sound: " + ex.Message);
            }
        }

        private void PlayEatYellowSound()
        {
            try
            {
                playEaYellowtSound.URL = "D:\\LTC#\\SnakeGame\\SnakeGame\\Sound\\eat_yellow.wav";
                playEaYellowtSound.settings.volume = 100;
                playEaYellowtSound.controls.play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error playing eat sound: " + ex.Message);
            }
        }

        private void PlayDeathSound()
        {
            try
            {
                playDeathtSound.URL = "D:\\LTC#\\SnakeGame\\SnakeGame\\Sound\\death.wav";
                playDeathtSound.settings.volume = 100;
                playDeathtSound.controls.play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error playing eat sound: " + ex.Message);
            }
        }

        private void PlayLevelUpSound()
        {
            try
            {
                playLevelUpSound.URL = "D:\\LTC#\\SnakeGame\\SnakeGame\\Sound\\level-up.wav";
                playLevelUpSound.settings.volume = 200;
                playLevelUpSound.controls.play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error playing eat sound: " + ex.Message);
            }
        }

        private void PlayStartSound()
        {
            try
            {
                playStartSound.URL = "D:\\LTC#\\SnakeGame\\SnakeGame\\Sound\\start.wav";
                playStartSound.settings.volume = 100;
                playStartSound.settings.setMode("loop", true);
                playStartSound.controls.play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error playing eat sound: " + ex.Message);
            }

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

        // Empty event handlers (can be removed if not needed)
        private void lblStart_Click(object sender, EventArgs e) { }
        private void lblSpeed_Click(object sender, EventArgs e) { }
        private void lblBest_Click(object sender, EventArgs e) { }
        private void lblPlayer_Click(object sender, EventArgs e) { }
        private void Form1_Load(object sender, EventArgs e) {
            PlayStartSound();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Exit();
        }
    }
}
