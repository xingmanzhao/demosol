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
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Threading;
using System.Deployment.Application;
using System.Reflection;
using Technewlogic.Samples.WpfModalDialog;
using System.ComponentModel;

namespace DemoWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private RoutedCommand messageBoxCommand;
        public static RoutedCommand MessageBoxCommand = new RoutedCommand();

        public static ExportDataCommand ExportCMD = new ExportDataCommand(new ExecutedRoutedEventHandler(ExportECdBinding_Executed));
        public MainWindow()
        {


            InitializeComponent();
           // mdMessage.SetParent(gridDemo);
            //messageBoxCommand = new RoutedCommand("MessageBoxCommand",typeof(DataGrid));
            //CommandBinding cmdBinding = new CommandBinding(MessageBoxCommand, cmdBinding_Executed, null);
            //this.CommandBindings.Add(cmdBinding);
            //this.btnCommand.Command = MessageBoxCommand;
            //this.btnCommand.Command = ExportCMD;
            
            this.Title = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

        }

       static void ExportECdBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is DataGrid)
            {
                DataGrid dg = (DataGrid)sender;
                dg.Export();
            }
        }

        void cmdBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.OriginalSource is DataGridCell)
            {
                DataGridCell cell = (DataGridCell)e.OriginalSource;
                if (e.Parameter is DataGrid)
                { }
            }
            if (e.Source is DataGrid)
            {
                DataGrid dg = (DataGrid)e.Source;
                dg.Export();
            }
        }

        private void btnTask_Click(object sender, RoutedEventArgs e)
        {
            Task t = new Task(() => {
                Dispatcher.Invoke((Action)delegate() { this.txtTask.Text = "dddddd"; });
            });
            t.Start();


            DataGrid dg = new DataGrid();
            dg.Export();

            //Task<string> tt = Task.Factory.StartNew<string>(() => {
            //    Dispatcher.Invoke((Action)delegate() { this.txtTask.Text = "dfdfd";});
            //    return "asdad"; }
            //    );


        }

        private void DoAction()
        {
            Dispatcher.CurrentDispatcher.Invoke((Action)delegate()
            {
                this.txtTask.Text = string.Format("do action {0}", string.Empty);
            });
        }

        private void btnTaskSchedular_Click(object sender, RoutedEventArgs e)
        {
            Task t = new Task(() => {
                Dispatcher.Invoke((Action)delegate() {
                    this.txtTask.Text = "afsd";
                });
            });
            t.Start();

        }

        private void btnAction_Click(object sender, RoutedEventArgs e)
        {
            Action a0 = () => { OutString(); };
            a0();
            Action<string> a = delegate(string s) { OutString(s); };
            a("adf");
            Action<string> ab = (s) => { OutString(s); };
            ab("dfdfdfd");

            Action<string, string> abcd = (t1, t2) => { OutString(t1, t2); };
            abcd("ad","dfdf");
        }

        private void OutString()
        {
            Dispatcher.Invoke((Action)delegate()
            {
                this.txtTask.Text = string.Format("{0},{1}", "arg0", this.txtTask.Text);
            });
        }

        private void OutString(string result)
        {
            Dispatcher.Invoke((Action)delegate(){
                this.txtTask.Text = string.Format("{0},{1}",result,this.txtTask.Text);
            });
        }
        private void OutString(string t1,string t2)
        {
            Dispatcher.Invoke((Action)delegate()
            {
                this.txtTask.Text = string.Format("{0}-{1},{2}", t1, t2, this.txtTask.Text);
            });
        }

        private void btnFunc_Click(object sender, RoutedEventArgs e)
        {
            //Func<string> f0 = () => { return OutFunc(); };
            //Action<string> a = delegate(string s) { OutString(s); };
            //string f0s = f0();
            //a(f0s);

            //Func<string, string> f1 = (s) => { return OutFunc(s); };
            //string f1s = f1("s1");
            //Action<string> a1 = ss => { OutString(ss); };
            //a1(f1s);

            Func<string, string, string> f2 = (s1, s2) => { return OutFunc(s1,s2); };
            string f2s = f2("f1","f2");
            Action<string, string> a2 = (s1, s2) => { OutString(s1,s2); };
            a2("dfdf",f2s);


        }

        private string OutFunc()
        {
            return "arg0";
        }

        private string OutFunc(string a)
        {
            return string.Format("arg1:{0}",a);
        }

        private string OutFunc(string a,string b)
        {
            return string.Format("arg2:{0}-{1}", a,b);
        }

        private void btnAppDeploy_Click(object sender, RoutedEventArgs e)
        {
              
        }


        int refValue = 10;
        int value = 10;
        private void btnInterLocked_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() => {
                AddInterLockedValue();
            });
            Task.Factory.StartNew(() => {
                ReduceInterLockedValue();
            });
        }

        private void AddInterLockedValue()
        {
            Interlocked.Add(ref refValue, 10);
            Dispatcher.Invoke((Action)delegate() 
            {
                this.txtTask.Text = refValue.ToString();
            });
        }

        private void ReduceInterLockedValue()
        {
            Interlocked.Add(ref refValue, -10);
            Dispatcher.Invoke((Action)delegate()
            {
                this.txtTask.Text = refValue.ToString();
            });
        }

        private void btnProcessorCount_Click(object sender, RoutedEventArgs e)
        {
            this.txtTask.Text = string.Format("{0}", Environment.ProcessorCount);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int[] array = { 1, 2, 3 };

                int a = array[4];
            }
            catch (Exception ex)
            {
                string exs = ex.Message;
            }
        }

        private  int MaxPointCount = 20;
        private  int RealPointCount = 30;
        private void btnCreateDataGrid_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RealPointCount = int.Parse(this.txtNumberCount.Text);

                List<Person> personList = new List<Person>();
                int index = 0;
                while (index < RealPointCount)
                {
                    Person p = new Person();
                    p.Name = string.Format("abc{0}",index+1);
                    p.Age = 15  + index;
                    p.Call1 = string.Format("Caller1 :  {0}", index + 1);
                    p.Call2 = string.Format("Caller 2:  {0}", index + 1);
                    p.Call3 = string.Format("Caller3 :  {0}", index + 1);
                    p.Call4 = string.Format("Caller4:  {0}", index + 1);
                    p.Call5 = string.Format("Caller5 :  {0}", index + 1);
                    p.Call6 = string.Format("Caller6 :  {0}", index + 1);
                    p.Call7 = string.Format("Caller7 :  {0}", index + 1);
                    p.Call8 = string.Format("Caller8 :  {0}", index + 1);
                    p.Call9 = string.Format("Caller9 :  {0}", index + 1);
                    p.Call10 = string.Format("Caller10 :  {0}", index + 1);
                    p.Call11 = string.Format("Caller11:  {0}", index + 1);
                    p.Call12 = string.Format("Caller12 :  {0}", index + 1);
                    p.Call13 = string.Format("Caller13 :  {0}", index + 1);
                    p.Call14 = string.Format("Caller14 :  {0}", index + 1);
                    p.Call15 = string.Format("Caller15 :  {0}", index + 1);
                    p.Call16 = string.Format("Caller16 :  {0}", index + 1);
                    p.Call17 = string.Format("Caller17 :  {0}", index + 1);
                    p.Call18 = string.Format("Caller18 :  {0}", index + 1);
                    p.Call19 = string.Format("Caller19 :  {0}", index + 1);
                    p.Call20 = string.Format("Caller20 :  {0}", index + 1);
                    personList.Add(p);
                    index++;
                }
                this.dgPersons.ItemsSource = personList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDescribeDataGrid_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.dgPersons.Export();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CopyAllToClipboardMenuItemClicked(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                ContextMenu cm = mi.CommandParameter as ContextMenu;
                if (cm != null)
                {
                    DataGrid targetGrid = cm.PlacementTarget as DataGrid;
                    CopyAllDataGirdByMenuItem(targetGrid);
                }
            }
        }

        private void CopyAllDataGirdByMenuItem(DataGrid targetGrid)
        {
            MaxPointCount = int.Parse(this.txtMaxPointCount.Text);
            if (targetGrid != null)
            {
                if ( targetGrid.Items.Count < MaxPointCount)
                {
                    targetGrid.SelectAllCells();
                    ApplicationCommands.Copy.Execute(null, targetGrid);
                    targetGrid.UnselectAllCells();
                }
                else
                {
                    //MessageBoxResult mbr = MessageBox.Show(string.Format("The DataGrid max count has out of the range , you could export all the data or just only copy {0} rows data", MaxPointCount), "", MessageBoxButton.YesNoCancel, MessageBoxImage.Information, MessageBoxResult.Yes, MessageBoxOptions.DefaultDesktopOnly);
                    
                    //if (mbr == MessageBoxResult.Yes)
                    //{ 
                    //// export all data
                    //    targetGrid.Export();
                    //}
                    //else if (mbr == MessageBoxResult.No)
                    //{
                    //// copy the max rows
                    //    this.CopyMaxRows(targetGrid);
                    //}
                    //else
                    //{
                    //    // cancel
                    //    return;
                    //}
                    
                }
            }
        }

        private void CopySelectedDataGirdByKeyBoard(DataGrid targetGrid)
        {
            MaxPointCount = int.Parse(this.txtMaxPointCount.Text);
            if (targetGrid != null && targetGrid.SelectedItems != null)
            {
                if (targetGrid.SelectedItems.Count > MaxPointCount)
                {
                    MessageBoxResult mbr = MessageBox.Show(string.Format("The DataGrid max count has out of the range , you could export all the data or just only copy {0} rows data", MaxPointCount), "", MessageBoxButton.YesNoCancel, MessageBoxImage.Information, MessageBoxResult.Yes, MessageBoxOptions.DefaultDesktopOnly);
                    if (mbr == MessageBoxResult.Yes)
                    {
                        //// export all data
                        //targetGrid.ExportSelectedItems();
                    }
                    else if (mbr == MessageBoxResult.No)
                    {
                        // copy the max rows
                        this.CopyMaxRows(targetGrid);
                    }
                    else
                    {
                        // cancel
                        return;
                    }

                }
            }
        }

        private void CopyMaxRows(DataGrid targetGrid)
        {
            DataGrid newDataGrid = new DataGrid();
            foreach (DataGridColumn col in targetGrid.Columns)
            {
                if (col is DataGridTextColumn)
                {
                    DataGridTextColumn textCol = (DataGridTextColumn)col;
                    newDataGrid.Columns.Add(new DataGridTextColumn() { Binding = textCol.Binding, ClipboardContentBinding = textCol.ClipboardContentBinding, Header = textCol.Header });
                }
                else if (col is DataGridTemplateColumn)
                {
                    DataGridTemplateColumn temCol = (DataGridTemplateColumn)col;
                    newDataGrid.Columns.Add(new DataGridTemplateColumn() { ClipboardContentBinding = temCol.ClipboardContentBinding, CellTemplate = temCol.CellTemplate, Header = temCol.Header, HeaderTemplate = temCol.HeaderTemplate });
                }
            }

            if (targetGrid.ItemsSource != null)
            {
                System.Collections.IEnumerator ienum = targetGrid.ItemsSource.GetEnumerator();
                bool loop = true;
                int index = 0;

                while (index < MaxPointCount && loop)
                {
                    index++;
                    loop = ienum.MoveNext();
                    newDataGrid.Items.Add(ienum.Current);
                }
                newDataGrid.SelectAllCells();
                ApplicationCommands.Copy.Execute(null, newDataGrid);
                newDataGrid.UnselectAllCells();
            }
        
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((MenuItem)sender).Parent is ContextMenu)
                {
                   ContextMenu cm =  (ContextMenu)((MenuItem)sender).Parent;
                   if (cm.PlacementTarget is DataGrid)
                   {
                       DataGrid dg = (DataGrid)cm.PlacementTarget;
                       dg.Export();
                   }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ExportProcessValue(int value)
        { 
        }

        private void dgPersons_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            object obj = e.Item;
            
        }

        Key firstKey = Key.None;
        private void dgPersons_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
                {
                    firstKey = e.Key;
                }
                this.txtRecord.AppendText(string.Format("DataGrid KeyDown: {0},{1}", e.Key, System.Environment.NewLine));
              
           }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgPersons_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == firstKey)
            {
                firstKey = Key.None;
            }
            if ((firstKey == Key.LeftCtrl || firstKey == Key.RightCtrl) && e.Key == Key.C)
            {
                CopySelectedDataGirdByKeyBoard((DataGrid)sender);
                firstKey = Key.None;
            }
            this.txtRecord.AppendText(string.Format("DataGrid KeyUp: {0},{1}", e.Key, System.Environment.NewLine));
            ((DataGrid)sender).Focus();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
                {
                    firstKey = e.Key;
                }

                if ((firstKey == Key.LeftCtrl || firstKey == Key.RightCtrl) && e.Key == Key.C)
                {
                }
                this.txtRecord.AppendText(string.Format("Windows KeyDown: {0},{1}", e.Key, System.Environment.NewLine));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == firstKey)
            {
                firstKey = Key.None;
            }
            this.txtRecord.AppendText(string.Format("Windows KeyUp: {0},{1}", e.Key,System.Environment.NewLine));
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            this.txtRecord.Clear();
        }

        bool alter = true;
        private void btnAlterWindowKeyDelegate_Click(object sender, RoutedEventArgs e)
        {
            if (alter)
            {
                this.KeyUp += Window_KeyUp;
                this.KeyDown += Window_KeyDown;
                alter = false;
            }
            else
            {
                this.KeyDown -= Window_KeyDown;
                this.KeyUp -= Window_KeyUp;
                alter = true;
            }



        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Clipboard operation occured!");
        }

        private void dgPersons_MouseDown(object sender, MouseButtonEventArgs e)
        {
            dgPersons.Focus();
        }

        private void btnCreateDataGridArray_Click(object sender, RoutedEventArgs e)
        {
            List<GroupedMemoryInfo> pArray = new List<GroupedMemoryInfo>();

            GroupedMemoryInfo g = new GroupedMemoryInfo();
            pArray.Add(g);
            g.TotalBytes = 12;
            g.AverageBytes = 6;
            BinaryMemoryInfo bi = new BinaryMemoryInfo();
            bi.Callers = new CallerDictionary();
            bi.Callers.Add(0, new Caller() { Function = "Caller 1 : OS_Task" });
            bi.Callers.Add(1, new Caller() { Function = "Caller 2 : OS_UI" });
            bi.SXCallers = new CallerDictionary();
            bi.SXCallers.Add(0, new Caller() { Function = "SXCaller 1 : OS_IO" });
            bi.SXCallers.Add(1, new Caller() { Function = "SXCaller 2 : OS_INPUT" });
            g.Add(bi);

            this.AllocationsByCallStackGroupsDataGrid.ItemsSource = pArray;
        }

        private void ExportDataToCSVMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuItem mi = sender as MenuItem;
                if (mi != null)
                {
                    ContextMenu cm = mi.CommandParameter as ContextMenu;
                    if (cm != null)
                    {
                        DataGrid targetGrid = cm.PlacementTarget as DataGrid;
                        if (targetGrid != null)
                        {
                            BackgroundWorker exportWorker = new BackgroundWorker();
                            exportWorker.DoWork += new DoWorkEventHandler(_exportWorker_DoWork);
                            exportWorker.RunWorkerAsync(targetGrid);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void _exportWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate()
            {
                DataGrid targetGrid = e.Argument as DataGrid;
                if (targetGrid != null)
                {
                    targetGrid.Export();
                }
            });
        }

        private void btnMasterBranch_Click(object sender, RoutedEventArgs e)
        {
            string msg = "add maintain branch : try to merge in git";
            MessageBox.Show(msg);
        }

        private void btnDevelopBranch_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder msgBuilder = new StringBuilder();
            msgBuilder.Append("develop branch msg : ");
            msgBuilder.Append("try to merge , rebase, ");
            msgBuilder.Append("add develop branch, ");
            msgBuilder.Append("add maintain branch, ");
            msgBuilder.Append("add dev_a branch, ");
            msgBuilder.Append("add msg on dev_a branch, ");
            msgBuilder.Append("add msg2 on dev_a branch, ");
            msgBuilder.Append("add msg3 on dev_a branch, ");
            msgBuilder.Append("add msg4 on dev_a branch, ");
            msgBuilder.Append("add msg5 on dev_a branch, ");
            msgBuilder.Append("add msg6 on dev_a branch, ");
            msgBuilder.Append("add msg7 on dev_a branch, ");
            msgBuilder.Append("add msg8 on dev_a branch, ");
            msgBuilder.Append("add msg0 on dev_e branch, ");
            msgBuilder.Append("add msg1 on dev_e branch, ");
            msgBuilder.Append("add msg2 on dev_e branch, ");
            msgBuilder.Append("add msg3 on dev_e branch, ");
            msgBuilder.Append("add passage0 on master branch, ");
            msgBuilder.Append("add passage1 on master branch, ");
            MessageBox.Show(msgBuilder.ToString());
        }
    }

    public class ExportTenOfIntegerTaskSchedular : TaskScheduler
    {
        private LinkedList<Task> _schedularTask = new LinkedList<Task>();

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            try
            {
                bool doFlg = Monitor.TryEnter(_schedularTask);
                if (doFlg) return _schedularTask.ToArray<Task>();
                else throw new NotSupportedException();
            }
            finally
            {
                Monitor.Exit(_schedularTask);
            }
        }

        protected override void QueueTask(Task task)
        {
            lock (_schedularTask)
            {
                _schedularTask.AddLast(task);
            }
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            throw new NotImplementedException();
        }

        protected sealed override bool TryDequeue(Task task)
        {
            try
            {
                bool doFlg = Monitor.TryEnter(_schedularTask);
                if (doFlg) return _schedularTask.Remove(task);
                else throw new NotSupportedException();
            }
            finally
            {
                Monitor.Exit(_schedularTask);
            }

        }
    }
}
