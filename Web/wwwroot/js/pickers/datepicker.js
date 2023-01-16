/* ------------------------------------------------------------------------------
 *
 *  # Date and time pickers
 *
 *  Demo JS code for picker_date.html page
 *
 * ---------------------------------------------------------------------------- */


// Setup module
// ------------------------------

var DateTimePickers = function () {



    // Pickadate picker
    var _componentPickadate = function () {
        if (!$().pickadate) {
            console.warn('Warning - picker.js and/or picker.date.js is not loaded.');
            return;
        }

        // Basic options
        $('.pickadate').pickadate();

        // Change day names
        $('.pickadate-strings').pickadate({
            weekdaysShort: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'],
            showMonthsShort: true
        });

        // Button options
        $('.pickadate-buttons').pickadate({
            today: '',
            close: '',
            clear: 'Clear selection'
        });

        // Accessibility labels
        $('.pickadate-accessibility').pickadate({
            format: 'dddd, dd mmmm, yyyy',
            formatSubmit: 'yyyy-mm-dd',
            labelMonthNext: 'Go to the next month',
            labelMonthPrev: 'Go to the previous month',
            labelMonthSelect: 'Pick a month from the dropdown',
            labelYearSelect: 'Pick a year from the dropdown',
            selectMonths: true,
            selectYears: true
        });
        $('.pickadatetime-accessibility').pickadate({
            format: 'dddd, dd mmmm, yyyy hh:mm',
            formatSubmit: 'yyyy-mm-dd hh:mm',
            labelMonthNext: 'Go to the next month',
            labelMonthPrev: 'Go to the previous month',
            labelMonthSelect: 'Pick a month from the dropdown',
            labelYearSelect: 'Pick a year from the dropdown',
            selectMonths: true,
            selectYears: true
        });


        // Format options
        //$('.pickadate-format').pickadate({
        //    // Escape any “rule” characters with an exclamation mark (!).
        //    format: 'd mmmm, yyy',
        //    formatSubmit: 'yyyy-mm-dd',
        //    hiddenPrefix: 'prefix__',
        //    hiddenSuffix: '__suffix'
        //});

        // Editable input
        var $input_date = $('.pickadate-editable').pickadate({
            editable: true,
            onClose: function () {
                $('.datepicker').focus();
            }
        });
        var picker_date = $input_date.pickadate('picker');
        $input_date.on('click', function (event) {
            if (picker_date.get('open')) {
                picker_date.close();
            } else {
                picker_date.open();
            }
            event.stopPropagation();
        });

        // Dropdown selectors
        $('.pickadate-selectors').pickadate({
            selectYears: true,
            selectMonths: true
        });

        // Year selector
        $('.pickadate-year').pickadate({
            selectYears: 4
        });

        // Set first weekday
        $('.pickadate-weekday').pickadate({
            firstDay: 1
        });

        // Date limits
        $('.pickadate-limits').pickadate({
            min: [2014, 3, 20],
            max: [2014, 7, 14]
        });

        // Disable certain dates
        $('.pickadate-disable').pickadate({
            disable: [
                [2015, 8, 3],
                [2015, 8, 12],
                [2015, 8, 20]
            ]
        });

        // Disable date range
        $('.pickadate-disable-range').pickadate({
            disable: [
                5,
                [2013, 10, 21, 'inverted'],
                { from: [2014, 3, 15], to: [2014, 3, 25] },
                [2014, 3, 20, 'inverted'],
                { from: [2014, 3, 17], to: [2014, 3, 18], inverted: true }
            ]
        });
    };



    //
    // Return objects assigned to module
    //

    return {
        init: function () {
            _componentPickadate();
        }
    }
}();


// Initialize module
// ------------------------------
function InitializeDatePicker(id) {
    $('#' + id).pickadate();
}
document.addEventListener('DOMContentLoaded', function () {
    DateTimePickers.init();
});
