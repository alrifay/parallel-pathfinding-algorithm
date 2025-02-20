﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PathFindingAlgorithem
{
    public class Maze
    {

        #region Attributes
        public String[][] Grid { get; set; }
        public int Columns { get; set; }
        public int Rows { get; set; }
        #endregion

        public Maze(int rows, int columns)
        {
            this.Columns = columns;
            this.Rows = rows;
            this.Grid = new String[rows][];
        }
        
        public List<Vector> GetPath(Vector start, Point finish)
        {
            List<Vector> open = new List<Vector>();
            List<Vector> closed = new List<Vector>();
            Vector visited;
            open.AddRange(this.GetNextMoves(start));
            while (open.Count != 0)
            {
                visited = open[0];
                open.RemoveAt(0);
                if (finish.Equals(visited.position))
                {
                    visited.previous.Add(visited);
                    return visited.previous;
                }
                foreach (Vector v in GetNextMoves(visited))
                {
                    if (!v.IsExistInList(closed) && !v.IsExistInList(open))
                    {
                        open.Add(v);
                    }
                }
                closed.Add(visited);
            }
            return new List<Vector>();
        }

        public List<Vector> GetNextMoves(Vector vector)
        {
            List<Vector> moves = new List<Vector>();
            Vector forward = vector.MoveForward(this);
            Vector right = vector.MoveRight(this);
            if (forward != null)
            {
                forward.previous.AddRange(vector.previous);
                forward.previous.Add(vector);
                moves.Add(forward);
            }
            if (right != null)
            {
                right.previous.AddRange(vector.previous);
                right.previous.Add(vector);
                moves.Add(right);
            }
            return moves;
        }

        #region HelperFunctions

        public static Maze GetMaze(string filename)
        {
            String[][] x = readMaze(filename).Result;
            Maze maze = new Maze(x.GetLength(0), x[0].GetLength(0))
            {
                Grid = x
            };
            return maze;
        }

        static async Task<string[][]> ReadMaze(string filename)
        {
            List<string[]> list = new List<string[]>();
            String temp;
            StreamReader reader = File.OpenText(filename);
            while (reader.Peek() >= 0)
            {
                temp = await reader.ReadLineAsync();
                list.Add(temp.Split());
            }
            string[][] Maze = list.Select(a => a.ToArray()).ToArray();
            reader.Close();
            return Maze;
        }

        public static async Task<String[][]> readMaze(String filename)
        {
            FileInfo fi = new FileInfo(filename);
            int bytes = (int)fi.Length;
            byte[] buf = new byte[bytes];

            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, bytes, true /*async*/ );

            var T1 = Task<int>.Factory.FromAsync(fs.BeginRead, fs.EndRead, buf, 0, bytes, null);

            Task<string[][]> T2 = T1.ContinueWith((antecedent) =>
            {
                fs.Close();
                //int bytesRead = antecedent.Result;
                String result = System.Text.Encoding.UTF8.GetString(buf).Trim();
                return result.Split("\n".ToCharArray()).Select(a => a.Trim().Split()).ToArray();
            });
            return await T2;
        }
        public Vector getStartAndDirection()
        {
            for (int i = 0; i < this.Grid.GetLength(0); i++)
            {
                for (int j = 0; j < this.Grid[0].GetLength(0); j++)
                {
                    if (this.Grid[i][j].Equals("w") || this.Grid[i][j].Equals("a") || this.Grid[i][j].Equals("s") || this.Grid[i][j].Equals("d"))
                    {
                        Direction x;
                        if (this.Grid[i][j].Equals("w"))
                            x = Direction.North;
                        else if (this.Grid[i][j].Equals("a"))
                            x = Direction.West;
                        else if (this.Grid[i][j].Equals("s"))
                            x = Direction.South;
                        else
                            x = Direction.East;
                        return new Vector(new Point(i, j), x);
                    }
                }
            }
            return null;
        }
        public Point getEnd()
        {
            for (int i = 0; i < this.Grid.GetLength(0); i++)
            {
                for (int j = 0; j < this.Grid[0].GetLength(0); j++)
                {
                    if (this.Grid[i][j].Equals("e"))
                    {
                        return new Point(i, j);
                    }
                }
            }
            return null;
        }

        public bool IsBlocked(int row, int column)
        {
            return Grid[row][column].Equals("-1");
        }

        public bool IsBlocked(Point point)
        {
            return IsBlocked(point.X, point.Y);
        }

        public bool IsExist(int x, int y)
        {
            return x >= 0 && x < Rows && y >= 0 && y < Columns;
        }

        public bool IsExist(Point point)
        {
            return IsExist(point.X, point.Y);
        }

        public override string ToString()
        {
            string str = "";
            for (int row = 0; row < this.Rows; row++)
            {
                for (int column = 0; column < this.Columns; column++)
                {
                    str += this.Grid[row][column] + (column + 1 == this.Columns ? "" : ",");
                }
                str += "\n";
            }
            return str;
        }
        #endregion
    }
}
