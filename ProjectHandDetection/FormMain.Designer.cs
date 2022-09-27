namespace ProjectHandDetection
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.startButton = new System.Windows.Forms.Button();
            this.cameraCombo = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rawCameraFrame = new System.Windows.Forms.PictureBox();
            this.resolutionCombo = new System.Windows.Forms.ComboBox();
            this.swipeOutput = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rawCameraFrame)).BeginInit();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(12, 12);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(114, 29);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // cameraCombo
            // 
            this.cameraCombo.FormattingEnabled = true;
            this.cameraCombo.Location = new System.Drawing.Point(132, 17);
            this.cameraCombo.Name = "cameraCombo";
            this.cameraCombo.Size = new System.Drawing.Size(262, 21);
            this.cameraCombo.TabIndex = 1;
            this.cameraCombo.SelectionChangeCommitted += new System.EventHandler(this.cameraCombo_SelectionChangeCommitted);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.rawCameraFrame, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 58);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(435, 330);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // rawCameraFrame
            // 
            this.rawCameraFrame.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rawCameraFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rawCameraFrame.Location = new System.Drawing.Point(3, 3);
            this.rawCameraFrame.Name = "rawCameraFrame";
            this.rawCameraFrame.Size = new System.Drawing.Size(429, 324);
            this.rawCameraFrame.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.rawCameraFrame.TabIndex = 0;
            this.rawCameraFrame.TabStop = false;
            // 
            // resolutionCombo
            // 
            this.resolutionCombo.FormattingEnabled = true;
            this.resolutionCombo.Location = new System.Drawing.Point(400, 17);
            this.resolutionCombo.Name = "resolutionCombo";
            this.resolutionCombo.Size = new System.Drawing.Size(262, 21);
            this.resolutionCombo.TabIndex = 3;
            // 
            // swipeOutput
            // 
            this.swipeOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.swipeOutput.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.swipeOutput.Location = new System.Drawing.Point(576, 61);
            this.swipeOutput.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.swipeOutput.Name = "swipeOutput";
            this.swipeOutput.Size = new System.Drawing.Size(136, 112);
            this.swipeOutput.TabIndex = 9;
            this.swipeOutput.Text = "Swipe Output";
            this.swipeOutput.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 451);
            this.Controls.Add(this.swipeOutput);
            this.Controls.Add(this.resolutionCombo);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.cameraCombo);
            this.Controls.Add(this.startButton);
            this.Name = "FormMain";
            this.Text = "Hand Detection";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rawCameraFrame)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.ComboBox cameraCombo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox rawCameraFrame;
        private System.Windows.Forms.ComboBox resolutionCombo;
        private System.Windows.Forms.Label swipeOutput;
    }
}

