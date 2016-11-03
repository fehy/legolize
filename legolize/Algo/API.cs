using System;
using System.Collections.Generic;

namespace Legolize.Algo
{
    interface IModel
    {
        bool this[int x, int y, int z] { get; set; }

        bool Can(Brick brick);
        void Set(Brick brick, bool value);

        bool HasAny();

        IModel DeepClone();
    }

    struct Brick
    {
        public Brick(Point leftLowNear, Point rightUpFar)
        {
            LeftLowNear = leftLowNear;
            RightUpFar = rightUpFar; 
        }
        public Point LeftLowNear { get; }
        public Point RightUpFar { get; }

        public int Volume => (RightUpFar.X - LeftLowNear.X) * (RightUpFar.Y - LeftLowNear.Y) * (RightUpFar.Z - LeftLowNear.Z);
    }
    

    struct Slot
    {
        public int Priority { get; private set; }
        public void IncreasePriority() => Priority++;                    
        public Brick Brick { get; }

        public Slot(Brick brick, int priority)
        {
            Priority = priority;
            Brick = brick;
        }
    }

    interface ISlotPriorityQueue : IList<Slot>
    {
        ISlotPriorityQueue DeepClone();
        void IncreasePriority(int iSlot);
        void Insert(Slot item);
    }

    interface IModelMaster
    {
        ISlotPriorityQueue Slots { get; }

        Stack<Brick> Bricks { get; }
        IModel Model { get; }


        Tuple<int, int> SlotsToSearch { set; }

        void MoveSlotPosition();

        int NRemainingSlotsToSearch { get; }

        // remove slot from SlotPriorityQueue
        // add brick to brick queue
        // remove invalid slots from SlotPriorityQueue
        // update slot priorities
        // update model to remove occupied possitions
        void SlotToBrick();

        // Take latest added Brick
        // Add new slots introduces by Brick
        bool CreateNewSlots();

        // clone SlotPriorityQueue
        // references Model
        // references BrickStack
        IModelMaster Clone();

        // clone SlotPriorityQueue
        // clone Model
        // clone BrickStack
        IModelMaster DeepClone();

        // Move forward
        // Clone(), SlotToBrick, CreateNewSlots

        // pop brick, update Model
        void MoveBack();        
    }



}
