using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollList<T> : MonoBehaviour, IList<T>
    {
        private ScrollRect _scrollRect;

        private List<T> internalList = new();
        private List<GameObject> fields = new();

        public delegate GameObject ItemHandler(T item, RectTransform content, GameObject prefab);
        public ItemHandler AddHandler = delegate { Debug.LogError("Addhandler not Implemented"); return null; };

        [SerializeField] GameObject _prefab;

        private void Start()
        {
            _scrollRect = GetComponent<ScrollRect>();
        }

        public T this[int index] { get => internalList[index]; set => throw new NotImplementedException(); }

        public bool IsReadOnly => ((ICollection<T>)internalList).IsReadOnly;

        public void Add(T item) 
        {
            internalList.Add(item);
            fields.Add(AddHandler(item, _scrollRect.content, _prefab));
        }

        public void Clear() 
        {
            internalList.Clear();
            foreach (GameObject gameObject in fields) 
            {
                Destroy(gameObject);
            }
            fields.Clear();
        }

        public bool Remove(T item) 
        {
            int index = internalList.IndexOf(item);
            Destroy(fields[index]);
            fields.RemoveAt(index);
            return internalList.Remove(item);
        }

        public void RemoveAt(int index) 
        {
            Destroy(fields[index]);
            fields.RemoveAt(index);
            internalList.RemoveAt(index);
        }

        #region DefaultListMethods
        public bool Contains(T item) => internalList.Contains(item);
        public IEnumerator<T> GetEnumerator() => internalList.GetEnumerator();
        public int IndexOf(T item) => internalList.IndexOf(item);
        IEnumerator IEnumerable.GetEnumerator() => internalList.GetEnumerator();
        public int Count => internalList.Count();
        public void CopyTo(T[] array, int arrayIndex) => throw new NotImplementedException();
        public void Insert(int index, T item) => throw new NotImplementedException();
        #endregion
    }
}
