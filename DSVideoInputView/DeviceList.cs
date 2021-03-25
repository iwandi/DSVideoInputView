using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using DirectShowLib;

namespace DSVideoInputView
{
    public class VideoInputDeviceList : DeviceList<DeviceEntry>
    {
        public VideoInputDeviceList() : base(FilterCategory.VideoInputDevice)
        {

        }

        protected override bool TryCreateEntry(string name, string displayName, IBaseFilter filter, IPropertyBag propertyBag, out DeviceEntry entry)
        {
            entry = new DeviceEntry(name, displayName, filter, propertyBag);
            return true;
        }
    }

    public class AudioInputDeviceList : DeviceList<DeviceEntry>
    {
        public AudioInputDeviceList() : base(FilterCategory.AudioInputDevice)
        {

        }

        protected override bool TryCreateEntry(string name, string displayName, IBaseFilter filter, IPropertyBag propertyBag, out DeviceEntry entry)
        {
            entry = new DeviceEntry(name, displayName, filter, propertyBag);
            return true;
        }
    }

    public class DeviceEntry : IDisposable
    {
        public string Name { get; private set; }
        public string DisplayName { get; private set; }
        public IBaseFilter Filter { get; private set; }
        public IPropertyBag PropertyBag { get; private set; }

        public DeviceEntry(string name,
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

    public abstract class DeviceList<T> : IDisposable where T : DeviceEntry
    {
        const string UnknownName = "Unknown";

        Guid deviceClass;

        protected DeviceList(Guid deviceClass)
        {
            this.deviceClass = deviceClass;
            FillList();
        }

        protected void FillList()
        {
            IEnumMoniker classEnum = null;
            IMoniker[] moniker = new IMoniker[1];

            ICreateDevEnum devEnum = (ICreateDevEnum)new CreateDevEnum();

            var hr = devEnum.CreateClassEnumerator(deviceClass, out classEnum, 0);
            DsError.ThrowExceptionForHR(hr);

            Marshal.ReleaseComObject(devEnum);
            devEnum = null;

            if (classEnum == null)
                return;

            int i = 0;
            while (classEnum.Next(moniker.Length, moniker, IntPtr.Zero) == 0)
            {
                if (TryGetBaseFilter(moniker[0], out var baseFilter))
                {
                    if (TryGetPropertyBag(moniker[0], out var propertyBag))
                    {
                        string name, displayName;
                        propertyBag.Read("FriendlyName", out var friendlyNameObj, null);
                        var friendlyName = friendlyNameObj.ToString();
                        displayName = friendlyName;
                        name = friendlyName;
                        if (TryCreateEntry(name, displayName, baseFilter, propertyBag, out var entry))
                        {
                            list.Add(entry);
                        }
                        else
                        {
                            Marshal.ReleaseComObject(propertyBag);
                            Marshal.ReleaseComObject(baseFilter);
                        }
                    }
                    else
                    {
                        Marshal.ReleaseComObject(baseFilter);
                    }
                }                    
                Marshal.ReleaseComObject(moniker[0]);
                i++;
            }

            Marshal.ReleaseComObject(classEnum);
        }

        bool TryGetBaseFilter(IMoniker moniker, out IBaseFilter baseFilter)
        {
            var iidBaseFilter = typeof(IBaseFilter).GUID;
            moniker.BindToObject(null, null, ref iidBaseFilter, out var @object);
            baseFilter = @object as IBaseFilter;
            if(baseFilter == null)
            {
                Marshal.ReleaseComObject(@object);
                baseFilter = null;
                return false;
            }
            return true;
        }

        bool TryGetPropertyBag(IMoniker moniker, out IPropertyBag propertyBag)
        {
            var iidPropertyBag = typeof(IPropertyBag).GUID;
            moniker.BindToStorage(null, null, ref iidPropertyBag, out var storage);
            propertyBag = storage as IPropertyBag;
            if (propertyBag == null)
            {
                Marshal.ReleaseComObject(storage);
                propertyBag = null;
                return false;
            }
            return true;
        }

        protected abstract bool TryCreateEntry(string name,
            string displayName,
            IBaseFilter filter,
            IPropertyBag propertyBag,
            out T entry);

        List<T> list = new List<T>();
        public IReadOnlyList<T> List { get { return list; } }

        protected void ClearList()
        {
            foreach (var entry in list)
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

        protected bool TryGetByIndexString(string indexStr, out T entry)
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

        public bool TryGetByName(string name, out T entry)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                entry = null;
                return false;
            }

            if (TryGetByIndexString(name, out entry))
                return true;

            List<T> mach = new List<T>();

            foreach (var checkEntry in list)
            {
                if (checkEntry.Name == name ||
                    checkEntry.DisplayName == name)
                {
                    entry = checkEntry;
                    return true;
                }
                if (checkEntry.Name.StartsWith(name) || checkEntry.DisplayName.StartsWith(name))
                    mach.Add(checkEntry);
            }

            if (mach.Count > 0)
            {
                entry = mach[0];
                return true;
            }

            if (name.StartsWith(UnknownName))
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
