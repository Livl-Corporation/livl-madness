using System.Collections.Generic;
using Models;

namespace Interfaces
{
    public interface IPlayerStatsObserver
    {
        void Update(Dictionary<string, PlayerStat> playerStats);
    }
}