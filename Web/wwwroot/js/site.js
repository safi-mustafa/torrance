// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function AppendSignInElement(element, sign) {
    var inputValue = $(element).val();

    // Remove existing "$" symbols and any non-digit characters except for "."
    var numericValue = inputValue.replace(/[^0-9.]/g, "");

    // Ensure there's only one "." in the numeric value
    numericValue = numericValue.replace(/(\..*)\./g, "$1");

    // Add "$" symbol and update the textbox value
    $(element).val(sign + numericValue);
}