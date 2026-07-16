using System;
using UnityEngine;

namespace Data.Offers
{
    public interface IOfferable
    {
        public void DestroyInOffer();
        public void SelectedInOffer();

        public void AddToOffer(int row, int col, int totalRows, int totalCols);
    }
}