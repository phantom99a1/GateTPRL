var nvsAlert = function ($title, $content, $fncallback) {
    try {
        $title = $title == undefined ? "Info" : $title;
        swal({
            title: $title,
            text: $content,
            allowEscapeKey: false
        }).then(function () {
            if (typeof $fncallback === "function") {
                $fncallback();
            }
        }, function (dismiss) {
            if (typeof $fncallback === "function") {
                $fncallback();
            }
        });
    } catch (e) {
    }
},
    nvsInfo = function ($content, $fncallback) {
        try {
            swal({
                title: "Info",
                text: $content,
                type: "info",
                showCancelButton: true,
                allowEscapeKey: false
            }).then(function () {
                if (typeof $fncallback === "function") {
                    $fncallback();
                }
            }, function (dismiss) {
                if (typeof $fncallback === "function") {
                    $fncallback();
                }
            });
        } catch (e) {
        }
    },
    nvsSuccess = function ($content, $fncallback) {
        try {
            swal({
                title: "Success",
                text: $content,
                type: "success"
            }).then(function () {
                if (typeof $fncallback === "function") {
                    $fncallback();
                }
            }, function (dismiss) {
                if (typeof $fncallback === "function") {
                    $fncallback();
                }
            });

        } catch (e) {
        }
    },
    nvsConfirm = function ($content, $fnokcallback, $fncancelcallback) {
        try {
            swal({
                title: "Warning",
                text: $content,
                type: "question",
                showCancelButton: true,
                allowEscapeKey: false,
                confirmButtonText: 'OK',
                cancelButtonText: "Cancel",
            }).then(function () {
                if (typeof $fnokcallback === "function") {
                    $fnokcallback();
                }
            }, function (dismiss) {
                if (typeof $fncancelcallback === "function") {
                    $fncancelcallback();

                }
            });
        } catch (e) {
        }
    },
    nvsConfirmInfo = function ($content, $fnokcallback, $fncancelcallback) {
        try {
            swal({
                title: "Info",
                text: $content,
                type: "info",
                showCancelButton: true,
                allowEscapeKey: false,
                confirmButtonText: 'OK',
                cancelButtonText: "Cancel",
            }).then(function () {
                if (typeof $fnokcallback === "function") {
                    $fnokcallback();
                }
            }, function (dismiss) {
                if (typeof $fncancelcallback === "function") {
                    $fncancelcallback();
                }
            });
        } catch (e) {
        }
    },
    nvsWarning = function ($content, $fnokcallback, $fncancelcallback) {
        try {
            swal({
                title: "Warning",
                text: $content,
                type: "warning",
                showCancelButton: true,
                confirmButtonText: 'OK',
                cancelButtonText: "Cancel",
            }).then(function () {
                if (typeof $fnokcallback === "function") {
                    $fnokcallback();
                }
            }, function (dismiss) {
                if (typeof $fncancelcallback === "function") {
                    $fncancelcallback();
                }
            });
        } catch (e) {
        }
    },
    nvsError = function ($content, $fncallback) {
        try {
            swal({
                title: "Error",
                text: $content,
                type: "error"
            }).then(function () {
                if (typeof $fncallback === "function") {
                    $fncallback();
                }
            }, function (dismiss) {
                if (typeof $fncallback === "function") {
                    $fncallback();
                }
            });
        } catch (e) {
        }
    },
    nvsAlertWithHtml = function ($title, $html, $type, $fncallback) {
        try {
            swal({
                title: $title,
                html: $html,
                type: $type,
                showCancelButton: true,
                confirmButtonText: 'OK',
                cancelButtonText: "Cancel",
            }).then(function () {
                if (typeof $fncallback === "function") {
                    $fncallback();
                }
            }, function (dismiss) {
                if (typeof $fncallback === "function") {
                    $fncallback();
                }
            });
        } catch (e) {
        }
    },
    nvsConfirmProcess = function ($content, $fnokcallback, $fncancelcallback) {
        try {
            swal({
                title: "Warning",
                text: $content,
                type: "question",
                showCancelButton: true,
                allowEscapeKey: false,
                confirmButtonText: 'Continue',
                cancelButtonText: "Back",
            }).then(function () {
                if (typeof $fnokcallback === "function") {
                    $fnokcallback();
                }
            }, function (dismiss) {
                if (typeof $fncancelcallback === "function") {
                    $fncancelcallback();

                }
            });
        } catch (e) {
        }
    },
    nvsConfirmProcess2 = function ($content, $confirmText, $cancelText, $fnokcallback, $fncancelcallback) {
        try {
            swal({
                title: "Warning",
                text: $content,
                type: "question",
                showCancelButton: true,
                allowEscapeKey: false,
                confirmButtonText: $confirmText,
                cancelButtonText: $cancelText,
            }).then(function () {
                if (typeof $fnokcallback === "function") {
                    $fnokcallback();
                }
            }, function (dismiss) {
                if (typeof $fncancelcallback === "function") {
                    $fncancelcallback();

                }
            });
        } catch (e) {
        }
    },
    nvsPrompt = function ($title, $content, $type, $fnokcallback, $fnreject, $fncancelcallback, $maxlength) {
        try {
            $title = $title == undefined ? "" : $title;
            swal({
                title: $title,
                text: $content,
                input: $type,
                showCancelButton: true,
                //closeOnConfirm: false,
                animation: "slide-from-top",
                inputPlaceholder: "",
                confirmButtonText: 'OK',
                cancelButtonText: 'Cancel',
                inputAttributes: { maxlength: $maxlength, id: 'txtPromt_p' },
                preConfirm: function (inputValue) {
                    return new Promise(function (resolve, reject) {
                        if (typeof $fnreject === "function") {
                            $fnreject(inputValue, resolve, reject);
                        }
                    })
                },
                allowOutsideClick: false

            }).then(function (inputValue) {
                if (typeof $fnokcallback === "function") {
                    $fnokcallback(inputValue);
                }
            }, function (dismiss) {
                if (typeof $fncancelcallback === "function") {
                    $fncancelcallback();
                }
            });
        } catch (e) {
        }
    };