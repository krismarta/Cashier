$(document).ready(function () {

    tableTransaction = $("#listTransaction").DataTable({
        "ajax": {
            "url": "/transaction/getAllTransaction",
            "dataSrc": ""
        },
        "pageLength": 10,
        "columns": [
            {
                "className": 'dt-control',
                "orderable": false,
                "data": null,
                "defaultContent": ''
            },
            {
                "data": "id"
            },
            {
                "data": "user.name"
            },
            {
                "data": "total"
            },
            {
                "data": "payment_type"
            },
            {
                "data": null,
                "render": function (data, type, row) {
                    var dtdate = row['date_trs'];
                    return formatDate(new Date(dtdate));

                }
            },
            {
                "data": null,
                "render": function (data, type, row) {
                    var status = row['status'];
                    if (status == "cancel") {
                        return `<span class="badge badge-danger">${status}</span>`;
                    } else if (status == "settlement") {
                        return `<span class="badge badge-success">${status}</span>`;
                    } else if (status == "pending") {
                        return `<span class="badge badge-primary">${status}</span>`;
                    }

                }
                
            }
        ],
        dom: "lBfrtip",
        buttons: [
            {
                extend: 'copyHtml5',
                exportOptions: {
                    columns: [1, 2, 3, 4,5]
                }
            },
            {
                extend: 'csv',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5]
                }
            },
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5]
                }
            },
            {
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5]
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5]
                }
            },
        ]

    });

    // Add event listener for opening and closing details
    $('#listTransaction tbody').on('click', 'td.dt-control', function () {
        var tr = $(this).closest('tr');
        var row = tableTransaction.row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row
            row.child(format(row.data())).show();
            tr.addClass('shown');
        }
    });
    $('.dt-button').removeClass().addClass("btn btn-info");

    $.ajax({
        url: "/transaction/getAllTransaction",
        type : "GET"
    }).done((result) => {
        console.log(result);
       
        //console.log(text)
    }).fail((error) => {
        console.log(error);
    });

});

function format(d) {
    var datax = "";
    console.log(d.detailTransactions.length);
    for (var i = 0; i < d.detailTransactions.length; i++) {
        
        datax += "<tr><td>" + d.detailTransactions[i].goods.id + "</td><td>" + d.detailTransactions[i].goods.name +
            "</td><td>" + d.detailTransactions[i].quantity + "</td><td>" + d.detailTransactions[i].goods.priceSell + "</td><td>" + d.detailTransactions[i].goods.category.name + "</td></tr>"
    }
    //return datax;
    // `d` is the original data object for the row
    return '<table class ="table table-hover table-bordered display" width="100%" cellspacing="0">' +
        '<tr>' +
        '<th>ID Goods</td>' +
        '<th>Name Goods</td>' +
        '<th>Quantity</td>' +
        '<th>Price Sell</td>' +
        '<th>Category</td>' +
        '</tr>' +
        datax
        '</table>';

}

function formatDate(date) {
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var ampm = hours >= 12 ? 'pm' : 'am';
    hours = hours % 12;
    hours = hours ? hours : 12;
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = hours + ':' + minutes + ' ' + ampm;
    return date.getDate() + "-" + (date.getMonth() + 1) + "-" + date.getFullYear() + " " + strTime;
}
