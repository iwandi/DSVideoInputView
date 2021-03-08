using System;
using System.Collections.Generic;
using DirectShowLib;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace DSVideoInputView
{
    public class CaptureSourceList : IDisposable
    {
        const string UnknownName = "Unknown";

        public class Entry : IDisposable
        {
            public string Name { get; private set; }
            public string DisplayName { get; private set; }
            public IBaseFilter Filter { get; private set; }
            public IPropertyBag PropertyBag { get; private set; }

            public Entry(string name,
                string displayName,
                IBaseFilter filter, 
                IPropertyBag propertyBag)
            {
                Name = name;
                DisplayName = name;
                Filter = filter;
                PropertyBag = propertyBag;
            }

            public void Dispose()
            {
                if (Filter != null)
                {
                    Marshal.ReleaseComObject(Filter);
                    Filter = null;
                }

                if (PropertyBag != null)
                {
                    Marshal.ReleaseComObject(PropertyBag);
                    PropertyBag = null;
                }
            }
        }

        List<Entry> list = new List<Entry>();
        public IReadOnlyList<Entry> List {  get { return list; } }

        public CaptureSourceList()
        {

        }

        public void UpdateList()
        {
            ClearList();
            FillList();
        }

        void FillList()
        { 
            IEnumMoniker classEnum = null;
            IMoniker[] moniker = new IMoniker[1];

            ICreateDevEnum devEnum = (ICreateDevEnum)new CreateDevEnum();

            var hr = devEnum.CreateClassEnumerator(FilterCategory.VideoInputDevice, out classEnum, 0);
            DsError.ThrowExceptionForHR(hr);

            Marshal.ReleaseComObject(devEnum);
            devEnum = null;

            if (classEnum == null)
                return;

            int i = 0;
            while (classEnum.Next(moniker.Length, moniker, IntPtr.Zero) == 0)
            {
                var iidBaseFilter = typeof(IBaseFilter).GUID;
                moniker[0].BindToObject(null, null, ref iidBaseFilter, out var source);
                if (source == null)
                    continue;
                IBaseFilter baseFilter = (IBaseFilter)source;
                if (baseFilter == null)
                    continue;

                

                var iidPropertyBag = typeof(IPropertyBag).GUID;
                moniker[0].BindToStorage(null, null, ref iidPropertyBag, out var storage);
                var propertyBag = (IPropertyBag)storage;

                string name, displayName;
                GetName(i, baseFilter, propertyBag, out name, out displayName);
                var entry = new Entry(name, displayName, baseFilter, propertyBag);
                list.Add(entry);

                Marshal.ReleaseComObject(moniker[0]);
                i++;
            }

            Marshal.ReleaseComObject(classEnum);
        }

        void GetName(int index, IBaseFilter baseFilter, IPropertyBag propertyBag,
            out string name, out string displayName)
        {
            // TODO : test "DisplayName"

            propertyBag.Read("FriendlyName", out var friendlyNameObj, null);
            var friendlyName = friendlyNameObj.ToString();
            displayName  = friendlyName;
            name = friendlyName;
            return;

            /*name = index.ToString();
            displayName = $"{UnknownName} ({index})";*/
        }

        void ClearList()
        {
            foreach(var entry in list)
            {
                if (entry != null)
                {
                    entry.Dispose();
                }
            }

            list.Clear();
        }

        public void Dispose()
        {
            ClearList();
        }

        bool TryGetByIndexString(string indexStr, out Entry entry)
        {
            if (int.TryParse(indexStr, out var index))
            {
                if (index >= 0 && index < list.Count)
                {
                    entry = list[index];
                    return true;
                }
            }
            entry = null;
            return false;
        }

        public bool TryGetByName(string name, out Entry entry)
        {
            if (TryGetByIndexString(name, out entry))
                return true;

            List<Entry> mach = new List<Entry>();

            foreach(var checkEntry in list)
            {
                if(checkEntry.Name == name ||
                    checkEntry.DisplayName == name)
                {
                    entry = checkEntry;
                    return true;
                }
                if (checkEntry.Name.StartsWith(name) || checkEntry.DisplayName.StartsWith(name))
                    mach.Add(checkEntry);
            }

            if(mach.Count > 0)
            {
                entry = mach[0];
                return true;
            }

            if(name.StartsWith(UnknownName))
            {
                string indexStr = System.Text.RegularExpressions.Regex.Match(name, @"\d+").Value;

                if (TryGetByIndexString(indexStr, out entry))
                    return true;
            }

            entry = null;
            return false;
        }
    }
}
