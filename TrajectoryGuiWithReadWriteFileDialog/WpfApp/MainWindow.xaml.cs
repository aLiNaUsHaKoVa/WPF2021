using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ConsoleApp;
using Microsoft.Win32;
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
        private Trajectory _lastTrajectory;
        
        private Trajectory LastTrajectory
        {
            get => _lastTrajectory;
            set
            {
                SaveButton.IsEnabled = true;
                _lastTrajectory = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ThrowButton_OnClick(object sender, RoutedEventArgs e)
        {
            var trajectory = CreateTrajectoryCalculator()
                .CalculateTrajectory(GetFloatFromBox(TimeBox));

            LastTrajectory = trajectory;

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

        private void LoadButton_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            
            if (openFileDialog.ShowDialog() == true)
            {
                var args = File.ReadAllLines(openFileDialog.FileName);
                SetDataInTextBoxes(
                    ArgsParser.CreateDict(args));
            }
        }

        private void SetDataInTextBoxes(Dictionary<string, string> namedArgs)
        {
            foreach (var argName in namedArgs.Keys)
            {
                var argValue = namedArgs[argName];
                switch (argName)
                {
                    case "speed":
                        SpeedBox.Text = argValue;
                        break;
                    case "angle":
                        AngleBox.Text = argValue;
                        break;
                    case "interval":
                        TimeBox.Text = argValue;
                        break;
                    case "mass":
                        MassBox.Text = argValue;
                        break;
                    case "k":
                        ResistanceBox.Text = argValue;
                        break;
                    case "x0":
                        X0Box.Text = argValue;
                        break;
                    case "y0":
                        Y0Box.Text = argValue;
                        break;
                }
            }
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";

            if (saveFileDialog.ShowDialog() == true)
            {
                var writingThread = new Thread(() =>
                    SaveTrajectoryInFile(saveFileDialog.FileName));
                writingThread.Start();
            }
        }

        private void SaveTrajectoryInFile(string filePath)
        {
            var serializedTrajectory = LastTrajectory.MoveStates
                .Select(state => state.ToString());
            
            File.WriteAllLines(filePath, serializedTrajectory);
        }
    }
}