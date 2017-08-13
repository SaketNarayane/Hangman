using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace Hangman
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string word="";
        int amount = 0;
        List<Label> labels= new List<Label>();
        enum BodyParts
        {
            head,
            eyes,
            mouth,
            body,
            r_arm,
            l_arm,
            r_leg,
            l_leg
        }
        
        void drawbodyparts(BodyParts bp)
        {
            Graphics g = panel1.CreateGraphics();
            Pen p = new Pen(Color.LawnGreen,2);
            if(bp==BodyParts.head)
            {
                g.DrawEllipse(p,40, 50, 40, 40);
            }
            else if(bp==BodyParts.eyes)
            {
                SolidBrush s = new SolidBrush(Color.Blue);
                g.FillEllipse(s, 50, 60, 5, 5);
                g.FillEllipse(s, 63, 60, 5, 5);
            }
            else if (bp == BodyParts.mouth)
            {
                g.DrawArc(p, 50, 60, 20, 20, 45, 90);
            }
            else if (bp == BodyParts.body)
            {
                g.DrawLine(p, new Point(60, 90), new Point(60, 170));
            }
            else if (bp == BodyParts.l_arm)
            {
                g.DrawLine(p, new Point(60, 100), new Point(30, 85));
            }
            else if (bp == BodyParts.r_arm)
            {
                g.DrawLine(p, new Point(60, 100), new Point(90, 85));
            }
            else if (bp == BodyParts.l_leg)
            {
                g.DrawLine(p, new Point(60, 170), new Point(30, 190));
            }
            else if (bp == BodyParts.r_leg)
            {
                g.DrawLine(p, new Point(60, 170), new Point(90, 190));
            }
        }
        void drawHangpost()
        {
            Graphics g = panel1.CreateGraphics();
            Pen p = new Pen(Color.Brown, 10);
            g.DrawLine(p, new Point(130, 218), new Point(130, 5));
            g.DrawLine(p, new Point(135, 5), new Point(65, 5));
            g.DrawLine(p, new Point(60, 0), new Point(60, 50));
            /*
            drawbodyparts(BodyParts.head);
            drawbodyparts(BodyParts.eyes);
            drawbodyparts(BodyParts.mouth);
            drawbodyparts(BodyParts.body);
            drawbodyparts(BodyParts.l_arm);
            drawbodyparts(BodyParts.r_arm);
            drawbodyparts(BodyParts.l_leg);
            drawbodyparts(BodyParts.r_leg);
            */
        }

        string getrandomword()
        {
            string wordslist = Properties.Resources.wordlist;
            string[] words = wordslist.Split('\n');
            Random r = new Random();
            return words[r.Next(0, words.Length - 1)];
        }
        void makelabels()
        {
            word = getrandomword();
            word += "\n";
            MessageBox.Show(word);
            char[] chars = word.ToCharArray();
            int between = 335 / chars.Length - 1;
            for(int i=0; i <chars.Length -1 ; i++)
            {
                labels.Add(new Label());
                labels[i].Location = new Point((i*between)+10,80);
                labels[i].Text= "_";
                labels[i].Parent = groupBox2;
                labels[i].BringToFront();
                labels[i].CreateControl();
            }
            label1.Text = "Word length : " + (chars.Length-1).ToString();
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            drawHangpost();
            makelabels();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            char c = textBox2.Text.ToLower().ToCharArray()[0];
            if(!char.IsLetter(c)||textBox2.Text.Length>1)
            {
                MessageBox.Show("only single letters are accepted as input", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (word.Contains(c))
            {
                for(int i=0 ; i<word.Length;i++)
                {
                    if (word[i] == c)
                    {
                        labels[i].Text = ""+c;
                    }
                }
                label2.ForeColor = Color.Green;
                label2.Text = "status : letter you have subbmitted is present";
                foreach(Label l in labels)
                {
                    if( l.Text=="_")
                    {
                        return;
                    }
                }
                MessageBox.Show("You have won!");
                reset();
            }
            else
            {
                label2.ForeColor = Color.Red;
                label2.Text = "status : letter you have subbmitted is not present";
                label3.Text += " " + c;
                drawbodyparts((BodyParts)amount);
                amount++;

            }
            textBox2.Text = "";
            if(amount==8)
            {
                MessageBox.Show("You Lost! the word was "+word);
                reset();
            }
        }

        void reset()
        {
            Graphics g = panel1.CreateGraphics();
            g.Clear(panel1.BackColor);
            word = getrandomword();
            makelabels();
            drawHangpost();
            label3.Text = " Missed letters : ";
            label2.Text = " status : ";
            textBox2.Text = "";
        }
        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if( textBox1.Text==word)
            {
                label2.ForeColor = Color.Green;
                label2.Text = "status : word that you have subbmitted is right!";
                MessageBox.Show("You have won!");
                reset();
            }
            else
            {
                label2.ForeColor = Color.Red;
                label2.Text = "status : word that you have subbmitted is wrong!";
                drawbodyparts((BodyParts)amount);
                amount++;
                if (amount == 8)
                {
                    MessageBox.Show("You Lost! the word was " + word);
                    reset();
                }
            }
        }
       
    }
}
