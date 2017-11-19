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
        public Mutex x;
        public List<Vector> Closed;
        //private List<Task> tasks;

        public ParallelSolver()
        {
            Soluations = new List<HashSet<Vector>>();
            x = new Mutex();
            //tasks = new List<Task>();
            Closed = new List<Vector>();
        }
        public void Solve(Maze maze, Vector start, Point end)
        {
            List<Task> innerTasks = new List<Task>();
            x.WaitOne();
            Closed.Add(start);
            x.ReleaseMutex();

            if (start.position.Equals(end))
            {
                Console.WriteLine("Done");
                start.previous.Add(start);
                Soluations.Add(start.previous);
            }
            foreach (Vector v in maze.GetNextMoves(start))
            {
                if (!v.IsExistInListAndNotLessPath(Closed))
                {
                    Task s = new Task(() => Solve(maze, v, end), TaskCreationOptions.AttachedToParent);
                    innerTasks.Add(s);
                }
            }
            foreach (Task s in innerTasks)
                s.Start();
            foreach (Task s in innerTasks)
                s.Wait();
        }
        public HashSet<Vector> StartSolve(Maze maze, Vector start, Point end)
        {
            Solve(maze, start, end);
            /*foreach (Task s in tasks.ToList())
            {
                s.Wait();
            }*/
            HashSet<Vector> Soluation = new HashSet<Vector>();
            int count = 0;
            if (Soluations.Count != 0)
            {
                Soluation = Soluations[0];
                count = Soluation.Count;
                foreach (HashSet<Vector> Sol in Soluations)
                {
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
