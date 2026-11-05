using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ImageMismatchFinder
{
    public class SortableBindingList<T> : BindingList<T>
    {
        private bool _isSorted;
        private ListSortDirection _sortDirection = ListSortDirection.Ascending;
        private PropertyDescriptor? _sortProperty;

        public SortableBindingList() : base() { }

        public SortableBindingList(IList<T> list) : base(list) { }

        protected override bool SupportsSortingCore => true;
        protected override bool IsSortedCore => _isSorted;
        protected override ListSortDirection SortDirectionCore => _sortDirection;
        protected override PropertyDescriptor? SortPropertyCore => _sortProperty;

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            var itemsList = (List<T>)Items;
            if (itemsList == null) return;

            var propInfo = typeof(T).GetProperty(prop.Name);
            if (propInfo == null) return;

            if (direction == ListSortDirection.Ascending)
            {
                itemsList.Sort((x, y) =>
                {
                    var valX = propInfo.GetValue(x);
                    var valY = propInfo.GetValue(y);
                    if (valX == null && valY == null) return 0;
                    if (valX == null) return -1;
                    if (valY == null) return 1;
                    if (valX is IComparable compX)
                        return compX.CompareTo(valY);
                    return string.Compare(valX.ToString(), valY.ToString(), StringComparison.OrdinalIgnoreCase);
                });
            }
            else
            {
                itemsList.Sort((x, y) =>
                {
                    var valX = propInfo.GetValue(x);
                    var valY = propInfo.GetValue(y);
                    if (valX == null && valY == null) return 0;
                    if (valX == null) return 1;
                    if (valY == null) return -1;
                    if (valY is IComparable compY)
                        return compY.CompareTo(valX);
                    return string.Compare(valY.ToString(), valX.ToString(), StringComparison.OrdinalIgnoreCase);
                });
            }

            _isSorted = true;
            _sortDirection = direction;
            _sortProperty = prop;
            ResetBindings();
        }

        protected override void RemoveSortCore()
        {
            _isSorted = false;
            _sortProperty = null;
            ResetBindings();
        }
    }
}
