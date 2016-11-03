using System.Collections;
using System.Collections.Generic;

namespace Legolize.Algo
{
    class SlotPriorityQueueList : ISlotPriorityQueue
    {
        private List<Slot> _list;

        public SlotPriorityQueueList()
        {
            _list = new List<Slot>();
        }
        public Slot this[int index]
        {
            get
            {
                return _list[index];
            }

            set
            {
                _list[index] = value;
            }
        }

        public int Count
        {
            get
            {
                return _list.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add(Slot item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(Slot item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(Slot[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public ISlotPriorityQueue DeepClone()
        {
            ISlotPriorityQueue newSPQ = new SlotPriorityQueueList();
            foreach (var slot in _list)
            {
                newSPQ.Add(slot);
            }
            return newSPQ;
        }

        public IEnumerator<Slot> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public void IncreasePriority(int iSlot, int amount = 1)
        {
            Slot s = _list[iSlot];
            _list.RemoveAt(iSlot);
            s.IncreasePriority(amount);

            Insert(s);
        }

        public int IndexOf(Slot item)
        {
            int index = 0;
            foreach (var v in _list)
            {
                if (item.Equals(v)) break;
                index++; 
            }
            return index;
        }

        public void Insert(int index, Slot item)
        {
            Insert(item);
        }

        public void Insert(Slot item)
        {
            //remove if the slot is already in
            Remove(item);
            int index = 0;
            foreach (var v in _list)
            {
                if (v.Priority <= item.Priority)
                { break; }
                index++;
            }
            _list.Insert(index, item);
        }

        public bool Remove(Slot item)
        {
            //remove slot with all priorities
            return _list.RemoveAll((Slot x) => x.Brick.Equals(item.Brick)) !=0;
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }

    public class QTester
    {
        public QTester()
        {
            Slot s1 = new Slot();
            s1.IncreasePriority();
            
            Slot s2 = new Algo.Slot();
            Slot s3 = new Algo.Slot();
            s3.IncreasePriority();
            s3.IncreasePriority();

            SlotPriorityQueueList sq = new SlotPriorityQueueList();

            sq.Add(s2);
            sq.Insert(s1);
            sq.Insert(s1);
            sq.Insert(s3);
            sq.Insert(s3);
        }
    }
}
