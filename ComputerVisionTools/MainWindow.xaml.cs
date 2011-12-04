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
            Initialize();
            InitializeStereo();
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
            }
            else
            {
                _sizeOnSensor = 0.0;
            }
            CheckCanCalculate();
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
            }
            else
            {
                _actualSize = 0.0;
            }
            CheckCanCalculate();
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
            }
            else
            {
                _distanceFromLens = 0.0;
            }
            CheckCanCalculate();
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
            }
            else
            {
                _focalLength = 0.0;
            }
            CheckCanCalculate();
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
        
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// Beginning of Stereo methods and fields
        private double _sizeX1;
        private double _sizeX2;
        private double _xStereo; // Calculated from _sizeX1 - _sizeX2
        private double _baseline;
        private double _distanceFromLensStereo;

        private Boolean _canCalculateStereo;
        private double _focalLengthStereo;

        protected void InitializeStereo()
        {
            _sizeX1 = 0.0;
            _sizeX2 = 0.0;
            _baseline = 0.0;
            _distanceFromLensStereo = 0.0;
            _focalLengthStereo = 0.0;
            textBoxX1.Text = "";
            textBoxX2.Text = "";
            textBoxDistanceFromLensStereo.Text = "";
            textBoxFocalLengthStereo.Text = "";
            textBoxBaseline.Text = "";
            _canCalculateStereo = false;
            buttonCalculateStereo.IsEnabled = false;
        }

        private Boolean CanCalculateStereo()
        {
            textBlockFormulaStereo.Text = ""; // Only display formula right after Calculate is clicked.
            _canCalculateStereo = false;
            // We need 3 values to calculate
            if (_sizeX1 == 0.0 && _sizeX2 == 0.0)
            {
                if (_baseline != 0.0 && _distanceFromLensStereo != 0.0 && _focalLengthStereo != 0.0) _canCalculateStereo = true;
            }
            else if (_baseline == 0.0)
            {
                if (_sizeX1 != 0.0 && _sizeX2 != 0.0 && _distanceFromLensStereo != 0.0 && _focalLengthStereo != 0.0) _canCalculateStereo = true;
            }
            else if (_distanceFromLensStereo == 0.0)
            {
                if (_baseline != 0.0 && _baseline != 0.0 && _focalLengthStereo != 0.0) _canCalculateStereo = true;
            }
            else if (_focalLengthStereo == 0.0)
            {
                if (_sizeX1 != 0.0 && _sizeX1 != 0.0 && _baseline != 0.0 && _distanceFromLensStereo != 0.0) _canCalculateStereo = true;
            }
            if (_canCalculateStereo)
            {
                buttonCalculateStereo.IsEnabled = true;
            }
            else
            {
                buttonCalculateStereo.IsEnabled = false;
            }
            if (_sizeX1 > _sizeX2) _xStereo = _sizeX1 - _sizeX2;
            else _xStereo = _sizeX2 - _sizeX1;
            labelDerivedX.Content = _xStereo.ToString();
            return _canCalculateStereo;
        }

        private void CheckCanCalculateStereo()
        {
            CanCalculateStereo();
        }
        
        private void TextBoxX1TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateX1();
        }

        private void comboBoxX1Units_DropDownClosed(object sender, EventArgs e)
        {
            UpdateX1();
        }

        private void UpdateX1()
        {
            Double x1;
            if (Double.TryParse(textBoxX1.GetLineText(0), out x1))
            {
                if ((string)comboBoxX1Units.SelectionBoxItem == "mm")
                    _sizeX1 = x1;
                else if ((string)comboBoxX1Units.SelectionBoxItem == "cm")
                    _sizeX1 = ConvertCentimeterToMillimeter(x1);
                else if ((string)comboBoxX1Units.SelectionBoxItem == "m")
                    _sizeX1 = ConvertMeterToCentimeter(x1);
                else if ((string)comboBoxX1Units.SelectionBoxItem == "km")
                    _sizeX1 = ConvertKilometerToCentimeter(x1);
            }
            else
            {
                _sizeX1 = 0.0;
            }
            CheckCanCalculateStereo();
        }


        private void TextBoxX2TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateX2();
        }

        private void ComboBoxX2UnitsDropDownClosed(object sender, EventArgs e)
        {
            UpdateX2();
        }

        private void UpdateX2()
        {
            Double x2;
            if (Double.TryParse(textBoxX2.GetLineText(0), out x2))
            {
                if ((string)comboBoxX2Units.SelectionBoxItem == "mm")
                    _sizeX2 = x2;
                else if ((string)comboBoxX2Units.SelectionBoxItem == "cm")
                    _sizeX2 = ConvertCentimeterToMillimeter(x2);
                else if ((string)comboBoxX2Units.SelectionBoxItem == "m")
                    _sizeX2 = ConvertMeterToCentimeter(x2);
                else if ((string)comboBoxX2Units.SelectionBoxItem == "km")
                    _sizeX2 = ConvertKilometerToCentimeter(x2);
            }
            else
            {
                _sizeX2 = 0.0;
            }
            CheckCanCalculateStereo();
        }


        private void OnTextBoxBaselineTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateBaseline();
        }

        private void OnComboBoxBaselineUnitsDropDownClosed(object sender, EventArgs e)
        {
            UpdateBaseline();
        }

        private void UpdateBaseline()
        {
            Double baseline;
            if (Double.TryParse(textBoxBaseline.GetLineText(0), out baseline))
            {
                if ((string)comboBoxBaselineUnits.SelectionBoxItem == "mm")
                    _baseline = baseline;
                else if ((string)comboBoxBaselineUnits.SelectionBoxItem == "cm")
                    _baseline = ConvertCentimeterToMillimeter(baseline);
                else if ((string)comboBoxBaselineUnits.SelectionBoxItem == "m")
                    _baseline = ConvertMeterToCentimeter(baseline);
                else if ((string)comboBoxBaselineUnits.SelectionBoxItem == "km")
                    _baseline = ConvertKilometerToCentimeter(baseline);
            }
            else
            {
                _baseline = 0.0;
            }
            CheckCanCalculateStereo();
        }


        private void OnTextBoxDistanceFromLensStereoTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateDistanceFromLensStereo();
        }

        private void OnComboBoxDistanceFromLensStereoUnitsDropDownClosed(object sender, EventArgs e)
        {
            UpdateDistanceFromLensStereo();
        }

        private void UpdateDistanceFromLensStereo()
        {
            Double distanceFromLensStereo;
            if (Double.TryParse(textBoxDistanceFromLensStereo.GetLineText(0), out distanceFromLensStereo))
            {
                if ((string)comboBoxDistanceFromLensUnitsStereo.SelectionBoxItem == "mm")
                    _distanceFromLensStereo = distanceFromLensStereo;
                else if ((string)comboBoxDistanceFromLensUnitsStereo.SelectionBoxItem == "cm")
                    _distanceFromLensStereo = ConvertCentimeterToMillimeter(distanceFromLensStereo);
                else if ((string)comboBoxDistanceFromLensUnitsStereo.SelectionBoxItem == "m")
                    _distanceFromLensStereo = ConvertMeterToCentimeter(distanceFromLensStereo);
                else if ((string)comboBoxDistanceFromLensUnitsStereo.SelectionBoxItem == "km")
                    _distanceFromLensStereo = ConvertKilometerToCentimeter(distanceFromLensStereo);
            }
            else
            {
                _distanceFromLensStereo = 0.0;
            }
            CheckCanCalculateStereo();
        }


        private void OnTextBoxFocalLengthStereoTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFocalLengthStereo();
        }

        private void OnComboBoxFocalLengthStereoUnitsDropDownClosed(object sender, EventArgs e)
        {
            UpdateFocalLengthStereo();
        }

        private void UpdateFocalLengthStereo()
        {
            Double focalLengthStereo;
            if (Double.TryParse(textBoxFocalLengthStereo.GetLineText(0), out focalLengthStereo))
            {
                if ((string)comboBoxFocalLengthUnitsStereo.SelectionBoxItem == "mm")
                    _focalLengthStereo = focalLengthStereo;
                else if ((string)comboBoxFocalLengthUnitsStereo.SelectionBoxItem == "cm")
                    _focalLengthStereo = ConvertCentimeterToMillimeter(focalLengthStereo);
                else if ((string)comboBoxFocalLengthUnitsStereo.SelectionBoxItem == "m")
                    _focalLengthStereo = ConvertMeterToCentimeter(focalLengthStereo);
                else if ((string)comboBoxFocalLengthUnitsStereo.SelectionBoxItem == "km")
                    _focalLengthStereo = ConvertKilometerToCentimeter(focalLengthStereo);
            }
            else
            {
                _focalLengthStereo = 0.0;
            }
            CheckCanCalculateStereo();
        }


        private void ButtonCalculateStereoClick(object sender, RoutedEventArgs e)
        {
            if (CanCalculateStereo()) // Shouldn't be here if we can't calculate, but check to be sure.
            {
                if (_xStereo == 0.0)
                {
                    _xStereo = _baseline*(_focalLengthStereo/_distanceFromLensStereo);
                    labelDerivedX.Content = _xStereo.ToString();
                    //comboBoxSizeOnSensorUnits.SelectedIndex = 0;
                    textBlockFormulaStereo.Text = "X = B * (f / Z)";
                }
                else if (_baseline == 0.0)
                {
                    _baseline = _xStereo/(_focalLengthStereo/_distanceFromLensStereo);
                    textBoxBaseline.Text = _baseline.ToString();
                    comboBoxBaselineUnits.SelectedIndex = 0;
                    textBlockFormulaStereo.Text = "B = x1 - x2 / (f / Z)";
                }
                else if (_distanceFromLensStereo == 0.0)
                {
                    _distanceFromLensStereo = (_focalLengthStereo*_baseline)/_xStereo;
                    textBoxDistanceFromLensStereo.Text = _distanceFromLensStereo.ToString();
                    comboBoxDistanceFromLensUnitsStereo.SelectedIndex = 0;
                    textBlockFormulaStereo.Text = "Z = (f * B) / x1 - x2)";
                }
                else if (_focalLengthStereo == 0.0)
                {
                    _focalLengthStereo = _xStereo*(_distanceFromLensStereo/_baseline);
                    textBoxFocalLengthStereo.Text = _focalLengthStereo.ToString();
                    comboBoxFocalLengthUnits.SelectedIndex = 0;
                    textBlockFormulaStereo.Text = "f = (x1 - x2) * (Z / B)";
                }
            }
        }

        private void ButtonResetStereoClick(object sender, RoutedEventArgs e)
        {
            InitializeStereo();
        }

    }

}

