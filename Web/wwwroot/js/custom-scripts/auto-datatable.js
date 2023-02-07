﻿var datatable;
var actionIcons = {};
var showSelectedFilters = true;
var isAjaxBasedCrud = true;
var enableButtons = true;
actionIcons["Update"] = "fa-solid fa-pen-to-square";
actionIcons["Profile"] = "fa-solid fa-user-plus";
actionIcons["Notes"] = "fa-solid fa-file";
actionIcons["View"] = "fa-solid fa-eye";
actionIcons["History"] = "fa-solid fa-history";
actionIcons["Detail"] = "fas fa-folder-open"
actionIcons["Approve"] = "icon-file-text"
actionIcons["Delete"] = "icon-trash";
actionIcons["POD"] = "fa-solid fa-file";
actionIcons["Timesheet"] = "fa-solid fa-file-spreadsheet";
actionIcons["Report"] = "icon-copy";
actionIcons["ResetPassword"] = "icon-key";
actionIcons["Approve"] = "icon-checkmark4";
actionIcons["AddToCart"] = "icon-cart2";
actionIcons["Comments"] = "fa-solid fa-comments";
actionIcons["Invoice"] = "fa-solid fa-file-invoice-dollar";
actionIcons["Print"] = "fa-solid fa-print";

function CallBackFunctionality() {
}
CallBackFunctionality.prototype.GetFunctionality = function () {
    return "";
}

