
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Auction_Site.Hubs;
using Auction_Site.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;

namespace Auction_Site.Services
{
    /// <summary>
    /// Auction service to create and update auctions
    /// </summary>
    public class AuctionService : IAuctionService
    {
        public Dictionary<int, Auction> AuctionMap = 
                        new Dictionary<int, Auction>();

        public IEnumerable<Auction> Auctions {get;set;}
        private Random _random = new Random();
        private Timer _timer;
        private IHubContext<AuctionHub> _auctionHubContext;
        
        public AuctionService(IApplicationLifetime appLifetime, IHubContext<AuctionHub> auctionHubContext)
        {
            appLifetime.ApplicationStarted.Register(() => InitializeTimer());

            _auctionHubContext = auctionHubContext;

            // Create 3 auctions with multiple item and add to map
            for (var i = 0; i < 3; i++)
            {
                var auction = CreateAuction();
                AuctionMap[auction.Id] = auction;
            }

            Auctions = AuctionMap.Values;
        }

        /// <summary>
        /// Timer thread to peridically update auctions on separate threads. 
        /// Updates auctions every second with a gap of a second
        /// TODO: Dispose correctly using "using"
        /// </summary>
        private void InitializeTimer()
        {
            _timer = new Timer(x => ((AuctionService)x).UpdateAuctions(), this, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        }

        /// <summary>
        /// Creates a new auction with multiple items
        /// Sets random start and end prices, random start and end times
        /// </summary>
        /// <returns></returns>
        public Auction CreateAuction()
        {
            var auctionItems = new[]
            {
                new AuctionItem 
                { 
                    Name = "Ferrari" 
                },
                new AuctionItem 
                { 
                    Name = "Mercedes" 
                },
                new AuctionItem 
                { 
                    Name = "Lamborghini" 
                }
            };

            // Min auction time of 2 min, max = 10 min
            var startTime = DateTimeOffset.Now.AddMinutes(_random.Next(2, 10));

            // Notify auction between 1 and 5 min remaining
            var endTime = startTime.AddMinutes(_random.Next(1, 5));

            // Item price values
            var startPrice = _random.Next(1, 100);

            // Bid price values
            var bidPrice = startPrice + _random.Next(1, 50);

            // Create new auction item by picking random from above list
            var auctionItem = new Auction(auctionItems[_random.Next(0, auctionItems.Length - 1)], startTime, endTime, startPrice, bidPrice);
            return auctionItem;
        }

        /// <summary>
        /// Update auctions if they have ended, otherwise keep them open
        /// </summary>
        public void UpdateAuctions()
        {
            foreach (var auction in AuctionMap)
            {
                if (auction.Value.EndTime < DateTime.Now)
                {
                    if (AuctionMap.Remove(auction.Key, out _))
                    {
                        _auctionHubContext.Clients.All.SendAsync("AuctionEnd", auction.Key);

                        //TODO: Add new auction item if auction has ended.
                    }
                }
                else if (!auction.Value.Active && auction.Value.StartTime < DateTime.Now)
                {
                    auction.Value.Active = true;
                    _auctionHubContext.Clients.All.SendAsync("AuctionStart", auction.Key);
                }
            }
        }
    }
}