﻿namespace Outlays.Data.Models
{
    public class CalculateRoomOutlays
    {
        public int UsersCount { get; set; }
        public int TotalCost { get; set; }
        public int CostPerUser => TotalCost / UsersCount;
    }
}
