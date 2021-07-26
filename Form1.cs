/*
Miles Weber
Period 8
4/17/21
This is my own work MBW
This program is a bingo sheet that lets users randomize correct values and have a free space in the middle. When the user gets
a bingo the sheet detects it automatically. The user is granted the ability to reset the board at any point

*/


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BingoSheet
{
    public partial class Form1 : Form
    {
        #region Vars
        List<List<Button>> grid = new List<List<Button>>();
        Random rand = new Random();
        bool bingo = false;
        #endregion
        #region Events
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //add event handlers to all buttons
            foreach (var button in Controls.OfType<Button>())
            {
                button.Click += Button_Click;
            }
            //put all the buttons on the grid into the grid list
            FillGrid();

            //remove the text from every button except free space
            ResetGrid();

            //set values
            SetButtonValues();
           
        }

        //when any button is clicked that is a bingo button this runs
        private void Button_Click(object sender, EventArgs e)
        {
            //run if the button is associated as a grid button
            if (((Button)sender).Name.Length == 2)
            {
                if (((Button)sender).BackColor == Color.Lime)
                    ((Button)sender).BackColor = SystemColors.Control;
                else
                    ((Button)sender).BackColor = Color.Lime;
                //check bingo
                if (bingo == false)
                {
                    if (HorizontalBingo() || VerticalBingo() || DiagonalBingo() || PostageBingo())
                    {
                        TitleLabel.Text = "You Have Bingo!";

                        //output with voice
                        SpeechSynthesizer tts = new SpeechSynthesizer();
                        
                        tts.Speak("Bingo!");
                        
                        tts.Dispose();

                        bingo = true;
                    }
                }
            }
            
        }
        private void ResetBoard_Click(object sender, EventArgs e)
        {
            //reset the entire board
            //remove the text from every button except free space
            ResetGrid();

            //set values
            SetButtonValues();

            //set bingo to false
            bingo = false;

            //reset bingo text
            TitleLabel.Text = "B I N G O";
        }
        #endregion
        #region methods
        //adds all buttons to the grid
        public void FillGrid()
        {
            //create columns
            List<Button> bColumn = new List<Button>();
            List<Button> iColumn = new List<Button>();
            List<Button> nColumn = new List<Button>();
            List<Button> gColumn = new List<Button>();
            List<Button> oColumn = new List<Button>();

            //fill collumns
            bColumn.Add(B1);
            bColumn.Add(B2);
            bColumn.Add(B3);
            bColumn.Add(B4);
            bColumn.Add(B5);

            iColumn.Add(I1);
            iColumn.Add(I2);
            iColumn.Add(I3);
            iColumn.Add(I4);
            iColumn.Add(I5);

            nColumn.Add(N1);
            nColumn.Add(N2);
            nColumn.Add(FreeSpace);
            nColumn.Add(N3);
            nColumn.Add(N4);

            gColumn.Add(G1);
            gColumn.Add(G2);
            gColumn.Add(G3);
            gColumn.Add(G4);
            gColumn.Add(G5);

            oColumn.Add(O1);
            oColumn.Add(O2);
            oColumn.Add(O3);
            oColumn.Add(O4);
            oColumn.Add(O5);

            //add columns to 2d list

            grid.Add(bColumn);
            grid.Add(iColumn);
            grid.Add(nColumn);
            grid.Add(gColumn);
            grid.Add(oColumn);
        }

        //resets all the values on the board and changes the colors of every button to normal
        public void ResetGrid()
        {

            foreach (List<Button> column in grid)
            {
                foreach (Button button in column)
                {

                    if (!(button.Name.Equals("freespace", StringComparison.OrdinalIgnoreCase)))
                    {
                        button.Text = "";
                        button.BackColor = SystemColors.Control;
                    }
                }
            }
        }

        //labels all buttons except the free space with new values
        public void SetButtonValues()
        {
            ResetGrid();
            //keeps track of values that were already picked
            List<int> rolledValues = new List<int>();
            int item = 0;
            foreach (List<Button> column in grid)
            {
                //clear after each column
                rolledValues.Clear();
                foreach (Button button in column)
                {
                    switch (button.Name.Substring(0, 1))
                    {
                        case "B":
                            item = RollValue(rolledValues, 1, 16);
                            rolledValues.Add(item);
                            button.Text = String.Concat("B", item);
                            break;
                        case "I":
                            item = RollValue(rolledValues, 16, 31);
                            rolledValues.Add(item);
                            button.Text = String.Concat("I", item);
                            break;
                        case "N":
                            item = RollValue(rolledValues, 31, 46);
                            rolledValues.Add(item);
                            button.Text = String.Concat("N", item);
                            break;
                        case "G":
                            item = RollValue(rolledValues, 46, 61);
                            rolledValues.Add(item);
                            button.Text = String.Concat("G", item);
                            break;
                        case "O":
                            item = RollValue(rolledValues, 61, 76);
                            rolledValues.Add(item);
                            button.Text = String.Concat("O", item);
                            break;

                    }
                }
            }
        }
        
        //rolls a suitable value for a grid space
        public int RollValue(List<int> rolledValues, int upperBound, int lowerBound)
        {
            int result = 0;
            bool valid = false;
            //repeat until valid number is rolled
            while(!valid)
            {
                result = rand.Next(upperBound, lowerBound);
                valid = true;
                foreach(int item in rolledValues)
                {
                    if (item == result)
                        valid = false;
                }
            }
            return result;
        }
        
        //checks all horizontals for a w
        public bool HorizontalBingo()
        {
            int count = 0;
            for(int x = 0; x < 5; x++)
            {
                for(int y = 0; y < 5; y++)
                {                    
                    //if not the free space
                    if (x != 2 || y != 2)
                    {
                        if (grid[y][x].BackColor == Color.Lime)
                        {
                            count++;
                        }
                    }
                    else
                        count++;
                }
                if (count == 5)
                    return true;
                count = 0;
            }
            return false;
        }
        //checks all verticals for a w
        public bool VerticalBingo()
        {
            int count = 0;
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    //if not the free space
                    if (x != 2 || y != 2)
                    {
                        if (grid[x][y].BackColor == Color.Lime)
                        {
                            count++;
                        }
                    }
                    else
                        count++;
                }
                if (count == 5)
                    return true;
                count = 0;
            }
            return false;
        }
        //checks both diagonals for a w
        public bool DiagonalBingo()
        {
            if (grid[4][0].BackColor == Color.Lime)
            {
                if (grid[3][1].BackColor == Color.Lime)
                    if (grid[1][3].BackColor == Color.Lime)
                        if (grid[0][4].BackColor == Color.Lime)
                            return true;
            }
            else if (grid[0][0].BackColor == Color.Lime)
            {
                if (grid[1][1].BackColor == Color.Lime)
                    if (grid[3][3].BackColor == Color.Lime)
                        if (grid[4][4].BackColor == Color.Lime)
                            return true;
            }
                    return false;
        }
        //check every 4 corners for bingo
        public bool PostageBingo()
        {
            if (grid[3][0].BackColor == Color.Lime)
            {
                if (grid[4][0].BackColor == Color.Lime)
                    if (grid[3][1].BackColor == Color.Lime)
                        if (grid[4][1].BackColor == Color.Lime)
                            return true;
            }           
            return false;
        }

        #endregion
       
       
    }
}
