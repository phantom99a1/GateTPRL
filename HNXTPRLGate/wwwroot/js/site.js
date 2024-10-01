// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var _countSpinLoading = 0;
var showPopup = false;
window.SpinLoading = function ($create) {
    try {
        if ($create) {
            _countSpinLoading++;
        }
        else {
            _countSpinLoading--;
        }
        //
        if (_countSpinLoading > 0) {
            if ($('.loading-container').length == 0) {
                var _loader = '<div class="loading-container"><div class="loading"><div class="circle cyan"></div><div class="circle magenta"></div><div class="circle yellow"></div></div></div>';
                $('body').append(_loader);
            }
        }
        else {
            _countSpinLoading = 0;
            $('.loading-container').remove();
        }

    } catch (e) {
        console.log(e);
    }
}

$(function () {
    console.log("init Form");

});

function ReloadData() {
    try {
        $.ajax({
            type: "GET",
            url: "/Home/_BoxConnect",
            success: function (data) {
                if (data != null && data) {
                    $("#mainData").html(data);
                    countDown();
                }
            },
            error: function (data) {
                console.log(data.error);
            }
        });
    } catch (e) {
        console.log(e);
    }
    function countDown() {
        var timeleft = 5;
        var downloadTimer = setInterval(function ()
        {
            //console.log("--callback", timeleft);
            if (timeleft <= 0) {
                clearInterval(downloadTimer);
                document.getElementById("countdown").innerHTML = timeleft + " seconds auto refresh";
                ReloadData();
                ReloadSessionFlag();
                ReloadDataError(showPopup);
                ReloadDataRejection();
                ReloadSearchListSecurities();
            } else {
                document.getElementById("countdown").innerHTML = timeleft + " seconds auto refresh";
            }
            timeleft -= 1;
        }, 1000);
    }
}

function ReloadSessionFlag() {
    try {
        $.ajax({
            type: "GET",
            url: "/Home/_SessionFlag",
            success: function (data) {
                if (data != null && data) {
                    $("#divSessionFlag").html(data);
                }
            },
            error: function (data) {
                console.log(data.error);
            }
        });
    } catch (e) {
        console.log(e);
    }
}
function fnHoseShowFormControl() {
    try {
        $.ajax({
            method: "POST",
            url: "/showformcontrol",
            processData: false,
            contentType: false,
            beforeSend: function () {
                SpinLoading(true);
            },
            success: function (data) {
                if (data.code > 0) {
                    $("#idPopupControl").html(data.message);
                    $("#idPopupControl").modal("show");
                }
                else {
                    nvsError(data.message);
                }
            },
            complete: function () {
                SpinLoading(false);
            }
        });
    } catch (e) {
        SpinLoading(false);
        console.log(e);
    }
}

function btnChangeGwPass() {
    try {
        var _txtOldPassword = $('#txtOldPassword').val().trim() || '';
        if (_txtOldPassword == '' || _txtOldPassword == null) {
            nvsError("Old password is not null!");
            return;
        }
        var _txtNewPassword = $('#txtNewPassword').val().trim() || '';
        if (_txtNewPassword == '' || _txtNewPassword == null) {
            nvsError("New password is not null!");
            return;
        }
        if (_txtOldPassword == _txtNewPassword) {
            nvsError("New password and Old password must not be the same!");
            return;
        }
        nvsConfirm("Are you sure to process change gateway password?", function () {
            vaultQueryCheckValidate(_txtOldPassword, _txtNewPassword);
            //sendMsgChangePassToExchange(_txtOldPassword, _txtNewPassword);
        });
    } catch (e) {
        console.log(e);
    }
}

function vaultQueryCheckValidate(pOldPass, pNewPass) {
    $.ajax({
        type: "GET",
        url: "/vault-query",
        data: {
            "pPassword": pOldPass,
            "pNewPass": pNewPass
        },
        beforeSend: function () {
            SpinLoading(true);
        },
        success: function (data)
        {
            if (data.code > 0) {
                sendMsgChangePassToExchange(pOldPass, pNewPass);
            }
            else {
                nvsError(data.message);
                SpinLoading(false);
            }
        },
        complete: function () {
            SpinLoading(false);
        }
    });
}

function sendMsgChangePassToExchange(pOldPass, pNewPass) {
    $.ajax({
        type: "POST",
        url: "/change-password",
        data: {
            "oldpass": pOldPass,
            "newpass": pNewPass
        },
        success: function (data) {
            if (data.code == 5) {
                $('#txtOldPassword').val('');
                $('#txtNewPassword').val('');
                nvsSuccess(data.message);
            }
            else {
                $('#txtOldPassword').val('');
                $('#txtNewPassword').val('');
                nvsError(data.message);
            }
            SpinLoading(false);
        },
        complete: function () {
            SpinLoading(false);
        }
    });
}

