using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFindingAlgorithem
{
	class Maze
	{
		int[,] maze;
		int width;
		int height;

		public Maze(int width, int height)
		{
			this.width = width;
			this.height = height;
			this.maze = new int[width, height];
		}

		public static Maze GetMaze()
		{
			Maze maze = new Maze(5, 5)
			{
				maze = new int[,]{
					/*{0, 0, 0, 0, 0 ,0, 0, 0,0, 0, 0,0, 0, 0,0, 0, 0,0, 0, 0,0, 0, 0},
					{0, -1, 0, 0, 0 ,0, 0, 0,0, 0, 0,0, 0, 0,0, 0, 0,0, 0, 0,0, 0, 0},
					{0, 0, 0, 0, 0 ,0, 0, 0,0, 0, 0,0, 0, 0,0, 0, 0,0, 0, 0,0, 0, 0},
					{0, 0, 0, 0, 0 ,0, 0, 0,0, 0, 0,0, 0, 0,0, 0, 0,0, 0, 0,0, 0, 0}*/
					{0, 0, 0, 0, 0 },
					{0, 0, 0, 0, 0 },
					{-1, -1, 0, -1, -1 },
					{0, 0, 0, 0, 0 },
					{0, 0, 0, 0, 0 }
				}
			};
			return maze;
		}

		public HashSet<Vector> GetPath(Vector start, Point finish)
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

					/*foreach (Vector vs in visited.previous)
                        Console.WriteLine(vs.position.x + " , " + vs.position.y + " : " + vs.direction.ToString());
                    Console.WriteLine(visited.position.x + " , " + visited.position.y + " : " + visited.direction.ToString());*/
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
			return new HashSet<Vector>();
		}

		public List<Vector> GetNextMoves(Vector vector)
		{
			List<Vector> moves = new List<Vector>();
			Vector forword = vector.MoveForword(this);
			Vector right = vector.MoveRight(this);
			if (forword != null)
			{
				foreach (Vector pre in vector.previous)
					forword.previous.Add(pre);
				forword.previous.Add(vector);
				moves.Add(forword);
			}
			if (right != null)
			{
				foreach (Vector pre in vector.previous)
					right.previous.Add(pre);
				right.previous.Add(vector);
				moves.Add(right);
			}
			return moves;
		}

		public bool IsBlocked(int x, int y)
		{
			return maze[x, y] == -1;
		}

		public bool IsBlocked(Point point)
		{
			return IsBlocked(point.X, point.Y);
		}

		public bool IsExist(int x, int y)
		{
			return x >= 0 && x < width && y >= 0 && y < height;
		}

		public bool IsExist(Point point)
		{
			return IsExist(point.X, point.Y);
		}

		public HashSet<Vector> GetPathParallel(Vector start, Point finish)
		{
			Task<HashSet<Vector>> s = new Task<HashSet<Vector>>(() => GetPathParallel(start.MoveRight(this), finish)); ;
			if (start.MoveRight(this) == null && start.MoveForword(this) != null)
			{
				Console.WriteLine("forword");
				return GetPathParallel(start.MoveForword(this), finish);
			}
			else if (start.MoveRight(this) != null)
			{
				Console.WriteLine("right");
				s.Start();
				//return s.Result;
			}
			HashSet<Vector> list = new HashSet<Vector>();
			if (start.MoveForword(this) != null)
				list = GetPath(start.MoveForword(this), finish);
			return s.Result.Count >= list.Count ? s.Result : list;
		}
	}
}
