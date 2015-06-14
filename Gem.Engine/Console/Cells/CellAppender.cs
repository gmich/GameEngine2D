﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Linq;

namespace Gem.Console
{
    public class CellAppender
    {

        #region Fields

        private readonly Func<char, ICell> cellGenerator;
        private readonly ObservableCollection<ICell> cells = new ObservableCollection<ICell>();

        #endregion

        #region Ctor

        public CellAppender(Func<char, ICell> cellGenerator)
        {
            this.cellGenerator = cellGenerator;
        }

        #endregion

        #region Add / Remove

        public void AddCellRange(IEnumerable<ICell> cellRange)
        {
            foreach (var cell in cellRange)
            {
                cells.Add(cell);
            }
        }

        public void AddCell(char cell)
        {
            cells.Add(cellGenerator(cell));
        }

        public bool AddCellAt(int index, char cell)
        {
            if (cells.Count > index)
            {
                cells.Insert(index, cellGenerator(cell));
                return true;
            }
            return false;
        }

        public bool RemoveCellAt(int index)
        {
            if (IndexExists(index))
            {
                cells.RemoveAt(index);
                return true;
            }
            return false;
        }

        #endregion

        #region Collection

        public IEnumerable<ICell> GetCells()
        {
            return cells;
        }

        public void Clear()
        {
            cells.Clear();
        }

        public int Count
        {
            get { return cells.Count; }
        }

        public override string ToString()
        {
            return String.Concat(cells);
        }

        #endregion

        #region Guard

        private bool IndexExists(int index)
        {
            return (cells.Count > index);
        }

        #endregion

        #region Events

        public void SubscribeObserver(IObserver<ICell> observer)
        {
            cells.Subscribe(observer);
        }

        public void OnCellAppend(NotifyCollectionChangedEventHandler collectionChangedEvent)
        {
            cells.CollectionChanged += collectionChangedEvent;
        }

        #endregion

    }
}
