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
        private const float clientSize = 600; // form size
        private const float lineLength = 400; // gridline length
        private const float block = lineLength / 8; // cell width & height 
        private const float offset = 100; // position of the upper LH corner of board
        private const float delta = 5;
        private const float dimension = 8; // # of board squares in rows and cols
        private const int gridSize = (int)(clientSize / block); // 12, divide form into squares of block size
        private enum CellSelection {N,Q, R, U}; // N = none, Q = queen, R = remove, U = unsafe
        private CellSelection[,] grid = new CellSelection[gridSize, gridSize];
        private int counter = 0; //  # of queens
        private bool hints = false; // hints checkbox ticked
        Point[] Upoints = new Point[400]; // set arbitrarily large
        private int Ucounter = 0;

        public Form1()
        {
            InitializeComponent();
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

            for (int i = 0; i<gridSize; ++i)
            {
                for (int j = 0; j<gridSize; ++j)
                {
                    if (grid[i, j] == CellSelection.Q)
                    {
                        if (hints == true)
                        {
                            g.FillRectangle(Brushes.Red, i * block, j * block, block, block);
                        }
                        if (i % 2 != j % 2)
                        {
                            g.DrawString(queen, font, Brushes.White, i*block, j*block);
                        }
                        else
                        {
                            g.DrawString(queen, font, Brushes.Black, i * block, j * block);
                        }
                            
                    }
                    if (grid[i, j]==CellSelection.R)
                    {
                        queen = string.Empty; // clear Q
                        queen = "Q"; // reset for next click
                    }
                    if (grid[i, j] == CellSelection.U)
                    {
                        if (hints == true)
                        {
                            g.FillRectangle(Brushes.Red, i * block, j * block, block, block);
                            for (int k = 0; k <= dimension; k++)
                            {
                                g.DrawLine(Pens.Black, 100, 100 + block * k, 100 + lineLength, 100 + block * k);
                                g.DrawLine(Pens.Black, 100 + block * k, 100, 100 + block * k, 100 + lineLength);
                            }
                        }
                        if (hints == false)
                        {
                            if (i %2 != j % 2)
                            {
                                g.FillRectangle(Brushes.Black, i * block, j * block, block, block);

                            }
                            else
                            {
                                g.FillRectangle(Brushes.White, i * block, j * block, block, block);

                            }
                            for (int k = 0; k <= dimension; k++)
                            {
                                g.DrawLine(Pens.Black, 100, 100 + block * k, 100 + lineLength, 100 + block * k);
                                g.DrawLine(Pens.Black, 100 + block * k, 100, 100 + block * k, 100 + lineLength);
                            }
                        }


                    }
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (hints == false)
            {
                hints = true;
            }
            else 
            { 
                hints = false;
            }
            Invalidate();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        // mark cells as unsafe
        private void mark(CellSelection[,] grid, int i, int j)
        {
            // rows
            for (int k = 2; k < dimension + 2; k++)
            {
                if (k != i)
                {
                    grid[k, j] = CellSelection.U;
                    Upoints[Ucounter] = new Point(k, j);
                    Ucounter++;
                }
            }

            // columns
            for (int k = 2; k<dimension+2; k++)
            {
                if (k != j)
                {
                    grid[i, k] = CellSelection.U;
                    Upoints[Ucounter] = new Point(i, k);
                    Ucounter++;
                }
            }

            // diagonal top left
            int d = j - 1;
            for (int k = i - 1; k > 1; k--)
            {
                if (d > 1)
                {
                    grid[k, d] = CellSelection.U;
                    Upoints[Ucounter] = new Point(k, d);
                    Ucounter++;
                    d--;
                }
            }

            // diagonal bottom left
            int e = j + 1;
            for (int k = i - 1; k > 1; k--)
            {
                if (e < 10)
                {
                    grid[k, e] = CellSelection.U;
                    Upoints[Ucounter] = new Point(k, e);
                    Ucounter++;
                    e++;
                }
            }

            // diagonal bottom right
            int f = j + 1;
            for (int k = i + 1; k < 10; k++)
            {
                if (f < 10)
                {
                    grid[k, f] = CellSelection.U;
                    Upoints[Ucounter] = new Point(k, f);
                    Ucounter++;
                    f++;
                }
            }

            // diagonal top right
            int g = j - 1;
            for (int k = i + 1; k < 10; k++)
            {
                if (g > 1)
                {
                    grid[k, g] = CellSelection.U;
                    Upoints[Ucounter] = new Point(k, g);
                    Ucounter++;
                    g--;
                }
            }

        }

        // search and find point frequency
       private int search(Point[] Upoints, int x, int y)
        {
            int tally = 0;
            Point find = new Point(x, y);
            for (int i = 0; i < Ucounter; i++)
            {
                if (Upoints[i] == find)
                {
                    tally++;
                }
            }
            return tally;
        }


        // unmark cells unique to the selected Queen
        private void unmark(CellSelection[,] grid, int i, int j)
        {
            // rows
            for (int k = 2; k < dimension + 2; k++)
            {
                if (k != i)
                {
                    if (search(Upoints, k, j) == 1)
                    {
                        grid[k, j] = CellSelection.R;
                    }
                }
            }

            // columns
            for (int k = 2; k < dimension + 2; k++)
            {
                if (k != j)
                {
                    if (search(Upoints, i, k) == 1)
                    {
                        grid[i, k] = CellSelection.R;
                    }
                }
            }

            // diagonal top left
            int d = j - 1;
            for (int k = i - 1; k > 1; k--)
            {
                if (d > 1)
                {
                    if (search(Upoints, k,d) == 1)
                    {
                        grid[k, d] = CellSelection.R;
                    }
                    d--;
                }
            }

            // diagonal bottom left
            int e = j + 1;
            for (int k = i - 1; k > 1; k--)
            {
                if (e < 10)
                {
                    if (search(Upoints, k, e) == 1)
                    {
                        grid[k, e] = CellSelection.R;
                    }
                    e++;
                }
            }

            // diagonal bottom right
            int f = j + 1;
            for (int k = i + 1; k < 10; k++)
            {
                if (f < 10)
                {
                    if (search(Upoints, k, f) == 1)
                    {
                        grid[k, f] = CellSelection.R;
                    }
                    f++;
                }
            }

            // diagonal top right
            int g = j - 1;
            for (int k = i + 1; k < 10; k++)
            {
                if (g > 1)
                {
                    if (search(Upoints, k, g) == 1)
                    {
                        grid[k, g] = CellSelection.R;
                    }
                    g--;
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

//            if (i % 2 != j % 2) return;
            if (i < 2 || j < 2) return;
            if (i > 9 || j > 9) return;

            // check if move is valid
            if (e.Button == MouseButtons.Left)
            {
                if (grid[i, j] == CellSelection.Q) // queen is already there
                {
                    System.Media.SystemSounds.Beep.Play();
                    return;
                }
                if (grid[i, j] == CellSelection.U)
                {
                    System.Media.SystemSounds.Beep.Play();
                    return;
                }

                grid[i, j] = CellSelection.Q;
                mark(grid, i, j);

                counter++;
                label1.Text = String.Concat("You have ", counter, " queens on the board.");
                if (counter == 8)
                {
                    MessageBox.Show("You did it!");
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                if (grid[i, j] == CellSelection.Q)
                {
                    grid[i, j] = CellSelection.R;
                    unmark(grid, i, j);
                    if (counter < 0) return;
                    counter--;
                    label1.Text = String.Concat("You have ", counter, " queens on the board.");
                }
            }
            Invalidate();
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 2; i<dimension+2; i++)
            {
                for (int j = 2; j< dimension+2; j++)
                {
                    grid[i, j] = CellSelection.R;
                }
                
            }
            counter = 0;
            label1.Text = String.Concat("You have ", counter, " queens on the board.");
            Invalidate();

        }
    }
}
