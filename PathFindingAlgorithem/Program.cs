using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PathFindingAlgorithem
{
    class Program
    {
        static void Main(string[] args)
        {
            Maze m = Maze.GetMaze();
            Vector v = new Vector(new Point(4, 0), Direction.North);
            ParallelSolver s = new ParallelSolver();
            foreach(Vector vs in s.StartSolve(m,v, new Point(0, 4)))
			{
				Console.WriteLine(vs.position.X + " , " + vs.position.Y + " : " + vs.direction.ToString());
			}
			Console.WriteLine(m.ToString());
        }
    }
}
