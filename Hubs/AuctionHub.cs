using Auction_Site.Services;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Auction_Site.Hubs
{
    /// <summary>
    /// Auction hub to initiate the auctions
    /// </summary>
    public class AuctionHub : Hub
    {
        private IAuctionService _auctionService;
        public AuctionHub(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }

        public override Task OnConnectedAsync()
        {
            return Clients.Client(Context.ConnectionId)
                .SendAsync("AddAuctions", _auctionService.Auctions);
        }
    }
}