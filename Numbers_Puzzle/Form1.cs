using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Numbers_Puzzle
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
               

        Button _GhostButton;
        byte[] _arrNumbers = new byte[]
        {
            1, 2, 3, 4,
            5, 6, 7,
            8
        };

        class clsStats
        {
            public int CurrentTime;
            public int Moves;

            public clsStats(int currentTime, int moves)
            {
                CurrentTime = currentTime;
                Moves = moves;
            }
        }

        clsStats Stats = new clsStats(0, 0);

        void ShuffleArray(byte[] inputArray)
        {
            Random Rnd = new Random();
            for (int i = inputArray.Length - 1; i > 0; i--)
            {
                int randomIndex = Rnd.Next(0, i + 1);

                byte temp = inputArray[i];
                inputArray[i] = inputArray[randomIndex];
                inputArray[randomIndex] = temp;
            }

        }

        void SwapButtons(Button GhostButton, Button B)
        {
            GhostButton.Enabled = true;
            GhostButton.Text = B.Text;
            GhostButton.BackColor = Color.FromArgb(224, 224, 224);

            B.Enabled = false;
            B.Text = "";
            B.BackColor = Color.Silver;
            _GhostButton = B;
        }

        void DisableOrEnableAllButtons(Control parent, bool Enable)
        {
            foreach (Control c in parent.Controls)
            {
                Button tb = c as Button;

                if (tb != null)
                {
                    tb.Enabled = Enable;
                }

            }
        }

        private void FillButtonsWithRndNums(Control parent, byte[] _arrNumbers)
        {      
            ShuffleArray(_arrNumbers);
            byte counter = 0;

            foreach (Control c in parent.Controls)
            {
                Button tb = c as Button;

                if (tb != null && tb.Enabled == true)
                {
                    tb.Text = _arrNumbers[counter].ToString();
                    counter++;
                }
                
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DisableOrEnableAllButtons(gbNumbers, false);     
        }

        bool ValidateSwapping(Button GhostButton, Button B)
        {
            byte ButtonNumber = Convert.ToByte(B.Name.Substring(3, B.Name.Length - 3));
            int GhostButtonNumber = Convert.ToByte(GhostButton.Name.Substring(3, GhostButton.Name.Length - 3));

            if ((ButtonNumber + 1) == GhostButtonNumber || (ButtonNumber - 1) == GhostButtonNumber)
            {
                 return true;
            }

            if ((ButtonNumber + 3) == GhostButtonNumber || (ButtonNumber - 3) == GhostButtonNumber)
            {
                return true;
            }

            return false;

        }

        void GhostRndButton(Control parent)
        {
            Random rnd = new Random();
            int rndButton = rnd.Next(1,8);

            foreach (Control c in parent.Controls)
            {
                Button tb = c as Button;

                if (tb != null && tb.Tag.ToString() == rndButton.ToString() && tb.Enabled == true)
                {
                    GhostButton(tb);
                    _GhostButton = tb;
                }

            }
        }

        void Perform_Click(Button B)
        {
            if (!ValidateSwapping(_GhostButton, B))         
                return;

            SwapButtons(_GhostButton, B);

            Stats.Moves++;
            lblMoves.Text = Stats.Moves.ToString();

            if (EndGame(gbNumbers))
            {
                timer1.Enabled = false;
                DisableOrEnableAllButtons(gbNumbers, false);            
                MessageBox.Show("Game Over");
            }
        }

        bool EndGame(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                Button tb = c as Button;

                if (tb.Enabled == true)
                {
                    if (tb == null || (tb.Tag.ToString() != tb.Text.ToString()))
                    {
                        return false;
                    }
                }

            }
        
            return true;
        }

        void GhostButton(Button B)
        {
            B.Enabled = false;
            B.Text = "";
            B.BackColor = Color.Silver;
        }

        void ResettButton(Button B)
        {
            B.Enabled = true;
            B.Text = "0";
            B.BackColor = Color.FromArgb(224, 224, 224);
        }

        void ResetGame(Control parent)
        {
            _GhostButton = null;
            Stats = new clsStats(0, 0);

            lblMoves.Text = "0";
            lblTime.Text = "0";


            foreach (Control c in parent.Controls)
            {
                Button tb = c as Button;

                if (tb != null)
                {
                    ResettButton(tb);
                }

            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            Perform_Click((Button)sender);
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            ResetGame(gbNumbers);
            DisableOrEnableAllButtons(gbNumbers, true);
            GhostRndButton(gbNumbers);         
            FillButtonsWithRndNums(gbNumbers, _arrNumbers);
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Stats.CurrentTime++;
            lblTime.Text = Stats.CurrentTime.ToString();
        }

        private void btnHowToPlay_Click(object sender, EventArgs e)
        {
            How_To_Play frm = new How_To_Play();
            frm.ShowDialog();
        }
    }
}
