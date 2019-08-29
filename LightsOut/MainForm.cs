using System;
using System.Drawing;
using System.Windows.Forms;

namespace LightsOut
{
    public partial class MainForm : Form
    {
        private const int GridOffset = 25;
        private int gridLength = 200;
        private int cellLength;

        private LightsOutGame lightsOutGame = new LightsOutGame();

        private Random rand;

        public MainForm()
        {
            InitializeComponent();

            rand = new Random();

            InitializeGameVariables(3);
            this.x3ToolStripMenuItem.Checked = true;
        }

        private void InitializeGameVariables(int cells)
        {

            lightsOutGame.InitializeGameVariables(cells);
            CalculateNewSize();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            for(int r = 0; r< lightsOutGame.GridSize; r++)
            {
                for (int c = 0; c< lightsOutGame.GridSize; c++)
                {
                    Brush brush;
                    Pen pen;

                    if (lightsOutGame.GetGridValue(r, c))
                    {
                        pen = Pens.Black;
                        brush = Brushes.White;
                    }
                    else
                    {
                        pen = Pens.White;
                        brush = Brushes.Black;
                    }

                    int x = c * cellLength + GridOffset;
                    int y = r * cellLength + GridOffset;

                    g.DrawRectangle(pen, x, y, cellLength, cellLength);
                    g.FillRectangle(brush, x + 1, y + 1, cellLength - 1, cellLength - 1);
                }
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.X < GridOffset || e.X > cellLength * lightsOutGame.GridSize + GridOffset ||
                e.Y < GridOffset || e.Y > cellLength * lightsOutGame.GridSize + GridOffset)
                return;

            int r = (e.Y - GridOffset) / cellLength;
            int c = (e.X - GridOffset) / cellLength;


            lightsOutGame.Move(r, c);

            this.Invalidate();

            if (lightsOutGame.IsGameOver())
            {
                MessageBox.Show(this, "Congratulations!  You've won!", "Lights Out!"
                    , MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            lightsOutGame.NewGame();
            this.Invalidate();
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGameButton_Click(sender, e);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutBox = new AboutForm();
            aboutBox.ShowDialog(this);
        }

        private void UncheckBoxes()
        {
            this.x3ToolStripMenuItem.Checked = false;
            this.x4ToolStripMenuItem.Checked = false;
            this.x5ToolStripMenuItem.Checked = false;
        }

        private void X3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UncheckBoxes();
            this.x3ToolStripMenuItem.Checked = true;
            InitializeGameVariables(3);
            Invalidate();
        }

        private void X4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UncheckBoxes();
            this.x4ToolStripMenuItem.Checked = true;
            InitializeGameVariables(4);
            Invalidate();
        }

        private void X5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UncheckBoxes();
            this.x5ToolStripMenuItem.Checked = true;
            InitializeGameVariables(5);
            Invalidate();
        }
        
        private void CalculateNewSize()
        {
            int height = this.Height - 25; //subtract 25 to account for the menu bar at the top
            int width = this.Width;

            int smaller;
            if (height < width)
            {
                smaller = height;
            }
            else
            {
                smaller = width;
            }

            //Determine offset needed to not overlap buttons at the bottom
            int heightDifference = height - smaller;
            if (heightDifference < 35)
            {
                smaller = height - 35;
            }

            gridLength = smaller - GridOffset * 2;
            cellLength = gridLength / lightsOutGame.GridSize;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            CalculateNewSize();
            Invalidate();
        }
    }
}

