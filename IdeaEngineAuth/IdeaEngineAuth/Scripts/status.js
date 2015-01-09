$(function ()
{
    var $idea_engine_status_select = $(".idea_engine_status_select");

    var $idea_engine_school_select = $(".idea_engine_school_select");
    var $m_n_add_option = $(".m_n_add_option");

    function assign()
    {
        var selected = $idea_engine_status_select.find(":selected");
        if (selected.length > 0)
        {
            var status = selected.val();
            if ("submitted" === status.trim().toLowerCase()) {
                $idea_engine_status_select.val("Assigned");
            }
        }
    };

    $idea_engine_school_select.on("change", function ()
    {
        var school_id = $(this).find(":selected").val();
        if ( 0 != school_id )
        {
            assign();
        }
    });

    $m_n_add_option.on("click", function ()
    {
        assign();
    });
});