$(function()
{
    var $file_table = $(".file_table");
    var $submit = $("input[type=submit]");

    $file_table.delegate(".file_row_button", "click", function () {
        $(this).closest(".file_row").remove();
    });

    $file_table.delegate(".file_add_button", "click", function () {
        $(this).closest(".file_row").find("input[type=file]").trigger("click");
    });

    $file_table.delegate("input[type=file]", "change", function ()
    {
        var $upload_input = $(this);

        var file_name = $upload_input.val();
        if (file_name != '')
        {
            var $row = $upload_input.closest(".file_row");

            var $table = $row.closest(".file_table");
            var $last_row_clone = $table.find(".file_row").last().clone();
            $table.append($last_row_clone);

            file_name = file_name.substring(Math.max(file_name.lastIndexOf('/'), file_name.lastIndexOf('\\')) + 1) + " (New)";
            $row.find(".file_row_name").html(file_name);
        }
    });

    $submit.on("click", function () {
        $file_table.each(function (index, element) {
            var $table = $(element);
            $table.find(".file_row_id").attr("name", function (index, attr) {
                return "Files[" + index + "].ID";
            });
            $table.find("input[type=file]").attr("name", function (index, attr) {
                return "UploadedFiles[" + index + "]";
            });
        });
    });
});