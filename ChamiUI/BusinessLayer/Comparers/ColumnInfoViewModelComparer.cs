using System;
using System.Collections.Generic;
using Chami.Db.Entities;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Comparers
{
    public class ColumnInfoViewModelComparer : IComparer<ColumnInfoViewModel>, IComparer<ColumnInfo>
    {
        public int Compare(ColumnInfoViewModel x, ColumnInfoViewModel y)
        {
            int xOrdinalPosition = 0;
            int yOrdinalPosition = 0;
            if (x != null)
            {
                xOrdinalPosition = x.OrdinalPosition;
            }

            if (y != null)
            {
                yOrdinalPosition = y.OrdinalPosition;
            }

            return xOrdinalPosition - yOrdinalPosition;
        }

        public int Compare(ColumnInfo x, ColumnInfo y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;
            return Nullable.Compare(x.OrdinalPosition, y.OrdinalPosition);
        }
    }
}