function btnTradingSessionRequest() {
    try {
        var _cboTradingSessionRequest = $('#cboTradingSessionRequest').val();
        var _txtName = $('#txtName').val().trim() || '';
        //
        if (_cboTradingSessionRequest == '2' || _cboTradingSessionRequest == '3') {
            if (_txtName == '' || _txtName == null) {
                nvsError("Name is not null!");
                return;
            }
        }
       
        nvsConfirm("Are you sure to process Trading session request?", function () {
            $.ajax({
                type: "POST",
                url: "/trading-session-req",
                data: {
                    "tradingCode": _cboTradingSessionRequest,
                    "tradingName": _txtName
                },
                success: function (data) {
                    if (data.code > 0) {
                        nvsSuccess(data.message);
                        $('#txtName').val('');
                    }
                    else {
                        nvsError(data.message);
                    }
                },
                error: function (data) {
                    console.log(data.error);
                    nvsError(data.error);
                }
            });
        });
    } catch (e) {
        console.log(e);
    }
}

function btnSecuritiesRequest() {
    try {
        var _cboSecurities = $('#cboSecurities').val();
        var _txtSecurities = $('#txtSecurities').val().trim() || '';
        //
        if (_cboSecurities == '3') {
            if (_txtSecurities == '' || _txtSecurities == null) {
                nvsError("Symbol is not null!");
                return;
            }
        }
        nvsConfirm("Are you sure to process securities status request?", function () {
            $.ajax({
                type: "POST",
                url: "/security-status-req",
                data: {
                    "tradingCode": _cboSecurities,
                    "symbol": _txtSecurities
                },
                success: function (data) {
                    if (data.code > 0) {
                        nvsSuccess(data.message);
                        $('#txtSecurities').val('');
                    }
                    else {
                        nvsError(data.message);
                    }
                },
                error: function (data) {
                    console.log(data.error);
                    nvsError(data.error);
                }
            });
        });
    } catch (e) {
        console.log(e);
    }
}


function fnLogOut()
{
    try {
        nvsConfirm("Are you sure to logout?", function () {
            $.ajax({
                type: "POST",
                url: "/logout",
                success: function (data) {
                    if (data.code > 0) {
                        window.location.href = "/login";
                    }
                    else {
                        nvsError(data.message);
                    }
                },
                error: function (data) {
                    console.log(data.error);
                }
            });
        });
    } catch (e) {
        console.log(e);
    }
}

function ReloadDataError(showPopup) {
    const pageIndex = 1;
    $.ajax({
        type: "GET",
        url: "/Home/ApplicationErrorPaging",
        data: {
            "pageIndex": pageIndex,
        },       
        success: function (data) {
            if (showPopup == false) {
                $("#ApplicationErrorID").html(data);
            }
        }        
    });
}

function ReloadDataRejection() {
    const pageIndex = 1;
    $.ajax({
        type: "GET",
        url: "/Home/RejectionPaging",
        data: {
            "pageIndex": pageIndex,
        },
        success: function (data) {
            $("#RejectAreaID").html(data);
        }
    });
}

function ReloadSearchListSecurities() {
    $.ajax({
        type: "GET",
        url: "/Home/ReloadSearchListSecurities",        
        success: function (data) {
            $("#SecuritiesAreaID").html(data);
        }
    });
}

function btnChangeGatewaySequence() {
    try {
        var _txtSequence = $('#txtSequence').val().trim() || '';
        if (_txtSequence == '' || _txtSequence == null) {
            nvsError("Sequence is required!");
            return;
        }
        var _txtLastSequence = $('#txtLastSequence').val().trim() || '';
        if (_txtLastSequence == '' || _txtLastSequence == null) {
            nvsError("LastSequence is required!");
            return;
        }

        var _numberSequence = Number(_txtSequence);
        var _numberLastSequence = Number(_txtLastSequence);
        if (isNaN(_numberSequence) || _numberSequence < 0) {
            nvsError("Sequence is invalid!");
            return;
        }
        if (isNaN(_numberLastSequence) || _numberLastSequence < 0) {
            nvsError("LastSequence is invalid!");
            return;
        }
        nvsConfirm("Are you sure to process change gateway sequence?", function () {
            $.ajax({
                type: "POST",
                url: "/change-gateway-sequence",
                data: {
                    "sequence": _txtSequence,
                    "lastprocessSequence": _txtLastSequence
                },
                success: function (data) {
                    if (data.code > 0) {
                        nvsSuccess(data.message);
                        $('#txtSequence').val('');
                        $('#txtLastSequence').val('');
                    }
                    else {
                        nvsError(data.message);
                    }
                },
                error: function (data) {
                    console.log(data.error);
                    nvsError(data.error);
                }
            });
        });
    } catch (e) {
        console.log(e);
    }
}