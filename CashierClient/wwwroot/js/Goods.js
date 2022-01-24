$(document).ready(function () {
    SuppliertoSelect();
    CategorytoSelect();
    tableGoods = $("#listGoods").DataTable({
        "ajax": {
            "url": "/Goods/getAllGoods",
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
                "data" : "priceSell"
            },
            {
                "data": "priceBuy"
            },
            {
                "data": "stok"
            },
            {
                "data": "supplier.companyName"
            },
            {
                "data": "category.name"
            },
            {
                "data": null,
                "render": function (data, type, row) {
                    var ids = row['id'];
                    return `<div class="btn-group">
                                    <button type="button" class="btn btn-sm btn-circle btn-warning" data-bs-toggle="modal" onclick="getGoodsByid('${ids}')" data-bs-target="#detailCharacterModal">
                                        <span class="fas fa-edit"></span>
                                    </button>
                                    &nbsp;
                                    <button type="button" class="btn btn-sm btn-circle btn-danger" onclick="DeleteGoods('${ids}')" id="btndeleteEmployee">
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
                    columns: [0, 1, 2,3,4,5,6]
                }
            },
            {
                extend: 'csv',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6]
                }
            },
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6]
                }
            },
            {
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6]
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6]
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
            $("#btnSaveGoods").click(function () {
                if (form.checkValidity() === false) {
                    event.preventDefault();
                    event.stopPropagation();
                } else {
                    var data_action = $(this).attr("data-name");
                    if (data_action == "insert") {
                        InsertGoods();
                    } else if (data_action == "update") {
                        console.log("langsung Update bray");
                        var id = $(this).attr("data-id");

                        UpdateActionGoods(id);
                    }
                    console.log(data_action);
                }

                form.classList.add('was-validated');

            })

        });
    }, false);
})();

function DeleteGoods(id) {
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
                url: "Goods/delete/" + id,
                data: { id },
                type: "DELETE",

            }).done((result) => {
                console.log(result);
                Swal.fire(
                    'Yeayy',
                    'Data Berhasil dihapus',
                    'success'
                )
                tableGoods.ajax.reload();
                $('.createGoods').modal('hide');

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

function getGoodsByid(id) {
    $.ajax({
        url: "/Goods/get/" + id
    }).done((result) => {
        console.log(result.supplierid)

        $('.createGoods').modal('show');
        $('#fGoods').removeClass('was-validated')
        $('#labelText').html("Update Goods");
        $('#inputId').prop('readonly', true);
        $('#inputId').val(id).readonly;
        $('#inputName').val(result.name);
        $('#inputpricesell').val(result.priceSell);
        $('#inputpricebuy').val(result.priceBuy);
        $('#inputstok').val(result.stok);
        $('#inputcategory').val(result.categoryid);
        $('#inputsupplier').val(result.supplierid);
        $('#btnSaveGoods').attr('data-name', 'update').html("<span class='fas fa-save'>&nbsp;</span>Update Goods")
        $('#btnSaveGoods').attr('data-id', id);
    }).fail((error) => {
        console.log(error);
    });
}


function UpdateActionGoods(id) {
    var obj = new Object();
    obj.id = $("#inputId").val();
    obj.name = $("#inputName").val();
    obj.priceSell = $("#inputpricesell").val();
    obj.priceBuy = $("#inputpricebuy").val();
    obj.stok = $("#inputstok").val();
    obj.Supplierid = $("#inputsupplier").val();
    obj.Categoryid = $("#inputcategory").val();
    if (obj.Supplierid == "0" || obj.Categoryid == 0) {
        Swal.fire(
            'Opps!',
            'Supplier or Category is Empty',
            'warning'
        )
    } else {
        console.log(JSON.stringify(obj));
        $.ajax({
            url: "Goods/Put/" + id,
            type: "PUT",
            data: obj
        }).done((result) => {
            Swal.fire(
                'Yeayy',
                'Data ' + obj.name + ' Berhasil diubah',
                'success'
            )
            tableGoods.ajax.reload();
            $('.createGoods').modal('hide');

        }).fail((error) => {

            Swal.fire(
                'Opps!',
                'Sepertinya terjadi kesalahan, periksa kembali!',
                'error'
            )
        });
    }
    
   
}

//insert
function showCreateGoods() {
    $('.createGoods').modal('show');
    clearFormGoods();
}

function clearFormGoods() {
    $('#labelText').html("Create New Goods");
    $('#inputId').prop('readonly', true);
    $("#inputId").val("B" + AutoGenerateID());
    $('#inputName').removeAttr('readonly');
    $('#inputName').val("");
    $('#inputpricesell').val("");
    $('#inputpricebuy').val("");
    $('#inputstok').val("");
    $('#inputsupplier').val("0");
    $('#inputcategory').val("0");
    $('#btnSaveGoods').attr('data-name', 'insert').html("<span class='fas fa-save'>&nbsp;</span>Save New Goods")
    $('#btnSaveGoods').removeAttr('data-nik');
    $('#fGoods').removeClass('was-validated')
}

function InsertGoods() {
    var obj = new Object();
    obj.id = $("#inputId").val();
    obj.name = $("#inputName").val();
    obj.priceSell = $("#inputpricesell").val();
    obj.priceBuy = $("#inputpricebuy").val();
    obj.stok = $("#inputstok").val();
    obj.Supplierid = $("#inputsupplier").val();
    obj.Categoryid = $("#inputcategory").val();

    if (obj.Supplierid == "0" || obj.Categoryid == 0) {
        Swal.fire(
            'Opps!',
            'Supplier or Category is Empty',
            'warning'
        )
    } else {
        console.log(JSON.stringify(obj));
        $.ajax({
            url: "/Goods/post",
            type: "POST",
            data: obj
        }).done((result) => {
            Swal.fire(
                'Yeayy',
                "Data Berhasil ditambahkan",
                'success'
            )
            tableGoods.ajax.reload();
            $('.createGoods').modal('hide');

        }).fail((error) => {
            Swal.fire(
                'Opps!',
                'Sepertinya terjadi kesalahan, periksa kembali!',
                'error'
            )
        })
    }
    

   
}

function AutoGenerateID() {
    return Math.floor(Math.random() * 9999999);
}

function SuppliertoSelect() {
    var text = "";
    $.ajax({
        url: "/Supplier/getAllSupplier"
    }).done((result) => {
        console.log(result);
        
        $.each(result, function (key, val) {
            text += `
             <option value="${val.id}">${val.companyName}</option>`;
        });
        $("#inputsupplier").html("<option value='0'>Choose Supplier</option>" +text);
        console.log(text)
    }).fail((error) => {
        console.log(error);
    });
}


function CategorytoSelect() {
    var text = "";
   
    $.ajax({
        url: "/Category/getAllCategory"
    }).done((result) => {
        console.log(result);
        $.each(result, function (key, val) {
            text += `
             <option value="${val.id}">${val.name}</option>`;
        });
        $("#inputcategory").html("<option value='0'>Choose Category</option>" +text);
        console.log(text)
    }).fail((error) => {
        console.log(error);
    });
}

