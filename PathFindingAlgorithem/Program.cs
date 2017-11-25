using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PathFindingAlgorithem
{
    class Program
    {
        static void Main(string[] args)
        {
            Maze m = Maze.GetMaze();
            Vector v = new Vector(new Point(4, 0), Direction.North);
            Point Start = new Point(0, 10);
            ParallelSolver s = new ParallelSolver();
            HashSet<Vector> sol;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            sol = s.StartSolve(m, v, Start);
            //sol = m.GetPath(v, Start);
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds.ToString() + ": " + sol.Count);
            Console.ReadKey();
            /*foreach(Vector vs in s.StartSolve(m,v, new Point(0, 4)))
			{
				Console.WriteLine(vs.position.X + " , " + vs.position.Y + " : " + vs.direction.ToString());
			}
			Console.WriteLine(m.ToString());*/
        }
    }
}
