using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PathFindingAlgorithem
{
	class Program
	{
		static void Main(string[] args)
		{
			Point p = new Point(5, 6);
			Console.WriteLine(p.x + " " + p.y);
		}
	}

	enum Direction
	{
		North,
		South,
		East,
		West
	}

	class Point : ICloneable
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
	}

	class Car : ICloneable
	{
		Direction direction;
		Point position;

		public Car(Point position, Direction direction)
		{
			this.position = position;
			this.direction = direction;
		}

		public object Clone()
		{
			Point position = (Point)this.position.Clone();
			return new Car(position, this.direction);
		}

		public Car moveForword(Maze maze)
		{
			Car newCar = (Car)Clone();

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
			return newCar;
		}

		public Car moveRight()
		{
			Car newCar = (Car)Clone();

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
			return newCar;
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
				{0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0}
			};
			return maze;
		}

		public LinkedList<Point> getPath(Point start, Point finish)
		{
			LinkedList<Point> path = new LinkedList<Point>();

			return path;
		}

		public LinkedList<Car> getNextMoves(Car car)
		{
			LinkedList<Car> moves = new LinkedList<Car>();

			if (isExist(x, y + 1) && !isBlocked())
			{

			}
			if (isExist(x + 1, y) && !isBlocked())
			{

			}
			return null;
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
