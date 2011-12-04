using System;
using System.Windows;
using System.Windows.Controls;

namespace ComputerVisionTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Double _sizeOnSensor;
        private Double _actualSize;
        private Double _distanceFromLens;
        private Double _focalLength;
        private Boolean _canCalculate;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected void Initialize()
        {
            _sizeOnSensor = 0.0;
            _actualSize = 0.0;
            _distanceFromLens = 0.0;
            _focalLength = 0.0;
            textBoxActualSize.Text = "";
            textBoxDistanceFromLens.Text = "";
            textBoxFocalLength.Text = "";
            textBoxSizeOnSensor.Text = "";
            _canCalculate = false;
            buttonCalculate.IsEnabled = false;
        }

        private void ButtonClearClick(object sender, RoutedEventArgs e)
        {
            Initialize();
        }

        public Double ConvertMillimeterToCentimeter(Double value) { return value * 0.1; }
        public Double ConvertMillimeterToMeter(Double value) { return value * 0.001; }
        public Double ConvertMillimeterToKilometer(Double value) { return value * 0.000001; }
        public Double ConvertCentimeterToMillimeter(Double value) { return value * 10.0; }
        public Double ConvertCentimeterToMeter(Double value) { return value * 0.01; }
        public Double ConvertCentimeterToKilometer(Double value) { return value * 0.00001; }
        public Double ConvertMeterToMillimeter(Double value) { return value * 1000; }
        public Double ConvertMeterToCentimeter(Double value) { return value * 100; }
        public Double ConvertMeterToKilometer(Double value) { return value * 0.001; }
        public Double ConvertKilometerToMillimeter(Double value) { return value * 1000000; }
        public Double ConvertKilometerToCentimeter(Double value) { return value * 100000; }
        public Double ConvertKilometerToMeter(Double value) { return value * 1000; }

        private Boolean CanCalculate()
        {
            _canCalculate = false;
            // We need 3 values to calculate
            if (_sizeOnSensor == 0.0)
            {
                if (_actualSize != 0.0 && _distanceFromLens != 0.0 && _focalLength != 0.0) _canCalculate = true;
            }
            else if (_actualSize == 0.0)
            {
                if (_sizeOnSensor != 0.0 && _distanceFromLens != 0.0 && _focalLength != 0.0) _canCalculate = true;
            }
            else if (_distanceFromLens == 0.0)
            {
                if (_sizeOnSensor != 0.0 && _actualSize != 0.0 && _focalLength != 0.0) _canCalculate = true;
            }
            else if (_focalLength == 0.0)
            {
                if (_sizeOnSensor != 0.0 && _actualSize != 0.0 && _distanceFromLens != 0.0) _canCalculate = true;
            }
            if (_canCalculate)
            {
                textBlockFormula.Text = "";
                buttonCalculate.IsEnabled = true;
            }
            else
            {
                buttonCalculate.IsEnabled = false;
            }
            return _canCalculate;
        }

        private void CheckCanCalculate()
        {
            CanCalculate();
        }

        private void ButtonCalculateClick(object sender, RoutedEventArgs e)
        {
            if (CanCalculate()) // Shouldn't be here if we can't calculate, but check to be sure.
            {
                if (_sizeOnSensor == 0.0)
                {
                    _sizeOnSensor = _actualSize*(_focalLength/_distanceFromLens);
                    textBoxSizeOnSensor.Text = _sizeOnSensor.ToString();
                    comboBoxSizeOnSensorUnits.SelectedIndex = 0;
                    textBlockFormula.Text = "x = X * (f / z)";
                }
                else if (_actualSize == 0.0)
                {
                    _actualSize = _sizeOnSensor/(_focalLength/_distanceFromLens);
                    textBoxActualSize.Text = _actualSize.ToString();
                    comboBoxActualSizeUnits.SelectedIndex = 0;
                    textBlockFormula.Text = "X = x / (f / z)";
                }
                else if (_distanceFromLens == 0.0)
                {
                    _distanceFromLens = _focalLength*(_actualSize/_sizeOnSensor);
                    textBoxDistanceFromLens.Text = _distanceFromLens.ToString();
                    comboBoxDistanceFromLensUnits.SelectedIndex = 0;
                    textBlockFormula.Text = "z = f * (X / x)";
                }
                else if (_focalLength == 0.0)
                {
                    _focalLength = _sizeOnSensor*(_distanceFromLens/_actualSize);
                    textBoxFocalLength.Text = _focalLength.ToString();
                    comboBoxFocalLengthUnits.SelectedIndex = 0;
                    textBlockFormula.Text = "f = x * (z / X)";
                }
            }
        }

        private void OnTextBoxSizeOnSensorChanged(object sender, TextChangedEventArgs e)
        {
            UpdateSizeOnSensor();
        }

        private void UpdateSizeOnSensor()
        {
            Double sizeOnSensor;
            if (Double.TryParse(textBoxSizeOnSensor.GetLineText(0), out sizeOnSensor))
            {
                if ((string) comboBoxSizeOnSensorUnits.SelectionBoxItem == "mm")
                    _sizeOnSensor = sizeOnSensor;
                else if ((string) comboBoxSizeOnSensorUnits.SelectionBoxItem == "cm")
                    _sizeOnSensor = ConvertCentimeterToMillimeter(sizeOnSensor);
                else if ((string) comboBoxSizeOnSensorUnits.SelectionBoxItem == "m")
                    _sizeOnSensor = ConvertMeterToCentimeter(sizeOnSensor);
                else if ((string) comboBoxSizeOnSensorUnits.SelectionBoxItem == "km")
                    _sizeOnSensor = ConvertKilometerToCentimeter(sizeOnSensor);
                CheckCanCalculate();
            }
            else
            {
                _sizeOnSensor = 0.0;
                if (buttonCalculate != null) buttonCalculate.IsEnabled = false;
            }
        }

        private void OnTextBoxActualSizeChanged(object sender, TextChangedEventArgs e)
        {
            UpdateActualSize();
        }

        private void UpdateActualSize()
        {
            Double actualSize;
            if (Double.TryParse(textBoxActualSize.GetLineText(0), out actualSize))
            {
                if ((string) comboBoxActualSizeUnits.SelectionBoxItem == "mm")
                    _actualSize = actualSize;
                else if ((string) comboBoxActualSizeUnits.SelectionBoxItem == "cm")
                    _actualSize = ConvertCentimeterToMillimeter(actualSize);
                else if ((string) comboBoxActualSizeUnits.SelectionBoxItem == "m")
                    _actualSize = ConvertMeterToCentimeter(actualSize);
                else if ((string) comboBoxActualSizeUnits.SelectionBoxItem == "km")
                    _actualSize = ConvertKilometerToCentimeter(actualSize);
                CheckCanCalculate();
            }
            else
            {
                _actualSize = 0.0;
                if (buttonCalculate != null) buttonCalculate.IsEnabled = false;
            }
        }

        private void OnTextBoxDistanceFromLensChanged(object sender, TextChangedEventArgs e)
        {
            UpdateDistanceFromLens();
        }

        private void UpdateDistanceFromLens()
        {
            Double distanceFromLens;
            if (Double.TryParse(textBoxDistanceFromLens.GetLineText(0), out distanceFromLens))
            {
                if ((string) comboBoxDistanceFromLensUnits.SelectionBoxItem == "mm")
                    _distanceFromLens = distanceFromLens;
                else if ((string) comboBoxDistanceFromLensUnits.SelectionBoxItem == "cm")
                    _distanceFromLens = ConvertCentimeterToMillimeter(distanceFromLens);
                else if ((string) comboBoxDistanceFromLensUnits.SelectionBoxItem == "m")
                    _distanceFromLens = ConvertMeterToCentimeter(distanceFromLens);
                else if ((string) comboBoxDistanceFromLensUnits.SelectionBoxItem == "km")
                    _distanceFromLens = ConvertKilometerToCentimeter(distanceFromLens);
                CheckCanCalculate();
            }
            else
            {
                _distanceFromLens = 0.0;
                if (buttonCalculate != null) buttonCalculate.IsEnabled = false;
            }
        }

        private void OnTextBoxFocalLengthChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFocalLength();
        }

        private void UpdateFocalLength()
        {
            Double focalLength;
            if (Double.TryParse(textBoxFocalLength.GetLineText(0), out focalLength))
            {
                if ((string) comboBoxFocalLengthUnits.SelectionBoxItem == "mm")
                    _focalLength = focalLength;
                else if ((string) comboBoxFocalLengthUnits.SelectionBoxItem == "cm")
                    _focalLength = ConvertCentimeterToMillimeter(focalLength);
                else if ((string) comboBoxFocalLengthUnits.SelectionBoxItem == "m")
                    _focalLength = ConvertMeterToCentimeter(focalLength);
                else if ((string) comboBoxFocalLengthUnits.SelectionBoxItem == "km")
                    _focalLength = ConvertKilometerToCentimeter(focalLength);
                CheckCanCalculate();
            }
            else
            {
                _focalLength = 0.0;
                if (buttonCalculate != null) buttonCalculate.IsEnabled = false;
            }
        }

        private void ComboBoxActualSizeUnitsDropDownClosed(object sender, EventArgs e)
        {
            UpdateActualSize();
        }

        private void ComboBoxSizeOnSensorUnitsDropDownClosed(object sender, EventArgs e)
        {
            UpdateSizeOnSensor();
        }

        private void ComboBoxDistanceFromLensUnitsDropDownClosed(object sender, EventArgs e)
        {
            UpdateDistanceFromLens();
        }

        private void ComboBoxFocalLengthUnitsDropDownClosed(object sender, EventArgs e)
        {
            UpdateFocalLength();
        }

    }

}

