$(document).ready(function () {
	/** open modal **/
	$(".edit-inventory").click(function (event) {
		$("#inventory-modal").modal("show");
	});

	$(".open-bom").click(function (event) {
		$("#bom-modal").modal("show");
	});

	$(".open-notes").click(function (event) {
		$("#notes-modal").modal("show");
	});

	$(".open-photo").click(function (event) {
		$("#photo-modal").modal("show");
	});

	$(".open-history").click(function (event) {
		$("#history-modal").modal("show");
	});
});