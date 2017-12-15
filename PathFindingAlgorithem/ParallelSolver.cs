using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PathFindingAlgorithem
{
    class ParallelSolver
    {
        public List<HashSet<Vector>> Soluations;
        public Mutex Lock;
        public List<Vector> Closed;
        public ParallelSolver()
        {
            Soluations = new List<HashSet<Vector>>();
            Lock = new Mutex();
            Closed = new List<Vector>();
        }
        public void Solve(Maze maze, Vector start, Point end)
        {
            Lock.WaitOne();
            Closed.Add(start);
            Lock.ReleaseMutex();
            if (start.position.Equals(end))
            {
                Console.WriteLine("Done");
                start.previous.Add(start);
                Soluations.Add(start.previous);
            }
            else
            {
                List<Vector> discover = maze.GetNextMoves(start);
                if (discover.Count != 0)
                {
                    if (discover.Count == 2)
                    {
                        Lock.WaitOne();
                        if (!discover[1].IsExistInListAndNotLessPath(Closed))
                        {
                            new Task(() => Solve(maze, discover[1], end), TaskCreationOptions.AttachedToParent).Start();
                        }
                        Lock.ReleaseMutex();
                    }
                    Lock.WaitOne();
                    if (!discover[0].IsExistInListAndNotLessPath(Closed))
                    {
                        Solve(maze, discover[0], end);
                    }
                    Lock.ReleaseMutex();
                }
            }
        }
        public HashSet<Vector> StartSolve(Maze maze, Vector start, Point end)
        {
            Task s = new Task(()=>Solve(maze, start, end));
            s.Start();
            s.Wait();
            HashSet<Vector> Soluation = new HashSet<Vector>();
            int count = 0;
            if (Soluations.Count != 0)
            {
                Soluation = Soluations[0];
                count = Soluation.Count;
                foreach (HashSet<Vector> Sol in Soluations)
                {
                    Console.WriteLine(Sol.Count);
                    if (count > Sol.Count)
                    {
                        Soluation = Sol;
                        count = Sol.Count;
                    }
                }
            }
            return Soluation;
        }
    }
}
