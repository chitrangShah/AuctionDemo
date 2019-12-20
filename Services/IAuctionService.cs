
using System.Collections.Concurrent;
using System.Collections.Generic;
using Auction_Site.Models;

namespace Auction_Site.Services
{
    public interface IAuctionService
    {
        IEnumerable<Auction> Auctions {get;set;}
        Auction CreateAuction();
        void UpdateAuctions();
    }
}