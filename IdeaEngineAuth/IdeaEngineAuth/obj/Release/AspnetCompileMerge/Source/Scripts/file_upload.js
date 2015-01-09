$(function()
{
    var $idea_engine_add_new_file = $(".idea_engine_add_new_file");
    var $idea_engine_file_upload_container = $(".idea_engine_file_upload_container");

    var $submit = $("input[type=submit]");

    $idea_engine_add_new_file.on('click', function () {
        $idea_engine_file_upload_container.append("<input type='file' name='null' />");
    });

    $submit.on("click", function () {
        $idea_engine_file_upload_container.each(function (index, element) {
            var $container = $(element);
            $container.find("input[type=file]").attr("name", function (index, attr) {
                return "UploadedFiles[" + index + "]";
            });
        });
    });
});