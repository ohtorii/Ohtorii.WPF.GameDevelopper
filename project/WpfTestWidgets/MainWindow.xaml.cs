using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

namespace WpfTestWidgets
{
	public class TextBoxViewModel : INotifyPropertyChanged
	{
		// INotifyPropertyChangedインターフェースの実装
		public event PropertyChangedEventHandler PropertyChanged;

		// 変更通知
		public void RaisePropertyChanged([CallerMemberName] string propertyName=null)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		// テキストボックスの入力プロパティ
		private string _Message="12345";
		public string Message
		{
			get { return _Message; }
			set { if (_Message != value) { _Message = value; RaisePropertyChanged(); } }
		}
	}

	public class MyIntViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		private int _Value = 12;
		public int HitPoint
		{
			get { return _Value; }
			set { if (_Value != value) { _Value = value; RaisePropertyChanged(); } }
		}
	}


	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		TextBoxViewModel vmTextBox = new TextBoxViewModel();
		MyIntViewModel vmMyInt=new MyIntViewModel();
		public MainWindow()
		{
			InitializeComponent();
			this.intHp.DataContext = vmMyInt;
			this.textBlock.DataContext= vmMyInt;
			//this.DataContext = vm;
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			vmMyInt.HitPoint--;
		}
	}
}
