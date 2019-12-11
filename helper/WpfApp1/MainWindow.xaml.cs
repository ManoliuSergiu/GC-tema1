using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        char[] split = { ',','.' };
        static DispatcherTimer timmy = new DispatcherTimer()
        {
            Interval = new TimeSpan(0, 0, 0, 0, 200),
            
        };
        public MainWindow()
        {
            InitializeComponent();
            Init();
            timmy.Tick += Timmy_Tick;
        }

        private void Timmy_Tick(object sender, EventArgs e)
        {
            Tick();
        }

        private void Tick()
        {
            Engine.Step();
            canvas.Children.Add(Engine.Frame());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            timmy.Stop();
            Init();
        }

        private void Init()
        {
            canvas.Children.Clear();
            string[] coords = sizeTB.Text.Split(split, StringSplitOptions.RemoveEmptyEntries);
            int x = int.Parse(coords[0]);
            int y = int.Parse(coords[1]);
            string[] dValues = dValuesTB.Text.Split(split, StringSplitOptions.RemoveEmptyEntries);
            Engine.dval1 = int.Parse(dValues[0]);
            Engine.dval2 = int.Parse(dValues[1]);
            string[] condValues = condValuesTB.Text.Split(split, StringSplitOptions.RemoveEmptyEntries);
            Engine.condval1 = int.Parse(condValues[0]);
            Engine.condval2 = int.Parse(condValues[1]);
            string[] incrEValues = incrEValuesTB.Text.Split(split, StringSplitOptions.RemoveEmptyEntries);
            Engine.incrEval1 = int.Parse(incrEValues[0]);
            Engine.incrEval2 = int.Parse(incrEValues[1]);
            string[] incrNEValues = incrNEValuesTB.Text.Split(split, StringSplitOptions.RemoveEmptyEntries);
            Engine.incrNEval1 = int.Parse(incrNEValues[0]);
            Engine.incrNEval2 = int.Parse(incrNEValues[1]);
            string[] conditionalValues = conditionalValuesTB.Text.Split(split, StringSplitOptions.RemoveEmptyEntries);
            Engine.cv1 = double.Parse(conditionalValues[0]);
            Engine.cv2 = double.Parse(conditionalValues[1]);
            Engine.cv3 = double.Parse(conditionalValues[2]);
            canvas.Children.Add(Engine.InitBackground(x, y, (int)canvas.Width, (int)canvas.Height));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            timmy.Stop();
            Tick();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            timmy.Start();

        }
    }
}
