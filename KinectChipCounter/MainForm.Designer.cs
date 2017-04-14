namespace KinectChipCounter
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.imageBox1 = new Emgu.CV.UI.ImageBox();
            this.btnCapture = new System.Windows.Forms.Button();
            this.imageBox2 = new Emgu.CV.UI.ImageBox();
            this.analysisImgBox = new Emgu.CV.UI.ImageBox();
            this.hueGraph = new Emgu.CV.UI.ImageBox();
            this.btnWhite = new System.Windows.Forms.Button();
            this.btnRed = new System.Windows.Forms.Button();
            this.btnBlue = new System.Windows.Forms.Button();
            this.btnGreen = new System.Windows.Forms.Button();
            this.btnBlack = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.analysisImgBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hueGraph)).BeginInit();
            this.SuspendLayout();
            // 
            // imageBox1
            // 
            this.imageBox1.Location = new System.Drawing.Point(81, 11);
            this.imageBox1.Margin = new System.Windows.Forms.Padding(2);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(480, 390);
            this.imageBox1.TabIndex = 2;
            this.imageBox1.TabStop = false;
            // 
            // btnCapture
            // 
            this.btnCapture.Location = new System.Drawing.Point(9, 231);
            this.btnCapture.Margin = new System.Windows.Forms.Padding(2);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(64, 24);
            this.btnCapture.TabIndex = 3;
            this.btnCapture.Text = "Capture";
            this.btnCapture.UseVisualStyleBackColor = true;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // imageBox2
            // 
            this.imageBox2.Location = new System.Drawing.Point(566, 10);
            this.imageBox2.Margin = new System.Windows.Forms.Padding(2);
            this.imageBox2.Name = "imageBox2";
            this.imageBox2.Size = new System.Drawing.Size(480, 390);
            this.imageBox2.TabIndex = 4;
            this.imageBox2.TabStop = false;
            // 
            // analysisImgBox
            // 
            this.analysisImgBox.Location = new System.Drawing.Point(81, 405);
            this.analysisImgBox.Margin = new System.Windows.Forms.Padding(2);
            this.analysisImgBox.Name = "analysisImgBox";
            this.analysisImgBox.Size = new System.Drawing.Size(480, 390);
            this.analysisImgBox.TabIndex = 7;
            this.analysisImgBox.TabStop = false;
            // 
            // hueGraph
            // 
            this.hueGraph.Location = new System.Drawing.Point(566, 407);
            this.hueGraph.Margin = new System.Windows.Forms.Padding(2);
            this.hueGraph.Name = "hueGraph";
            this.hueGraph.Size = new System.Drawing.Size(480, 390);
            this.hueGraph.TabIndex = 8;
            this.hueGraph.TabStop = false;
            // 
            // btnWhite
            // 
            this.btnWhite.Location = new System.Drawing.Point(9, 289);
            this.btnWhite.Name = "btnWhite";
            this.btnWhite.Size = new System.Drawing.Size(64, 20);
            this.btnWhite.TabIndex = 9;
            this.btnWhite.Text = "White";
            this.btnWhite.UseVisualStyleBackColor = true;
            this.btnWhite.Click += new System.EventHandler(this.btnWhite_Click);
            // 
            // btnRed
            // 
            this.btnRed.Location = new System.Drawing.Point(9, 315);
            this.btnRed.Name = "btnRed";
            this.btnRed.Size = new System.Drawing.Size(64, 20);
            this.btnRed.TabIndex = 10;
            this.btnRed.Text = "Red";
            this.btnRed.UseVisualStyleBackColor = true;
            this.btnRed.Click += new System.EventHandler(this.btnRed_Click);
            // 
            // btnBlue
            // 
            this.btnBlue.Location = new System.Drawing.Point(9, 341);
            this.btnBlue.Name = "btnBlue";
            this.btnBlue.Size = new System.Drawing.Size(64, 20);
            this.btnBlue.TabIndex = 11;
            this.btnBlue.Text = "Blue";
            this.btnBlue.UseVisualStyleBackColor = true;
            this.btnBlue.Click += new System.EventHandler(this.btnBlue_Click);
            // 
            // btnGreen
            // 
            this.btnGreen.Location = new System.Drawing.Point(9, 367);
            this.btnGreen.Name = "btnGreen";
            this.btnGreen.Size = new System.Drawing.Size(64, 20);
            this.btnGreen.TabIndex = 12;
            this.btnGreen.Text = "Green";
            this.btnGreen.UseVisualStyleBackColor = true;
            this.btnGreen.Click += new System.EventHandler(this.btnGreen_Click);
            // 
            // btnBlack
            // 
            this.btnBlack.Location = new System.Drawing.Point(9, 393);
            this.btnBlack.Name = "btnBlack";
            this.btnBlack.Size = new System.Drawing.Size(64, 20);
            this.btnBlack.TabIndex = 13;
            this.btnBlack.Text = "Black";
            this.btnBlack.UseVisualStyleBackColor = true;
            this.btnBlack.Click += new System.EventHandler(this.btnBlack_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(9, 452);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(64, 20);
            this.button6.TabIndex = 14;
            this.button6.Text = "Retrain";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 807);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.btnBlack);
            this.Controls.Add(this.btnGreen);
            this.Controls.Add(this.btnBlue);
            this.Controls.Add(this.btnRed);
            this.Controls.Add(this.btnWhite);
            this.Controls.Add(this.hueGraph);
            this.Controls.Add(this.analysisImgBox);
            this.Controls.Add(this.imageBox2);
            this.Controls.Add(this.btnCapture);
            this.Controls.Add(this.imageBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.analysisImgBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hueGraph)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Emgu.CV.UI.ImageBox imageBox1;
        private System.Windows.Forms.Button btnCapture;
        private Emgu.CV.UI.ImageBox imageBox2;
        private Emgu.CV.UI.ImageBox analysisImgBox;
        private Emgu.CV.UI.ImageBox hueGraph;
        private System.Windows.Forms.Button btnWhite;
        private System.Windows.Forms.Button btnRed;
        private System.Windows.Forms.Button btnBlue;
        private System.Windows.Forms.Button btnGreen;
        private System.Windows.Forms.Button btnBlack;
        private System.Windows.Forms.Button button6;
    }
}