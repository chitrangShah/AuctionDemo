"use strict";

document.addEventListener('DOMContentLoaded', function() {

    console.log("event listener activated");

    var auctionHub = new signalR.HubConnectionBuilder().withUrl("/auction").build();

    auctionHub.on('AddAuctions', function (auctions) {
        console.log('AddAuctions', JSON.stringify(auctions));
        //TODO: Add auction to HTML and update
    });

    auctionHub.on('AuctionStart', function (auctionId) {
        console.log('Auction is starting', JSON.stringify(auctionId));
        //TODO: Activate bid button, update auction status
    });

    auctionHub.on('AuctionEnd', function (auctionId) {
        console.log('AuctionEnd', auctionId);
        //TODO: Disable bid button, update auction status
    });

    auctionHub.start()
        .then(function () { })
        .catch(function (e) {
            console.log("Connection error" + e.message);
    });

    auctionHub.onClosed = function (e) {
        console.log("Connection closed" + e.message);
    };
});

function addAuction(auction) {
    console.log(auction);
    //TODO: add auction to table and insert values in new rows.
    // Update auction status
}
    