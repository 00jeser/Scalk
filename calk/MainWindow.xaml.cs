using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace calk
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public List<Point> points = new List<Point>();
        public List<decimal> lens = new List<decimal>();
        public bool end;
        public Brush PolyC = new SolidColorBrush(Color.FromRgb(200, 155, 255));
        public Brush DotC = Brushes.Blue;
        public Brush LineC = Brushes.Black;

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!end)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    try
                    {
                        Point p = e.GetPosition(this);
                        if (points.Count >= 1)
                        {
                            InputBox.InputBox inputBox = new InputBox.InputBox();
                            lens.Add(decimal.Parse(inputBox.getString()));
                        }
                        points.Add(new Point(p.X - 10, p.Y - 30));
                    }catch{}
                }
                draw();
            }
            else
            {
                points = new List<Point>();
                lens = new List<decimal>();
                gr.Children.Clear();
                end = false;
            }
        }

        private decimal calk()
        {
            decimal height = 0;
            try
            {
                height = decimal.Parse(h.Text);
            }
            catch
            {
                InputBox.InputBox inputBox = new InputBox.InputBox("Введите высоту");
                h.Text = inputBox.getString();
                height = decimal.Parse(h.Text);
            }
            decimal rezult = 0;
            foreach (decimal d in lens)
            {
                rezult += d * height;
            }
            foreach (Grid g in otv.Items)
            {
                rezult -= decimal.Parse(((TextBox)g.Children[0]).Text) * decimal.Parse(((TextBox)g.Children[1]).Text);
            }
            return rezult;
        }

        private void calk(object sender, RoutedEventArgs e)
        {
            if (points.Count != lens.Count)
            {
                InputBox.InputBox inputBox = new InputBox.InputBox();
                lens.Add(decimal.Parse(inputBox.getString()));
            }
            rez.Text = calk().ToString();
        }

        private void draw()
        {
            gr.Children.Clear();
            gr.Children.Add(new Polygon() { Points = new PointCollection(points), Fill = PolyC });
            foreach (Point p in points)
            {
                gr.Children.Add(
                    new Ellipse()
                    {
                        Height = 4,
                        Width = 4,
                        Margin = new Thickness(p.X - 2, p.Y - 2, 0, 0),
                        Fill = DotC,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top
                    }
                    );
                if (!end)
                {
                    gr.Children.Add(new Line() { X1 = 0, Y1 = p.Y, X2 = 10000, Y2 = p.Y, Stroke = LineC, StrokeThickness = 1 });
                    gr.Children.Add(new Line() { X1 = p.X, Y1 = 0, X2 = p.X, Y2 = 10000, Stroke = LineC, StrokeThickness = 1 });
                }
            }
        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void PolyColor(object sender, RoutedEventArgs e)
        {
            PolyC = (sender as MenuItem).Background;
        }

        private void DotColor(object sender, RoutedEventArgs e)
        {
            DotC = (sender as MenuItem).Background;
        }

        private void LineColor(object sender, RoutedEventArgs e)
        {
            LineC = (sender as MenuItem).Background;
        }

        private void Button_Plus(object sender, RoutedEventArgs e)
        {
            Grid g = new Grid { Background = Brushes.Black, Width = 105, Height = 35 };
            g.Children.Add(new TextBox { HorizontalAlignment = HorizontalAlignment.Left, Width = 50, Height = 25, Text = "0" });
            g.Children.Add(new TextBox { HorizontalAlignment = HorizontalAlignment.Right, Width = 50, Height = 25, Text = "0" });
            otv.Items.Add(g);
        }

        private void New(object sender, RoutedEventArgs e)
        {
            points.Clear();
            lens.Clear();
            gr.Children.Clear();
            otv.Items.Clear();
        }
    }
}
