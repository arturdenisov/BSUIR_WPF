using System;
using System.Windows.Forms;

namespace Lab1_WinForms
{
    class MainForm : Form
    {
        Label labelX = new Label();
        Label labelY = new Label();
        Label labelZ = new Label();
        TextBox tbX = new TextBox();
        TextBox tbY = new TextBox();
        TextBox tbZ = new TextBox();
        Button button = new Button();
        TextBox tbResult = new TextBox();

        public MainForm()
        {
            InitComponent();
        }

        private void InitComponent()
        {
            Width = 400;
            Height = 350;
            Text = "Денисов А.Ю., СВПП, лабораторная 1.";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;

            labelX.Top = 30;
            labelX.Left = 20;
            labelX.Width = 30;
            labelX.Text = "X=";

            labelY.Top = 60;
            labelY.Left = 20;
            labelY.Width = 30;
            labelY.Text = "Y=";

            labelZ.Top = 90;
            labelZ.Left = 20;
            labelZ.Width = 30;
            labelZ.Text = "Z=";

            Controls.AddRange(new[] { labelX, labelY, labelZ });

            tbX.Top = 30;
            tbX.Left = 50;
            tbY.Top = 60;
            tbY.Left = 50;
            tbZ.Top = 90;
            tbZ.Left = 50;

            Controls.AddRange(new[] { tbX, tbY, tbZ });
            button.Top = 120;
            button.Left = 20;
            button.Text = "Calculate";

            button.Click += new System.EventHandler(button_Click);
            Controls.Add(button);

            tbResult.Top = 160;
            tbResult.Left = 20;
            tbResult.ReadOnly = true;
            tbResult.Multiline = true;
            tbResult.Width = 200;
            tbResult.Height = 100;

            Controls.Add(tbResult);
        }

        private void button_Click(object sender, System.EventArgs e)
        {
            try
            {
                double x = double.Parse(tbX.Text);
                double y = double.Parse(tbY.Text);
                double z = double.Parse(tbZ.Text);
                Calculator calculator = new Calculator(x, y, z);
                tbResult.Text = "Lab1" + Environment.NewLine;
                tbResult.Text += string.Format("t= {0:0.00000e000}", calculator.DoCalculation());
            }
            catch
            {
                MessageBox.Show("Input data again!");
            }
        }
    }
}
