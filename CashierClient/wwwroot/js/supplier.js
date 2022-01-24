$(document).ready(function () {
    
    tableSupplier = $("#listsupplier").DataTable({
        "ajax": {
            "url": "/Supplier/getAllSupplier",
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
                "data": "companyName"
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
                                    <button type="button" class="btn btn-sm btn-circle btn-warning" data-bs-toggle="modal" onclick="getSupplierByid('${ids}')" data-bs-target="#detailCharacterModal">
                                        <span class="fas fa-edit"></span>
                                    </button>
                                    &nbsp;
                                    <button type="button" class="btn btn-sm btn-circle btn-danger" onclick="DeleteSupplier('${ids}')" id="btndeleteEmployee">
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
                    columns: [0, 1, 2, 3, 4,5]
                }
            },
            {
                extend: 'csv',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5]
                }
            },
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5]
                }
            },
            {
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5]
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5]
                }
            },
        ]

    });
    
    $('.dt-button').removeClass().addClass("btn btn-info");
    $("#inputId").val("S" + AutoGenerateID());
});

(function () {
    'use strict';
    window.addEventListener('load', function () {
        var forms = document.getElementsByClassName('needs-validation');
        var validation = Array.prototype.filter.call(forms, function (form) {
            $("#btnSaveSupplier").click(function () {
                if (form.checkValidity() === false) {
                    event.preventDefault();
                    event.stopPropagation();
                } else {
                    var data_action = $(this).attr("data-name");
                    if (data_action == "insert") {
                        InsertSupplier();
                    } else if (data_action == "update") {
                        console.log("langsung Update bray");
                        var id = $(this).attr("data-id");

                        UpdateActionSupplier(id);
                    }
                    console.log(data_action);
                }

                form.classList.add('was-validated');

            })

        });
    }, false);
})();

function DeleteSupplier(id) {
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
                url: "Supplier/delete/" + id,
                type: "DELETE",

            }).done((result) => {
                Swal.fire(
                    'Yeayy',
                    'Data Berhasil dihapus',
                    'success'
                )
                tableSupplier.ajax.reload();
                $('.createSupplier').modal('hide');

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

function getSupplierByid(id) {
    $.ajax({
        url: "/supplier/get/" + id
    }).done((result) => {
        console.log(result)
        $('.createSupplier').modal('show');
        $('#fSupplier').removeClass('was-validated')
        $('#labelText').html("Update Supplier");
        $('#inputId').prop('readonly', true);
        $('#inputId').val(id).readonly;
        $('#inputName').val(result.name);
        $('#inputNameCompany').val(result.companyName);
        
        $('#inputEmail').val(result.email);
        $('#inputPhone').val(result.phone);
        $('#btnSaveSupplier').attr('data-name', 'update').html("<span class='fas fa-save'>&nbsp;</span>Update Supplier")
        $('#btnSaveSupplier').attr('data-id', id);
    }).fail((error) => {
        console.log(error);
    });
}


function UpdateActionSupplier(id) {
    var obj = new Object();
    obj.id = $("#inputId").val();
    obj.name = $("#inputName").val();
    obj.email = $("#inputEmail").val();
    obj.phone = $("#inputPhone").val();
    obj.companyName = $("#inputNameCompany").val();
 
    console.log(JSON.stringify(obj));
    $.ajax({
        url: "Supplier/Put/" + id,
        type: "PUT",
        data: obj
    }).done((result) => {
        Swal.fire(
            'Yeayy',
            'Data ' + id + ' Berhasil diubah',
            'success'
        )
        tableSupplier.ajax.reload();
        $('.createSupplier').modal('hide');

    }).fail((error) => {

        Swal.fire(
            'Opps!',
            'Sepertinya terjadi kesalahan, periksa kembali!',
            'error'
        )
    });
}

//insert
function showCreateSupplier() {
    $('.createSupplier').modal('show');
    clearFormSupplier();
}

function clearFormSupplier() {
   
    $('#labelText').html("Create New Supplier");
    $('#inputId').prop('readonly', true);
    $('#inputid').val("Auto Generate");
    $('#inputName').removeAttr('readonly');
    $('#inputNameCompany').val("");
    $('#inputemail').val("");
    $('#inputPhone').val("");
    $('#btnSaveSupplier').attr('data-name', 'insert').html("<span class='fas fa-save'>&nbsp;</span>Save New Supplier")
    $('#btnSaveSupplier').removeAttr('data-nik');
    $('#fSupplier').removeClass('was-validated')
}


function InsertSupplier() {
    var obj = new Object();

    obj.id = $("#inputId").val();
    obj.name = $("#inputName").val();
    obj.email = $("#inputEmail").val();
    obj.phone = $("#inputPhone").val();
    obj.companyName = $("#inputNameCompany").val();
    console.log(obj);

    $.ajax({
        url: "/Supplier/post",
        type: "POST",
        data: obj
    }).done((result) => {
        Swal.fire(
            'Yeayy',
            "Data Berhasil ditambahkan",
            'success'
        )
        tableSupplier.ajax.reload();
        $('.createSupplier').modal('hide');

    }).fail((error) => {
        Swal.fire(
            'Opps!',
            'Sepertinya terjadi kesalahan, periksa kembali!',
            'error'
        )
    })
}

function AutoGenerateID() {
    return Math.floor(Math.random() * 9999999);
}