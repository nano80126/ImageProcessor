using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ObservableCollections
{
    public sealed partial class ObservableStack<T> : Stack<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public object SyncRoot { get; } = new object();

        #region 建構子
        public ObservableStack() : base()
        {
        }

        public ObservableStack(IEnumerable<T> collection) : base(collection)
        {
        }

        public ObservableStack(int capacity) : base(capacity)
        {
        }
        #endregion

        public new void Push(T item)
        {
            lock (SyncRoot)
            {
                base.Push(item);
                OnCollectionChanged(NotifyCollectionChangedAction.Add, item);
            }
        }

        public new T Pop()
        {
            lock (SyncRoot)
            {
                T item = base.Pop();
                OnCollectionChanged(NotifyCollectionChangedAction.Remove, item);
                return item;
            }
        }

        public new void Clear()
        {
            lock (SyncRoot)
            {
                base.Clear();
                OnCollectionChanged(NotifyCollectionChangedAction.Reset);
            }
        }

        #region Property & Collection Changed
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Reset
        /// </summary>
        /// <param name="action"></param>
        private void OnCollectionChanged(NotifyCollectionChangedAction action)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action));
            OnPropertyChanged(nameof(Count));
        }

        /// <summary>
        /// Add & Remove
        /// </summary>
        /// <param name="action"></param>
        /// <param name="item"></param>
        private void OnCollectionChanged(NotifyCollectionChangedAction action, T item)
        {
            int changedIndex = item == null ? -1 : 0;
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, item, changedIndex));
            OnPropertyChanged(nameof(Count));
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } 
        #endregion
    }

    public sealed partial class ObservableQueue<T> : Queue<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        Queue<T> queue = new Queue<T>();
        Stack<T> stack = new Stack<T>();

        public object SyncRoot { get; } = new object();

        #region 建構子
        public ObservableQueue()
        {
        }

        public ObservableQueue(IEnumerable<T> collection) : base(collection)
        {
        }

        public ObservableQueue(int capacity) : base(capacity)
        {
        }
        #endregion

        public new void Enqueue(T item)
        {
            lock (SyncRoot)
            {
                base.Enqueue(item);
                OnCollectionAdded(item);
            }
        }

        public new T Dequeue()
        {
            lock (SyncRoot)
            {
                T item = base.Dequeue();
                OnCollectionRemoved(item);
                return item;
            }
        }

        public new void Clear()
        {
            lock (SyncRoot)
            {
                base.Clear();
                OnCollectionReset();
            }
        }

        #region Property & Collection Changed
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;


        /// <summary>
        /// Reset
        /// </summary>
        /// <param name="action"></param>
        private void OnCollectionReset()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            OnPropertyChanged(nameof(Count));
        }

        /// <summary>
        /// Add & Remove
        /// </summary>
        /// <param name="action"></param>
        /// <param name="item"></param>
        private void OnCollectionAdded(T item)
        {
            int changedIndex = Count - 1;
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, changedIndex));
            OnPropertyChanged(nameof(Count));
        }

        private void OnCollectionRemoved(T item)
        {
            int changedIndex = 0;
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, changedIndex));
            OnPropertyChanged(nameof(Count));
        }


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }


    public sealed partial class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public object SyncRoot { get; } = new object();

        #region 建構子
        public ObservableDictionary()
        {
        }

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary)
        {
        }

        public ObservableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection) : base(collection)
        {
        }

        public ObservableDictionary(IEqualityComparer<TKey> comparer) : base(comparer)
        {
        }

        public ObservableDictionary(int capacity) : base(capacity)
        {
        }

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(dictionary, comparer)
        {
        }

        public ObservableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer) : base(collection, comparer)
        {
        }

        public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer)
        {
        }

        public ObservableDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        #endregion

        public new TValue this[TKey key]
        {
            get => base.ContainsKey(key) ? base[key] : default;
            set
            {
                if (base.ContainsKey(key))
                {
                    Update(key, value);
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        public new void Add(TKey key, TValue value)
        {
            lock (SyncRoot)
            {
                base.Add(key, value);
                OnCollectionAdded(new KeyValuePair<TKey, TValue>(key, value));
            }
        }

        public new bool TryAdd(TKey key, TValue value)
        {
            lock (SyncRoot)
            {
                if (base.ContainsKey(key))
                {
                    return false;
                }
                else
                {
                    Add(key, value);
                    return true;
                }
            }
        }

        public new bool Remove(TKey key)
        {
            lock (SyncRoot)
            {
                if (base.Remove(key, out TValue value))
                {
                    OnCollectionRemoved(new KeyValuePair<TKey, TValue>(key, value));
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public new bool Remove(TKey key, out TValue value)
        {
            lock (SyncRoot)
            {
                if (base.Remove(key, out value))
                {
                    OnCollectionRemoved(new KeyValuePair<TKey, TValue>(key, value));
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public new void Clear()
        {
            lock(SyncRoot)
            {
                base.Clear();
                OnCollectionReset();
            }
        }

        private void Update(TKey key, TValue value)
        {
            lock (SyncRoot)
            {
                TValue oldValue = base[key];

                base[key] = value;
                KeyValuePair<TKey, TValue> newItem = new(key, value);
                KeyValuePair<TKey, TValue> oldItem = new(key, oldValue);
                OnCollectionReplaced(newItem, oldItem);
            }
        }

        #region Property & Collection Changed
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnCollectionAdded(KeyValuePair<TKey, TValue> pair)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, pair, -1));

            OnPropertyChanged(nameof(Keys));
            OnPropertyChanged(nameof(Values));
            OnPropertyChanged(nameof(Count));
            OnPropertyChanged("Item[]");
        }

        private void OnCollectionRemoved(KeyValuePair<TKey, TValue> pair)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, pair, -1));

            OnPropertyChanged(nameof(Keys));
            OnPropertyChanged(nameof(Values));
            OnPropertyChanged(nameof(Count));
            OnPropertyChanged("Item[]");
        }

        private void OnCollectionReplaced(KeyValuePair<TKey, TValue> newItem, KeyValuePair<TKey, TValue> oldItem)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem));

            // Keys 不會變更
            OnPropertyChanged(nameof(Values));
            OnPropertyChanged(nameof(Count));
            OnPropertyChanged("Item[]");
        }

        private void OnCollectionReset()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

            OnPropertyChanged(nameof(Keys));
            OnPropertyChanged(nameof(Values));
            OnPropertyChanged(nameof(Count));
            OnPropertyChanged("Item[]");
        } 
        #endregion
    }

}
