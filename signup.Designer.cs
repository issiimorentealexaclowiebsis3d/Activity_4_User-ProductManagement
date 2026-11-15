namespace morente_sample
{
    partial class Signup
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
            label1 = new Label();
            label5 = new Label();
            button1 = new Button();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            label2 = new Label();
            panel1 = new Panel();
            label3 = new Label();
            linkLabel1 = new LinkLabel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.CausesValidation = false;
            label1.Font = new Font("Showcard Gothic", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(128, 64, 64);
            label1.Location = new Point(80, 35);
            label1.Name = "label1";
            label1.Size = new Size(514, 37);
            label1.TabIndex = 5;
            label1.Text = "INVENTORY MANAGEMENT SYSTEM";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10.8F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.Black;
            label5.Location = new Point(257, 72);
            label5.Name = "label5";
            label5.Size = new Size(172, 25);
            label5.TabIndex = 9;
            label5.Text = "Create your Account";
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(128, 64, 64);
            button1.ForeColor = SystemColors.ButtonFace;
            button1.Location = new Point(257, 387);
            button1.Name = "button1";
            button1.Size = new Size(187, 44);
            button1.TabIndex = 14;
            button1.Text = "Register";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(228, 197);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "Username:";
            textBox1.Size = new Size(246, 27);
            textBox1.TabIndex = 15;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(228, 139);
            textBox2.Name = "textBox2";
            textBox2.PlaceholderText = "Fullname:";
            textBox2.Size = new Size(246, 27);
            textBox2.TabIndex = 16;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(228, 255);
            textBox3.Name = "textBox3";
            textBox3.PlaceholderText = "Password:";
            textBox3.Size = new Size(246, 27);
            textBox3.TabIndex = 17;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(228, 313);
            textBox4.Name = "textBox4";
            textBox4.PlaceholderText = "Re-enter Password:";
            textBox4.Size = new Size(246, 27);
            textBox4.TabIndex = 18;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(146, 469);
            label2.Name = "label2";
            label2.Size = new Size(0, 20);
            label2.TabIndex = 19;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(linkLabel1);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(textBox4);
            panel1.Controls.Add(textBox3);
            panel1.Controls.Add(textBox2);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(175, 43);
            panel1.Name = "panel1";
            panel1.Size = new Size(694, 541);
            panel1.TabIndex = 0;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10.2F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.Black;
            label3.Location = new Point(201, 450);
            label3.Name = "label3";
            label3.Size = new Size(197, 23);
            label3.TabIndex = 20;
            label3.Text = "Already have an account?";
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Font = new Font("Segoe UI", 10.2F, FontStyle.Italic, GraphicsUnit.Point, 0);
            linkLabel1.Location = new Point(392, 450);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(90, 23);
            linkLabel1.TabIndex = 21;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "Login here.";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // Signup
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(75, 46, 33);
            ClientSize = new Size(1029, 631);
            Controls.Add(panel1);
            ForeColor = Color.Coral;
            ImeMode = ImeMode.HangulFull;
            KeyPreview = true;
            Name = "Signup";
            Text = "Form3";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private Label label5;
        private Button button1;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private Label label2;
        private Panel panel1;
        private Label label3;
        private LinkLabel linkLabel1;
    }
}