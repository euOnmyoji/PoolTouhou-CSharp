using System.Collections;
using System.Collections.Generic;

namespace PoolTouhou.Utils {
    public sealed class LinkedList<T> : ICollection<T> {
        public LinkedListNode<T> Header { get; private set; }
        public LinkedListNode<T> Footer { get; private set; }
        public int Count { get; set; }
        public bool IsReadOnly => false;

        public void Add(T item) {
            if (item != null) {
                if (Header == null) {
                    Header = new LinkedListNode<T>(item, this);
                    Footer = Header;
                } else {
                    Footer.Next = new LinkedListNode<T>(item, this);
                    Footer = Footer.Next;
                }
                ++Count;
            }
        }

        public void Clear() {
            Header = null;
            Footer = null;
            Count = 0;
        }


        //NO USED METHOD THAT IN MY MIND (X)     FEEL FREE TO *

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public bool Contains(T item) {
            throw new System.NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex) {
            throw new System.NotImplementedException();
        }

        public bool Remove(T item) {
            throw new System.NotImplementedException();
        }


        public IEnumerator<T> GetEnumerator() {
            throw new System.NotImplementedException();
        }
    }

    public class LinkedListNode<T> {
        private readonly LinkedList<T> list;
        public LinkedListNode<T> Previous { get; set; }
        public LinkedListNode<T> Next { get; set; }
        public bool HasNext => Next != null;
        public readonly T value;

        internal LinkedListNode(T v, LinkedList<T> l) {
            value = v;
            list = l;
        }

        public void Remove() {
            var pre = Previous;
            Previous = null;
            if (pre != null) {
                pre.Next = Next;
            }
            var next = Next;
            if (next != null) {
                next.Previous = pre;
            }
            --list.Count;
        }
    }
}