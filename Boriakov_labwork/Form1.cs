using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Boriakov_labwork.Analyzers;

namespace Boriakov_labwork
{
    //   VAR A  :char; D :ARRAY [  2..12 , 22 .. 33  ] OF word ; e , t :integer; yui:real;
    //   VAR A  :CHAR ; D :ARRAY [  2..12 , 22 .. 33  ] OF WORD ; e , t :INTEGER;  yui:REAL ;
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox7.Clear();

            label1.Text = "Список ошибок: \n";
            string error = "";

            Mainanalyzer mainanalyzer = new Mainanalyzer(textBox1.Text);
            mainanalyzer.MainAnalyze(ref error);


            textBox1.SelectionStart = mainanalyzer.Index;

            textBox1.Focus();


            if (mainanalyzer.CurrentState != StateMain.E)
            {
                label1.Text += "Ошибок не найдено";

                textBox2.Clear();
            }
            else
            {
                label1.Text += error;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string error = "";

            Mainanalyzer mainanalyzer = new Mainanalyzer(textBox1.Text);
            mainanalyzer.MainAnalyze(ref error);

            if (mainanalyzer.CurrentState != StateMain.E)
            {
                textBox2.Clear();
                textBox2.Text = "ИМЯ              ТИП ПЕРЕМЕННОЙ              ПАМЯТЬ" + '\r' + '\n';

                for (int i = 0; i < mainanalyzer.Table.Count; i++)
                {
                    string table = mainanalyzer.Table[i];
                    textBox2.Text += table + '\r' + '\n';
                }
            }
            else
            {
                textBox2.Clear();
                label1.Text = "Ошибки в анализе языка\nИссправьте ошибки";
            }

            if (mainanalyzer.CurrentState != StateMain.E)
            {
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox7.Clear();

                for (int i = 0; i < mainanalyzer.Name.Count; i++)
                {
                    textBox7.Text += mainanalyzer.Name[i] + '\r' + '\n';
                    textBox3.Text += mainanalyzer.Type[i] + '\r' + '\n';
                    textBox4.Text += mainanalyzer.Dimension[i] + '\r' + '\n';
                    textBox5.Text += mainanalyzer.Volume[i] + '\r' + '\n';
                }
            }
            else
            {
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox7.Clear();
                label1.Text = "Ошибки в анализе языка\nИссправьте ошибки";
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
