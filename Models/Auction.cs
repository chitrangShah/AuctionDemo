
using System;
using System.Threading;

namespace Auction_Site.Models
{
    /// <summary>
    /// Auction item
    /// </summary>
    public class Auction
    {
        static int _id = 0;
        public int Id { get; }
        public AuctionItem Item { get; }
        public DateTimeOffset StartTime { get; }
        public DateTimeOffset EndTime { get; }
        public decimal StartingPrice { get; }
        public decimal BidPrice { get; }
        public bool Active { get; set; }

        public Auction(AuctionItem item, DateTimeOffset startTime, 
                    DateTimeOffset endTime, decimal startingPrice, decimal bid)
        {
            // Auto incrementing Id
            Id = Interlocked.Increment(ref _id);
            Item = item;
            StartTime = startTime;
            EndTime = endTime;
            StartingPrice = startingPrice;
        }
    }
}