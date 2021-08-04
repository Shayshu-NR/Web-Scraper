<%@ Page Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="WebScraper._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Web Scraper</title>
    <link rel="icon" href="code_black_24dp.svg" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js" integrity="sha256-/xUj+3OJU5yExlq6GSYGSHk7tPXikynS7ogEvDej/m4=" crossorigin="anonymous"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js" integrity="sha256-VazP97ZCwtekAsvgPBSUwPFKdrwD3unUfSGVYrahUqU=" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
</head>
<body>
    <div id="scrapeUI" runat="server">

    </div>
</body>

<script>
    $(document).ready(function () {
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })

        $(".btn.btn-primary").on("click", function () {
            // Make sure both the url and path are valid, if no path is specified then use ./
            var url = $("#requestURL").val();
            var path = $("#downloadLocation").val();
            var dataDict = JSON.stringify({ "requestInfo": [url, path] } );
            console.log(dataDict);

            $.ajax({
                type: "POST",
                url: "default.aspx/download",
                dataType: "json",
                data: dataDict,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    
                    var recvData = JSON.parse(data.d);
                    console.log(data.d);
                    console.log(recvData);
                }
            });
        })
    })
</script>
</html>
