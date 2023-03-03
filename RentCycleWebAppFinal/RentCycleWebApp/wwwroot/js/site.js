// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

jQuery(document).ready(function ($) {


    var map;
    var latlng;
    var infowindow;
    $(document).ready(function () {
        //get data set from the backend in a json structure
        var data = [{
            "description": "RENT500001",
            "location": "G133",
            "latitude": "18.67505065",
            "longitude": "73.8921434"
        },
        {
            "description": "RENT500002",
            "location": "G133",
            "latitude": "18.673516864",
            "longitude": "73.89358033"
        },
        {
            "description": "RENT500003",
            "location": "G133",
            "latitude": "18.671978182",
            "longitude": "73.890532884"
        },
        {
            "description": "RENT500004",
            "location": "G133",
            "latitude": "18.666432584",
            "longitude": "73.8893230239"
        },
        {
            "description": "RENT500005",
            "location": "G133",
            "latitude": "18.674589988",
            "longitude": "73.884321070"
        },
        {
            "description": "RENT500006",
            "location": "G133",
            "latitude": "18.661803132",
            "longitude": "73.8867235006"
        },
        {
            "description": "RENT500007",
            "location": "G133",
            "latitude": "18.675615486",
            "longitude": "73.880622555"
        },
        {
            "description": "RENT500008",
            "location": "G133",
            "latitude": "18.675615486",
            "longitude": "73.880622555"
        },

        ]
        ViewCustInGoogleMap(data);
    });

    function ViewCustInGoogleMap(data) {
        var gm = google.maps;
        var mapOptions = {
            center: new google.maps.LatLng(18.675538525542592, 73.89077019369685),
            // mapTypeId: google.maps.MapTypeId.SATELLITE,
            zoom: 14.5,
        };
        map = new google.maps.Map(document.getElementById("map-canvas"), mapOptions);
        infowindow = new google.maps.InfoWindow();
        var marker, i;
        var MarkerImg ="/images/redmark.png";


        var tipObj = null;


        for (var i = 0; i < data.length; i++) {
            marker = new gm.Marker({
                position: new gm.LatLng(data[i]['latitude'], data[i]['longitude']),
                map: map,
                icon: MarkerImg,
            });

            google.maps.event.addListener(
                marker,
                'mouseover',
                (
                    function (marker, i) {
                        return function () {
                            tipObj = document.createElement("div");
                            tipObj.style.width = '250px';
                            tipObj.style.height = '50px';
                            tipObj.style.background = "#9c8d7c";
                            tipObj.style.borderRadius = "5px";
                            tipObj.style.padding = "2px";
                            tipObj.style.fontFamily = "Arial,Helvetica";
                            tipObj.style.textAlign = "center";
                            tipObj.style.fontSize = "12";
                            tipObj.style.fontWeight = "900";
                            tipObj.style.color = "black";
                            tipObj.innerHTML = data[i]['description'] + "<br>" + (data[i]['latitude']) + "," + (data[i]['longitude'])
                            infowindow.setContent(tipObj);
                            infowindow.open(map, marker);
                        };
                    }
                )(marker, i)
            );

        }


        //create the polygon object using these coordinates
        var coords = [
            new google.maps.LatLng(18.696975108955428, 73.91781379851292),
            new google.maps.LatLng(18.679375864990718, 73.84905964644226),
            new google.maps.LatLng(18.664895598960925, 73.84877186694177),
            new google.maps.LatLng(18.632616309943003, 73.8748504063968),
            new google.maps.LatLng(18.646964425678814, 73.90679494507602),
            new google.maps.LatLng(18.675096570734464, 73.90556645959141),
        ];




        polygon = new google.maps.Polygon({
            paths: coords,
            strokeColor: "#00912c",
            strokeOpacity: 0.6,
            strokeWeight: 2,
            fillColor: "#e4f7e9",
            fillOpacity: 0.4,
            someRandomData: "Geo-Fencing"
        });

        polygon.setMap(map);
    }

});

