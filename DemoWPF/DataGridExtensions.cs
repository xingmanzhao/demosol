using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using Microsoft.Win32;
using DemoWPF;

public delegate void ExportProcessValueHandler(int process);
public delegate void ExportProcessStartHandler();
public delegate void ExportProcessCompletedHandler();
public static class DataGridExtensions
{
    public static void Export(this DataGrid dg)
    {
        ExportAllData(dg);
    }

    private static void ExportAllData(DataGrid dGrid)
    {
        SaveFileDialog objSFD = new SaveFileDialog() { DefaultExt = "csv", Filter = "CSV Files (*.csv)|*.csv|All files (*.*)|*.*", FilterIndex = 1 };
        if (objSFD.ShowDialog() == true)
        {
            string strFormat = objSFD.SafeFileName.Substring(objSFD.SafeFileName.IndexOf('.') + 1).ToUpper();
            StringBuilder strBuilder = new StringBuilder();
            if (dGrid.ItemsSource == null) return;
            List<string> lstFields = new List<string>();
            if (dGrid.HeadersVisibility == DataGridHeadersVisibility.Column || dGrid.HeadersVisibility == DataGridHeadersVisibility.All)
            {
                foreach (DataGridColumn dgcol in dGrid.Columns)
                {
                    if (dgcol.Visibility == Visibility.Visible)
                    {
                        lstFields.Add(FormatField(dgcol.Header.ToString(), strFormat));
                    }
                }

                BuildStringOfRow(strBuilder, lstFields, strFormat);
            }
            foreach (object data in dGrid.ItemsSource)
            {

                lstFields.Clear();
                foreach (DataGridColumn col in dGrid.Columns)
                {
                    string strValue = "";
                    object obj = null;
                    Binding objBinding = null;
                    if (col.Visibility == Visibility.Visible)
                    {
                        if (col is DataGridBoundColumn)
                        { objBinding = (Binding)(col as DataGridBoundColumn).Binding; }
                        else if (col is DataGridTemplateColumn)
                        {
                            //This is a template column... let us see the underlying dependency object
                            DependencyObject objDO = (col as DataGridTemplateColumn).CellTemplate.LoadContent();
                            FrameworkElement oFE = (FrameworkElement)objDO;
                            FieldInfo oFI = oFE.GetType().GetField("TextProperty");
                            if (oFI != null)
                            {
                                if (oFI.GetValue(null) != null)
                                {
                                    if (oFE.GetBindingExpression((DependencyProperty)oFI.GetValue(null)) != null)
                                        objBinding = oFE.GetBindingExpression((DependencyProperty)oFI.GetValue(null)).ParentBinding;
                                }
                            }
                        }
                        if (objBinding != null)
                        {
                            if (objBinding.Path.Path != "")
                            {
                                string path = objBinding.Path.Path;
                                if (path.Contains("."))
                                {
                                    string[] split = path.Split('.');
                                    object oob = data;
                                    foreach (string pa in split)
                                    {
                                        if (oob == null)
                                        {
                                            strValue = string.Empty;
                                            break;
                                        }
                                        else
                                        {
                                            if (pa.Contains("[") && pa.Contains("]"))
                                            {
                                                if (pa.StartsWith("[") && pa.EndsWith("]"))
                                                {
                                                    int index = int.Parse(pa.Substring(pa.IndexOf("[") + 1, pa.LastIndexOf("]") - pa.IndexOf("[") - 1));
                                                    if (data is IEnumerable)
                                                    {
                                                        IEnumerator ieum = ((IEnumerable)data).GetEnumerator();
                                                        int i = 0;
                                                        while (i < index && ieum.MoveNext())
                                                        {
                                                            i++;
                                                        }
                                                        if (i == index) {
                                                            ieum.MoveNext();
                                                            oob = ieum.Current;
                                                        }
                                                    }
                                                    else if (data is IEnumerator)
                                                    {
                                               
                                                    }
                                                }
                                                else
                                                {
                                                    string cpa = pa.Substring(0, pa.IndexOf("["));
                                                    int index = int.Parse(pa.Substring(pa.IndexOf("[") + 1, pa.LastIndexOf("]") - pa.IndexOf("[") - 1));
                                                    PropertyInfo pi = oob.GetType().GetProperty(cpa);
                                                    if (pi != null)
                                                    {
                                                        object objArray = pi.GetValue(oob, null);

                                                       
                                                        if (objArray is IEnumerable)
                                                        {
                                                            IEnumerator ieum = ((IEnumerable)objArray).GetEnumerator();
                                                            int i = 0;
                                                            while (i < index && ieum.MoveNext())
                                                            {
                                                                i++;
                                                            }
                                                            if (i == index)
                                                            {
                                                                ieum.MoveNext();
                                                                oob = ieum.Current;
                                                            }
                                                        }
                                                        else  if (objArray is CallerDictionary)
                                                        {
                                                            CallerDictionary cd = (CallerDictionary)objArray;
                                                            oob = cd[(byte)index];
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                PropertyInfo pi = oob.GetType().GetProperty(pa);
                                                if (pi != null)
                                                {
                                                    oob = pi.GetValue(oob, null);
                                                }
                                            }
                                        }
                                    }
                                    if (oob != null)
                                    { strValue = oob.ToString(); }
                                }
                                else
                                {
                                    PropertyInfo pi = data.GetType().GetProperty(objBinding.Path.Path);
                                    if (pi != null)
                                    {
                                        obj = pi.GetValue(data, null);
                                        if (obj != null)
                                        {
                                            strValue = obj.ToString();
                                        }
                                    }
                                }
                            }
                            if (objBinding.Converter != null)
                            {
                                if (obj == null)
                                {
                                    strValue = objBinding.Converter.Convert(data, typeof(string), objBinding.ConverterParameter, objBinding.ConverterCulture).ToString();
                                }
                                else
                                {
                                    strValue = objBinding.Converter.Convert(obj, typeof(string), objBinding.ConverterParameter, objBinding.ConverterCulture).ToString();
                                }
                            }
                        }
                        lstFields.Add(FormatField(strValue, strFormat));
                    }
                }
                BuildStringOfRow(strBuilder, lstFields, strFormat);
            }
            using (StreamWriter sw = new StreamWriter(objSFD.OpenFile()))
            {
                sw.Write(strBuilder.ToString());
            }
        }
    }

    private static void BuildStringOfRow(StringBuilder strBuilder, List<string> lstFields, string strFormat)
    {
        switch (strFormat)
        {
            case "CSV":
                strBuilder.AppendLine(String.Join(",", lstFields.ToArray()));
                break;
        }
    }

    private static string FormatField(string data, string format)
    {
        switch (format)
        {
            case "CSV":
                return String.Format("\"{0}\"", data.Replace("\"", "\"\"\"").Replace("\n", "").Replace("\r", ""));
        }
        return data;
    }
}