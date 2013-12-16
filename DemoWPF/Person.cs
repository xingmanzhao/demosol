using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace DemoWPF
{
    public class RangeEnabledObservableCollection<T> : ObservableCollection<T>
    {
        public void InsertRange(IEnumerable<T> items)
        {
            this.CheckReentrancy();
            foreach (var item in items)
            {
                this.Items.Add(item);
            }
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }

    public class GroupedMemoryInfo : ObservableCollection<BinaryMemoryInfo>, INotifyPropertyChanged
    {
        public int TotalBytes { get; set; }
        public int AverageBytes { get; set; }
    }

    public class BinaryMemoryInfo : IEquatable<BinaryMemoryInfo>
    {
        public uint Address { get; set; }
        public string AddressString { get { return Address.ToString("X"); } }
        public string AllocationType { get; set; }
        public Int32 AllocationSizeReserved { get; set; }
        public Int32 AllocationSizeRequested { get; set; }
        public DateTime AllocationTimestamp { get; set; }
        public DateTime DeallocationTimestamp { get; set; }
        public string SourceFileName { get; set; }
        public CallerDictionary Callers { get; set; }

        public CallerDictionary SXCallers { get; set; }
        public string SXNamespace { get; set; }
        public long BinaryLogLineNumber { get; set; }

        public bool Matched { get; set; }

        public BinaryMemoryInfo()
        {
        }

        public BinaryMemoryInfo(UInt32 address, Int32 task)
            : this()
        {
            this.Address = address;
        }

        public bool Equals(BinaryMemoryInfo other)
        {
            if (!this.Matched)
            {
                if (this.Callers.Equals(other.Callers))
                {
                    this.Matched = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(1000);
            builder.Append(AllocationTimestamp.ToString("dd/MM HH:mm:ss.fffffff") + " ");
            builder.Append(Address + " ");
            builder.Append(SourceFileName + " ");
            builder.Append(SXNamespace + " ");
            builder.Append(AllocationType + " ");
            builder.Append(AllocationSizeReserved + " ");
            builder.Append(AllocationSizeRequested + " ");
            if (Callers != null)
            {
                if (Callers[0] != null) builder.Append(Callers[0].FullName + " ");
                if (Callers[1] != null) builder.Append(Callers[1].FullName + " ");
                if (Callers[2] != null) builder.Append(Callers[2].FullName + " ");
                if (Callers[3] != null) builder.Append(Callers[3].FullName + " ");
                if (Callers[4] != null) builder.Append(Callers[4].FullName + " ");
                if (Callers[5] != null) builder.Append(Callers[5].FullName + " ");
                if (Callers[6] != null) builder.Append(Callers[6].FullName + " ");
                if (Callers[7] != null) builder.Append(Callers[7].FullName + " ");
            }
            if (SXCallers != null)
            {
                if (SXCallers[0] != null) builder.Append(SXCallers[0].FullName + " ");
                if (SXCallers[1] != null) builder.Append(SXCallers[1].FullName + " ");
                if (SXCallers[2] != null) builder.Append(SXCallers[2].FullName + " ");
                if (SXCallers[3] != null) builder.Append(SXCallers[3].FullName + " ");
                if (SXCallers[4] != null) builder.Append(SXCallers[4].FullName + " ");
                if (SXCallers[5] != null) builder.Append(SXCallers[5].FullName + " ");
                if (SXCallers[6] != null) builder.Append(SXCallers[6].FullName + " ");
                if (SXCallers[7] != null) builder.Append(SXCallers[7].FullName);
            }

            return builder.ToString();
        }
    }

    public class CallerDictionary : IEquatable<CallerDictionary>
    {
        public Dictionary<byte, Caller> Dictionary;

        public int Count
        {
            get
            {
                if (Dictionary == null)
                {
                    return 0;
                }
                else
                {
                    return Dictionary.Count;
                }
            }
        }

        public CallerDictionary()
        {
            Dictionary = new Dictionary<byte, Caller>();
        }

        public bool Equals(CallerDictionary other)
        {
            if (this == other) return true;
            if (this.Dictionary == null || other.Dictionary == null) return false;
            if (this.Count != other.Count) return false;

            foreach (var item in Dictionary)
            {
                Caller otherValue;
                //othervalue is null?
                if (!other.TryGetValue(item.Key, out otherValue)) return false;
                if (!item.Value.Equals(otherValue)) return false;
            }
            return true;
        }

        public Caller this[byte key]
        {
            get
            {
                Caller value = null;
                if (Dictionary != null && Dictionary.TryGetValue(key, out value))
                {
                    return value;
                }
                else return null;
            }
            set
            {
                Dictionary[key] = value;
            }
        }

        public void Add(byte key, Caller value)
        {
            Dictionary.Add(key, value);
        }

        public bool ContainsKey(byte key)
        {
            if (Dictionary.ContainsKey(key)) return true;
            else return false;
        }

        public ICollection<byte> Keys
        {
            get { return Dictionary.Keys; }
        }

        public bool Remove(byte key)
        {
            Dictionary.Remove(key);
            return true;
        }

        public bool TryGetValue(byte key, out Caller value)
        {
            value = null;
            if (Dictionary.TryGetValue(key, out value)) return true;
            else return false;
        }

        public ICollection<Caller> Values
        {
            get { return Dictionary.Values; }
        }

        public void Add(KeyValuePair<byte, Caller> item)
        {
            Dictionary.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            Dictionary.Clear();
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public IEnumerator<KeyValuePair<byte, Caller>> GetEnumerator()
        {
            return Dictionary.GetEnumerator();
        }
    }

    public class Caller
    {
        //at the moment only needed for deep copying of memorystatuses in memory leak processing. consider dropping after proper revision of memory leak sequences parsing.
        //[ProtoMember(1)]
        public UInt32 Pointer { get; set; }
        public string Path { get; set; }
        public string File { get; set; }
        public string Function { get; set; }

        public string FullName
        {
            get
            {
                string name = "";
                if (!string.IsNullOrWhiteSpace(Function))
                {
                    if (!string.IsNullOrWhiteSpace(File))
                    {
                        name = File + " : ";
                    }
                    name += Function;
                }
                return name;
            }
        }

        public bool Solved { get; set; }
    }

    public  class Person 
    {
        public RangeEnabledObservableCollection<GroupedMemoryInfo> GroupedMemoryInfos { get; private set; }

        public Person()
        {
            GroupedMemoryInfos = new RangeEnabledObservableCollection<GroupedMemoryInfo>();
        }
        public string Name
        {
            get;
            set;
        }
        int age;

        public int Age
        {
            get;
            set;
        }

        public string Call1
        {
            get;
            set;
        }
        public string Call2
        {
            get;
            set;
        }
        public string Call3
        {
            get;
            set;
        }
        public string Call4
        {
            get;
            set;
        }
        public string Call5
        {
            get;
            set;
        }
        public string Call6
        {
            get;
            set;
        }
        public string Call7
        {
            get;
            set;
        }
        public string Call8
        {
            get;
            set;
        }
        public string Call9
        {
            get;
            set;
        }
        public string Call10
        {
            get;
            set;
        }
        public string Call11
        {
            get;
            set;
        }
        public string Call12
        {
            get;
            set;
        }
        public string Call13
        {
            get;
            set;
        }
        public string Call14
        {
            get;
            set;
        }
        public string Call15
        {
            get;
            set;
        }
        public string Call16
        {
            get;
            set;
        }
        public string Call17
        {
            get;
            set;
        }
        public string Call18
        {
            get;
            set;
        }
        public string Call19
        {
            get;
            set;
        }
        public string Call20
        {
            get;
            set;
        }

    }

    public class ExportDataCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public event ExecutedRoutedEventHandler Executed;
        public void Execute(object parameter)
        {
            Executed(parameter, null);
        }

        public ExportDataCommand(ExecutedRoutedEventHandler executed)
        {
            Executed = executed;
        }
    }
}
