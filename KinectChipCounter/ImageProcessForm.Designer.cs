namespace KinectChipCounter
{
    partial class ImageProcessForm
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
            this.originalImageBox = new Emgu.CV.UI.ImageBox();
            this.triangleRectangleImageBox = new Emgu.CV.UI.ImageBox();
            this.circleImageBox = new Emgu.CV.UI.ImageBox();
            this.lineImageBox = new Emgu.CV.UI.ImageBox();
            this.txtImageFile = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.originalImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.triangleRectangleImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.circleImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lineImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // originalImageBox
            // 
            this.originalImageBox.Location = new System.Drawing.Point(12, 41);
            this.originalImageBox.Name = "originalImageBox";
            this.originalImageBox.Size = new System.Drawing.Size(250, 250);
            this.originalImageBox.TabIndex = 2;
            this.originalImageBox.TabStop = false;
            // 
            // triangleRectangleImageBox
            // 
            this.triangleRectangleImageBox.Location = new System.Drawing.Point(268, 41);
            this.triangleRectangleImageBox.Name = "triangleRectangleImageBox";
            this.triangleRectangleImageBox.Size = new System.Drawing.Size(250, 250);
            this.triangleRectangleImageBox.TabIndex = 3;
            this.triangleRectangleImageBox.TabStop = false;
            // 
            // circleImageBox
            // 
            this.circleImageBox.Location = new System.Drawing.Point(12, 297);
            this.circleImageBox.Name = "circleImageBox";
            this.circleImageBox.Size = new System.Drawing.Size(250, 250);
            this.circleImageBox.TabIndex = 4;
            this.circleImageBox.TabStop = false;
            // 
            // lineImageBox
            // 
            this.lineImageBox.Location = new System.Drawing.Point(268, 297);
            this.lineImageBox.Name = "lineImageBox";
            this.lineImageBox.Size = new System.Drawing.Size(250, 250);
            this.lineImageBox.TabIndex = 5;
            this.lineImageBox.TabStop = false;
            // 
            // txtImageFile
            // 
            this.txtImageFile.Location = new System.Drawing.Point(13, 13);
            this.txtImageFile.Name = "txtImageFile";
            this.txtImageFile.Size = new System.Drawing.Size(100, 22);
            this.txtImageFile.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(127, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(71, 20);
            this.button1.TabIndex = 7;
            this.button1.Text = "Select...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ImageProcessForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 553);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtImageFile);
            this.Controls.Add(this.lineImageBox);
            this.Controls.Add(this.circleImageBox);
            this.Controls.Add(this.triangleRectangleImageBox);
            this.Controls.Add(this.originalImageBox);
            this.Name = "ImageProcessForm";
            this.Text = "ImageProcessForm";
            ((System.ComponentModel.ISupportInitialize)(this.originalImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.triangleRectangleImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.circleImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lineImageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Emgu.CV.UI.ImageBox originalImageBox;
        private Emgu.CV.UI.ImageBox triangleRectangleImageBox;
        private Emgu.CV.UI.ImageBox circleImageBox;
        private Emgu.CV.UI.ImageBox lineImageBox;
        private System.Windows.Forms.TextBox txtImageFile;
        private System.Windows.Forms.Button button1;
    }
}