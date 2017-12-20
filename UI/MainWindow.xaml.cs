using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using PathFindingAlgorithem;
using Microsoft.Win32;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Rectangle[,] rectangles;
        private Rectangle carImage;
        private Maze maze;
        private PathFindingAlgorithem.Vector start;
        private PathFindingAlgorithem.Point end;
        private ParallelSolverImprove solver;
        private Boolean working = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void renderRectangles(string[][] matrix, PathFindingAlgorithem.Vector car, PathFindingAlgorithem.Point end)
        {
            rectanglesGrid.Children.Clear();
            rectanglesGrid.RowDefinitions.Clear();
            rectanglesGrid.ColumnDefinitions.Clear();
            rectangles = new Rectangle[matrix.GetLength(0), matrix[0].GetLength(0)];
            int rectangleWidth = 60;
            int rectangelHeight = rectangleWidth;

            // Rendering columns for Grid 
            for (int rowCounter = 0; rowCounter < matrix.GetLength(0); rowCounter++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(rectangelHeight);
                rectanglesGrid.RowDefinitions.Add(row);
            }

            // Rendering rows for Grid
            for (int columnCounter = 0; columnCounter < matrix[0].GetLength(0); columnCounter++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(rectangleWidth);
                rectanglesGrid.ColumnDefinitions.Add(col);
            }
            //MessageBox.Show(matrix.GetLength(0) + " " + matrix[0].GetLength(0));
            // Rendering rectangles
            for (int rowCounter = 0; rowCounter < matrix.GetLength(0); rowCounter++)
            {
                for (int columnCounter = 0; columnCounter < matrix[0].GetLength(0); columnCounter++)
                {
                    Rectangle newRecatngle = new Rectangle()
                    {
                        Width = rectangleWidth,
                        Height = rectangelHeight,
                        Fill = (matrix[rowCounter][columnCounter].Equals("-1")) ? Brushes.Black : Brushes.White,
                        Stroke = Brushes.Gray,
                        StrokeThickness = 0.5,
                    };
                    if (rowCounter == end.X && columnCounter == end.Y)
                        newRecatngle.Fill = Brushes.RoyalBlue;
                    rectangles[rowCounter, columnCounter] = newRecatngle;

                    Grid.SetRow(newRecatngle, rowCounter);
                    Grid.SetColumn(newRecatngle, columnCounter);
                    rectanglesGrid.Children.Add(newRecatngle);
                }
            }

            ImageBrush uniformBrush = new ImageBrush();
            uniformBrush.ImageSource =
                new BitmapImage(new Uri("car.png", UriKind.Relative));
            uniformBrush.Stretch = Stretch.Uniform;

            carImage = new Rectangle()
            {
                Width = rectangleWidth,
                Height = rectangelHeight,
                Fill = uniformBrush,
            };

            rectanglesGrid.Children.Add(carImage);
            this.MoveCar(car.position.X, car.position.Y, car.direction);
        }

        private void SetCarRoute(HashSet<PathFindingAlgorithem.Vector> points)
        {
            working = true;
            foreach (PathFindingAlgorithem.Vector point in points)
            {
                rectanglesGrid.Dispatcher.Invoke(() =>
                {
                    this.MoveCar(point.position.X, point.position.Y, point.direction);
                }, System.Windows.Threading.DispatcherPriority.Background);
                Thread.Sleep(250);
            }
            working = false;
        }

        private void MoveCar(int x, int y, Direction direction)
        {
            Grid.SetRow(carImage, x);
            Grid.SetColumn(carImage, y);
            RotateTransform newDirection = new RotateTransform((double)direction, carImage.Width / 2, carImage.Height / 2);
            carImage.RenderTransform = newDirection;
        }

        private void readfile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            var result = file.ShowDialog();
            if (result != null && result.Value != false)
            {
                new Task(() =>
                {
                    this.maze = Maze.GetMaze(file.FileName);
                    this.start = maze.getStartAndDirection();
                    this.end = maze.getEnd();
                    this.rectanglesGrid.Dispatcher.Invoke(() =>
                    {
                        this.renderRectangles(maze.Grid, this.start, this.end);
                    }, System.Windows.Threading.DispatcherPriority.Render);

                }).Start();
            }
        }

        private void SolveSeq_Click(object sender, RoutedEventArgs e)
        {
            if (working)
                return;
            if (this.maze != null)
            {
                rectanglesGrid.Dispatcher.Invoke(() =>
                {
                    this.SetCarRoute(this.maze.GetPath(this.start, this.end));
                }, System.Windows.Threading.DispatcherPriority.Background);
            }
        }

        private void SolveParallel_Click(object sender, RoutedEventArgs e)
        {
            if (working)
                return;
            if (this.maze != null)
            {
                rectanglesGrid.Dispatcher.Invoke(() =>
                {
                    solver = new ParallelSolverImprove(this.maze);
                    this.SetCarRoute(solver.GetPathasync(this.start, this.end));
                    /*ParallelSolver s = new ParallelSolver();
                    this.SetCarRoute(s.StartSolve(this.maze, this.start, this.end));
                   */
                }, System.Windows.Threading.DispatcherPriority.Background);
            }
        }
    }
}
