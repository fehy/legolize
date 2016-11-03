using System;
using System.Collections.Generic;
using System.Linq;

namespace Legolize.Algo
{
    class ModelMaster : IModelMaster
    {
        public ISlotPriorityQueue Slots { get; }
        public Stack<Brick> Bricks { get; }
        public IModel Model { get; }

        private int _nextSlotPosition = 0;

        public void MoveSlotPosition() => _nextSlotPosition++;

        public int NSlotsToSearch => Slots.Count - _nextSlotPosition;

        public void SlotToBrick()
        {
            // remove slot from SlotPriorityQueue
            var slot = Slots[_nextSlotPosition];
            Slots.RemoveAt(_nextSlotPosition);

            var brick = slot.Brick;
            // add brick to brick queue
            Bricks.Push(brick);

            // update model to remove occupied possitions
            Model.Set(brick, false);

            // remove invalid slots from SlotPriorityQueue
            var n = Slots.Count;
            for(var i = 0; i < n; i++)
                if(Slots[i].Brick.InCollision(brick))
                {
                    Slots.RemoveAt(i);
                    i--;
                    n--;
                }

            // update slot priorities
            for (var i = 0; i < n; i++)
                if (Slots[i].Brick.InTouch(brick))
                {
                    Slots.IncreasePriority(i);
                }

            _nextSlotPosition = 0;
        }

        private static Brick[] AllNewBricksFor(Brick brick)
        {
            // hardcoding all bricks 

            // 1x1
            //for(var ix = brick; ix )
            return Array.Empty<Brick>();
        }

        // Take latest added Brick
        // Add new slots introduces by Brick
        public bool CreateNewSlots()
        {
            var brick = Bricks.Pop();
            var potentialBricks = AllNewBricksFor(brick);

            var before = Slots.Count;
            foreach(var brickCandidate in potentialBricks)
            {
                if (!Model.Can(brickCandidate))
                    continue;

                var touches = Bricks.Select(x => brickCandidate.InTouch(x) ? 1 : 0).Sum();
                Slots.Insert(new Slot(brickCandidate, touches));
            }

            return Slots.Count > before;
        }

        public ModelMaster(Model model)
        {
            Model = model;
            Bricks = new Stack<Brick>();
            Slots = new SlotPriorityQueueList();
        }

        private ModelMaster(ModelMaster from, bool deepClone)
        {
            Slots = from.Slots.DeepClone();
            _nextSlotPosition = from._nextSlotPosition;
            if (!deepClone)
            {
                Bricks = from.Bricks;
                Model = from.Model;
                
            }
            else
            {
                Bricks = new Stack<Brick>(from.Bricks);
                Model = from.Model.DeepClone();
            }
        }

        public IModelMaster Clone()
        {
            return new ModelMaster(this, false);
        }

        public IModelMaster DeepClone()
        {
            return new ModelMaster(this, true);
        }
        
        public void MoveBack()
        {
            // pop brick, update Model
            Model.Set(Bricks.Pop(), true);
        }        
    }
}
