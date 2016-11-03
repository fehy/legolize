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

        private Brick[] AllNewBricksFor(Brick brick)
        {
            List<Brick> result = new List<Brick>();


            int zt = brick.RightUpFar.Z; //on top
            int zb = brick.LeftLowNear.Z-1;//at bottom

            // hardcoding all bricks 
            // 1x1
            for (var x = brick.LeftLowNear.X; x < brick.RightUpFar.X; x++)
            {
                for (var y = brick.LeftLowNear.Y; y < brick.RightUpFar.Y; y++)
                {
                    if (Model[x, y, zt])
                        result.Add(new Brick(new Point(x, y, zt), new Point(x+1, y+1, zt+1)));

                    if (Model[x, y, zb])
                        result.Add(new Brick(new Point(x, y, zb), new Point(x+1, y+1, zb+1)));
                }

            }

            // 2x2 top
            for (var x = brick.LeftLowNear.X-1; x < brick.RightUpFar.X; x++)
            {
                for (var y = brick.LeftLowNear.Y-1; y < brick.RightUpFar.Y; y++)
                {
                    if (!Model[x, y+1, zt] || !Model[x+1, y+1, zt])
                      { y++; continue; }
                    if (!Model[x, y, zt] || !Model[x + 1, y, zt])
                        continue;
                    result.Add(new Brick(new Point(x, y, zt), new Point(x+2, y+2, zt+1)));
                }
            }

            // 2x2 bottom
            for (var x = brick.LeftLowNear.X-1; x < brick.RightUpFar.X; x++)
            {
                for (var y = brick.LeftLowNear.Y-1; y < brick.RightUpFar.Y; y++)
                {
                    if (!Model[x, y+1, zb] || !Model[x+1, y+1, zb])
                    { y++; continue; }
                    if (!Model[x, y, zb] || !Model[x + 1, y, zb])
                        continue;
                    result.Add(new Brick(new Point(x, y, zb), new Point(x + 2, y + 2, zb+1)));
                }
            }



            // 4x2 top (rotation stretching in y)
            for (var x = brick.LeftLowNear.X - 1; x < brick.RightUpFar.X; x++)
            {
                for (var y = brick.LeftLowNear.Y - 3; y < brick.RightUpFar.Y; y++)
                {
                    if (!Model[x, y + 3, zt] || !Model[x + 1, y + 3, zt])
                    { y += 3; continue; }
                    if (!Model[x, y + 2, zt] || !Model[x + 1, y + 2, zt])
                    { y+=2; continue; }
                    if (!Model[x, y + 1, zt] || !Model[x + 1, y+1, zt])
                    { y++; continue; }
                    if (!Model[x, y, zt] || !Model[x + 1, y+1, zt])
                        continue;
                    result.Add(new Brick(new Point(x, y, zt), new Point(x + 2, y + 4, zt+1)));
                }
            }

            // 4x2 bottom (rotation stretching in y)
            for (var x = brick.LeftLowNear.X - 1; x < brick.RightUpFar.X; x++)
            {
                for (var y = brick.LeftLowNear.Y - 3; y < brick.RightUpFar.Y; y++)
                {
                    if (!Model[x, y + 3, zb] || !Model[x + 1, y + 3, zb])
                    { y += 3; continue; }
                    if (!Model[x, y + 2, zb] || !Model[x + 1, y + 2, zb])
                    { y += 2; continue; }
                    if (!Model[x, y + 1, zb] || !Model[x + 1, y + 1, zb])
                    { y++; continue; }
                    if (!Model[x, y, zb] || !Model[x + 1, y, zb])
                        continue;
                    result.Add(new Brick(new Point(x, y, zb), new Point(x + 2, y + 4, zb+1)));
                }
            }

            // 2x4 top (rotation stretching in x)
            for (var y = brick.LeftLowNear.Y - 1; y < brick.RightUpFar.Y; y++)
            {
                for (var x = brick.LeftLowNear.X - 3; x < brick.RightUpFar.X; x++)
                {
                    if (!Model[x+3, y, zt] || !Model[x + 3, y+1, zt])
                    { x += 3; continue; }
                    if (!Model[x+2, y, zt] || !Model[x + 2, y+1, zt])
                    { x += 2; continue; }
                    if (!Model[x+1, y, zt] || Model[x + 1, y + 1, zt])
                    { x++; continue; }
                    if (!Model[x, y, zt] || !Model[x, y+1, zt])
                        continue;
                    result.Add(new Brick(new Point(x, y, zt), new Point(x + 4, y + 2, zt+1)));
                }
            }



            // 2x4 bottom (rotation stretching in x)
            for (var y = brick.LeftLowNear.Y - 1; y < brick.RightUpFar.Y; y++)
            {
                for (var x = brick.LeftLowNear.X - 3; x < brick.RightUpFar.X; x++)
                {
                    if (!Model[x + 3, y, zb] || !Model[x + 3, y + 1, zb])
                    { x += 3; continue; }
                    if (!Model[x + 2, y, zb] || !Model[x + 2, y + 1, zb])
                    { x += 2; continue; }
                    if (!Model[x + 1, y, zb] || !Model[x + 1, y + 1, zb])
                    { x++; continue; }
                    if (!Model[x, y, zb] || !Model[x, y + 1, zb])
                        continue;
                    result.Add(new Brick(new Point(x, y, zb), new Point(x + 4, y + 2, zb+1)));
                }
            }

            return result.ToArray();
        }

        // Take latest added Brick
        // Add new slots introduces by Brick
        public bool CreateNewSlots()
        {
            var brick = Bricks.Peek();
            var potentialBricks = AllNewBricksFor(brick);

            var before = Slots.Count;
            foreach(var brickCandidate in potentialBricks)
            {
                if (!Model.Can(brickCandidate))
                    continue;

                var touches = Bricks.Select(x => brickCandidate.InTouch(x) ? 1 : 0).Sum();
                Slots.Insert(new Slot(brickCandidate, touches + brickCandidate.Volume));
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
