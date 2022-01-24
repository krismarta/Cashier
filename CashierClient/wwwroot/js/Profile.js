
$(document).ready(function () {
    $.ajax({
        url: "/users/get/" + $("#edtidlogin").val(),
        type: "GET"
    }).done((result) => {
        console.log(result);
        $("#edtidlogin").val(result.id);
        $("#edtemail").val(result.email);
        $("#edtname").val(result.name);
        $("#edtphone").val(result.phone);

        //console.log(text)
    }).fail((error) => {
        console.log(error);
    });
});
function saveProfile() {
    var obj = new Object();
    obj.id = $("#edtidlogin").val();
    obj.email = $("#edtemail").val();
    obj.name = $("#edtname").val();
    obj.phone = $("#edtphone").val();

    if (obj.id == "" || obj.email == "" || obj.name == "" || obj.phone == "") {
        Swal.fire(
            'Data masih kosong',
            'Sepertinya data masih kosong',
            'info'
        )
    } else {
        $.ajax({
            url: "/users/put/" + edtidlogin,
            type: "PUT",
            data : obj
        }).done((result) => {
            if (result == "200") {
                Swal.fire(
                    'Yeayy',
                    'Data Berhasil diperbarui',
                    'success'
                )
            }
            //console.log(text)
        }).fail((error) => {
            console.log(error);
        });
    }
}


function ChangePassword() {
    var obj = new Object();
    obj.email = $("#edtemail").val();
    obj.NewPassword = $("#edtpassword").val();
    var edtpassword = $("#edtpassword").val();
    var edtnewpass = $("#edtnewpass").val();
    console.log(obj);
    if (obj.email == "" || obj.NewPassword == "") {
        Swal.fire(
            'Data masih kosong',
            'Sepertinya data masih kosong',
            'info'
        );
    } else {
        if (edtpassword != edtnewpass) {
            Swal.fire(
                'Password tidak sama',
                'Periksa kembali password baru kamu',
                'info'
            )
        } else {
            $.ajax({
                url: "/users/changePassword",
                type: "POST",
                data: obj
            }).done((result) => {
                console.log(result);
                if (result == "200") {
                    Swal.fire(
                        'Yeayy',
                        'Data Berhasil diperbarui',
                        'success'
                    )
                }
                //console.log(text)
            }).fail((error) => {
                console.log(error);
            });
        }
    }
}
