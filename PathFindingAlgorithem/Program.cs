using System;
using System.Collections;
using System.Collections.Generic;

namespace PathFindingAlgorithem
{
	class Program
	{
        static void Main(string[] args)
        {
            Maze m = Maze.getMaze();
            Vector v = new Vector(new Point(4, 0), Direction.North);
            Console.WriteLine(m.getPath(v, new Point(0, 4)));
        }
	}

	enum Direction
	{
		North,
		South,
		East,
		West
	}

	class Point : ICloneable , IEquatable<Point>
    {
		public Point(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public Point() : this(0, 0) { }
		public int x { get; set; }
		public int y { get; set; }

		public object Clone()
		{
			return new Point(this.x, this.y);
		}

        public static bool compare(List<Point> list , Point p)
        {
            return list.Find(item => item.Equals(p)) != null;
        }

        public bool Equals(Point other)
        {
            return x == other.x && y == other.y;
        }
    }

	class Vector : ICloneable , IEqualityComparer<Vector>
    {
        public Direction direction;
		public Point position;
        //public List<Vector> previous;
        public HashSet<Vector> previous;
        public Vector() { }

		public Vector(Point position, Direction direction)
		{
			this.position = position;
			this.direction = direction;
            previous = new HashSet<Vector>(new Vector());//new List<Vector>();
        }
        public object Clone()
		{
			Point position = (Point)this.position.Clone();
			return new Vector(position, this.direction);
		}

		public Vector moveForword(Maze maze)
		{
			Vector newCar = (Vector)Clone();

			switch (this.direction)
			{
				case Direction.North:
					newCar.position.x--;
					break;
				case Direction.East:
					newCar.position.y++;
					break;
				case Direction.South:
					newCar.position.x++;
					break;
				case Direction.West:
					newCar.position.y--;
					break;
			}
            if (newCar.canMove(maze) == true) {
                return newCar;
            }
            else
                return null;
		}

		public Vector moveRight(Maze maze)
		{
			Vector newCar = (Vector)Clone();

			switch (this.direction)
			{
				case Direction.North:
					newCar.direction = Direction.East;
					newCar.position.y++;
					break;
				case Direction.East:
					newCar.direction = Direction.South;
					newCar.position.x++;
					break;
				case Direction.South:
					newCar.direction = Direction.West;
					newCar.position.y--;
					break;
				case Direction.West:
					newCar.direction = Direction.North;
					newCar.position.x--;
					break;
			}
            if (newCar.canMove(maze) == true) { 
                return newCar;
            }
            else
                return null;
        }

		public bool isExist(Maze maze)
		{
			return maze.isExist(this.position);
		}

		public bool isBlocked(Maze maze)
		{
			return maze.isBlocked(this.position);
		}

		public bool canMove(Maze maze)
		{
			return isExist(maze) && !isBlocked(maze);
		}

        public bool isExistInList(List<Vector> list)
        {
            return list.Find(c=> c.Equals(this) )!= null;
        }

        public bool Equals(Vector other)
        {
            return this.position.Equals(other.position) && this.direction == other.direction;
        }

        public bool Equals(Vector x, Vector y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(Vector obj)
        {
            return 1;
        }
    }

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

		public static Maze getMaze()
		{
			Maze maze = new Maze(5, 5);
			maze.maze = new int[,]{
				{0, 0, 0, 0, 0},
				{-1, -1, 0, -1, -1},
				{0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0}
			};
			return maze;
		}

		public bool getPath(Vector start, Point finish)
		{
            List<Vector> open = new List<Vector>();
            List<Vector> closed = new List<Vector>();
            Vector visited;
            open.AddRange(this.getNextMoves(start));
            while(open.Count !=0)
            {
                visited = open[0];
                open.RemoveAt(0);
                if (finish.Equals(visited.position))
                {
                    foreach (Vector vs in visited.previous)
                        Console.WriteLine(vs.position.x + " , " + vs.position.y  + " : " + vs.direction.ToString());
                    return true;
                }
                foreach (Vector v in getNextMoves(visited)) { 
                if (!v.isExistInList(closed) && !v.isExistInList(open)) { 
                        open.Add(v);
                    }
                }
                closed.Add(visited);
            }
			return false;
		}

		public List<Vector> getNextMoves(Vector car)
		{
            List<Vector> moves = new List<Vector>();
            Vector forword = car.moveForword(this);
            Vector right = car.moveRight(this);
			if (forword != null)
			{
                foreach (Vector pre in car.previous)
                    forword.previous.Add(pre);
                forword.previous.Add(car);
                moves.Add(forword);
			}
			if (right !=null)
			{
                foreach (Vector pre in car.previous)
                    right.previous.Add(pre);
                right.previous.Add(car);
                moves.Add(right);
            }
			return moves;
		}

		public bool isBlocked(int x, int y)
		{
			return maze[x, y] == -1;
		}

		public bool isBlocked(Point point)
		{
			return isBlocked(point.x, point.y);
		}

		public bool isExist(int x, int y)
		{
			return x >= 0 && x < width && y >= 0 && y < height;
		}

		public bool isExist(Point point)
		{
			return isExist(point.x, point.y);
		}

	}
}
