$(document).ready(function () {
    
    tableCategory = $("#listCategory").DataTable({
        "ajax": {
            "url": "/Category/getAllCategory",
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
                "data": null,
                "render": function (data, type, row) {
                    var ids = row['id'];
                    return `<div class="btn-group">
                                    <button type="button" class="btn btn-sm btn-circle btn-warning" data-bs-toggle="modal" onclick="getCategoryByid('${ids}')" data-bs-target="#detailCharacterModal">
                                        <span class="fas fa-edit"></span>
                                    </button>
                                    &nbsp;
                                    <button type="button" class="btn btn-sm btn-circle btn-danger" onclick="DeleteCategory('${ids}')" id="btndeleteEmployee">
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
                    columns: [0, 1, 2]
                }
            },
            {
                extend: 'csv',
                exportOptions: {
                    columns: [0, 1, 2]
                }
            },
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [0, 1, 2]
                }
            },
            {
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: [0, 1, 2]
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: [0, 1, 2]
                }
            },
        ]

    });
    
    $('.dt-button').removeClass().addClass("btn btn-info");
});

(function () {
    'use strict';
    window.addEventListener('load', function () {
        var forms = document.getElementsByClassName('needs-validation');
        var validation = Array.prototype.filter.call(forms, function (form) {
            $("#btnSaveCategory").click(function () {
                if (form.checkValidity() === false) {
                    event.preventDefault();
                    event.stopPropagation();
                } else {
                    var data_action = $(this).attr("data-name");
                    if (data_action == "insert") {
                        InsertCategory();
                    } else if (data_action == "update") {
                        console.log("langsung Update bray");
                        var id = $(this).attr("data-id");

                        UpdateActionCategory(id);
                    }
                    console.log(data_action);
                }

                form.classList.add('was-validated');

            })

        });
    }, false);
})();

function DeleteCategory(id) {
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
                url: "Category/delete/" + id,
                type: "DELETE",

            }).done((result) => {
                Swal.fire(
                    'Yeayy',
                    'Data Berhasil dihapus',
                    'success'
                )
                tableCategory.ajax.reload();
                $('.createCategory').modal('hide');

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

function getCategoryByid(id) {
    $.ajax({
        url: "/Category/get/" + id
    }).done((result) => {
        console.log(result)
        $('.createCategory').modal('show');
        $('#fCategory').removeClass('was-validated')
        $('#labelText').html("Update Category");
        $('#inputId').prop('readonly', true);
        $('#inputId').val(id).readonly;
        $('#inputName').val(result.name);
        $('#btnSaveCategory').attr('data-name', 'update').html("<span class='fas fa-save'>&nbsp;</span>Update Category")
        $('#btnSaveCategory').attr('data-id', id);
    }).fail((error) => {
        console.log(error);
    });
}


function UpdateActionCategory(id) {
    var obj = new Object();
    obj.id = $("#inputId").val();
    obj.name = $("#inputName").val();
 
    console.log(JSON.stringify(obj));
    $.ajax({
        url: "Category/Put/" + id,
        type: "PUT",
        data: obj
    }).done((result) => {
        Swal.fire(
            'Yeayy',
            'Data ' + obj.name + ' Berhasil diubah',
            'success'
        )
        tableCategory.ajax.reload();
        $('.createCategory').modal('hide');

    }).fail((error) => {

        Swal.fire(
            'Opps!',
            'Sepertinya terjadi kesalahan, periksa kembali!',
            'error'
        )
    });
}

//insert
function showCreateCategory() {
    $('.createCategory').modal('show');
    clearFormCategory();
}

function clearFormCategory() {
   
    $('#labelText').html("Create New Category");
    $('#inputId').prop('readonly', true);
    $('#inputId').val("Auto Generate");
    $('#inputName').removeAttr('readonly');
    $('#inputName').val("");
    $('#btnSaveCategory').attr('data-name', 'insert').html("<span class='fas fa-save'>&nbsp;</span>Save New Category")
    $('#btnSaveCategory').removeAttr('data-nik');
    $('#fCategory').removeClass('was-validated')
}


function InsertCategory() {
    var obj = new Object();

    obj.id = $("#inputId").val();
    obj.name = $("#inputName").val();
    console.log(obj);

    $.ajax({
        url: "/Category/post",
        type: "POST",
        data: obj
    }).done((result) => {
        Swal.fire(
            'Yeayy',
            "Data Berhasil ditambahkan",
            'success'
        )
        tableCategory.ajax.reload();
        $('.createCategory').modal('hide');

    }).fail((error) => {
        Swal.fire(
            'Opps!',
            'Sepertinya terjadi kesalahan, periksa kembali!',
            'error'
        )
    })
}
