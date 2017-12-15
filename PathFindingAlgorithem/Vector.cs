using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFindingAlgorithem
{
    enum Direction
    {
        North,
        South,
        East,
        West
    }

    class Vector : ICloneable, IEqualityComparer<Vector>
    {
        public Direction direction;
        public Point position;
        public HashSet<Vector> previous;
        public Vector() { }

        public Vector(Point position, Direction direction)
        {
            this.position = position;
            this.direction = direction;
            previous = new HashSet<Vector>(new Vector());
        }
        public object Clone()
        {
            Point position = (Point)this.position.Clone();
            return new Vector(position, this.direction);
        }

        public Vector MoveForward(Maze maze)
        {
            Vector newCar = (Vector)Clone();

            switch (this.direction)
            {
                case Direction.North:
                    newCar.position.X--;
                    break;
                case Direction.East:
                    newCar.position.Y++;
                    break;
                case Direction.South:
                    newCar.position.X++;
                    break;
                case Direction.West:
                    newCar.position.Y--;
                    break;
            }
            if (newCar.CanMove(maze) == true)
            {
                return newCar;
            }
            else
                return null;
        }

        public Vector MoveRight(Maze maze)
        {
            Vector newCar = (Vector)Clone();

            switch (this.direction)
            {
                case Direction.North:
                    newCar.direction = Direction.East;
                    newCar.position.Y++;
                    break;
                case Direction.East:
                    newCar.direction = Direction.South;
                    newCar.position.X++;
                    break;
                case Direction.South:
                    newCar.direction = Direction.West;
                    newCar.position.Y--;
                    break;
                case Direction.West:
                    newCar.direction = Direction.North;
                    newCar.position.X--;
                    break;
            }
            if (newCar.CanMove(maze) == true)
            {
                return newCar;
            }
            else
                return null;
        }

        public bool IsExist(Maze maze)
        {
            return maze.IsExist(this.position);
        }

        public bool IsBlocked(Maze maze)
        {
            return maze.IsBlocked(this.position);
        }

        public bool CanMove(Maze maze)
        {
            return IsExist(maze) && !IsBlocked(maze);
        }

        public bool IsExistInList(List<Vector> list)
        {
            return list.Find(c => c.Equals(this)) != null;
        }

        public bool IsExistInListAndNotLessPath(List<Vector> list)
        {
            if (list.Count > 0)
            {
                return list.Find(c => c.EqualsAndNotLessPath(this)) != null;
            }
            return false;
        }

        public bool Equals(Vector other)
        {
            return this.position.Equals(other.position) && this.direction == other.direction;
        }

        public bool EqualsAndNotLessPath(Vector other)
        {
            return this.Equals(other) && this.previous.Count < other.previous.Count;
        }

        public bool Equals(Vector x, Vector y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(Vector obj)
        {
            return 1;
        }
        override
        public String ToString()
        {
            return this.position.ToString() + ":" + this.direction.ToString() + "\""+ this.previous.Count+ "\"";
        }
    }
}
