const idArray = [];
const namaArray = [];
const priceArray = [];
const qtyArray = [];
const subtotalArray = [];

$(document).ready(function () {
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    var yyyy = today.getFullYear();
    today = mm + '/' + dd + '/' + yyyy;

    getAllGoods(); 
    getMyinfo(idLogin);
    $('#idtransaction').val("T" + AutoGenerateID());
    $('#edtdate').val(today);

    $("#btnaddtmp").click(function () {
        var id = $('#edtidgoods').val();
        var name = $('#edtnamegoods').val();
        var qty = $('#edtqtygoods').val();
        var stok = $('#edtstokgoods').val();
        var harga= $('#edtpricegoods').val()
        //var sub = parseInt(stokreq) * parseInt(hargaGoods);

        //console.log("INI SUB" + sub)
        if (id == "" || name == "" || qty == "" || stok == "" || harga == "") {
            Swal.fire(
                'Opps!',
                'Data harus terisi semua.',
                'info'
            )
        } else {
            if (parseInt(qty) > parseInt(stok)) {
                Swal.fire(
                    'Stok barang tidak mencukupi',
                    'Lakukan request stok ke supplier',
                    'info'
                )
            } else {
                inserttmpTable(id, name, qty, stok, harga);
            }
           
        }


    });
});

function inserttmpTable(id, nama, qty, stok, price) {
    var check = idArray.includes(id);

    if (check) {
        var posisi = idArray.indexOf(id);
        var getprice = priceArray[posisi];
        var getqty = qtyArray[posisi];
        var plusin = parseInt(getqty) + parseInt(qty);
        var plusinharga = parseInt(plusin) * parseInt(getprice);

        console.log(posisi);
        console.log(getprice);
        console.log(getqty);
        console.log(plusin);
        console.log(plusinharga);

        qtyArray[posisi] = plusin;
        subtotalArray[posisi] = plusinharga;
    } else {
        var sub = parseInt(qty) * parseInt(price);
        idArray.push(id);
        namaArray.push(nama);
        priceArray.push(price);
        qtyArray.push(qty);
        subtotalArray.push(sub);
        
    }
    loadcart();
    
    clearAfterProsesTMp();
    
}

function SendRequest() {
    var obj = new Object();
    var uangcustomer = $('#edtuangcust').val();
    obj.id = $('#idtransaction').val();
    obj.total = $('#subtotaltxt').html();
    obj.idUser = $('#edtid').val();
    obj.payment = $('#payment_type').val();
    obj.quantity = qtyArray;
    obj.idGoods = idArray;
    obj.namaGoods = namaArray;
    obj.priceGoods = priceArray;
    console.log(obj);
    if (obj.id == "" || obj.total == "" || obj.idUser == "" || obj.payment == "0" || obj.quantity == "" || obj.idGoods == "") {
        Swal.fire(
            'Data belum lengkap',
            'Lengkapi data untuk melanjutkan transaksi',
            'info'
        )
    } else {
        if (obj.payment == "cash") {
            if (uangcustomer == "") {
                Swal.fire(
                    'Uang untuk pembayaran masih kurang',
                    'Sepertinya uang customer kurang dari total biaya',
                    'info'
                )
            } else {
                if (parseInt(uangcustomer) < parseInt(obj.total)) {
                    Swal.fire(
                        'Uang untuk pembayaran masih kurang',
                        'Sepertinya uang customer kurang dari total biaya',
                        'info'
                    )
                } else {
                    PaymentTransaction();
                }
            }
            
            
        } else if (obj.payment == "midtrans") {
            //midtrans kalo keburu
            PaymentWithMidtrans();

        } else {
            Swal.fire(
                'Tipe pembayaran belum didukung',
                'Sepertinya tipe pembayaran belu tersedia',
                'info'
            )
        }
    }
}

function PaymentTransaction() {
    var obj = new Object();
    var uangcustomer = $('#edtuangcust').val();
    obj.id = $('#idtransaction').val();
    obj.total = $('#subtotaltxt').html();
    obj.idUser = $('#edtid').val();
    obj.payment = $('#payment_type').val();
    obj.quantity = qtyArray;
    obj.idGoods = idArray;
    obj.namaGoods = namaArray;
    obj.priceGoods = priceArray;
    console.log(obj);
    $.ajax({
        url: "Transaction/TransactionInsert/",
        type: "POST",
        data: obj
    }).done((result) => {
        console.log(result);
        if (result == 200) {
            var kembalian =0;
            if (obj.payment == "cash") {
                kembalian = parseInt(uangcustomer) - parseInt(obj.total);
                Swal.fire({
                    title: 'Transaksi Berhasil',
                    html:
                        'Kembalian untuk customer : <br>' +
                        ' <h4><b>Rp ' + kembalian + '</b></h4>' +
                        'Please wait <br>' +
                        '<strong></strong> detik <br>',
                    icon: 'success',
                    timer: 10000,
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
                        window.location.href = '/transaction';
                    }
                })
            } else {
                Swal.fire({
                    title: 'Menunggu Pembayaran',
                    html:
                        'Jangan tutup atau refresh pop up ini<br>' +
                        'Sampai Status Transaksi berhasil<br>' +
                        'Mohon Tunggu <br>',
                    icon: 'success',
                    timer: 10000,
                    showConfirmButton: false,
                    allowOutsideClick: false,
                    didOpen: () => {
                        timerInterval = setInterval(() => {
                            Swal.getHtmlContainer().querySelector('strong')
                                .textContent = (Swal.getTimerLeft() / 1000)
                                    .toFixed(0)
                        }, 100)

                        var trigger = setInterval(function () {
                            $.ajax({
                                url: "Transaction/StatusPayment/" + obj.id,
                                type: "GET",

                            }).done((result) => {
                                status = result.result.transaction_status;
                                console.log(result.result);
                                console.log("TERUS BERPUTAR");
                                if (result.result.transaction_status == "settlement") {
                                    //Swal.close();
                                    clearInterval(trigger);
                                    Swal.fire({
                                        title: 'Pembayaran Berhasil diterima',
                                        html:
                                            'Kembalian untuk customer : <br>' +
                                            ' <h4><b>Rp ' + kembalian + '</b></h4>' +
                                            'Please wait <br>' +
                                            '<strong></strong> detik <br>',
                                        icon: 'success',
                                        timer: 3000,
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
                                            window.location.href = '/transaction';
                                        }
                                    })
                                } else {
                                    Swal.increaseTimer(5000);
                                }
                            }).fail((error) => {
                                Swal.fire(
                                    'Opps!',
                                    'Sepertinya terjadi kesalahan, periksa kembali!',
                                    'error'
                                )
                            });
                        }, 3000)
                    },
                    willClose: () => {
                        clearInterval(timerInterval)
                        //alert('done');
                        window.location.href = '/transaction';
                    }
                })
                
              

            }
          
        }
    }).fail((error) => {
        console.log(result);
    });
}

