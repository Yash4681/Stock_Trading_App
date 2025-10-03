const { get } = require("jquery")

$(document).ready(fuction(){
    $('#loadViewComponentbtn').click(function () {
        $.ajax({
            url: @Url.Action('GetSelectedStockViewComponent', 'Stocks'),
            type: 'GEt',
            data: {stockSymbol = }
        })
    })
})