using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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
using OxyPlot;
using TrajectoryClasses;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int AxisAdditionForGoodViewing = 4;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ThrowButton_OnClick(object sender, RoutedEventArgs e)
        {
            var trajectory = CreateTrajectoryCalculator()
                .CalculateTrajectory(GetFloatFromBox(TimeBox));

            UpdateLabels(trajectory);
            AnimateTrajectory(trajectory);
        }

        private TrajectoryCalculator CreateTrajectoryCalculator()
        {
            return new TrajectoryCalculator(
                new PointF(
                    GetFloatFromBox(X0Box),
                    GetFloatFromBox(Y0Box)),
                GetFloatFromBox(SpeedBox),
                GetFloatFromBox(AngleBox),
                GetFloatFromBox(MassBox),
                GetFloatFromBox(ResistanceBox));
        }

        private static float GetFloatFromBox(TextBox box)
        {
            return float.Parse(box.Text, CultureInfo.InvariantCulture);
        }

        private void UpdateLabels(Trajectory trajectory)
        {
            MaxHeightLabel.Content = trajectory.MaxHeight;
            DistanceLabel.Content = trajectory.Distance;
            FlightTimeLabel.Content = trajectory.FlightTimeInSeconds;
        }

        private void AnimateTrajectory(Trajectory trajectory)
        {
            PreparePlotForAnimation(trajectory);

            var sleepTime = (int)(GetFloatFromBox(TimeBox) * 1000);
            Task.Run(() => StartTrajectoryAnimation(
                trajectory,
                sleepTime));
        }

        private void PreparePlotForAnimation(Trajectory trajectory)
        {
            LineSeries.ItemsSource = new List<DataPoint>(new[]
            {
                new DataPoint(trajectory.StartPoint.X, trajectory.StartPoint.Y)
            });
            XAxis.Maximum = trajectory.Distance + AxisAdditionForGoodViewing;
            YAxis.Maximum = trajectory.MaxHeight + AxisAdditionForGoodViewing;
            
            Dispatcher.Invoke(() => TrajectoryPlot.ActualModel.InvalidatePlot(true));
        }

        private void StartTrajectoryAnimation(Trajectory trajectory, int sleepTime)
        {
            var lst = LineSeries.ItemsSource as List<DataPoint>;
            foreach (var state in trajectory.MoveStates.Skip(1))
            {
                var point = new DataPoint(state.Coords.X, state.Coords.Y);
                lst.Add(point);
                Dispatcher.Invoke(() => TrajectoryPlot.ActualModel.InvalidatePlot(true));

                Thread.Sleep(sleepTime);
            }
        }

        private void DrawTrajectory(Trajectory trajectory)
        {
            LineSeries.ItemsSource = trajectory.MoveStates
                .Select(state => new DataPoint(state.Coords.X, state.Coords.Y));
        }
    }
}