function InitializeDataTables(dtColumns, dataUrl = "", enableButtonsParam = true, isAjaxBasedCrudParam = true, isResponsive = true, selectableRow = false) {
    isAjaxBasedCrud = isAjaxBasedCrudParam;
    enableButtons = enableButtonsParam;
    var currentController = window.location.pathname.split('/')[1];
    var dataAjaxUrl = dataUrl;
    if (dataAjaxUrl === "") {
        dataAjaxUrl = "/" + currentController + "/Search";
    }
    //custom modification for cbo index in activity 
    var tableId = $("table.datatable-basic.dataTable").attr('id');
    var formId = $(".search-form").attr('id');
    var actionsList = [];
    //For Showing Loader
    $("#" + tableId).append("<button id='loader' style='display:none' type='button' class='btn bg-custom-dark btn-float rounded-round'><i class='icon-spinner4 spinner'></i></button>");

    $(document).off('click', '.clear-form-btn');
    $(document).on('click', '.clear-form-btn', function () {
        ClearDatatableSearch(dataAjaxUrl, tableId, formId, actionsList, dtColumns);
    });
    $(document).off('click', '.badge-datatable-clear');
    $(document).on('click', '.badge-datatable-clear', function () {
        ClearDatatableSearch(dataAjaxUrl, tableId, formId, actionsList, dtColumns);
    });
    $(document).off('click', '.badge-datatable-search');
    $(document).on('click', '.badge-datatable-search', function () {
        var name = $(this).attr('data-input-name');
        RemoveSearchFilterInput(name, dataAjaxUrl, tableId, formId, actionsList, dtColumns);
    });

    $(document).off('click', '.search-form-btn');
    $(document).on('click', '.search-form-btn', function () {
        SearchDataTable(dataAjaxUrl, tableId, formId, actionsList, dtColumns);
    });
    $(document).off('click', '#search-filters');
    $(document).on('click', '#search-filters', function () {
        $(".search-filter-container").show();
        $(".list-container").hide();
    });
    $(document).off('click', '#back-to-list');
    $(document).on('click', '#back-to-list', function () {
        $(".search-filter-container").hide();
        $(".list-container").show();
    });
    $(document).off('click', '.delete');
    $(document).on('click', '.delete', function (e) {
        e.preventDefault();
        let deleteObj = {
            ajaxSearchUrl: dataAjaxUrl,
            tableId: tableId,
            formId: formId,
            actionsList: actionsList,
            dtColumns: dtColumns,
            deleteUrl: $(this).attr('href'),
            confirmBtnText: "",
            cancelBtnText: "",
            deleteReturnUrl: ""
        }

        DeleteDataItem(deleteObj);

    });
    $(document).off('click', '.cancel');
    $(document).on('click', '.cancel', function (e) {
        e.preventDefault();
        let deleteObj = {
            ajaxSearchUrl: dataAjaxUrl,
            tableId: tableId,
            formId: formId,
            actionsList: actionsList,
            dtColumns: dtColumns,
            deleteUrl: $(this).attr('href'),
            confirmBtnText: "Yes, cancel it!",
            cancelBtnText: "No!",
            deleteReturnUrl: $(this).data("return-url")
        }
        DeleteDataItem(deleteObj);

    });
    FilterDataTable(dataAjaxUrl, tableId, formId, actionsList, dtColumns, isResponsive, selectableRow);
}
function FilterDataTable(dataAjaxUrl, tableId, formId, actionsList, dtColumns, isResponsive, selectableRow) {
    if (typeof isResponsive != "boolean") {
        isResponsive = isResponsive == "true";
    }
    var selectableRowObj = {}
    if (selectableRow) {
        selectableRowObj = {
            orderable: false,
            className: 'select-checkbox',
            targets: 0,
            data: null,
            defaultContent: ''
        };
    }

    var columnsIndexExcludingAction = [];
    for (var i = 0; i <= (dtColumns.length - 1); i++) {
        columnsIndexExcludingAction.push(i);
    }
    datatable = $('#' + tableId).dataTable({
        "serverSide": true,
        "proccessing": true,
        "searching": true,
        "responsive": isResponsive,
        "ordering": true,
        "order": GetColumnSortings(dtColumns),
        "pagingType": "full_numbers",
        "lengthMenu": [[10, 25, 50, 250], [10, 25, 50, 250]],
        //lBfrtipF
        "dom": "<'datatable-header'B<'float-right'l>f>" + // Remove B to remove buttons
            "<'datatable-scroll'tr>" +
            "<'datatable-footer'ip>",
        "ajax": {
            url: dataAjaxUrl,
            type: 'POST',
            dataType: "json",
            "data": function (searchParams) {
                $("#" + tableId + " td").removeAttr("colspan");
                if ($("#loader").length > 0) {
                    $('#' + tableId).block({
                        message: $("#loader"),
                        centerY: false,
                        centerX: false,
                        css: {
                            margin: 'auto',
                            border: 'none',
                            padding: '15px',
                            backgroundColor: 'transparent',
                            '-webkit-border-radius': '10px',
                            '-moz-border-radius': '10px',
                            color: '#fff'
                        }

                    });
                }
                if ($('#' + formId).length > 0) {
                    $('#' + formId + ' :input, #' + formId + ' select').each(function (key, val) {
                        if (val.value !== "") {
                            searchParams[val.name] = $(val).val();
                        }
                    });
                }
                if (searchParams.length === -1) {
                    searchParams["Pagination"] = false;
                }
                else {
                    searchParams["Pagination"] = true;
                }
                searchParams["CurrentPage"] = (searchParams.start / searchParams.length) + 1;
                searchParams["PerPage"] = searchParams.length;
                searchParams["CalculateTotal"] = true;
                if (searchParams.order.length > 0) {
                    searchParams["OrderByColumn"] = dtColumns[searchParams.order[0].column].sortingColumn;
                    searchParams["OrderDir"] = searchParams.order[0].dir;
                }
                searchParams["Draw"] = searchParams.draw;
                return searchParams;
            },
            "dataSrc": function (json) {
                actionsList = json.ActionsList;
                showSelectedFilters = json.ShowSelectedFilters;
                SetSearchFilters();
                return json.Items;
            }
        },
        "language": {
            "search": "",
            "searchPlaceholder": "Search"
        },
        columns: dtColumns,
        "columnDefs": [
            selectableRowObj,
            {
                "targets": columnsIndexExcludingAction,
                "render": function (data, type, row, meta) {
                    if (dtColumns[meta.col].title !== 'Action') {
                        if (dtColumns[meta.col].format === 'html') {
                            return RenderHtml(data, dtColumns, meta);
                        }
                        else if (dtColumns[meta.col].format === 'numeric') {
                            return RenderNumericValue(data, dtColumns, meta);
                        }
                        else if (dtColumns[meta.col].title == '' || dtColumns[meta.col].data == '') {
                            return '';
                        }
                        else {
                            return data;
                        }
                    }
                    else {
                        return GetActionLinks(actionsList, data);
                    }
                }
            },
            {
                "targets": -1,
                "className": "text-right",
            },
            //{
            //    "targets": -1,
            //    "createdCell": function (td, cellData, rowData, row, col) {
            //        if (dtColumns[col].title === "Action") {
            //            GetActionLinks(actionsList, cellData, td);
            //        }
            //    }
            //}
        ],
        select: {
            style: 'multi',
            selector: 'td:first-child'
        },
        "buttons": {
            dom: {
                button: {
                    tag: 'button',
                    className: 'btn rounded-round bg-custom-dark datatable-extension-button'
                }
            },
            'buttons': [
                {
                    extend: 'copy',
                    exportOptions: {
                        columns: getColumnsToExport,
                        page: 'current'
                    }
                },
                {
                    extend: 'csv',
                    exportOptions: {
                        columns: getColumnsToExport,
                        page: 'current'
                    }
                },
                {
                    extend: 'excel',
                    exportOptions: {
                        columns: getColumnsToExport,
                        page: 'current'
                    }
                },
                {
                    extend: 'pdf',
                    exportOptions: {
                        columns: getColumnsToExport,
                        page: 'current',
                    },
                    customize: customizePdfExport
                },
                {
                    extend: 'print',
                    exportOptions: {
                        columns: getColumnsToExport,
                        page: 'current'
                    }
                },
                'colvis'
            ]
        },
        "drawCallback": function (settings) {
            $('#' + tableId).unblock();
            new CallBackFunctionality().GetFunctionality();
            maskDatatableCurrency("td.dt-currency", ('#' + tableId));
        }
    });
    SetSearchFilters();
}
function GetColumnSortings(columns) {
    var sortedColumns = [];
    $.each(columns, function (i, v) {
        if (v.orderable) {
            console.log("orderable column");
            var sortedColumn = [i, 'asc'];
            sortedColumns.push(sortedColumn);
        }
    });
    return sortedColumns;
}
function GetActionLinks(actionsList, cellData) {
    var actionHtml = "";
    if (actionsList.length > 4) {

        actionHtml = "<div class='list-icons'>";
        actionHtml += "<div class='dropdown show'>";
        actionHtml += "<div class='dropdown show'>";
        actionHtml += "<a href='#' class='list-icons-item' data-toggle='dropdown' aria-expanded='true'>";
        actionHtml += "<i class='icon-menu9'></i>";
        actionHtml += "</a>";
        actionHtml += "<div class='dropdown-menu dropdown-menu-right'>";

        $.each(actionsList, function (index, actionItem) {
            if (cellData.IsUserDefined === false || cellData.IsUserDefined === undefined)
                actionHtml += GetHref(actionItem, cellData);
        });
        actionHtml += "</div>";
    }
    else {
        $.each(actionsList, function (index, actionItem) {
            //to avoid the system defined record in Account table to be deleted.
            if (cellData.IsUserDefined === false || cellData.IsUserDefined === undefined)
                actionHtml += GetHref(actionItem, cellData);
        });
    }
    return actionHtml;
    //$(td).html(actionHtml);
}
function GetHref(actionItem, cellData) {
    var link = "javascript:void(0)";
    if (actionItem.Href !== '') {
        link = actionItem.Href;
        if (link.split('/').length > 3) {
            link = "/" + link.split('/')[1] + "/" + link.split('/')[2] + "/" + GetCellObjectValue(cellData, link.split('/')[3]);
        }
        if (link.split('/').length > 2) {
            linkAndQueryParams = actionItem.Href.split('?');
            if (linkAndQueryParams.length > 1) {
                var queryParams = linkAndQueryParams[1].split('&');
                link = actionItem.Href.split('?')[0] + "?";
                $.each(queryParams, function (index, value) {
                    link = link + $.trim(value.split('=')[0]) + "=" + GetCellObjectValue(cellData, $.trim(value.split('=')[0])) + "&";

                });
                if (queryParams.length > 1) {
                    link = link.slice(0, -1);
                }
            }
        }
    }
    var appendClass = "";
    var dataAttributes = "";
    if (actionItem.Action === "Delete") {
        appendClass = "delete";
    }
    //if (actionItem.Action === "Approve") {
    //    appendClass = "review-user";
    //}
    if (actionItem.Action === "Cancel") {
        appendClass = "cancel";
        dataAttributes = "data-return-url='" + actionItem.ReturnUrl + "'";
    }
    if (actionItem.Class !== null && actionItem.Class !== "") {
        appendClass = " " + actionItem.Class;
    }
    var customAttributes = "";
    if (actionItem.Attr !== null && actionItem.Attr.length > 0) {
        actionItem.Attr.forEach(function (v, i) {
            customAttributes += "attr-" + v.toLowerCase() + '="' + GetCellObjectValue(cellData, v) + '" ';
        });
    }
    if (isAjaxBasedCrud && actionItem.Title != "Delete") {
        return `<a href="#" onclick="loadUpdateAndDetailModalPanel('${link}')" ${dataAttributes} class="datatable-action ${appendClass}" ${customAttributes}> <i class="${actionIcons[actionItem.Title]}"></i>${actionItem.LinkTitle}</a>`
    }
    return '<a href="' + link + '" ' + dataAttributes + ' class="datatable-action ' + appendClass + '" ' + customAttributes + '> <i class="' + actionIcons[actionItem.Title] + '"></i> ' + actionItem.LinkTitle + '</a > ';
}

