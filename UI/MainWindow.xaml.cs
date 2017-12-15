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

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private System.Windows.Threading.DispatcherTimer dispatcherTimer;
        private Rectangle[,] rectangles;
        private Rectangle carImage;

        public MainWindow()
        {
            InitializeComponent();
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 5, 0);

            int[][] Grid = new int[][]{
                    new int[]{-1, 0, -1, 0, -1, 0, -1, 0, },
                    new int[]{-1, 0, -1, 0, -1, 0, -1, 0, },
                    new int[]{-1, 0, -1, 0, -1, 0, -1, 0, },
                    new int[]{-1, 0, -1, 0, -1, 0, -1, 0, },
                };

            this.renderRectangles(Grid, 3, 0, Direction.West);
        }

        private void renderRectangles(int[][] matrix, int carX, int carY, Direction carDirection)
        {
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

            // Rendering rectangles
            for (int rowCounter = 0; rowCounter < matrix.GetLength(0); rowCounter++)
            {
                for (int columnCounter = 0; columnCounter < matrix[0].GetLength(0); columnCounter++)
                {
                    Rectangle newRecatngle = new Rectangle()
                    {
                        Width = rectangleWidth,
                        Height = rectangelHeight,
                        Fill = (matrix[rowCounter][columnCounter] == 0) ? Brushes.White : Brushes.Black,
                        Stroke = Brushes.Gray,
                        StrokeThickness = 0.5,
                    };
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
            this.MoveCar(carX, carY, carDirection);
        }

        private void SetCarRoute(List<PathFindingAlgorithem.Vector> points)
        {
            foreach (PathFindingAlgorithem.Vector point in points)
            {
                this.MoveCar(point.position.X, point.position.Y, point.direction);
            }
        }

        private void MoveCar(int x, int y, Direction direction)
        {
            Grid.SetRow(carImage, y);
            Grid.SetColumn(carImage, x);
            RotateTransform newDirection = new RotateTransform((double)direction, carImage.Width / 2, carImage.Height / 2);
            carImage.RenderTransform = newDirection;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            List<PathFindingAlgorithem.Vector> path = new List<PathFindingAlgorithem.Vector>()
            {
                new PathFindingAlgorithem.Vector(new PathFindingAlgorithem.Point(1, 0), Direction.North),
                new PathFindingAlgorithem.Vector(new PathFindingAlgorithem.Point(0, 2), Direction.North),
                new PathFindingAlgorithem.Vector(new PathFindingAlgorithem.Point(1, 4), Direction.North),
            };
            SetCarRoute(path);
        }
    }
}
