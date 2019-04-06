using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRC.Client.Base;
using System.ComponentModel;
using System.Collections.Specialized;

namespace IRC.Client
{
    public sealed class Changeable<T>: InfiniteMarshalByRefObject, IList<T>, IChangeable<T>, IListSource, INotifyCollectionChanged
    {
        private List<T> list = null;
        private object sync = new object();

        public event Action<object, T[]> ItemsAdded;
        public event Action<object, T[]> ItemsRemoved;

        public object SyncRoot
        {
            get { return this.sync; }
        }

        public Changeable()
        {
            this.list = new List<T>();
        }

        public Changeable(IEnumerable<T> collection)
        {
            this.list = new List<T>(collection);
        }

        private void OnItemsAdded(params T[] newItems)
        {
            if (ItemsAdded != null)
                ItemsAdded(this, newItems);

            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems));
        }

        private void OnItemsRemoved(params T[] oldItems)
        {
            if (ItemsRemoved != null)
                ItemsRemoved(this, oldItems);

            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            lock (this.sync)
            {
                return this.list.IndexOf(item);
            }
        }

        public void Insert(int index, T item)
        {
            lock (this.sync)
            {
                this.list.Insert(index, item);
                OnItemsAdded(item);
            }
        }

        public void RemoveAt(int index)
        {
            lock (this.sync)
            {
                var item = this.list[index];
                this.list.RemoveAt(index);
                OnItemsRemoved(item);
            }
        }

        public T this[int index]
        {
            get
            {
                lock (this.sync)
                {
                    return this.list[index];
                }
            }
            set
            {
                lock (this.sync)
                {
                    this.list[index] = value;
                }
            }
        }

        public void Add(T item)
        {
            lock (this.sync)
            {
                this.list.Add(item);
                OnItemsAdded(item);
            }
        }

        public void Clear()
        {
            lock (this.sync)
            {
                var items = this.list.ToArray();
                this.list.Clear();
                OnItemsRemoved(items);
            }
        }

        public bool Contains(T item)
        {
            lock (this.sync)
            {
                return this.list.Contains(item);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (this.sync)
            {
                this.list.CopyTo(array, arrayIndex);
            }
        }

        public int Count
        {
            get
            {
                lock (this.sync)
                {
                    return this.list.Count;
                }
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            lock (this.sync)
            {
                var result = this.list.Remove(item);
                OnItemsRemoved(item);
                return result;
            }
        }

        public void Sort(Comparison<T> comparison)
        {
            lock (this.sync)
            {
                this.list.Sort(comparison);
            }
        }

        public int FindIndex(Predicate<T> match)
        {
            lock (this.sync)
            {
                return this.list.FindIndex(match);
            }
        }

        public bool ContainsListCollection
        {
            get { return true; }
        }

        public System.Collections.IList GetList()
        {
            return this.list;
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
