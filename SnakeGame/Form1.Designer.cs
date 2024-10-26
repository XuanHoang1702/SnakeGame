namespace SnakeGame
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            gamePanel = new Panel();
            lblStart = new Label();
            gameTimer = new System.Windows.Forms.Timer(components);
            lblCountdown = new Label();
            lblScore = new Label();
            btnPlayAgain = new Button();
            lblSpeed = new Label();
            lblBest = new Label();
            lblPlayer = new Label();
            gamePanel.SuspendLayout();
            SuspendLayout();
            // 
            // gamePanel
            // 
            gamePanel.Controls.Add(lblStart);
            gamePanel.Location = new Point(3, 47);
            gamePanel.Name = "gamePanel";
            gamePanel.Size = new Size(590, 456);
            gamePanel.TabIndex = 0;
            gamePanel.Paint += gamePanel_Paint;
            // 
            // lblStart
            // 
            lblStart.AutoSize = true;
            lblStart.ForeColor = Color.Snow;
            lblStart.Location = new Point(201, 199);
            lblStart.Name = "lblStart";
            lblStart.Size = new Size(146, 20);
            lblStart.TabIndex = 0;
            lblStart.Text = "Press any key to start";
            lblStart.Click += lblStart_Click;
            // 
            // gameTimer
            // 
            gameTimer.Tick += GameTimer_Tick;
            // 
            // lblCountdown
            // 
            lblCountdown.AutoSize = true;
            lblCountdown.Font = new Font("Arial", 24F, FontStyle.Bold);
            lblCountdown.ForeColor = Color.White;
            lblCountdown.Location = new Point(350, 200);
            lblCountdown.Name = "lblCountdown";
            lblCountdown.Size = new Size(0, 46);
            lblCountdown.TabIndex = 1;
            lblCountdown.TextAlign = ContentAlignment.MiddleCenter;
            lblCountdown.Visible = false;
            // 
            // lblScore
            // 
            lblScore.AutoSize = true;
            lblScore.ForeColor = Color.White;
            lblScore.Location = new Point(12, 9);
            lblScore.Name = "lblScore";
            lblScore.Size = new Size(50, 20);
            lblScore.TabIndex = 2;
            lblScore.Text = "label1";
            // 
            // btnPlayAgain
            // 
            btnPlayAgain.Location = new Point(245, 3);
            btnPlayAgain.Name = "btnPlayAgain";
            btnPlayAgain.Size = new Size(94, 29);
            btnPlayAgain.TabIndex = 3;
            btnPlayAgain.Text = "Play again";
            btnPlayAgain.UseVisualStyleBackColor = true;
            btnPlayAgain.Click += btnPlayAgain_Click;
            // 
            // lblSpeed
            // 
            lblSpeed.AutoSize = true;
            lblSpeed.ForeColor = Color.White;
            lblSpeed.Location = new Point(87, 9);
            lblSpeed.Name = "lblSpeed";
            lblSpeed.Size = new Size(50, 20);
            lblSpeed.TabIndex = 4;
            lblSpeed.Text = "label1";
            lblSpeed.Click += lblSpeed_Click;
            // 
            // lblBest
            // 
            lblBest.AutoSize = true;
            lblBest.ForeColor = Color.White;
            lblBest.Location = new Point(367, 12);
            lblBest.Name = "lblBest";
            lblBest.Size = new Size(50, 20);
            lblBest.TabIndex = 5;
            lblBest.Text = "label1";
            lblBest.Click += lblBest_Click;
            // 
            // lblPlayer
            // 
            lblPlayer.AutoSize = true;
            lblPlayer.ForeColor = Color.White;
            lblPlayer.Location = new Point(466, 12);
            lblPlayer.Name = "lblPlayer";
            lblPlayer.Size = new Size(50, 20);
            lblPlayer.TabIndex = 6;
            lblPlayer.Text = "label1";
            lblPlayer.Click += lblPlayer_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(592, 500);
            Controls.Add(lblPlayer);
            Controls.Add(lblBest);
            Controls.Add(lblSpeed);
            Controls.Add(btnPlayAgain);
            Controls.Add(lblScore);
            Controls.Add(lblCountdown);
            Controls.Add(gamePanel);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            KeyDown += Form1_KeyDown;
            gamePanel.ResumeLayout(false);
            gamePanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel gamePanel;
        private System.Windows.Forms.Timer gameTimer;
        private Label lblCountdown;
        private Label lblScore;
        private Button btnPlayAgain;
        private Label lblStart;
        private Label lblSpeed;
        private Label lblBest;
        private Label lblPlayer;
    }
}
