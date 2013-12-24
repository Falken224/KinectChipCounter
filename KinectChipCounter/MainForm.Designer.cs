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
            this.btnTiltUp = new System.Windows.Forms.Button();
            this.btnTiltDown = new System.Windows.Forms.Button();
            this.lblAngle = new System.Windows.Forms.Label();
            this.imageBox1 = new Emgu.CV.UI.ImageBox();
            this.btnCapture = new System.Windows.Forms.Button();
            this.imageBox2 = new Emgu.CV.UI.ImageBox();
            this.analysisImgBox = new Emgu.CV.UI.ImageBox();
            this.hueGraph = new Emgu.CV.UI.ImageBox();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.analysisImgBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hueGraph)).BeginInit();
            this.SuspendLayout();
            // 
            // btnTiltUp
            // 
            this.btnTiltUp.Location = new System.Drawing.Point(12, 12);
            this.btnTiltUp.Name = "btnTiltUp";
            this.btnTiltUp.Size = new System.Drawing.Size(89, 37);
            this.btnTiltUp.TabIndex = 0;
            this.btnTiltUp.Text = "Tilt Up";
            this.btnTiltUp.UseVisualStyleBackColor = true;
            this.btnTiltUp.Click += new System.EventHandler(this.btnTiltUp_Click);
            // 
            // btnTiltDown
            // 
            this.btnTiltDown.Location = new System.Drawing.Point(15, 72);
            this.btnTiltDown.Name = "btnTiltDown";
            this.btnTiltDown.Size = new System.Drawing.Size(89, 37);
            this.btnTiltDown.TabIndex = 1;
            this.btnTiltDown.Text = "Tilt Down";
            this.btnTiltDown.UseVisualStyleBackColor = true;
            this.btnTiltDown.Click += new System.EventHandler(this.btnTiltDown_Click);
            // 
            // lblAngle
            // 
            this.lblAngle.AutoSize = true;
            this.lblAngle.Location = new System.Drawing.Point(12, 52);
            this.lblAngle.Name = "lblAngle";
            this.lblAngle.Size = new System.Drawing.Size(52, 17);
            this.lblAngle.TabIndex = 2;
            this.lblAngle.Text = "Angle: ";
            // 
            // imageBox1
            // 
            this.imageBox1.Location = new System.Drawing.Point(108, 13);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(640, 480);
            this.imageBox1.TabIndex = 2;
            this.imageBox1.TabStop = false;
            // 
            // btnCapture
            // 
            this.btnCapture.Location = new System.Drawing.Point(12, 284);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(86, 30);
            this.btnCapture.TabIndex = 3;
            this.btnCapture.Text = "Capture";
            this.btnCapture.UseVisualStyleBackColor = true;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // imageBox2
            // 
            this.imageBox2.Location = new System.Drawing.Point(754, 12);
            this.imageBox2.Name = "imageBox2";
            this.imageBox2.Size = new System.Drawing.Size(640, 480);
            this.imageBox2.TabIndex = 4;
            this.imageBox2.TabStop = false;
            // 
            // analysisImgBox
            // 
            this.analysisImgBox.Location = new System.Drawing.Point(108, 499);
            this.analysisImgBox.Name = "analysisImgBox";
            this.analysisImgBox.Size = new System.Drawing.Size(640, 480);
            this.analysisImgBox.TabIndex = 7;
            this.analysisImgBox.TabStop = false;
            // 
            // hueGraph
            // 
            this.hueGraph.Location = new System.Drawing.Point(754, 501);
            this.hueGraph.Name = "hueGraph";
            this.hueGraph.Size = new System.Drawing.Size(640, 480);
            this.hueGraph.TabIndex = 8;
            this.hueGraph.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1409, 993);
            this.Controls.Add(this.hueGraph);
            this.Controls.Add(this.analysisImgBox);
            this.Controls.Add(this.imageBox2);
            this.Controls.Add(this.btnCapture);
            this.Controls.Add(this.imageBox1);
            this.Controls.Add(this.lblAngle);
            this.Controls.Add(this.btnTiltDown);
            this.Controls.Add(this.btnTiltUp);
            this.Name = "MainForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.analysisImgBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hueGraph)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTiltUp;
        private System.Windows.Forms.Button btnTiltDown;
        private System.Windows.Forms.Label lblAngle;
        private Emgu.CV.UI.ImageBox imageBox1;
        private System.Windows.Forms.Button btnCapture;
        private Emgu.CV.UI.ImageBox imageBox2;
        private Emgu.CV.UI.ImageBox analysisImgBox;
        private Emgu.CV.UI.ImageBox hueGraph;

    }
}