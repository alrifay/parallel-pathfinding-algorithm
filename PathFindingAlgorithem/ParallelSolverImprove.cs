using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PathFindingAlgorithem
{
    public class ParallelSolverImprove
    {
        public List<Vector> open { get; set; }
        public List<Vector> closed { get; set; }
        public Maze maze { get; set; }
        public Object Lock { get; set; }
        public List<Task<List<Vector>>> tasks { get; set; }
        public ParallelSolverImprove()
        {
            open = new List<Vector>();
            closed = new List<Vector>();
            Lock = new object();
            tasks = new List<Task<List<Vector>>>();
        }
        public ParallelSolverImprove(Maze maze)
        {
            this.maze = maze;
            open = new List<Vector>();
            closed = new List<Vector>();
            Lock = new object();
            tasks = new List<Task<List<Vector>>>();
        }
        public List<Vector> GetPathasync(Vector start, Point finish)
        {
            Vector visited;
            this.open.AddRange(this.maze.GetNextMoves(start));
            while (this.open.Count < 4 && this.open.Count != 0)
            {
                visited = open[0];
                open.RemoveAt(0);
                if (finish.Equals(visited.position))
                {
                    visited.previous.Add(visited);
                    return visited.previous;
                }
                foreach (Vector v in this.maze.GetNextMoves(visited))
                {
                    if (!v.IsExistInList(closed) && !v.IsExistInList(open))
                    {
                        open.Add(v);
                    }
                }
                closed.Add(visited);
            }
            Parallel.ForEach(open, (newStart) =>
            {
                Task<List<Vector>> s = new Task<List<Vector>>(() => GetPath(newStart, finish));
                s.Start();
                tasks.Add(s);
            });
            Task.WaitAll(tasks.ToArray());
            List<Vector> min = tasks[0].Result;
            tasks.RemoveAt(0);
            Parallel.ForEach(tasks, (task) =>
            {
                List<Vector> result = task.Result;
                if (result.Count != 0)
                {
                    lock (Lock)
                    {
                        if (min.Count == 0)
                            min = result;
                        else if (min.Count > result.Count)
                            min = result;
                    }
                }
            });
            
            /*foreach (Task<HashSet<Vector>> task in tasks)
            {
                HashSet<Vector> result = task.Result;
                if (result.Count != 0)
                {
                    if (min.Count == 0)
                        min = result;
                    else if (min.Count > result.Count)
                        min = result;
                }
            }*/
            return min;
        }
        public List<Vector> GetPath(Vector start, Point finish)
        {
            if (finish.Equals(start.position))
            {
                start.previous.Add(start);
                return start.previous;
            }
            List<Vector> CurrentOpenList = new List<Vector>();
            Vector visited;
            CurrentOpenList.AddRange(this.maze.GetNextMoves(start));
            while (CurrentOpenList.Count != 0)
            {
                visited = CurrentOpenList[0];
                CurrentOpenList.RemoveAt(0);
                if (finish.Equals(visited.position))
                {
                    visited.previous.Add(visited);
                    return visited.previous;
                }
                foreach (Vector v in this.maze.GetNextMoves(visited))
                {
                    lock (Lock)
                    {
                        if (!v.IsExistInListAndNotLessPath(closed) && !v.IsExistInList(CurrentOpenList))
                        {
                            CurrentOpenList.Add(v);
                        }
                    }
                }
                lock (Lock)
                {
                    closed.Add(visited);
                }
            }
            return new List<Vector>();
        }
    }
}
