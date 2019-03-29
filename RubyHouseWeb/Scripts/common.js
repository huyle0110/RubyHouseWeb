var dateOption = {
    clearBtn: true,
    language: "vi",
    format: "dd/mm/yyyy",
    orientation: "bottom left",
    keyboardNavigation: false,
    showWeek: false,
    todayHighlight: true,
    autoclose: true,
    //calendarWeeks: true,
}
$(document).ready(function () {
    var lst_dateinput = $('input.FullDateInput');
    var length = lst_dateinput.length;
    for (var i = 0; i < length; i++) {
        var item = lst_dateinput[i];
        $(item).datepicker({
            clearBtn: true,
            language: "vi",
            format: "dd/mm/yyyy",
            orientation: "bottom left",
            keyboardNavigation: false,
            showWeek: false,
            todayHighlight: true,
            autoclose: true,
        });
    }

    $(document).on('show.bs.modal', '.modal', function () {
        var zIndex = 1040 + (10 * $('.modal:visible').length);
        $(this).css('z-index', zIndex);
        //setTimeout(function () {
        //    $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
        //}, 0);
    });
});

function setrangedate(f, t, datef, datet) {
    setTimeout(function () {
        if (datef && datef) {
            f.datepicker('setEndDate', datet);
            t.datepicker('setStartDate', datef);
        }
        else {
            f.datepicker('setEndDate', t.datepicker('getDate'));
            t.datepicker('setStartDate', f.datepicker('getDate'));

        }
        if (datef && datef) {
            f.datepicker().on('changeDate', function (e) {
                f.datepicker('setEndDate', datet);
            });

            t.datepicker().on('changeDate', function (e) {
                t.datepicker('setStartDate', datef);
            });
        }
        else {
            f.datepicker().on('changeDate', function (e) {
                t.datepicker('setStartDate', f.datepicker('getDate'));

            });

            t.datepicker().on('changeDate', function (e) {
                f.datepicker('setEndDate', t.datepicker('getDate'));

            });
        }
    }, 200);

}