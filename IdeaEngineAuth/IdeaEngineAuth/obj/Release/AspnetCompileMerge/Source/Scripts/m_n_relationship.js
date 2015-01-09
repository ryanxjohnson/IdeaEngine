$(function ()
{
    var $m_n_table = $(".m_n_table");
    var $m_n_add_option = $(".m_n_add_option");
    var $submit = $("input[type=submit]");
    var $m_n_select = $(".m_n_select");

    $m_n_table.delegate(".m_n_row_button", "click", function () {
        var $row = $(this).closest(".m_n_row");

        var id = $row.find(".m_n_row_id").val();
        var name = $row.find(".m_n_row_name").html();
        var html = "<option class='m_n_option' value='" + id + "'>" + name + "</option>";

        var $selectContainer = $row.closest(".m_n_table").find(".m_n_select_container");
        var $select = $selectContainer.find(".m_n_select");

        $select.append(html);
        $selectContainer.show();

        $row.remove();
    });

    $m_n_add_option.on("click", function () {
        var $selectContainer = $(this).closest(".m_n_select_container");
        var $select = $selectContainer.find(".m_n_select");
        var $option = $select.find(":selected");

        var id = $option.val();
        var name = $option.text();

        var html = "<tr class='m_n_row'>" +
                        "<td class='m_n_row_name'>" + name + "</td>" +
                        "<td>" +
                            "<input class='m_n_row_id' name='null' type='hidden' value='" + id + "'>" +
                            "<p class='btn btn-xs m_n_row_button'>Remove</p>" +
                        "</td>" +
                    "</tr>";

        $selectContainer.before(html);

        $option.remove();

        if ($select.children("option").length <= 0) {
            $selectContainer.hide();
        }
    });

    $submit.on("click", function () {
        $m_n_table.each(function (index, element) {
            var $table = $(element);
            var setName = $table.attr("data-m-n-set-name");
            $table.find(".m_n_row_id").attr("name", function (index, attr) {
                return setName + "[" + index + "].ID";
            });
        });
    });

    $m_n_select.each(function (index, element) {
        var $select = $(element);
        if ($select.children("option").length <= 0) {
            $select.closest(".m_n_select_container").hide();
        }
    });
});