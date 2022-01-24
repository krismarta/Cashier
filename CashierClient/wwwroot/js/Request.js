$(document).ready(function () {
    tableRequest = $("#listRequest").DataTable({
        "ajax": {
            "url": "/Request/getAllRequest",
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
                "data": "supplier.name"
            },
            {
                "data": "supplier.email"
            },
            {
                "data": "supplier.phone"
            },
            {
                "data": "supplier.companyName"
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
                    } else if (status == "success") {
                        return `<span class="badge badge-success">${status}</span>`;
                    } else if (status == "pending") {
                        return `<span class="badge badge-primary">${status}</span>`;
                    }
                    
                }
            },
            {
                "data": null,
                "render": function (data, type, row) {
                    var ids = row['id'];
                    return `<div class="btn-group">
                                    <button type="button" class="btn btn-sm btn-circle btn-info" data-bs-toggle="modal" onclick="getRequestByid('${ids}')" data-bs-target="#detailCharacterModal">
                                        <span class="fas fa-eye"></span>
                                    </button>
                                </div>
                            `;
                },
                "orderable": false
            }
        ],
        dom: "lBfrtip",
        buttons: [
            {
                extend: 'copyHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5,6]
                }
            },
            {
                extend: 'csv',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5,6,7]
                }
            },
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5,6]
                }
            },
            {
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5,6]
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5,6]
                }
            },
        ]

    });

    // Add event listener for opening and closing details
    $('#listRequest tbody').on('click', 'td.dt-control', function () {
        var tr = $(this).closest('tr');
        var row = tableRequest.row(tr);

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
        url: "/Request/getAllRequest",
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
    console.log(d.detailRequests.length);
    for (var i = 0; i < d.detailRequests.length; i++) {
        
        datax += "<tr><td>" + d.detailRequests[i].goods.id + "</td><td>" + d.detailRequests[i].goods.name +
            "</td><td>" + d.detailRequests[i].quantity + "</td><td>" + d.detailRequests[i].goods.stok + "</td><td>" + d.detailRequests[i].goods.category.name + "</td></tr>"
    }
    //return datax;
    // `d` is the original data object for the row
    return '<table class ="table table-hover table-bordered display" width="100%" cellspacing="0">' +
        '<tr>' +
        '<th>ID Goods</td>' +
        '<th>Name Goods</td>' +
        '<th>Stok Request</td>' +
        '<th>Current Stok</td>' +
        '<th>Category</td>' +
        '</tr>' +
        datax
        '</table>';

}

(function () {
    'use strict';
    window.addEventListener('load', function () {
        var forms = document.getElementsByClassName('needs-validation');
        var validation = Array.prototype.filter.call(forms, function (form) {
            $("#btnSaveRequest").click(function () {
                if (form.checkValidity() === false) {
                    event.preventDefault();
                    event.stopPropagation();
                } else {
                    var data_action = $(this).attr("data-name");
                    if (data_action == "insert") {
                        InsertRequest();
                    } else if (data_action == "update") {
                        console.log("langsung Update bray");
                        var id = $(this).attr("data-id");

                        UpdateActionRequest(id);
                    }
                    console.log(data_action);
                }

                form.classList.add('was-validated');

            })

        });
    }, false);
})();


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



function DeleteRequest(id) {
    Swal.fire({
        title: 'Yakin ingin dihapus?',
        text: "Data akan dihapus dari database.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Hapus Sekarang!',
        cancelButtonText: 'Batalkan'

    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "Request/delete/" + id,
                data: { id },
                type: "DELETE",

            }).done((result) => {
                console.log(result);
                Swal.fire(
                    'Yeayy',
                    'Data Berhasil dihapus',
                    'success'
                )
                tableRequest.ajax.reload();
                $('.createRequest').modal('hide');

            }).fail((error) => {

                Swal.fire(
                    'Opps!',
                    'Sepertinya terjadi kesalahan, periksa kembali!',
                    'error'
                )
            })
        }
    })
}

function getRequestByid(id) {
    $.ajax({
        url: "/Request/get/" + id
    }).done((result) => {
        console.log(result)
        $('.createRequest').modal('show');
        $('#fRequest').removeClass('was-validated')
        $('#labelText').html("Update Status");
        $('#inputId').prop('readonly', true);
        $('#inputId').val(id).readonly;
        $('#selectstatus').val("0");
        $('#btnSaveRequest').attr('data-name', 'update').html("<span class='fas fa-save'>&nbsp;</span>Update Status")
        $('#btnSaveRequest').attr('data-id', id);
    }).fail((error) => {
        console.log(error);
    });
}


function UpdateActionRequest(id) {
    var obj = new Object();
    obj.id = $("#inputId").val();
    obj.status = $("#selectstatus").val();

    if (obj.status == "0" || obj.id == "") {
        Swal.fire(
            'Opps!',
            'Data harus terisi dengan lengkap',
            'warning'
        )
    } else {
        console.log(obj);
        $.ajax({
            url: "/Request/UpdateStatusRequest",
            type: "POST",
            data: obj
        }).done((result) => {
            console.log("RESULT : " + result);
            if (result == "200") {
                Swal.fire(
                    'Status Request Berhasil diperbarui',
                    'Status berhasil diperbarui, stok berhasil ditambahkan',
                    'success'
                );
            } else if (result == "409") {
                Swal.fire(
                    'Status Request Tidak dapat diperbarui ',
                    'request stok ini sudah pernah dilakukan pembaruan',
                    'warning'
                );
            }
            else {
                Swal.fire(
                    'Opps!',
                    'Sepertinya terjadi kesalahan, periksa kembali!',
                    'error'
                )
            } 
            tableRequest.ajax.reload();
            $('.createRequest').modal('hide');

        }).fail((error) => {
            Swal.fire(
                'Opps!',
                'Sepertinya terjadi kesalahan, periksa kembali!',
                'error'
            )
        });
    }
    
   
}

function AutoGenerateID() {
    return Math.floor(Math.random() * 9999999);
}
