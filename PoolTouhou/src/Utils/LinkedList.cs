using System.Collections;
using System.Collections.Generic;

namespace PoolTouhou.Utils {
    public sealed class LinkedList<T> : ICollection<T> {
        public LinkedListNode Header { get; private set; }
        public LinkedListNode Footer { get; private set; }
        public int Count { get; private set; }
        public bool IsReadOnly => false;

        public void Add(T item) {
            if (item != null) {
                if (Header == null) {
                    Header = new LinkedListNode(item, this);
                    Footer = Header;
                } else {
                    Footer.Next = new LinkedListNode(item, this);
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

        public sealed class LinkedListNode {
            private LinkedList<T> list;
            public LinkedListNode Previous { get; internal set; }
            public LinkedListNode Next { get; internal set; }
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
                if (this == list.Footer) {
                    list.Footer = pre;
                }
                if (this == list.Header) {
                    list.Header = Next;
                }
            }
        }
    }
}