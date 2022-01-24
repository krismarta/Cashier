
const idArray = [];
const namaArray = [];
const curstokArray = [];
const stokArray = [];
const subtotalArray = [];

$(document).ready(function () {
    getMyinfo(idLogin);
    SuppliertoSelect();
    $("#btnaddtmp").click(function () {
        var id = $('#edtidgoods').val();
        var name = $('#edtnamegoods').val();
        var stokcur = $('#edtstokgoods').val();
        var stokreq = $('#edtstokgoodsreq').val();
        var hargaGoods = $('#edtbuygoods').val()
        var sub = parseInt(stokreq) * parseInt(hargaGoods);
        
        console.log("INI SUB" + sub)
        if (id == "" || name == "" || stokcur == "" || stokreq == "" || hargaGoods == "" || sub == "" ){
            Swal.fire(
                'Opps!',
                'Data harus terisi semua.',
                'info'
            )
        } else {
            inserttmpTable(id, name, stokcur, stokreq, sub);
        }
        
        
    });
});

function inserttmpTable(id, nama, curstok, stok,subtotal) {
    $("#inputsupplier").prop('disabled', true);
    var check = idArray.includes(id);
    if (check) {
        var posisi = idArray.indexOf(id);
        var getprice = parseInt(subtotalArray[posisi]) / parseInt(stokArray[posisi]);
        var getstok = stokArray[posisi];
        var plusin = parseInt(getstok) + parseInt(stok);
        var plusinHarga = parseInt(getprice) * plusin;
        stokArray[posisi] = plusin;

        subtotalArray[posisi] = plusinHarga;
    } else {
        idArray.push(id);
        namaArray.push(nama);
        curstokArray.push(curstok);
        stokArray.push(stok);
        subtotalArray.push(subtotal);
    }
    loadTmp();
    clearAfterProsesTMp();
}

function clearAfterProsesTMp() {
    $("#inputgoods").val("0");
    $("#edtstokgoodsreq").val("");
}
function loadTmp() {
    if (idArray.length == 0) {
        $("#inputsupplier").prop('disabled', false);
    }
    var text2 = "";
    $("#listtempbody").html("");
    for (var i = 0; i < idArray.length; i++) {
        text2 += `<tr>
                    <td>${idArray[i]}</td>
                    <td>${namaArray[i]}</td>
                    <td>${curstokArray[i]}</td>
                    <td>${stokArray[i]}</td>
                    <td>${subtotalArray[i]}</td>
                    <td><button class="btn btn-xs btn-warning" onclick="deleteRow(${i})"> Delete</button></td>
                </tr>`;
    }
    $("#listtempbody").html(text2);
}


function deleteRow(r) {
    idArray.splice(r,1);
    namaArray.splice(r,1);
    curstokArray.splice(r,1);
    stokArray.splice(r,1);
    subtotalArray.splice(r,1);
    loadTmp();
}


function SendRequest() {
    var obj = new Object();
    obj.id = "RB" + AutoGenerateID();
    obj.idsupplier = $("#edtidsup").val();
    obj.iduser = $("#edtid").val();
    obj.idbarang = idArray;
    obj.quantity = stokArray;
    var sum = 0;
    for (var i = 0; i < subtotalArray.length; i++) {
        sum += subtotalArray[i];
    }
    obj.subtotal = sum;
    console.log(obj);
    Swal.fire({
        title: 'Yakin ingin dilanjutkan?',
        text: "Request akan dikirim ke supplier dan disimpan kedalam database",
        icon: 'info',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Lanjutkan',
        cancelButtonText: 'Batalkan'

    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "request/requestGoodsSupplier/",
                type: "POST",
                data: obj
            }).done((result) => {
                console.log(result);
                if (result == 200) {
                    Swal.fire({
                        title: 'Request Stok Berhasil',
                        html:
                            'Request stok berhasil dikirim ke supplier! <br>'+
                            'Please wait <br>' +
                            '<strong></strong> detik <br>' +
                            'Direct to Dashboard',
                        icon: 'success',
                        timer: 2000,
                        showConfirmButton: false,
                        allowOutsideClick: false,
                        didOpen: () => {
                            timerInterval = setInterval(() => {
                                Swal.getHtmlContainer().querySelector('strong')
                                    .textContent = (Swal.getTimerLeft() / 1000)
                                        .toFixed(0)
                            }, 100)
                        },
                        willClose: () => {
                            clearInterval(timerInterval)
                            //alert('done');
                            window.location.href = '/dashboard';
                        }
                    })
                } else {
                    Swal.fire(
                        'Opps!',
                        'Sepertinya terjadi kesalahan, periksa kembali!',
                        'error'
                    )
                }

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


function changeSup(obj) {
    var btnValue = obj.options[obj.selectedIndex].value;

    if (btnValue == "0") {
        //do nothing
        $('#ednamesup').val("");
        $('#edtidsup').val("");
        $('#edtmailsup').val("");
        $('#edtcompsup').val("");
        $('#edtphonesup').val("");
        return;
    }

    // Do your thing here
    console.log(btnValue + " OK ");
    getInfoSupplier(btnValue);
    getGoodsBySupplier(btnValue);
}

function changeGoods(obj) {
    var btnValue = obj.options[obj.selectedIndex].value;

    if (btnValue == "0") {
        //do nothing
        $('#edtidgoods').val("");
        $('#edtidsup').val("");
        $('#ednamesup').val("");
        $('#edtmailsup').val("");
        $('#edtcompsup').val("");
        $('#edtphonesup').val("");
        return;
    }

    // Do your thing here
    console.log(btnValue + " OK ");
    getInfoGoods(btnValue);
}

function getInfoGoods(id) {
    $.ajax({
        url: "/Goods/get/" + id
    }).done((result) => {
        console.log(result);
        $('#edtidgoods').val(result.id);
        $('#edtnamegoods').val(result.name);
        $('#edtsellgoods').val(result.priceSell);
        $('#edtbuygoods').val(result.priceBuy);
        $('#edtcategorygoods').val(result.category.name);
        $('#edtstokgoods').val(result.stok);
    }).fail((error) => {
        console.log(error);
    });
}


function getInfoSupplier(id) {
    $.ajax({
        url: "/supplier/get/" + id
    }).done((result) => {
        console.log(result);
        $('#edtidsup').val(result.id);
        $('#ednamesup').val(result.name);
        $('#edtmailsup').val(result.email);
        $('#edtcompsup').val(result.companyname);
        $('#edtphonesup').val(result.phone);
    }).fail((error) => {
        console.log(error);
    });
}

function getGoodsBySupplier(id) {
    var text = "";
    $.ajax({
        url: "/goods/getgoodBySupplier/" + id
    }).done((result) => {
        console.log(result)
        $.each(result, function (key, val) {
            text += `
             <option value="${val.id}">${val.name}</option>`;
        });
        $("#inputgoods").html("<option value='0'>Choose Goods</option>" + text);
        
    }).fail((error) => {
        console.log(error);
    });
}

function getMyinfo(id) {
    $.ajax({
        url: "/users/get/" + id
    }).done((result) => {
        console.log(result)
        $('#edtid').val(result.id);
        $('#edtname').val(result.name);
        $('#edtemail').val(result.email);
        $('#edtphone').val(result.phone);
    }).fail((error) => {
        console.log(error);
    });
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
        $("#inputsupplier").html("<option value='0'>Choose Supplier</option>" + text);
        console.log(text)
    }).fail((error) => {
        console.log(error);
    });
}

function AutoGenerateID() {
    return Math.floor(Math.random() * 9999999);
}
