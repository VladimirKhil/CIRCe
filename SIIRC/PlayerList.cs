using System;
using System.Collections.Generic;
using System.Text;

namespace SIIRC
{
    [Serializable]
    public sealed class PlayerList: List<Player>
    {
        public PlayerList() { }
        
        public new void Add(Player player)
        {
            foreach (Player item in this)
            {
                if (item.Equals(player))
                    return;
            }
            base.Add(player);
        }
    }
}
