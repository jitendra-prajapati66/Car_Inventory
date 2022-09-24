$(function () {
    var PlaceHolder = $("#placeHolder");
    $('button[data-toggle="model"]').click(function (event) {
        var url = $(this).data("url");
        var decodedUrl = decodeURIComponent(url);
        $.get(decodedUrl).done(function (data) {
            PlaceHolder.html(data);
            PlaceHolder.find(".modal").modal('show');
        });
    });
    PlaceHolder.on("click", '[data-dismiss="modal"]', function () {
        /*PlaceHolder.find(".modal").modal('hide');*/
        $(".modal-backdrop").hide();
    })
   
});
$(function () {
    $("#updateCarData").click(function () {
        var brand = $("#Brand").val();
        var model = $("#Model").val();
        var price = parseFloat($("#Price").val());
        console.log(price);
        var year = parseInt($("#Year").val());
        var isNew = $("#isNew").val();
        if (price < 500000) {
            alert("Price must be greater than 500000 and ");
            return false;
        } else if (year > 2022 || year < 1900) {
            alert("year must less than 2022 and greater than 1900");
            return false;
        }
    });
});


