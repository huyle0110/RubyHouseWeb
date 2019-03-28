var dateOption = {
    clearBtn: true,
    orientation: "bottom left",
    keyboardNavigation: false,
    showWeek: false,
    todayHighlight: true,
    autoclose: true,
    language = "vi",
    format = "dd/mm/yyyy",
    //calendarWeeks: true,
}
$(document).ready(function () {
    $('.FullDateOption').datepicker('set', dateOption);
});