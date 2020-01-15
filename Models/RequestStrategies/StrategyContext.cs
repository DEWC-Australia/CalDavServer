using CalDav.Models.FileSystem.Folder;
using CalDav.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalDav.Models.RequestStrategies
{
    public class StrategyContext
    {
        private IStrategy strategy { get; set; }

        public StrategyContext(IStrategy strategy)
        {
            this.strategy = strategy;
        }

        public Result executeStrategy()
        {
            return strategy.DoOperation();
        }
    }
}
