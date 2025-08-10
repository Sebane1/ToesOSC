namespace ToesOSC
{
    partial class ToeManagerWindow
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
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToeManagerWindow));
            pictureBox1 = new PictureBox();
            leftToeButton1 = new Button();
            leftToeButton2 = new Button();
            leftToeButton3 = new Button();
            leftToeButton4 = new Button();
            rightToeButton1 = new Button();
            rightToeButton2 = new Button();
            rightToeButton3 = new Button();
            rightToeButton4 = new Button();
            bendAllToesLeft = new Button();
            bendAllToesRight = new Button();
            bendSmallToesButtonLeft = new Button();
            bendSmallToesButtonRight = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImage = (Image)resources.GetObject("pictureBox1.BackgroundImage");
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(463, 448);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // leftToeButton1
            // 
            leftToeButton1.Location = new Point(180, 12);
            leftToeButton1.Name = "leftToeButton1";
            leftToeButton1.Size = new Size(37, 45);
            leftToeButton1.TabIndex = 1;
            leftToeButton1.Text = "1";
            leftToeButton1.UseVisualStyleBackColor = true;
            // 
            // leftToeButton2
            // 
            leftToeButton2.Location = new Point(124, 18);
            leftToeButton2.Name = "leftToeButton2";
            leftToeButton2.Size = new Size(27, 33);
            leftToeButton2.TabIndex = 2;
            leftToeButton2.Text = "2";
            leftToeButton2.UseVisualStyleBackColor = true;
            // 
            // leftToeButton3
            // 
            leftToeButton3.Location = new Point(72, 24);
            leftToeButton3.Name = "leftToeButton3";
            leftToeButton3.Size = new Size(27, 33);
            leftToeButton3.TabIndex = 3;
            leftToeButton3.Text = "3";
            leftToeButton3.UseVisualStyleBackColor = true;
            // 
            // leftToeButton4
            // 
            leftToeButton4.Location = new Point(29, 55);
            leftToeButton4.Name = "leftToeButton4";
            leftToeButton4.Size = new Size(27, 33);
            leftToeButton4.TabIndex = 4;
            leftToeButton4.Text = "4";
            leftToeButton4.UseVisualStyleBackColor = true;
            // 
            // rightToeButton1
            // 
            rightToeButton1.Location = new Point(252, 12);
            rightToeButton1.Name = "rightToeButton1";
            rightToeButton1.Size = new Size(37, 45);
            rightToeButton1.TabIndex = 5;
            rightToeButton1.Text = "1";
            rightToeButton1.UseVisualStyleBackColor = true;
            // 
            // rightToeButton2
            // 
            rightToeButton2.Location = new Point(316, 24);
            rightToeButton2.Name = "rightToeButton2";
            rightToeButton2.Size = new Size(27, 33);
            rightToeButton2.TabIndex = 6;
            rightToeButton2.Text = "2";
            rightToeButton2.UseVisualStyleBackColor = true;
            // 
            // rightToeButton3
            // 
            rightToeButton3.Location = new Point(364, 37);
            rightToeButton3.Name = "rightToeButton3";
            rightToeButton3.Size = new Size(27, 33);
            rightToeButton3.TabIndex = 7;
            rightToeButton3.Text = "3";
            rightToeButton3.UseVisualStyleBackColor = true;
            // 
            // rightToeButton4
            // 
            rightToeButton4.Location = new Point(409, 58);
            rightToeButton4.Name = "rightToeButton4";
            rightToeButton4.Size = new Size(27, 33);
            rightToeButton4.TabIndex = 8;
            rightToeButton4.Text = "4";
            rightToeButton4.UseVisualStyleBackColor = true;
            rightToeButton4.Click += rightToeButton4_Click;
            // 
            // bendAllToesLeft
            // 
            bendAllToesLeft.Location = new Point(29, 136);
            bendAllToesLeft.Name = "bendAllToesLeft";
            bendAllToesLeft.Size = new Size(188, 45);
            bendAllToesLeft.TabIndex = 9;
            bendAllToesLeft.Text = "Bend All";
            bendAllToesLeft.UseVisualStyleBackColor = true;
            bendAllToesLeft.Click += bendAllToesLeft_Click;
            // 
            // bendAllToesRight
            // 
            bendAllToesRight.Location = new Point(252, 136);
            bendAllToesRight.Name = "bendAllToesRight";
            bendAllToesRight.Size = new Size(188, 45);
            bendAllToesRight.TabIndex = 10;
            bendAllToesRight.Text = "Bend All";
            bendAllToesRight.UseVisualStyleBackColor = true;
            bendAllToesRight.Click += bendAllToesRight_Click;
            // 
            // bendSmallToesButtonLeft
            // 
            bendSmallToesButtonLeft.Location = new Point(29, 94);
            bendSmallToesButtonLeft.Name = "bendSmallToesButtonLeft";
            bendSmallToesButtonLeft.Size = new Size(122, 36);
            bendSmallToesButtonLeft.TabIndex = 11;
            bendSmallToesButtonLeft.Text = "Bend Small Toes";
            bendSmallToesButtonLeft.UseVisualStyleBackColor = true;
            bendSmallToesButtonLeft.Click += bendSmallToesButtonLeft_Click;
            // 
            // bendSmallToesButtonRight
            // 
            bendSmallToesButtonRight.Location = new Point(316, 94);
            bendSmallToesButtonRight.Name = "bendSmallToesButtonRight";
            bendSmallToesButtonRight.Size = new Size(122, 36);
            bendSmallToesButtonRight.TabIndex = 12;
            bendSmallToesButtonRight.Text = "Bend Small Toes";
            bendSmallToesButtonRight.UseVisualStyleBackColor = true;
            bendSmallToesButtonRight.Click += bendSmallToesButtonRight_Click;
            // 
            // ToeManagerWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(463, 448);
            Controls.Add(bendSmallToesButtonRight);
            Controls.Add(bendSmallToesButtonLeft);
            Controls.Add(bendAllToesRight);
            Controls.Add(bendAllToesLeft);
            Controls.Add(rightToeButton4);
            Controls.Add(rightToeButton3);
            Controls.Add(rightToeButton2);
            Controls.Add(rightToeButton1);
            Controls.Add(leftToeButton4);
            Controls.Add(leftToeButton3);
            Controls.Add(leftToeButton2);
            Controls.Add(leftToeButton1);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "ToeManagerWindow";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Toe Tester";
            KeyDown += ToeManagerWindow_KeyDown;
            KeyPress += ToeManagerWindow_KeyPress;
            KeyUp += ToeManagerWindow_KeyUp;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private Button leftToeButton1;
        private Button leftToeButton2;
        private Button leftToeButton3;
        private Button leftToeButton4;
        private Button rightToeButton1;
        private Button rightToeButton2;
        private Button rightToeButton3;
        private Button rightToeButton4;
        private Button bendAllToesLeft;
        private Button bendAllToesRight;
        private Button bendSmallToesButtonLeft;
        private Button bendSmallToesButtonRight;
    }
}
