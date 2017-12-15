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
           /* Maze m = Maze.GetMaze();
            Vector start = Maze.getStartAndDirection();
            //Point end = Maze.getEnd();
            ParallelSolverImprove solver = new ParallelSolverImprove(m);
            //solver.GetPathasync(start, end);
            /*ParallelSolver s = new ParallelSolver();
            //HashSet<Vector> sol;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            //sol = s.StartSolve(m, start, end);
            //sol = m.GetPath(start, end);
            //sol = solver.GetPath(start, end);
            //sol = solver.GetPathasync(start, end);
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds.ToString() + ": " + sol.Count);
            /*foreach(Vector vs in sol)
			{
				Console.WriteLine(vs.position.X + " , " + vs.position.Y + " : " + vs.direction.ToString());
			}*/
			//Console.WriteLine(m.ToString());
        }
    }
}
