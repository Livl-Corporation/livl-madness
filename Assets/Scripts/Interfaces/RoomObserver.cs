using System.Collections.Generic;
using Models;

namespace Interfaces
{
    public interface IRoomObserver
    {
        void UpdateRoom(Dictionary<uint, PlayerRoom> players);
    }
}