function GetCellObjectValue(cellData, Prop) {
    if (Prop.indexOf(".") !== -1) {
        return cellData["" + Prop.split('.')[0]]["" + Prop.split('.')[1]];
    }
    else {
        return cellData['' + Prop];
    }
}
function RenderHtml(data, dtColumns, meta) {
    if (dtColumns[meta.col].formatValue === "checkbox") {
        return '<input type="checkbox" class="checkbox-items ' + dtColumns[meta.col].className + '" value="' + data + '" />';
    }
    else if (dtColumns[meta.col].formatValue === "textbox") {
        return '<input type="text" class="text-box-items ' + dtColumns[meta.col].className + '" value="' + data + '" />';
    }
    else if (dtColumns[meta.col].formatValue === "number-textbox") {
        return '<input type="number" class="number-text-box-items ' + dtColumns[meta.col].className + '" value="' + data + '" />';
    }
    else if (dtColumns[meta.col].formatValue === "badge") {
        return '<span class="badge ' + data + '">' + data + '</span>';
    }
    else if (dtColumns[meta.col].formatValue === "link") {
        return '<a href="' + data + '" target="_blank"><i class="fa-solid fa-link"></i></a>';
    }
    else if (dtColumns[meta.col].formatValue === "image") {
        return ' <img src="' + data + '" class="rounded" alt="Image">';
    }
    else if (dtColumns[meta.col].formatValue === "status") {
        return '<span class="badge ' + data + '"></span>';
    }
    else if (dtColumns[meta.col].formatValue === "barcode") {
        return '<span data-barcode="' + data + '"><i class="fa fa-barcode"></i></span>';
    }
    else if (dtColumns[meta.col].formatValue === "detail") {
        return '<span class="details-control" data-url="' + dtColumns[meta.col].detailUrl + '">' + (data === undefined ? "" : data) + '</span>';
    }
    else if (dtColumns[meta.col].formatValue === "hidden") {
        return '<div>' + data + '</div><input type="hidden" class="hidden ' + dtColumns[meta.col].className + '" value="' + data + '">';
    }
    else if (dtColumns[meta.col].formatValue === "hidden-div") {
        return '<input type="hidden" class="hidden ' + dtColumns[meta.col].className + '" value="' + data + '">';
    }
}
function RenderNumericValue(data, dtColumns, meta) {
    if (dtColumns[meta.col].formatValue !== null && dtColumns[meta.col].formatValue !== "") {
        return data.toFixed(2);
    }
    return data.toFixed(dtColumns[meta.col].formatValue);
}
function SetSearchFilters() {
    var containerHtml = "";
    if (showSelectedFilters) {
        $('.search-filter-container input, .search-filter-container select').each(
            function (index) {
                var input = $(this);
                if (input.attr('type') != "hidden" && input.val() != "") {
                    var value = input.val();
                    var inputName = input.attr('name');
                    if ($(input).data('select2')) {
                        value = $(input).select2('data')[0].text
                    }
                    else if ($(input).is('select')) {
                        value = $(input).find(":selected").text();
                    }
                    if (containerHtml == "") {
                        containerHtml = "<span class='mr-1'>Filters: </span>";
                    }
                    containerHtml += "<span class='badge badge-datatable-search mb-1' data-input-name='" + inputName + "'>" + $(input).parent().find("label").html() + " : " + value + "</span>";
                }
            }
        );
        if (containerHtml != "") {
            containerHtml += "<span class='datatable-clear-container'><span class='badge badge-datatable-clear mb-1'>Clear</span></span>";
            containerHtml = "<div class='row col-12'>" + containerHtml + "</div>";
        }
        $(".selected-filters-container").html(containerHtml);
    }
    else {
        $(".selected-filters-container").html("");
    }
}
function ClearDatatableSearch(dataAjaxUrl, tableId, formId, actionsList, dtColumns) {
    $('#' + formId).trigger("reset");
    $('select[class*="select2"]').each(function (i, element) {
        $(element).val('').trigger('change');
    });
    SearchDataTable(dataAjaxUrl, tableId, formId, actionsList, dtColumns);
}
function RemoveSearchFilterInput(name, dataAjaxUrl, tableId, formId, actionsList, dtColumns) {
    var input = $("#" + formId + " input[name=" + name + "]");
    if ($(input).data('select2')) {
        $(input).val('').trigger('change');
    }
    else {
        $(input).val("");
    }
    SearchDataTable(dataAjaxUrl, tableId, formId, actionsList, dtColumns);
}
function SearchDataTable(dataAjaxUrl, tableId, formId, actionsList, dtColumns) {
    $(".list-container").show();
    $(".search-filter-container").hide();
    $("#" + tableId).dataTable().fnDestroy();
    FilterDataTable(dataAjaxUrl, tableId, formId, actionsList, dtColumns);
}

