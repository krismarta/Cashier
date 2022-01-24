(function () {
    'use strict';
    window.addEventListener('load', function () {
        var forms = document.getElementsByClassName('needs-validation');
        var validation = Array.prototype.filter.call(forms, function (form) {
            $("#btnlogin").click(function () {

                if (form.checkValidity() === false) {
                    event.preventDefault();
                    event.stopPropagation();
                } else {

                    LoginCustomer();

                }

                form.classList.add('was-validated');

            })

        });
    }, false);
})();

function LoginCustomer() {
    var obj = new Object();
    obj.email = $("#email").val();
    obj.password = $("#password").val();
    console.log(obj);
    buttonDisabled();
    $.ajax({
        url: "/Auth",
        type: "POST",
        data: obj,
        success: function (response) {
            if (response.result.idtoken == null && response.result.statusCode == 0) {
                console.log(response);
                buttonEnabled();
                Swal.fire(
                    'Opps!',
                    'Email or Password is not correct.',
                    'error'
                )
            }
            else {
                buttonEnabled();
                console.log(response);
                Swal.fire({
                    title: 'Yeay!! Login Successful',
                    html:
                        'Please wait <br>' +
                        '<strong></strong> detik <br>' +
                        'Direct to Main Page',
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
            }
        },
        error: function (response) {
            console.log(response);
            buttonEnabled();
            Swal.fire(
                'Opps!',
                'Looks like something went wrong, check again',
                'error'
            )
        }
    })
}


function Forgotpassword() {
    var obj = new Object();
    obj.email = $("#emailforgot").val();
    buttonDisabledFG();
    $.ajax({
        url: "/Accounts/forgot",
        type: "POST",
        data: obj,
        success: function (response) {
            buttonEnabledFG();
            console.log(response);
            if (response == "200") {
                Swal.fire(
                    'Berhasil mengirim password',
                    'Check email kamu, kami telah mengirim password baru.',
                    'success'
                )
            } else if (response == "404") {
                Swal.fire(
                    'Data tidak ditemukan',
                    'Kami tidak menemukan data kamu di sistem kami.',
                    'warning'
                )
            } else {
                Swal.fire(
                    'Opps!',
                    'Looks like something went wrong, check again',
                    'error'
                )
            }
        },
        error: function (response) {
            console.log(response);
            buttonEnabledFG();
            Swal.fire(
                'Opps!',
                'Looks like something went wrong, check again',
                'error'
            )
        }
    })

}

function buttonDisabled() {
    document.getElementById("btnlogin").disabled = true;
    document.getElementById("btnlogin").innerHTML = "Please wait ... ";
}
function buttonEnabled() {
    document.getElementById("btnlogin").disabled = false;
    document.getElementById("btnlogin").innerHTML = "Login to System";
}

function buttonEnabledFG() {
    document.getElementById("btnforgot").disabled = false;
    document.getElementById("btnforgot").innerHTML = "Send New Password";
    
}

function buttonDisabledFG() {
    document.getElementById("btnforgot").disabled = true;
    document.getElementById("btnforgot").innerHTML = "Please wait ... ";
}