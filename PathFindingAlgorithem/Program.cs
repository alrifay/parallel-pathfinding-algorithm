﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PathFindingAlgorithem
{
    class Program
    {
        static void Main(string[] args)
        {
            Maze m = Maze.GetMaze(10, 25);
            Vector v = new Vector(new Point(4, 0), Direction.North);
            foreach(Vector vs in m.GetPath(v, new Point(8, 22)))
			{
				Console.WriteLine(vs.position.X + " , " + vs.position.Y + " : " + vs.direction.ToString());
			}
			Console.WriteLine(m.ToString());
        }
    }
}
