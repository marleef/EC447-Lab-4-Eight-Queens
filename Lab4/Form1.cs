using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab4
{
    public partial class Form1 : Form
    {
        private const float clientSize = 600;
        private const float lineLength = 400; // gridline length
        private const float block = lineLength / 8; // cell width & height 
        private const float offset = 100; // position of the upper LH corner of board
        private const float delta = 5;
        private const float dimension = 8; // # of board squares in rows and cols
        private const int gridSize = (int)(clientSize / block); // 12, divide form into squares of block size
        private enum CellSelection {N,Q, R, U}; // N = none, Q = queen, R = remove, U = unsafe
        private CellSelection[,] grid = new CellSelection[gridSize, gridSize];
        private int counter = 0;
        private CellSelection[,] track = new CellSelection[gridSize, gridSize];


        public Form1()
        {
            InitializeComponent();
            ResizeRedraw = true; // what does this do
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Eight Queens by Marlee Feltham";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Size = new Size((int)clientSize, (int)clientSize);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
         
            // board background
            g.FillRectangle(Brushes.White, 100, 100, lineLength, lineLength);

            // cell borders
            for (int i = 0; i<=dimension; i++)
            {
                g.DrawLine(Pens.Black, 100, 100+block*i, 100+lineLength, 100 + block*i);
                g.DrawLine(Pens.Black, 100 + block * i, 100, 100 + block * i, 100 + lineLength);
            }

            // fill black cells
            for (int i = 0; i<dimension; i++)
            {
                for (int j = 0; j<dimension; j++)
                {
                    if ((i+j)%2 != 0)
                    {
                        g.FillRectangle(Brushes.Black, 100 + block * i, 100 + block * j, block, block);
                    }
                }
            }

            Font font = new Font("Arial", 30, FontStyle.Bold);
            string queen = "Q";
            string n = "N";

            for (int i = 0; i<gridSize; ++i)
            {
                for (int j = 0; j<gridSize; ++j)
                {
                    if (grid[i, j] == CellSelection.Q)
                    {
                        g.DrawString(queen, font, Brushes.Black, i*block, j*block);
                        mark(grid, i, j);
                        // add red here
                    }
                    if (grid[i, j]==CellSelection.R)
                    {
                        queen = string.Empty;
                        queen = "Q";
                    }
                    if (grid[i, j] == CellSelection.U)
                    {
                        //return;
                        g.DrawString("u", font, Brushes.Black, i * block, j * block);
                        //g.FillRectangle(Brushes.Red, i*block, j*block, 50, 50);
                        //mark(grid, i, j);
                        // add red here
                    }

                    // add control to paint squares red and mark as unsafe
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void mark(CellSelection[,] grid, int i, int j)
        {
            for (int k = 2; k < dimension + 2; k++)
            {
                if (k != i || k !=j)
                {
                    grid[k, j] = CellSelection.U;
                    grid[i, k] = CellSelection.U;
                }

            }

            int d = j - 1;
            int e = j + 1;

            for (int k = i -1; k > 1; k--)
            {
                if (d>1)
                {
                    grid[k, d] = CellSelection.U;
                    d--;
                }
            }

            for (int k = i - 1; k > 1; k--)
            {
                if (e < 10)
                {
                    grid[k, e] = CellSelection.U;
                    e++;
                }
            }
            }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            Graphics g = CreateGraphics();
            PointF[] p = { new Point(e.X, e.Y) };
            g.TransformPoints(CoordinateSpace.World, CoordinateSpace.Device, p);
            if (p[0].X < 0 || p[0].Y < 0) return;
            int i = (int)(p[0].X / block);
            int j = (int)(p[0].Y / block);

            // only allow placing Queens on white checkerboard places
            if (i % 2 != j % 2) return;
            if (i < 2 || j < 2) return;
            if (i > 9 || j > 9) return;

            // check if move is valid
            if (e.Button == MouseButtons.Left)
            {
                if (grid[i, j] == CellSelection.Q) return;// queen is already there
                if (grid[i, j] == CellSelection.U) return;

                grid[i, j] = CellSelection.Q;
                mark(grid, i, j);

                counter++;
                label1.Text = String.Concat("You have ", counter, " queens on the board.");

                if (e.Button == MouseButtons.Right)
                {
                    if (grid[i, j] == CellSelection.Q)
                    {
                        grid[i, j] = CellSelection.R;
                        if (counter < 0) return;
                        counter--;
                        label1.Text = String.Concat("You have ", counter, " queens on the board.");
                    }
                }
                Invalidate();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
