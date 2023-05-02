using System.Collections.Generic;
using Models;

namespace Interfaces
{
    public interface IPlayerStatsObserver
    {
        void UpdatePlayerStats(Dictionary<string, PlayerStat> playerStats);
    }
}