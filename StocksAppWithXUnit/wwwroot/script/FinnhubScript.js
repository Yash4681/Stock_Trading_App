

var token = document.querySelector("#hiddenFinnhubToken").value;
var stockSymbol = document.querySelector("#hiddenStockSymbol").value;
var socket = new WebSocket(`wss://ws.finnhub.io?token=${token}`);

socket.addEventListener("open", function (event) {
    socket.send(JSON.stringify({ 'type': 'subscribe', 'symbol': stockSymbol }))
});

socket.addEventListener('message', function (event) {
    if (event.data.type == 'error') {
        $(".price").text(event.data.msg);
        return;
    }

    var eventData = JSON.parse(event.data);
    if (eventData) {
        if (eventData.data) {
            var updatedPrice = JSON.parse(event.data).data[0].p;
            var timeStamp = JSON.parse(event.data).data[0].t;

            $(".price").text(updatedPrice.toFixed(2));
        }
    }
});

var unsubsribe = function (symbol) {
    socket.send(JSON.stringify({ 'type': 'unsubscribe', 'symbol': symbol }));
}

window.onunload = function () {
    unsubsribe(stockSymbol);
}
//data received from server
//console.log('Message from server ', event.data);

/* Sample response:
{"data":[{"p":220.89,"s":"MSFT","t":1575526691134,"v":100}],"type":"trade"}
type: message type
data: [ list of trades ]
s: symbol of the company
p: Last price
t: UNIX milliseconds timestamp
v: volume (number of orders)
c: trade conditions (if any)
*/