using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
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
using System.Xml;
using System.Xml.Linq;
using WpfTestWidgets.Utils;

namespace WpfTestWidgets
{
    /// <summary>
    /// IntControl.xaml の相互作用ロジック
    /// </summary>
    public partial class IntControl : UserControl
    {
		const int m_DefaultValue = 0;
		ViewModel _ViewModel = new ViewModel();


		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register(
				"Value",
				typeof(int),
				typeof(IntControl),
				new FrameworkPropertyMetadata(m_DefaultValue,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)	
		);
		public int Value
		{
			get { return (int)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); _ViewModel.Value = value; }
		}

		public static readonly DependencyProperty DefaultValueProperty =
			DependencyProperty.Register(
				"DefaultValue",
				typeof(int),
				typeof(IntControl),
				new FrameworkPropertyMetadata(m_DefaultValue,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)	
		);
		public int DefaultValue
		{
			get { return (int)GetValue(DefaultValueProperty); }
			set { SetValue(DefaultValueProperty, value); _ViewModel.DefaultValue = value; }
		}
		
		public IntControl()
        {
            InitializeComponent();
			this.btnSetDefault.DataContext = _ViewModel;
		}

	

		class ViewModel : INotifyPropertyChanged
		{
			int _Value;
			int _DefaultValue;
			public event PropertyChangedEventHandler PropertyChanged;			
			public void Update()
			{
				const bool DummyBool = false;
				IsEnable = DummyBool;
			}

			public int Value
			{
				set
				{
					if (_Value == value)
					{
						//pass
					}
					else { 
						_Value = value;
					}
					Update();
				}
			}
			
			public int DefaultValue
			{
				set
				{
					if (_DefaultValue == value)
					{
						//pass
					}
					else {
						_DefaultValue = value;
					}
					Update();
				}
			}
			
			public void RaisePropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			
			public bool IsEnable
			{
				get { return m_DefaultValue != _Value; }
				set { RaisePropertyChanged(); }
			}

		}

		#region Event
		private void btnMinus_Click(object sender, RoutedEventArgs e)
		{
			Value--;
		}

		private void btnPlus_Click(object sender, RoutedEventArgs e)
		{
			Value++;
		}
		private void textBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (_ViewModel == null)
			{
				return;
			}
			int result;
			if (! int.TryParse(((TextBox)sender).Text, out result)) {
				return;
			}
			Value = result;
		}

		readonly HashSet<Key> IntKeys = new HashSet<Key>() {
			Key.OemPlus,Key.OemMinus,
			Key.D0,Key.NumPad0,
			Key.D1,Key.NumPad1,
			Key.D2,Key.NumPad2,
			Key.D3,Key.NumPad3,
			Key.D4,Key.NumPad4,
			Key.D5,Key.NumPad5,
			Key.D6,Key.NumPad6,
			Key.D7,Key.NumPad8,
			Key.D9,Key.NumPad9,
		};
		readonly HashSet<Key> ContrlKeys = new HashSet<Key>()
		{
			Key.Enter,Key.Return,
			Key.Delete, Key.Insert, Key.Back,
			Key.Up,Key.Down,Key.Left,Key.Right,
		};
		private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (ContrlKeys.Contains(e.Key))
			{
				e.Handled = false;
				return;
			}
			if (! IntKeys.Contains(e.Key))
			{
				e.Handled= true;
				return;
			}
			e.Handled = false;
		}
		#endregion

		private void btnSetDefault_Click(object sender, RoutedEventArgs e)
		{
			Value = DefaultValue;
		}

		private void btnCalc_Click(object sender, RoutedEventArgs e)
		{
			var btn = (Button)sender;
			var underButton = DpiHelper.UpdateScreenPositionWidthCurrentDPI(this.PointToScreen(btn.TranslatePoint(new Point(0, btn.ActualHeight), this)), this);
			Window window = new Window
			{
				Title = "",
				WindowStyle=WindowStyle.None,
				ResizeMode =ResizeMode.NoResize,
				SizeToContent=SizeToContent.WidthAndHeight,
				Top = underButton.Y,
				Left = underButton.X,
				WindowStartupLocation =WindowStartupLocation.Manual,
				Content = new NumPadControl()
			};
			
			window.Deactivated += (object? sender, EventArgs e) => { (sender as Window)?.Close(); };

			window.Show();
			
		}
	}

	public class NumberValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			if (((string)value).Any(c => !char.IsNumber(c)))
			{
				return new ValidationResult(false, "Invalid character.");
			}
			else
			{
				return new ValidationResult(true, null);
			}
		}
	}

#if false
	[ValueConversion(typeof(bool), typeof(bool))]
	public class InverseBooleanConverter : IValueConverter
	{
	#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture)
		{
			if (targetType != typeof(bool))
				throw new InvalidOperationException("The target must be a boolean");

			return !(bool)value;
		}

		public object ConvertBack(object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture)
		{
			throw new NotSupportedException();
		}

	#endregion
	}
#endif
}
