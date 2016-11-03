using System;
using System.Collections.Generic;
using System.Linq;

namespace Legolize.Algo
{
    class BruteForceAlgo
    {
        private readonly Stack<IModelMaster> _masters = new Stack<IModelMaster>();
        private Brick[] _bestSoFar;
        private int _volumeSoFar = 0;

        public BruteForceAlgo(IModelMaster master)
        {
            _masters.Push(master);
        }

        public void StepForward()
        {
            var master = _masters.Peek().Clone();
            _masters.Peek().MoveSlotPosition();

            master.SlotToBrick();
            master.CreateNewSlots();

            // should we go forward or backward ? 
            var volume = master.Bricks.Select(x => x.Volume).Sum();
            if (volume > _volumeSoFar)
            {
                _volumeSoFar = volume;
                _bestSoFar = new Brick[master.Bricks.Count];
                master.Bricks.CopyTo(_bestSoFar, 0);
            }
            
            _masters.Push(master);
        }

        public void StepBack()
        {
            _masters.Pop();
            _masters.Peek().MoveBack();
        }

        public Brick[] Go(int cycles)
        {
            for (var i = 0; i < cycles; i++)
            {
                while (_masters.Count > 0 && _masters.Peek().NSlotsToSearch == 0)
                    StepBack();

                if(_masters.Count == 0)
                    return _bestSoFar;

                StepForward();
                if (!_masters.Peek().Model.HasAny())
                    return _bestSoFar;

                Console.WriteLine($"Cycle: {i}");
                Console.WriteLine(_masters.Peek().Model);
            }

            return _bestSoFar;
        }

    }
}
