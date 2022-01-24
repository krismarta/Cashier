$(document).ready(function () {
    tableCashier = $("#listcashier").DataTable({
        "ajax": {
            "url": "/Cashier/GetAllCashier",
            "dataSrc": ""
        },
        "pageLength": 10,
        "columns": [
            {
                "data": "id"
            },
            {
                "data": "name"
            },
            {
                "data": "phone"
            },
            {
                "data": "email"
            },
            
            {
                "data": null,
                "render": function (data, type, row) {
                    var ids = row['id'];
                    return `<div class="btn-group">
                                    <button type="button" class="btn btn-sm btn-circle btn-warning" data-bs-toggle="modal" onclick="getCashierByid('${ids}')" data-bs-target="#detailCharacterModal">
                                        <span class="fas fa-edit"></span>
                                    </button>
                                    &nbsp;
                                    <button type="button" class="btn btn-sm btn-circle btn-danger" onclick="DeleteCashier('${ids}')" id="btndeleteEmployee">
                                        <span class="fas fa-trash"></span>
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
                    columns: [0, 1, 2, 3, 4]
                }
            },
            {
                extend: 'csv',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4]
                }
            },
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4]
                }
            },
            {
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4]
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4]
                }
            },
        ]

    });
    //$('.dt-buttons').addClass('btn btn-info');
    $('.dt-button').removeClass().addClass("btn btn-info");
});

(function () {
    'use strict';
    window.addEventListener('load', function () {
        var forms = document.getElementsByClassName('needs-validation');
        var validation = Array.prototype.filter.call(forms, function (form) {
            $("#btnSaveCashier").click(function () {
                if (form.checkValidity() === false) {
                    event.preventDefault();
                    event.stopPropagation();
                } else {
                    var data_action = $(this).attr("data-name");
                    if (data_action == "insert") {
                        InsertCashier();
                    } else if (data_action == "update") {
                        console.log("langsung Update bray");
                        var id = $(this).attr("data-id");

                        UpdateActionCashier(id);
                    }
                    console.log(data_action);
                }

                form.classList.add('was-validated');

            })

        });
    }, false);
})();

function DeleteCashier(id) {
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
                url: "users/delete/" + id,
                type: "DELETE",

            }).done((result) => {
                Swal.fire(
                    'Yeayy',
                    'Data Berhasil dihapus',
                    'success'
                )
                tableCashier.ajax.reload();
                $('.createCashier').modal('hide');

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

function getCashierByid(id) {
    $.ajax({
        url: "/users/get/" + id
    }).done((result) => {
        console.log(result)
        $('.createCashier').modal('show');
        $('#fCashier').removeClass('was-validated')
        $('#labelText').html("Update Cashier");
        $('#inputId').prop('readonly', true);
        $('#inputId').val(id).readonly;
        $('#inputName').val(result.name);
        $('#inputEmail').prop('readonly', true);
        $('#inputEmail').val(result.email);
        $('#inputPhone').val(result.phone);
        $('#btnSaveCashier').attr('data-name', 'update').html("<span class='fas fa-save'>&nbsp;</span>Update Cashier")
        $('#btnSaveCashier').attr('data-id', id);
    }).fail((error) => {
        console.log(error);
    });
}


function UpdateActionCashier(id) {
    var obj = new Object();
    obj.id = $("#inputId").val();
    obj.name = $("#inputName").val();
    obj.email = $("#inputEmail").val();
    obj.phone = $("#inputPhone").val();
 
    console.log(JSON.stringify(obj));
    $.ajax({
        url: "users/Put/" + id,
        type: "PUT",
        data: obj
    }).done((result) => {
        Swal.fire(
            'Yeayy',
            'Data ' + id + ' Berhasil diubah',
            'success'
        )
        tableCashier.ajax.reload();
        $('.createCashier').modal('hide');

    }).fail((error) => {

        Swal.fire(
            'Opps!',
            'Sepertinya terjadi kesalahan, periksa kembali!',
            'error'
        )
    });
}

//insert
function showCreateCashier() {
    $('.createCashier').modal('show');
    clearFormCashier();
}

function clearFormCashier() {
    $("#inputId").val("AUTO GENERATE");
    $('#labelText').html("Create New Cashier");
    $('#inputId').prop('readonly', true);
    $('#inputId').val("Auto Generate");
    $('#inputName').removeAttr('readonly');
    $('#inputemail').val("");
    $('#inputPhone').val("");

    $('#btnSaveCashier').attr('data-name', 'insert').html("<span class='fas fa-save'>&nbsp;</span>Save New Cashier")
    $('#btnSaveCashier').removeAttr('data-nik');
    $('#fCashier').removeClass('was-validated')
}


function InsertCashier() {
    
    var obj = new Object();
    obj.id = $("#inputId").val();
    obj.name = $("#inputName").val();
    obj.email = $("#inputEmail").val();
    obj.phone = $("#inputPhone").val();
    console.log(obj);

    $.ajax({
        url: "/Users/RegisterAccount",
        type: "POST",
        data: obj
    }).done((result) => {
        Swal.fire(
            'Yeayy',
            "Data Berhasil ditambahkan",
            'success'
        )
        tableCashier.ajax.reload();
        $('.createCashier').modal('hide');

    }).fail((error) => {
        Swal.fire(
            'Opps!',
            'Sepertinya terjadi kesalahan, periksa kembali!',
            'error'
        )
    })
}