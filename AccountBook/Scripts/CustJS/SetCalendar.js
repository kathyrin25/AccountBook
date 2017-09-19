function SetCalendar_datetimepicker() {
    /*注意屬性名稱 :data-datetimepicker 和 DateTimeInput.cshtml : data_datetimepicker 對應*/
    $('[data-datetimepicker]').each(function () {
        var id = $(this).attr('id');

        $(this).pickadate({
            format: 'yyyy/mm/dd',
            selectYears: true,
            selectMonths: true,
            selectYears: 4
        });

    }).css("cursor", "pointer");
}