function DeleteDataItem(deleteObj) {

    let confirmBtnText = deleteObj.confirmBtnText;
    let cancelBtnText = deleteObj.cancelBtnText;
    let deleteReturnUrl = deleteObj.deleteReturnUrl;
    if (confirmBtnText === "") {
        confirmBtnText = "Yes, delete it!";
    }
    if (cancelBtnText === "") {
        cancelBtnText = "No, cancel!";
    }
    var deleteUrl = deleteObj.deleteUrl;
    swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        type: 'error',
        //icon: 'warning',
        showCancelButton: true,
        confirmButtonText: confirmBtnText,
        cancelButtonText: cancelBtnText,
        confirmButtonClass: 'btn btn-success',
        cancelButtonClass: 'btn btn-danger',
        buttonsStyling: false
    }).then(function (result) {
        if (result.value) {
            DeleteCardItem(deleteUrl).then(function (ajaxResult) {
                if (ajaxResult.Success) {
                    if (ajaxResult.ReloadDatatable) {
                        SearchDataTable(deleteObj.ajaxSearchUrl, deleteObj.tableId, deleteObj.formId, deleteObj.actionsList, deleteObj.dtColumns);
                    }
                    else {
                        if (deleteReturnUrl === "" || deleteReturnUrl === null || deleteReturnUrl === undefined) {
                            location.reload();
                        }
                        else {
                            window.location.href = deleteReturnUrl;
                        }
                    }

                }
                else {
                    Swal.fire("Couldn't delete. Try again later.")
                }
            });

        }
        else if (result.dismiss === swal.DismissReason.cancel) {
        }
    });
}
function DeleteCardItem(url) {
    return $.ajax({
        url: url,
        type: 'POST',
        success: function (res) {
        }
    });
}
function loadUpdateAndDetailModalPanel(contentUrl) {
    loadModalPanel(contentUrl, "crudModalPanel", "crudModalPanelBody")
}
function getColumnsToExport(idx, data, node) {
    if (shouldColumnBeIgnoredForExport(node)) {
        return false;
    }
    return true;
}
function shouldColumnBeIgnoredForExport(element) {
    if ($(element).hasClass("exclude-form-export") || $(element).is(":visible") === false)
        return true;
    return false;

}
function shouldBeIgnoredInPdfExport(element) {
    let th = $(element).closest('table').find('th').eq($(element).index())
    return shouldColumnBeIgnoredForExport(th);
}
function customizePdfExport(doc) {
    var colCount = new Array();
    $(datatable).find('tbody tr:first-child td').each(function () {
        if (!shouldBeIgnoredInPdfExport($(this))) {
            if ($(this).attr('colspan')) {
                for (var i = 1; i <= $(this).attr('colspan'); $i++) {
                    colCount.push('*');
                }
            } else {
                colCount.push('*');
            }
        }

    });
    if (colCount.length < 8)// Cuts columns for table with more than 8 columns
        doc.content[1].table.widths = colCount;
    var rowCount = datatable[0].rows.length;
    for (i = 0; i < rowCount; i++) {
        for (j = 0; j < colCount.length; j++) {
            doc.content[1].table.body[i][j].alignment = 'center';
        }

    };
}

//datatable.on("click", "th.select-checkbox", function () {
//    if ($("th.select-checkbox").hasClass("selected")) {
//        example.rows().deselect();
//        $("th.select-checkbox").removeClass("selected");
//    } else {
//        example.rows().select();
//        $("th.select-checkbox").addClass("selected");
//    }
//}).on("select deselect", function () {
//    ("Some selection or deselection going on")
//    if (example.rows({
//        selected: true
//    }).count() !== example.rows().count()) {
//        $("th.select-checkbox").removeClass("selected");
//    } else {
//        $("th.select-checkbox").addClass("selected");
//    }
//});

$("th.select-checkbox").on("click", function (e) {
    debugger;
    if ($(this).is(":checked")) {
        datatable.rows().select();
    } else {
        datatable.rows().deselect();
    }
});