function PaymentWithMidtrans() {
    var obj = new Object();
    obj.id = $('#idtransaction').val();
    obj.total = $('#subtotaltxt').html();
    obj.idUser = $('#edtid').val();
    obj.payment = $('#payment_type').val();
    obj.quantity = qtyArray;
    obj.idGoods = idArray;
    obj.namaGoods = namaArray;
    obj.priceGoods = priceArray;
    console.log(obj);
    $.ajax({
        url: "Transaction/PaymentWithMidtrans/",
        type: "POST",
        data: obj,
        success: function (response) {
            var token = response.result.token;
            snap.pay(token, {
                onSuccess: function (result) { PaymentTransaction(); },
                onPending: function (result) { PaymentTransaction(); },
                onError: function (result) {
                    Swal.fire(
                        'Opps!',
                        'Looks like something went wrong, check again',
                        'error'
                    )},
                onClose: function () {
                    Swal.fire(
                        'Opps!',
                        'Do not close the payment window please',
                        'warning'
                    ) }
            })
        },
        error: function (response) {

        }
    });
        
}

function GetStatusPaymentMT(id) {
    var result = new Object();
    $.ajax({
        url: "Transaction/StatusPayment/" + id,
        type: "GET",

    }).done((result) => {
        result['order_id'] = result.result.order_id;
        result['payment_type'] = result.result.payment_type;
        result['transaction_status'] = result.result.transaction_status;
    }).fail((error) => {
        Swal.fire(
            'Opps!',
            'Sepertinya terjadi kesalahan, periksa kembali!',
            'error'
        )
    })
    return result;
}

function clearAfterProsesTMp() {
    $("#inputgoods").val("0");
    $("#edtidgoods").val("");
    $("#edtnamegoods").val("");
    $("#edtqtygoods").val("");
    $("#edtstokgoods").val("");
    $("#edtpricegoods").val("");
   
}

function loadcart() {
    var text2 = "";
    $("#listtempbody").html("");
    for (var i = 0; i < idArray.length; i++) {
        text2 += `<tr>
                    <td>${idArray[i]}</td>
                    <td>${namaArray[i]}</td>
                    <td>${priceArray[i]}</td>
                    <td>${qtyArray[i]}</td>
                    <td>${subtotalArray[i]}</td>
                    <td><button class="btn btn-xs btn-warning" onclick="deleteRow(${i})"> Delete</button></td>
                </tr>`;
    }
    $("#listtempbody").html(text2);
    var sumtotal = 0;
    for (var i = 0; i < idArray.length; i++) {
        sumtotal += parseInt(subtotalArray[i]);
    }
    console.log(sumtotal.toString());
    $("#subtotaltxt").html(sumtotal.toString());
    clearAfterProsesTMp();
}
function deleteRow(r) {
    idArray.splice(r, 1);
    namaArray.splice(r, 1);
    priceArray.splice(r, 1);
    qtyArray.splice(r, 1);
    subtotalArray.splice(r, 1);
    loadcart();
}

function getMyinfo(id) {
    $.ajax({
        url: "/users/get/" + id
    }).done((result) => {
        console.log(result)

        $('#edtid').val(result.id);
        $('#edtnamekasir').val(result.name);
        $('#edtrole').val(result.account.role.name);
    }).fail((error) => {
        console.log(error);
    });
}

function getAllGoods() {
    var text = "";
    $.ajax({
        url: "/Goods/getAllGoods/"
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

function getInfoGoods(id) {
    $.ajax({
        url: "/Goods/get/" + id
    }).done((result) => {
        console.log(result);
        $('#edtidgoods').val(result.id);
        $('#edtnamegoods').val(result.name);
        $('#edtpricegoods').val(result.priceSell);
        $('#edtstokgoods').val(result.stok);
    }).fail((error) => {
        console.log(error);
    });
}

function changeGoods(obj) {
    var btnValue = obj.options[obj.selectedIndex].value;

    if (btnValue == "0") {
        //do nothing
        $('#edtidgoods').val("");
        $('#edtnamegoods').val("");
        $('#edtqtygoods').val("");
        $('#edtpricegoods').val("");
        $('#edtstokgoods').val("");
        return;
    }

    // Do your thing here
    console.log(btnValue + " OK ");
    getInfoGoods(btnValue);
}

function AutoGenerateID() {
    return Math.floor(Math.random() * 9999999);
}
