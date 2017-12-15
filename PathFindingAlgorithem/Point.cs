using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFindingAlgorithem
{
	public class Point : ICloneable, IEquatable<Point>
	{
		public Point(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		public Point() : this(0, 0) { }
		public int X { get; set; }
		public int Y { get; set; }

		public object Clone()
		{
			return new Point(this.X, this.Y);
		}

		public static bool HasPoint(List<Point> list, Point p)
		{
			return list.Find(item => item.Equals(p)) != null;
		}

		public bool Equals(Point other)
		{
			return X == other.X && Y == other.Y;
		}
	}
}
