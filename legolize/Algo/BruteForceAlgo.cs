﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Legolize.Algo
{
    class BruteForceAlgo
    {
        private readonly Stack<IModelMaster> _masters = new Stack<IModelMaster>();
        private readonly Config _config;

        private object _lockBest = new object();
        private Brick[] _bestSoFar;
        private int _volumeSoFar = 0;
        private IModel _bestModel;
        

        public BruteForceAlgo(IModelMaster master)
        {
            _masters.Push(master);
            _config = new Config();
        }

        public void StepForward()
        {
            var master = _masters.Peek().Clone();
            _masters.Peek().MoveSlotPosition();

            master.SlotToBrick();
            master.CreateNewSlots();
            
            var volume = master.Bricks.Select(x => x.Volume).Sum();
            if (volume > _volumeSoFar)
            {
                lock(_lockBest)
                {
                    if (volume > _volumeSoFar)
                    {
                        _volumeSoFar = volume;
                        _bestSoFar = new Brick[master.Bricks.Count];
                        master.Bricks.CopyTo(_bestSoFar, 0);
                        _bestModel = master.Model.DeepClone();
                    }
                }
                
            }
            
            _masters.Push(master);
        }

        public void StepBack()
        {
            _masters.Pop();
            if(_masters.Count != 0)
                _masters.Peek().MoveBack();
        }

        public Brick[] Go(int cycles)
        {

            for (var i = 0; i < cycles; i++)
            {
                if ((i & 0xdff) == 0)
                    Console.WriteLine(i);

                while (_masters.Count > 0 && _masters.Peek().NRemainingSlotsToSearch == 0)
                    StepBack();

                if(_masters.Count == 0)
                    return _bestSoFar;

                StepForward();
                if (!_masters.Peek().Model.HasAny())
                    return _bestSoFar;

                if (_config.DebugStepping)
                {
                    Console.WriteLine($"Cycle: {i}");
                    Console.WriteLine(_masters.Peek().Model);
                    Console.ReadKey();
                }
            }
            Console.WriteLine(_bestModel);
            return _bestSoFar;
        }

    }
}
