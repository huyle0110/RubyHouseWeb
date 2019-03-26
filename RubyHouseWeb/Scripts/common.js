var dateOption = {
    autoclose: true,
    format: 'mm/dd/yyyy',
    language: 'vn',
    todayHighlight: true,

};
$(document).ready(function () {
    $('.FullDateOption').datepicker('set', dateOption);
});