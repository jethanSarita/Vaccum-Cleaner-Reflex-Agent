using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace VaccumCleanerAgent
{
    public partial class Form1 : Form
    {
        Random rng = new Random();
        Point vaccLoc;
        int locBase;
        bool statA;
        bool statB;
        bool statC;
        bool statD;
        int xDir;
        int yDir;
        int moveSpeed = 5;
        int currLoc;
        string action;

        static Dictionary<string, string> actionTable = new Dictionary<string, string>
        {
            //A
            {"LocA-Clean", "moveRight" },
            {"LocA-Dirty", "vaccum" },
            //B
            {"LocB-Clean", "moveDown" },
            {"LocB-Dirty", "vaccum" },
            //C
            {"LocC-Clean", "moveUp" },
            {"LocC-Dirty", "vaccum" },
            //D
            {"LocD-Clean", "moveLeft" },
            {"LocD-Dirty", "vaccum" },
        };

        public Form1()
        {
            InitializeComponent();

            //fixed form size
            FormBorderStyle = FormBorderStyle.FixedSingle;
            int nonClientWidth = Width - ClientSize.Width;
            int nonClientHeight = Height - ClientSize.Height;
            ClientSize = new Size(500, 500);
            Size = new Size(ClientSize.Width + nonClientWidth, ClientSize.Height + nonClientHeight);

            locBase = rng.Next(1, 5);
            vaccLoc = new Point(70, 70);
            switch (locBase)
            {
                case 1:
                    vaccLoc = new Point(70, 70);
                    break;
                case 2:
                    vaccLoc = new Point(325, 70);
                    break;
                case 3:
                    vaccLoc = new Point(70, 325);
                    break;
                case 4:
                    vaccLoc = new Point(325, 325);
                    break;
            }

            //randomizing location statuses
            statA = randomBool();
            statB = randomBool();
            statC = randomBool();
            statD = randomBool();

            FrameAnimator.Start();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen line = new Pen(Color.White);
            Brush fillWhite = new SolidBrush(Color.White);
            Brush fillYellow = new SolidBrush(Color.Yellow);

            //draw cross
            g.FillRectangle(fillWhite, 245, 0, 10, 500);
            g.FillRectangle(fillWhite, 0, 245, 500, 10);

            //Draw letters
            g.DrawString("A", new Font("Arial", 75), isDirtyColor(statA), 70, 70);
            g.DrawString("B", new Font("Arial", 75), isDirtyColor(statB), 325, 70);
            g.DrawString("C", new Font("Arial", 75), isDirtyColor(statC), 70, 325);
            g.DrawString("D", new Font("Arial", 75), isDirtyColor(statD), 325, 325);

            //Draw vaccum
            g.FillRectangle(fillYellow, vaccLoc.X, vaccLoc.Y, 90, 5);
            g.FillRectangle(fillYellow, vaccLoc.X, vaccLoc.Y, 5, 90);
            g.FillRectangle(fillYellow, vaccLoc.X + 85, vaccLoc.Y, 5, 90);
            g.FillRectangle(fillYellow, vaccLoc.X, vaccLoc.Y + 85, 90, 5);
        }

        private bool randomBool()
        {
            if (rng.Next(0, 2) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Brush isDirtyColor(bool stat)
        {
            if (stat)
            {
                //clean
                return new SolidBrush(Color.Red);
            }
            else
            {
                //not dirty (clean)
                return new SolidBrush(Color.Green);
            }
        }

        private void FrameAnimator_Tick(object sender, EventArgs e)
        {
            currLoc = checkLocation(vaccLoc);
            switch (currLoc)
            {
                case 'A':
                    action = decideAction("LocA", isDirtyToString(statA));

                    doAction(action);
                    
                    action = decideAction("LocA", isDirtyToString(statA));

                    doAction(action);
                    break;
                case 'B':
                    action = decideAction("LocB", isDirtyToString(statB));

                    doAction(action);

                    action = decideAction("LocB", isDirtyToString(statB));

                    doAction(action);
                    break;
                case 'C':
                    action = decideAction("LocC", isDirtyToString(statC));

                    doAction(action);

                    action = decideAction("LocC", isDirtyToString(statC));

                    doAction(action);
                    break;
                case 'D':
                    action = decideAction("LocD", isDirtyToString(statD));

                    doAction(action);

                    action = decideAction("LocD", isDirtyToString(statD));

                    doAction(action);
                    break;
                case 'E':
                    break;
            }

            //Movement code for vaccum
            vaccLoc.X += xDir * moveSpeed;
            vaccLoc.Y += yDir * moveSpeed;

            Invalidate();
        }

        private string decideAction(string loc, string status)
        {
            string key = $"{loc}-{status}";

            if (actionTable.ContainsKey(key))
            {
                return actionTable[key];
            }
            else
            {
                return "N/A";
            }
        }

        private string isDirtyToString(bool stat)
        {
            if (stat)
            {
                return "Dirty";
            }
            else
            {
                return "Clean";
            }
        }

        private void doAction(string decision)
        {
            if (string.Equals(decision, "moveRight"))
            {
                xDir = 1;
                yDir = 0;
            }
            if (string.Equals(decision, "moveDown"))
            {
                xDir = 0;
                yDir = 1;
            }
            if (string.Equals(decision, "moveLeft"))
            {
                xDir = -1;
                yDir = 0;
            }
            if (string.Equals(decision, "moveUp"))
            {
                xDir = 0;
                yDir = -1;
            }
            if (string.Equals(decision, "vaccum"))
            {
                Thread.Sleep(2000);
                switch (currLoc)
                {
                    case 'A':
                        statA = false;
                        break;
                    case 'B':
                        statB = false;
                        break;
                    case 'C':
                        statC = false;
                        break;
                    case 'D':
                        statD = false;
                        break;
                    case 'E':
                        break;
                }
            }
        }
        
        private char checkLocation(Point location)
        {
            if (location.X == 70 && location.Y == 70)
            {
                return 'A';
            }
            //B location
            else if (location.X == 325 && location.Y == 70)
            {
                return 'B';
            }
            //C location
            else if (location.X == 70 && location.Y == 325)
            {
                return 'C';
            }
            //D location
            else if (location.X == 325 && location.Y == 325)
            {
                return 'D';
            }
            else
            {
                return 'E';
            }
        }
    }
}
