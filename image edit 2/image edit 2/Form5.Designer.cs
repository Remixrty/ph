
namespace image_edit_2
{
	partial class Form5
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
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.button4 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button7 = new System.Windows.Forms.Button();
			this.button9 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pictureBox1.Location = new System.Drawing.Point(12, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(360, 360);
			this.pictureBox1.TabIndex = 14;
			this.pictureBox1.TabStop = false;
			// 
			// pictureBox2
			// 
			this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pictureBox2.Location = new System.Drawing.Point(479, 12);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(360, 360);
			this.pictureBox2.TabIndex = 13;
			this.pictureBox2.TabStop = false;
			// 
			// button4
			// 
			this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button4.Location = new System.Drawing.Point(128, 610);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(110, 26);
			this.button4.TabIndex = 122;
			this.button4.Text = "Сохранить";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// button2
			// 
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button2.Location = new System.Drawing.Point(12, 610);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(110, 26);
			this.button2.TabIndex = 121;
			this.button2.Text = "Открыть";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button1
			// 
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Location = new System.Drawing.Point(12, 504);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(110, 26);
			this.button1.TabIndex = 123;
			this.button1.Text = "Матричная фильтрация";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button3
			// 
			this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button3.Location = new System.Drawing.Point(143, 504);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(110, 26);
			this.button3.TabIndex = 124;
			this.button3.Text = "Медианная";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// richTextBox1
			// 
			this.richTextBox1.Location = new System.Drawing.Point(12, 403);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new System.Drawing.Size(110, 95);
			this.richTextBox1.TabIndex = 126;
			this.richTextBox1.Text = "0 0 0\n0 0 0\n0 0 0";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(143, 478);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(110, 20);
			this.textBox1.TabIndex = 127;
			this.textBox1.Text = "5";
			// 
			// button7
			// 
			this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button7.Location = new System.Drawing.Point(729, 610);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(110, 26);
			this.button7.TabIndex = 129;
			this.button7.Text = "Выход";
			this.button7.UseVisualStyleBackColor = true;
			this.button7.Click += new System.EventHandler(this.button7_Click);
			// 
			// button9
			// 
			this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button9.Location = new System.Drawing.Point(601, 610);
			this.button9.Name = "button9";
			this.button9.Size = new System.Drawing.Size(110, 26);
			this.button9.TabIndex = 128;
			this.button9.Text = "Назад";
			this.button9.UseVisualStyleBackColor = true;
			this.button9.Click += new System.EventHandler(this.button9_Click);
			// 
			// Form5
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.WindowFrame;
			this.ClientSize = new System.Drawing.Size(851, 648);
			this.Controls.Add(this.button7);
			this.Controls.Add(this.button9);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.richTextBox1);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.pictureBox2);
			this.Name = "Form5";
			this.Load += new System.EventHandler(this.Form5_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.Button button9;
	}
}