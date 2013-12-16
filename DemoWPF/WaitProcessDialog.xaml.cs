using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Windows.Threading;

namespace DemoWPF
{
	/// <summary>
	/// Interaction logic for ModalDialog.xaml
	/// </summary>
    public partial class WaitProcessDialog : UserControl
	{
		public WaitProcessDialog()
		{
			InitializeComponent();
			Visibility = Visibility.Hidden;
		}
		private bool _hideRequest = false;

        #region Owner
        public UIElement Owner
        {
            get { return (UIElement)GetValue(OwnerProperty); }
            set {SetValue(OwnerProperty,value);}
        }
        public static readonly DependencyProperty OwnerProperty =
            DependencyProperty.Register("Owner", typeof(UIElement), typeof(WaitProcessDialog), new UIPropertyMetadata(null));
        #endregion

		#region Message

		public string Message
		{
			get { return (string)GetValue(MessageProperty); }
			set { SetValue(MessageProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Message.
		// This enables animation, styling, binding, etc...
		public static readonly DependencyProperty MessageProperty =
			DependencyProperty.Register(
                "Message", typeof(string), typeof(WaitProcessDialog), new UIPropertyMetadata(string.Empty));

		#endregion

        private Visibility _animationVisibility;
        public Visibility AnimationVisibility
        {
            get { return _animationVisibility; }
            private set
            {
                _animationVisibility = value;
            }
        }


		public void ShowHandlerDialog(string message)
		{
			Message = message;
			Visibility = Visibility.Visible;
            _animationVisibility = System.Windows.Visibility.Visible;
            if (Owner != null)
            { Owner.IsEnabled = false; }
			_hideRequest = false;
            //while (!_hideRequest)
            //{
            //    // HACK: Stop the thread if the application is about to close
            //    if (this.Dispatcher.HasShutdownStarted ||
            //        this.Dispatcher.HasShutdownFinished)
            //    {
            //        break;
            //    }

            //    // HACK: Simulate "DoEvents"
            //    this.Dispatcher.Invoke(
            //        DispatcherPriority.Background,
            //        new ThreadStart(delegate { }));
            //    Thread.Sleep(20);
            //}
		}

        public void HideHandlerDialog()
		{
            if(Visibility == System.Windows.Visibility.Visible)
            {
                _hideRequest = true;
                Visibility = Visibility.Hidden;
                _animationVisibility = System.Windows.Visibility.Hidden;
                if (Owner != null)
                { Owner.IsEnabled = true; }
            }
		}
	}
}
