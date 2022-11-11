$(function () {
    var userLoginButton = $("#UserLogin button[name='login']").click(onUserLoginClick);

    function onUserLoginClick() {
        var url = "UserAuth/Login";

        var antiForgeryToken = $("#UserLogin input[name='__RequestVerificationToken']").val();

        var email = $("#UserLogin input[name = 'Email']").val();
        var password = $("#UserLogin input[name = 'Password']").val();
        var rememberMe = $("#UserLogin input[name = 'RememberMe']").prop('checked');

        var userInput = {
            __RequestVerificationToken: antiForgeryToken,
            Email: email,
            Password: password,
            RememberMe: rememberMe
        };

        $.ajax({
            type: "POST",
            url: url,
            data: userInput,
            success: function (data) {
                var parsed = $.parseHTML(data);
                var hasErrors = $(parsed).find("input[name='LoginInvalid']").val() == "true";

                if (hasErrors == true) {
                    $("#UserLogin").html(data);
                    userLoginButton = $("#UserLogin button[name='login']").click(onUserLoginClick)
                } else {
                    location.href = 'Home/Index';
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                var errorText = "Status: " + xhr.status + " - " + xhr.statusText;

                PresentClosableBootstrapAlert("#alert_placeholder_login", "danger", "Error!", errorText);

                console.error(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText); 
            }
        });
    }
});