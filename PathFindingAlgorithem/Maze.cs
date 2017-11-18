using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PathFindingAlgorithem
{
	class Maze
	{

		#region Attributes
		public int[,] Grid { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		#endregion

		public Maze(int width, int height)
		{
			this.Width = width;
			this.Height = height;
			this.Grid = new int[width, height];
		}

		public HashSet<Vector> GetPathParallel(Vector start, Point finish)
		{
			throw new NotImplementedException();
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
			Vector forward = vector.MoveForward(this);
			Vector right = vector.MoveRight(this);
			if (forward != null)
			{
				foreach (Vector pre in vector.previous)
					forward.previous.Add(pre);
				forward.previous.Add(vector);
				moves.Add(forward);
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

		#region HelperFunctions

		public static Maze GetMaze()
		{
			Maze maze = new Maze(5, 5)
			{
				Grid = new int[,]{
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

		public bool IsBlocked(int x, int y)
		{
			return Grid[x, y] == -1;
		}

		public bool IsBlocked(Point point)
		{
			return IsBlocked(point.X, point.Y);
		}

		public bool IsExist(int x, int y)
		{
			return x >= 0 && x < Width && y >= 0 && y < Height;
		}

		public bool IsExist(Point point)
		{
			return IsExist(point.X, point.Y);
		}

		#endregion
	}
}
